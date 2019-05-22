#import <AdSupport/ASIdentifierManager.h>
#import <CommonCrypto/CommonDigest.h>

#import "CDVAdMob.h"

#import <GoogleMobileAds/GADExtras.h>

@interface CDVAdMob()

- (void) __setOptions:(NSDictionary*) options;
- (void) __createBanner;
- (void) __showAd:(BOOL)show;
- (BOOL) __showInterstitial:(BOOL)show;
- (void) __showRewardedVideo:(BOOL)show;
- (GADRequest*) __buildAdRequest;
- (NSString*) __md5: (NSString*) s;
- (NSString *) __getAdMobDeviceId;

- (void)resizeViews;

- (GADAdSize)__AdSizeFromString:(NSString *)string;

- (void)deviceOrientationChange:(NSNotification *)notification;
- (void) fireEvent:(NSString *)obj event:(NSString *)eventName withData:(NSString *)jsonStr;

@end

@implementation CDVAdMob

@synthesize bannerView = bannerView_;
@synthesize interstitialView = interstitialView_;
@synthesize rewardVideoView = rewardVideoView_;

@synthesize publisherId, interstitialAdId, rewardVideoId, adSize;
@synthesize bannerAtTop, bannerOverlap, offsetTopBar;
@synthesize isTesting, adExtras;

@synthesize bannerIsVisible, bannerIsInitialized;
@synthesize bannerShow, autoShow, autoShowBanner, autoShowInterstitial, autoShowRewardVideo;


@synthesize gender, forChild;

@synthesize rewardedVideoLock, isRewardedVideoLoading;

#define DEFAULT_BANNER_ID    @"ca-app-pub-3940256099942544/2934735716"
#define DEFAULT_INTERSTITIAL_ID @"ca-app-pub-3940256099942544/4411468910"
#define DEFAULT_REWARD_VIDEO_ID @"ca-app-pub-3940256099942544/1712485313"

#define OPT_PUBLISHER_ID    @"publisherId"
#define OPT_INTERSTITIAL_ADID   @"interstitialAdId"
#define OPT_REWARD_VIDEO_ID   @"rewardVideoId"
#define OPT_AD_SIZE         @"adSize"
#define OPT_BANNER_AT_TOP   @"bannerAtTop"
#define OPT_OVERLAP         @"overlap"
#define OPT_OFFSET_TOPBAR   @"offsetTopBar"
#define OPT_IS_TESTING      @"isTesting"
#define OPT_AD_EXTRAS       @"adExtras"
#define OPT_AUTO_SHOW       @"autoShow"

#define OPT_GENDER          @"gender"
#define OPT_FORCHILD        @"forChild"

#pragma mark Cordova JS bridge

- (void)pluginInitialize {
    [super pluginInitialize];
    if (self) {
        // These notifications are required for re-placing the ad on orientation
        // changes. Start listening for notifications here since we need to
        // translate the Smart Banner constants according to the orientation.
        [[UIDevice currentDevice] beginGeneratingDeviceOrientationNotifications];
        [[NSNotificationCenter defaultCenter]
         addObserver:self
         selector:@selector(deviceOrientationChange:)
         name:UIDeviceOrientationDidChangeNotification
         object:nil];
    }

    bannerShow = true;
    publisherId = DEFAULT_BANNER_ID;
    interstitialAdId = DEFAULT_INTERSTITIAL_ID;
    rewardVideoId = DEFAULT_REWARD_VIDEO_ID;
    adSize = [self __AdSizeFromString:@"SMART_BANNER"];

    bannerAtTop = false;
    bannerOverlap = false;
    offsetTopBar = false;
    isTesting = false;

    autoShow = true;
    autoShowBanner = true;
    autoShowInterstitial = false;
    autoShowRewardVideo = false;

    bannerIsInitialized = false;
    bannerIsVisible = false;

    gender = nil;
    forChild = nil;

    isRewardedVideoLoading = false;
    rewardedVideoLock = nil;

    srand((unsigned int)time(NULL));

    [self initializeSafeAreaBackgroundView];
}

- (void) setOptions:(CDVInvokedUrlCommand *)command {
    NSLog(@"setOptions");

    CDVPluginResult *pluginResult;
    NSString *callbackId = command.callbackId;
    NSArray* args = command.arguments;

    NSUInteger argc = [args count];
    if( argc >= 1 ) {
        NSDictionary* options = [command argumentAtIndex:0 withDefault:[NSNull null]];

        [self __setOptions:options];
    }

    pluginResult = [CDVPluginResult resultWithStatus:CDVCommandStatus_OK];
    [self.commandDelegate sendPluginResult:pluginResult callbackId:callbackId];
}

// The javascript from the AdMob plugin calls this when createBannerView is

// invoked. This method parses the arguments passed in.

- (void)createBannerView:(CDVInvokedUrlCommand *)command {
    NSLog(@"createBannerView");

    CDVPluginResult *pluginResult;
    NSString *callbackId = command.callbackId;
    NSArray* args = command.arguments;

    NSUInteger argc = [args count];
    if( argc >= 1 ) {
        NSDictionary* options = [command argumentAtIndex:0 withDefault:[NSNull null]];
        [self __setOptions:options];
        autoShowBanner = autoShow;
    }

    if(! self.bannerView) {
        [self __createBanner];
    }

    if(autoShowBanner) {
        bannerShow = autoShowBanner;

        [self __showAd:YES];
    }

    pluginResult = [CDVPluginResult resultWithStatus:CDVCommandStatus_OK];
    [self.commandDelegate sendPluginResult:pluginResult callbackId:callbackId];
}

- (void)destroyBannerView:(CDVInvokedUrlCommand *)command {
    NSLog(@"destroyBannerView");

    CDVPluginResult *pluginResult;
    NSString *callbackId = command.callbackId;

    if(self.bannerView) {
        [self.bannerView setDelegate:nil];
        [self.bannerView removeFromSuperview];
        self.bannerView = nil;

        [self resizeViews];
    }

    // Call the success callback that was passed in through the javascript.
    pluginResult = [CDVPluginResult resultWithStatus:CDVCommandStatus_OK];
    [self.commandDelegate sendPluginResult:pluginResult callbackId:callbackId];
}

- (void)prepareInterstitial:(CDVInvokedUrlCommand *)command {
    NSLog(@"prepareInterstitial");

    CDVPluginResult *pluginResult;
    NSString *callbackId = command.callbackId;
    NSArray* args = command.arguments;

    NSUInteger argc = [args count];
    if (argc >= 1) {
        NSDictionary* options = [command argumentAtIndex:0 withDefault:[NSNull null]];
        [self __setOptions:options];
        autoShowInterstitial = autoShow;
    }

    [self __cycleInterstitial];
    [self.interstitialView loadRequest:[self __buildAdRequest]];

    pluginResult = [CDVPluginResult resultWithStatus:CDVCommandStatus_OK];
    [self.commandDelegate sendPluginResult:pluginResult callbackId:callbackId];

}

- (void)createInterstitialView:(CDVInvokedUrlCommand *)command {
    NSLog(@"createInterstitialView");

    CDVPluginResult *pluginResult;
    NSString *callbackId = command.callbackId;
    NSArray* args = command.arguments;

    NSUInteger argc = [args count];
    if (argc >= 1) {
        NSDictionary* options = [command argumentAtIndex:0 withDefault:[NSNull null]];
        [self __setOptions:options];
        autoShowInterstitial = autoShow;
    }

    [self __cycleInterstitial];
    pluginResult = [CDVPluginResult resultWithStatus:CDVCommandStatus_OK];

    [self.commandDelegate sendPluginResult:pluginResult callbackId:callbackId];
}

- (void)showAd:(CDVInvokedUrlCommand *)command {
    NSLog(@"showAd");

    CDVPluginResult *pluginResult;
    NSString *callbackId = command.callbackId;
    NSArray* arguments = command.arguments;

    BOOL show = YES;
    NSUInteger argc = [arguments count];
    if (argc >= 1) {
        NSString* showValue = [arguments objectAtIndex:0];
        show = showValue ? [showValue boolValue] : YES;
    }

    bannerShow = show;

    if(! self.bannerView) {
        pluginResult = [CDVPluginResult resultWithStatus:CDVCommandStatus_ERROR messageAsString:@"adView is null, call createBannerView first."];

    } else {
        [self __showAd:show];
        pluginResult = [CDVPluginResult resultWithStatus:CDVCommandStatus_OK];

    }

    [self.commandDelegate sendPluginResult:pluginResult callbackId:callbackId];
}

- (void)showInterstitialAd:(CDVInvokedUrlCommand *)command {
    NSLog(@"showInterstitial");

    CDVPluginResult *pluginResult;
    NSString *callbackId = command.callbackId;

    if(! self.interstitialView) {
        pluginResult = [CDVPluginResult resultWithStatus:CDVCommandStatus_ERROR messageAsString:@"interstitialAd is null, call createInterstitialView first."];

    } else {
        BOOL showed = [self __showInterstitial:YES];
        if (showed) {
            pluginResult = [CDVPluginResult resultWithStatus:CDVCommandStatus_OK];
        } else {
            pluginResult = [CDVPluginResult resultWithStatus:CDVCommandStatus_ERROR messageAsString:@"interstitial not ready yet."];
        }
    }

    [self.commandDelegate sendPluginResult:pluginResult callbackId:callbackId];
}

- (void)isInterstitialReady:(CDVInvokedUrlCommand *)command {
    NSLog(@"isInterstitialReady");

    CDVPluginResult *pluginResult;
    NSString *callbackId = command.callbackId;

    if (self.interstitialView && self.interstitialView.isReady) {
        pluginResult = [CDVPluginResult resultWithStatus:CDVCommandStatus_OK messageAsBool:true];
    } else {
        pluginResult = [CDVPluginResult resultWithStatus:CDVCommandStatus_OK messageAsBool:false];
    }

    [self.commandDelegate sendPluginResult:pluginResult callbackId:callbackId];
}

- (void)requestAd:(CDVInvokedUrlCommand *)command {
    NSLog(@"requestAd");

    CDVPluginResult *pluginResult;
    NSString *callbackId = command.callbackId;
    NSArray* args = command.arguments;

    NSUInteger argc = [args count];
    if (argc >= 1) {
        NSDictionary* options = [command argumentAtIndex:0 withDefault:[NSNull null]];
        [self __setOptions:options];
    }

    if(! self.bannerView) {
        [self __createBanner];

    } else {
        [self.bannerView loadRequest:[self __buildAdRequest]];
    }

    pluginResult = [CDVPluginResult resultWithStatus:CDVCommandStatus_OK];
    [self.commandDelegate sendPluginResult:pluginResult callbackId:callbackId];
}

- (void)requestInterstitialAd:(CDVInvokedUrlCommand *)command {
    NSLog(@"requestInterstitialAd");

    CDVPluginResult *pluginResult;
    NSString *callbackId = command.callbackId;
    NSArray* args = command.arguments;

    NSUInteger argc = [args count];
    if (argc >= 1) {
        NSDictionary* options = [command argumentAtIndex:0 withDefault:[NSNull null]];
        [self __setOptions:options];
    }

    if(! self.interstitialView) {
        [self __cycleInterstitial];

    } else {
        [self.interstitialView loadRequest:[self __buildAdRequest]];
    }

    pluginResult = [CDVPluginResult resultWithStatus:CDVCommandStatus_OK];
    [self.commandDelegate sendPluginResult:pluginResult callbackId:callbackId];
}

- (void)createRewardVideo:(CDVInvokedUrlCommand *)command {
    NSLog(@"createRewardVideo");

    CDVPluginResult *pluginResult;
    NSString *callbackId = command.callbackId;
    NSArray* args = command.arguments;

    NSUInteger argc = [args count];
    if (argc >= 1) {
        NSDictionary* options = [command argumentAtIndex:0 withDefault:[NSNull null]];
        [self __setOptions:options];
        autoShowRewardVideo = autoShow;
    }

    [self __cycleRewardVideo];
    pluginResult = [CDVPluginResult resultWithStatus:CDVCommandStatus_OK];

    [self.commandDelegate sendPluginResult:pluginResult callbackId:callbackId];

}


- (void)showRewardVideo:(CDVInvokedUrlCommand *)command {
    NSLog(@"showRewardVideo");

    CDVPluginResult *pluginResult;
    NSString *callbackId = command.callbackId;

    if(! self.rewardVideoView) {
        pluginResult = [CDVPluginResult resultWithStatus:CDVCommandStatus_ERROR messageAsString:@"rewardVideoAd is null, call createRewardVideo first."];

    } else {
        [self __showRewardedVideo:YES];
        pluginResult = [CDVPluginResult resultWithStatus:CDVCommandStatus_OK];

    }

    [self.commandDelegate sendPluginResult:pluginResult callbackId:callbackId];

}

- (void)isRewardVideoReady:(CDVInvokedUrlCommand *)command {
    NSLog(@"isRewardVideoReady");

    CDVPluginResult *pluginResult;
    NSString *callbackId = command.callbackId;

    if (self.rewardVideoView && self.rewardVideoView.isReady) {
        pluginResult = [CDVPluginResult resultWithStatus:CDVCommandStatus_OK messageAsBool:true];
    } else {
        pluginResult = [CDVPluginResult resultWithStatus:CDVCommandStatus_OK messageAsBool:false];
    }

    [self.commandDelegate sendPluginResult:pluginResult callbackId:callbackId];
}

- (void) __cycleRewardVideo {
    NSLog(@"__cycleRewardVideo");

    @synchronized(self.rewardedVideoLock) {
        if (!self.isRewardedVideoLoading) {
            self.isRewardedVideoLoading = true;

            // Clean up the old video...
            if (self.rewardVideoView) {
                self.rewardVideoView.delegate = nil;
                self.rewardVideoView = nil;
            }

            // and create a new video. We set the delegate so that we can be notified of when
            if (!self.rewardVideoView){
                self.rewardVideoView = [GADRewardBasedVideoAd sharedInstance];
                self.rewardVideoView.delegate = self;

                [self.rewardVideoView loadRequest:[GADRequest request] withAdUnitID:self.rewardVideoId];
            }
        }
    }
}


- (GADAdSize)__AdSizeFromString:(NSString *)string {
    if ([string isEqualToString:@"BANNER"]) {
        return kGADAdSizeBanner;
    } else if ([string isEqualToString:@"IAB_MRECT"]) {
        return kGADAdSizeMediumRectangle;
    } else if ([string isEqualToString:@"IAB_BANNER"]) {
        return kGADAdSizeFullBanner;
    } else if ([string isEqualToString:@"IAB_LEADERBOARD"]) {
        return kGADAdSizeLeaderboard;
    } else if ([string isEqualToString:@"LARGE_BANNER"]) {
        return kGADAdSizeLargeBanner;
    } else if ([string isEqualToString:@"FLUID"]) {
        return kGADAdSizeFluid;
    } else if ([string isEqualToString:@"SMART_BANNER"]) {
        CGRect pr = self.webView.superview.bounds;
        if(pr.size.width > pr.size.height) {
            return kGADAdSizeSmartBannerLandscape;
        }
        else {
            return kGADAdSizeSmartBannerPortrait;
        }
    } else {
        return kGADAdSizeInvalid;
    }
}

- (NSString *) __getAdMobDeviceId {
    NSUUID* adid = [[ASIdentifierManager sharedManager] advertisingIdentifier];
    return [self __md5:adid.UUIDString];
}

- (NSString*) __md5:(NSString *) s {
    const char *cstr = [s UTF8String];
    unsigned char result[16];
    CC_MD5(cstr, (CC_LONG)strlen(cstr), result);

    return [NSString stringWithFormat:
            @"%02X%02X%02X%02X%02X%02X%02X%02X%02X%02X%02X%02X%02X%02X%02X%02X",
            result[0], result[1], result[2], result[3],
            result[4], result[5], result[6], result[7],
            result[8], result[9], result[10], result[11],
            result[12], result[13], result[14], result[15]
            ];
}

#pragma mark Ad Banner logic

- (void) __setOptions:(NSDictionary*) options {
    if ((NSNull *)options == [NSNull null]) return;

    NSString* str = nil;

    str = [options objectForKey:OPT_PUBLISHER_ID];
    if (str && [str length] > 0) {
        publisherId = str;
    }

    str = [options objectForKey:OPT_INTERSTITIAL_ADID];
    if (str && [str length] > 0) {
        interstitialAdId = str;
    }

    str = [options objectForKey:OPT_REWARD_VIDEO_ID];
    if (str && [str length] > 0) {
        rewardVideoId = str;
    }

    str = [options objectForKey:OPT_AD_SIZE];
    if (str) {
        adSize = [self __AdSizeFromString:str];
    }

    str = [options objectForKey:OPT_BANNER_AT_TOP];
    if (str) {
        bannerAtTop = [str boolValue];
    }

    str = [options objectForKey:OPT_OVERLAP];
    if (str) {
        bannerOverlap = [str boolValue];
    }

    str = [options objectForKey:OPT_OFFSET_TOPBAR];
    if (str) {
        offsetTopBar = [str boolValue];
    }

    str = [options objectForKey:OPT_IS_TESTING];
    if (str) {
        isTesting = [str boolValue];
    }

    NSDictionary* dict = [options objectForKey:OPT_AD_EXTRAS];
    if (dict) {
        adExtras = dict;
    }

    str = [options objectForKey:OPT_AUTO_SHOW];
    if (str) {
        autoShow = [str boolValue];
    }

    str = [options objectForKey:OPT_GENDER];
    if (str && [str length] > 0) {
        gender = str;
    }
    str = [options objectForKey:OPT_FORCHILD];
    if (str && [str length] > 0) {
        forChild = str;
    } else if (str && [str length] == 0) {
        forChild = nil;
    }
}

- (void) __createBanner {
    NSLog(@"__createBanner");

    // set background color to black
    //self.webView.superview.backgroundColor = [UIColor blackColor];
    //self.webView.superview.tintColor = [UIColor whiteColor];

    if (!self.bannerView){
        self.bannerView = [[GADBannerView alloc] initWithAdSize:adSize];
        self.bannerView.adUnitID = [self publisherId];
        self.bannerView.delegate = self;
        self.bannerView.rootViewController = self.viewController;

        self.bannerIsInitialized = YES;
        self.bannerIsVisible = NO;

        [self resizeViews];

        [self.bannerView loadRequest:[self __buildAdRequest]];
    }
}

- (GADRequest*) __buildAdRequest {
    GADRequest *request = [GADRequest request];

    if (self.isTesting) {
        NSString* deviceId = [self __getAdMobDeviceId];
        request.testDevices = @[ kGADSimulatorID, deviceId, [deviceId lowercaseString] ];
        NSLog(@"request.testDevices: %@", deviceId);
    }
    if (self.adExtras) {
        GADExtras *extras = [[GADExtras alloc] init];
        NSMutableDictionary *modifiedExtrasDict =
        [[NSMutableDictionary alloc] initWithDictionary:self.adExtras];
        [modifiedExtrasDict removeObjectForKey:@"cordova"];
        [modifiedExtrasDict setValue:@"1" forKey:@"cordova"];
        extras.additionalParameters = modifiedExtrasDict;
        [request registerAdNetworkExtras:extras];
    }

    if (self.gender != nil) {
        if ([self.gender caseInsensitiveCompare:@"male"] == NSOrderedSame) {
            request.gender = kGADGenderMale;
        } else if ([self.gender caseInsensitiveCompare:@"female"] == NSOrderedSame) {
            request.gender = kGADGenderFemale;
        } else {
            request.gender = kGADGenderUnknown;
        }
    }

    if (self.forChild != nil) {
        if ([self.forChild caseInsensitiveCompare:@"yes"] == NSOrderedSame) {
            [request tagForChildDirectedTreatment:YES];
        } else if ([self.forChild caseInsensitiveCompare:@"no"] == NSOrderedSame) {
            [request tagForChildDirectedTreatment:NO];
        }
    }

    return request;
}

- (void) __showAd:(BOOL)show {
    //NSLog(@"Show Ad: %d", show);

    if (!self.bannerIsInitialized){
        [self __createBanner];
    }

    if (show == self.bannerIsVisible) { // same state, nothing to do
        //NSLog(@"already show: %d", show);
        [self resizeViews];
    } else if (show) {
        //NSLog(@"show now: %d", show);

        UIView* parentView = self.bannerOverlap ? self.webView : [self.webView superview];
        [parentView addSubview:self.bannerView];
        [parentView bringSubviewToFront:self.bannerView];
        [self resizeViews];

        self.bannerIsVisible = YES;
    } else {
        [self.bannerView removeFromSuperview];
        [self resizeViews];

        self.bannerIsVisible = NO;
    }
}

- (void) __cycleInterstitial {
    NSLog(@"__cycleInterstitial");

    // Clean up the old interstitial...
    if (self.interstitialView) {
        self.interstitialView.delegate = nil;
        self.interstitialView = nil;
    }

    // and create a new interstitial. We set the delegate so that we can be notified of when
    if (!self.interstitialView){
        self.interstitialView = [[GADInterstitial alloc] initWithAdUnitID:self.interstitialAdId];
        self.interstitialView.delegate = self;

        [self.interstitialView loadRequest:[self __buildAdRequest]];
    }
}

- (BOOL) __showInterstitial:(BOOL)show {
    NSLog(@"__showInterstitial");

    if (!self.interstitialView){
        [self __cycleInterstitial];
    }

    if (self.interstitialView && self.interstitialView.isReady) {
        [self.interstitialView presentFromRootViewController:self.viewController];
        return true;
    } else {
        NSLog(@"Ad wasn't ready");
        return false;
    }
}

- (void) __showRewardedVideo:(BOOL)show {
    NSLog(@"__showRewardedVideo");

    if (!self.rewardVideoView){
        [self __cycleRewardVideo];
    }

    if (self.rewardVideoView && self.rewardVideoView.isReady) {
        [self.rewardVideoView presentFromRootViewController:self.viewController];
    } else {
        NSLog(@"RewardVideo wasn't ready");
    }
}

- (void) initializeSafeAreaBackgroundView
{
    if (@available(iOS 11.0, *)) {

        UIView* parentView = self.bannerOverlap ? self.webView : [self.webView superview];
        CGRect pr = self.webView.superview.bounds;

        CGRect safeAreaFrame = CGRectMake(0, 0, 0, 0);

        safeAreaFrame.origin.y = pr.size.height - parentView.safeAreaInsets.bottom;
        safeAreaFrame.size.width = pr.size.width;
        safeAreaFrame.size.height = parentView.safeAreaInsets.bottom;

        _safeAreaBackgroundView = [[UIView alloc] initWithFrame:safeAreaFrame];
        _safeAreaBackgroundView.backgroundColor = [UIColor blackColor];
        _safeAreaBackgroundView.autoresizingMask = (UIViewAutoresizingFlexibleWidth  | UIViewAutoresizingFlexibleBottomMargin);
        _safeAreaBackgroundView.autoresizesSubviews = YES;
        _safeAreaBackgroundView.hidden = true;

        [self.webView.superview addSubview:_safeAreaBackgroundView];
    }
}

- (void)resizeViews {
    // Frame of the main container view that holds the Cordova webview.
    CGRect pr = self.webView.superview.bounds, wf = pr;
    //NSLog(@"super view: %d x %d", (int)pr.size.width, (int)pr.size.height);

    // iOS7 Hack, handle the Statusbar
    //BOOL isIOS7 = ([[UIDevice currentDevice].systemVersion floatValue] >= 7);
    //CGRect sf = [[UIApplication sharedApplication] statusBarFrame];
    //CGFloat top = isIOS7 ? MIN(sf.size.height, sf.size.width) : 0.0;
    float top = 0.0;

    //if(! self.offsetTopBar) top = 0.0;

    wf.origin.y = top;
    wf.size.height = pr.size.height - top;

    if( self.bannerView ) {
        if( pr.size.width > pr.size.height ) {
            if(GADAdSizeEqualToSize(self.bannerView.adSize, kGADAdSizeSmartBannerPortrait)) {
                self.bannerView.adSize = kGADAdSizeSmartBannerLandscape;
            }
        } else {
            if(GADAdSizeEqualToSize(self.bannerView.adSize, kGADAdSizeSmartBannerLandscape)) {
                self.bannerView.adSize = kGADAdSizeSmartBannerPortrait;
            }
        }

        CGRect bf = self.bannerView.frame;

        // If the ad is not showing or the ad is hidden, we don't want to resize anything.
        UIView* parentView = self.bannerOverlap ? self.webView : [self.webView superview];
        BOOL adIsShowing = ([self.bannerView isDescendantOfView:parentView]) && (! self.bannerView.hidden);

        if( adIsShowing ) {
            //NSLog( @"banner visible" );
            if( bannerAtTop ) {
                if(bannerOverlap) {
                    wf.origin.y = top;
                    bf.origin.y = 0; // banner is subview of webview

                    if (@available(iOS 11.0, *)) {
                        bf.origin.y = parentView.safeAreaInsets.top;
                        bf.size.width = wf.size.width - parentView.safeAreaInsets.left - parentView.safeAreaInsets.right;
                    }
                } else {
                    bf.origin.y = top;
                    wf.origin.y = bf.origin.y + bf.size.height;

                    if (@available(iOS 11.0, *)) {
                        bf.origin.y += parentView.safeAreaInsets.top;
                        wf.origin.y += parentView.safeAreaInsets.top;
                        bf.size.width = wf.size.width - parentView.safeAreaInsets.left - parentView.safeAreaInsets.right;
                        wf.size.height -= parentView.safeAreaInsets.top;

                        //If safeAreBackground was turned turned off, turn it back on
                        _safeAreaBackgroundView.hidden = false;

                        CGRect saf = _safeAreaBackgroundView.frame;
                        saf.origin.y = top;
                        saf.size.width = pr.size.width;
                        saf.size.height = parentView.safeAreaInsets.top;

                        _safeAreaBackgroundView.frame = saf;
                        _safeAreaBackgroundView.bounds = saf;
                    }
                }

            } else {
                // move webview to top
                wf.origin.y = top;

                if( bannerOverlap ) {
                    bf.origin.y = wf.size.height - bf.size.height; // banner is subview of webview

                    if (@available(iOS 11.0, *)) {
                        bf.origin.y -= parentView.safeAreaInsets.bottom;
                        bf.size.width = wf.size.width - parentView.safeAreaInsets.left - parentView.safeAreaInsets.right;
                    }
                } else {
                    bf.origin.y = pr.size.height - bf.size.height;

                    if (@available(iOS 11.0, *)) {
                        bf.origin.y -= parentView.safeAreaInsets.bottom;
                        bf.size.width = wf.size.width - parentView.safeAreaInsets.left - parentView.safeAreaInsets.right;
                        wf.size.height -= parentView.safeAreaInsets.bottom;

                        //If safeAreBackground was turned turned off, turn it back on
                        _safeAreaBackgroundView.hidden = false;

                        CGRect saf = _safeAreaBackgroundView.frame;
                        saf.origin.y = pr.size.height - parentView.safeAreaInsets.bottom;
                        saf.size.width = pr.size.width;
                        saf.size.height = parentView.safeAreaInsets.bottom;

                        _safeAreaBackgroundView.frame = saf;
                        _safeAreaBackgroundView.bounds = saf;
                    }
                }
            }

            if(! bannerOverlap) wf.size.height -= bf.size.height;

            bf.origin.x = (pr.size.width - bf.size.width) * 0.5f;

            self.bannerView.frame = bf;

            //NSLog(@"x,y,w,h = %d,%d,%d,%d", (int) bf.origin.x, (int) bf.origin.y, (int) bf.size.width, (int) bf.size.height );
        } else {
            //Hide safe area background if visibile and banner ad does not exist
            _safeAreaBackgroundView.hidden = true;
        }
    } else {
        //Hide safe area background if visibile and banner ad does not exist
        _safeAreaBackgroundView.hidden = true;
    }

    self.webView.frame = wf;

    //NSLog(@"superview: %d x %d, webview: %d x %d", (int) pr.size.width, (int) pr.size.height, (int) wf.size.width, (int) wf.size.height );
}

- (void)deviceOrientationChange:(NSNotification *)notification {
    [self resizeViews];
}

- (void) fireEvent:(NSString *)obj event:(NSString *)eventName withData:(NSString *)jsonStr {
    NSString* js;
    if(obj && [obj isEqualToString:@"window"]) {
        js = [NSString stringWithFormat:@"var evt=document.createEvent(\"UIEvents\");evt.initUIEvent(\"%@\",true,false,window,0);window.dispatchEvent(evt);", eventName];
    } else if(jsonStr && [jsonStr length]>0) {
        js = [NSString stringWithFormat:@"javascript:cordova.fireDocumentEvent('%@',%@);", eventName, jsonStr];
    } else {
        js = [NSString stringWithFormat:@"javascript:cordova.fireDocumentEvent('%@');", eventName];
    }
    [self.commandDelegate evalJs:js];
}

#pragma mark GADBannerViewDelegate implementation

- (void)adViewDidReceiveAd:(GADBannerView *)adView {
    if(self.bannerShow) {
        [self __showAd:YES];
    }
    NSString* jsonData = [NSString stringWithFormat:@"{ 'bannerHeight': '%d' }", (int)self.bannerView.frame.size.height];
    [self fireEvent:@"" event:@"admob.banner.events.LOAD" withData:jsonData];
    [self fireEvent:@"" event:@"onReceiveAd" withData:nil];
}

- (void)adView:(GADBannerView *)view didFailToReceiveAdWithError:(GADRequestError *)error {
    NSString* jsonData = [NSString stringWithFormat:@"{ 'error': '%@', 'adType':'banner' }", [error localizedFailureReason]];
    [self fireEvent:@"" event:@"admob.banner.events.LOAD_FAIL" withData:jsonData];
    [self fireEvent:@"" event:@"onFailedToReceiveAd" withData:jsonData];
}

- (void)adViewWillLeaveApplication:(GADBannerView *)adView {
    NSString* jsonData = @"{ 'adType':'banner' }";
    [self fireEvent:@"" event:@"admob.banner.events.EXIT_APP" withData:jsonData];
    [self fireEvent:@"" event:@"onLeaveToAd" withData:jsonData];
}

- (void)adViewWillPresentScreen:(GADBannerView *)adView {
    [self fireEvent:@"" event:@"admob.banner.events.OPEN" withData:nil];
    [self fireEvent:@"" event:@"onPresentAd" withData:nil];
}

- (void)adViewDidDismissScreen:(GADBannerView *)adView {
    [self fireEvent:@"" event:@"admob.banner.events.CLOSE" withData:nil];
    [self fireEvent:@"" event:@"onDismissAd" withData:nil];
}

#pragma mark GADInterstitialDelegate implementation

- (void)interstitial:(GADInterstitial *)ad
    didFailToReceiveAdWithError:(GADRequestError *)error {
    NSString* jsonData = [NSString stringWithFormat:@"{ 'error': '%@', 'adType':'interstitial' }", [error localizedFailureReason]];
    [self fireEvent:@"" event:@"admob.interstitial.events.LOAD_FAIL" withData:jsonData];
    [self fireEvent:@"" event:@"onFailedToReceiveAd" withData:jsonData];
}

- (void)interstitialWillLeaveApplication:(GADInterstitial *)interstitial {
    NSString* jsonData = @"{ 'adType':'interstitial' }";
    [self fireEvent:@"" event:@"admob.interstitial.events.EXIT_APP" withData:jsonData];
    [self fireEvent:@"" event:@"onLeaveToAd" withData:jsonData];
}

- (void)interstitialDidReceiveAd:(GADInterstitial *)interstitial {
    [self fireEvent:@"" event:@"admob.interstitial.events.LOAD" withData:nil];
    [self fireEvent:@"" event:@"onReceiveInterstitialAd" withData:nil];
    if (self.interstitialView){
        if(self.autoShowInterstitial) {
            [self __showInterstitial:YES];
        }
    }
}

- (void)interstitialWillPresentScreen:(GADInterstitial *)interstitial {
    [self fireEvent:@"" event:@"admob.interstitial.events.OPEN" withData:nil];
    [self fireEvent:@"" event:@"onPresentInterstitialAd" withData:nil];
}


- (void)interstitialDidDismissScreen:(GADInterstitial *)interstitial {
    [self fireEvent:@"" event:@"admob.interstitial.events.CLOSE" withData:nil];
    [self fireEvent:@"" event:@"onDismissInterstitialAd" withData:nil];
    if (self.interstitialView) {
        self.interstitialView.delegate = nil;
        self.interstitialView = nil;
        [self resizeViews];
    }
}


#pragma mark GADRewardBasedVideoAdDelegate implementation

- (void)rewardBasedVideoAd:(GADRewardBasedVideoAd *)rewardBasedVideoAd
   didRewardUserWithReward:(GADAdReward *)reward {
    NSString* obj = @"AdMob";
    NSString* jsonData = [NSString stringWithFormat:@"{'adNetwork':'%@','adType':'rewardvideo','adEvent':'onRewardedVideo','rewardType':'%@','rewardAmount':%lf}",
                        obj, reward.type, [reward.amount doubleValue]];
    [self fireEvent:@"" event:@"admob.rewardvideo.events.REWARD" withData:jsonData];
}

- (void)rewardBasedVideoAdDidReceiveAd:(GADRewardBasedVideoAd *)rewardBasedVideoAd {
    @synchronized(self.rewardedVideoLock) {
        self.isRewardedVideoLoading = false;
    }
    [self fireEvent:@"" event:@"admob.rewardvideo.events.LOAD" withData:nil];
    if (self.rewardVideoView){
        if(self.autoShowRewardVideo) {
            [self __showRewardedVideo:YES];
        }
    }
}

- (void)rewardBasedVideoAdDidOpen:(GADRewardBasedVideoAd *)rewardBasedVideoAd {
    [self fireEvent:@"" event:@"admob.rewardvideo.events.OPEN" withData:nil];
}

- (void)rewardBasedVideoAdDidStartPlaying:(GADRewardBasedVideoAd *)rewardBasedVideoAd {
    [self fireEvent:@"" event:@"admob.rewardvideo.events.START" withData:nil];
}

- (void)rewardBasedVideoAdDidClose:(GADRewardBasedVideoAd *)rewardBasedVideoAd {
    [self fireEvent:@"" event:@"admob.rewardvideo.events.CLOSE" withData:nil];
    if (self.rewardVideoView) {
        self.rewardVideoView.delegate = nil;
        self.rewardVideoView = nil;
    }
}

- (void)rewardBasedVideoAdWillLeaveApplication:(GADRewardBasedVideoAd *)rewardBasedVideoAd {
    NSString* jsonData = @"{ 'adType':'rewardvideo' }";
    [self fireEvent:@"" event:@"admob.rewardvideo.events.EXIT_APP" withData:jsonData];
}

- (void)rewardBasedVideoAd:(GADRewardBasedVideoAd *)rewardBasedVideoAd
    didFailToLoadWithError:(NSError *)error {
    @synchronized(self.rewardedVideoLock) {
        self.isRewardedVideoLoading = false;
    }
    NSString* jsonData = [NSString stringWithFormat:@"{ 'error': '%@', 'adType':'rewardvideo' }", [error localizedFailureReason]];
    [self fireEvent:@"" event:@"admob.rewardvideo.events.LOAD_FAIL" withData:jsonData];
}



#pragma mark Cleanup

- (void)dealloc {
    [[UIDevice currentDevice] endGeneratingDeviceOrientationNotifications];
    [[NSNotificationCenter defaultCenter]
     removeObserver:self
     name:UIDeviceOrientationDidChangeNotification
     object:nil];

    bannerView_.delegate = nil;
    bannerView_ = nil;
    interstitialView_.delegate = nil;
    interstitialView_ = nil;
    rewardVideoView_.delegate = nil;
    rewardVideoView_ =  nil;

    self.bannerView = nil;
    self.interstitialView = nil;
    self.rewardVideoView = nil;
}

@end
