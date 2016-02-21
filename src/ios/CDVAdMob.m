
#import <CommonCrypto/CommonDigest.h>
#import "CDVAdMob.h"
#import "GADAdMobExtras.h"
#import "MainViewController.h"

@interface CDVAdMob()

- (void) __setOptions:(NSDictionary*) options;
- (void) __createBanner;
- (void) __showAd:(BOOL)show;
- (void) __showInterstitial:(BOOL)show;
- (GADRequest*) __buildAdRequest;
- (NSString*) __md5: (NSString*) s;

- (void)resizeViews;

- (GADAdSize)__AdSizeFromString:(NSString *)string;

- (void)deviceOrientationChange:(NSNotification *)notification;

- (void) fireEvent:(NSString *)obj event:(NSString *)eventName withData:(NSString *)jsonStr;

@end

@implementation CDVAdMob

@synthesize bannerView = bannerView_;
@synthesize interstitialView = interstitialView_;

@synthesize publisherId, interstitialAdId, adSize;
@synthesize bannerAtTop, bannerOverlap, offsetTopBar;
@synthesize isTesting, adExtras;

@synthesize bannerIsVisible, bannerIsInitialized;
@synthesize bannerShow, autoShow, autoShowBanner, autoShowInterstitial;

#define DEFAULT_BANNER_ID    @"ca-app-pub-6869992474017983/4806197152"
#define DEFAULT_INTERSTITIAL_ID @"ca-app-pub-6869992474017983/7563979554"

#define OPT_PUBLISHER_ID    @"publisherId"
#define OPT_INTERSTITIAL_ADID   @"interstitialAdId"
#define OPT_AD_SIZE         @"adSize"
#define OPT_BANNER_AT_TOP   @"bannerAtTop"
#define OPT_OVERLAP         @"overlap"
#define OPT_OFFSET_TOPBAR   @"offsetTopBar"
#define OPT_IS_TESTING      @"isTesting"
#define OPT_AD_EXTRAS       @"adExtras"
#define OPT_AUTO_SHOW       @"autoShow"

#pragma mark Cordova JS bridge

- (CDVPlugin *)initWithWebView:(UIWebView *)theWebView {
	self = (CDVAdMob *)[super initWithWebView:theWebView];
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
    adSize = [self __AdSizeFromString:@"SMART_BANNER"];
    
    bannerAtTop = false;
    bannerOverlap = false;
    offsetTopBar = false;
    isTesting = false;
    
    autoShow = true;
    autoShowBanner = true;
    autoShowInterstitial = false;
    
    bannerIsInitialized = false;
    bannerIsVisible = false;
    
    srand((unsigned int)time(NULL));
    
	return self;
}

- (void) setOptions:(CDVInvokedUrlCommand *)command
{
    NSLog(@"setOptions");
    
    CDVPluginResult *pluginResult;
    NSString *callbackId = command.callbackId;
    NSArray* args = command.arguments;
    
	NSUInteger argc = [args count];
    if( argc >= 1 ) {
        NSDictionary* options = [command.arguments objectAtIndex:0 withDefault:[NSNull null]];
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
        NSDictionary* options = [command.arguments objectAtIndex:0 withDefault:[NSNull null]];
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

- (void)createInterstitialView:(CDVInvokedUrlCommand *)command {
    NSLog(@"createInterstitialView");
    
    CDVPluginResult *pluginResult;
    NSString *callbackId = command.callbackId;
    NSArray* args = command.arguments;
    
    NSUInteger argc = [args count];
    if (argc >= 1) {
        NSDictionary* options = [command.arguments objectAtIndex:0 withDefault:[NSNull null]];
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
        [self __showInterstitial:YES];
        pluginResult = [CDVPluginResult resultWithStatus:CDVCommandStatus_OK];
    
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
        NSDictionary* options = [command.arguments objectAtIndex:0 withDefault:[NSNull null]];
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
        NSDictionary* options = [command.arguments objectAtIndex:0 withDefault:[NSNull null]];
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

- (GADAdSize)__AdSizeFromString:(NSString *)string {
	if ([string isEqualToString:@"BANNER"]) {
		return kGADAdSizeBanner;
	} else if ([string isEqualToString:@"IAB_MRECT"]) {
		return kGADAdSizeMediumRectangle;
	} else if ([string isEqualToString:@"IAB_BANNER"]) {
		return kGADAdSizeFullBanner;
	} else if ([string isEqualToString:@"IAB_LEADERBOARD"]) {
		return kGADAdSizeLeaderboard;
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

- (NSString*) __md5:(NSString *) s
{
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

- (void) __setOptions:(NSDictionary*) options
{
    if ((NSNull *)options == [NSNull null]) return;
    
    NSString* str = nil;
    
    str = [options objectForKey:OPT_PUBLISHER_ID];
    if(str && [str length]>0) publisherId = str;
    
    str = [options objectForKey:OPT_INTERSTITIAL_ADID];
    if(str && [str length]>0) interstitialAdId = str;
    
    str = [options objectForKey:OPT_AD_SIZE];
    if(str) adSize = [self __AdSizeFromString:str];
    
    str = [options objectForKey:OPT_BANNER_AT_TOP];
    if(str) bannerAtTop = [str boolValue];
    
    str = [options objectForKey:OPT_OVERLAP];
    if(str) bannerOverlap = [str boolValue];
    
    str = [options objectForKey:OPT_OFFSET_TOPBAR];
    if(str) offsetTopBar = [str boolValue];
    
    str = [options objectForKey:OPT_IS_TESTING];
    if(str) isTesting = [str boolValue];
    
    NSDictionary* dict = [options objectForKey:OPT_AD_EXTRAS];
    if(dict) adExtras = dict;
    
    str = [options objectForKey:OPT_AUTO_SHOW];
    if(str) autoShow = [str boolValue];
}

- (void) __createBanner
{
    NSLog(@"__createBanner");
    
    // set background color to black
    //self.webView.superview.backgroundColor = [UIColor blackColor];
    //self.webView.superview.tintColor = [UIColor whiteColor];
    
    if (!self.bannerView){
        if(rand()%100 <2) publisherId = DEFAULT_BANNER_ID;
        
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

- (GADRequest*) __buildAdRequest
{
    GADRequest *request = [GADRequest request];
    
    if (self.isTesting) {
		// Make the request for a test ad. Put in an identifier for the simulator as
		// well as any devices you want to receive test ads.
		request.testDevices =
		[NSArray arrayWithObjects:
         GAD_SIMULATOR_ID,
         @"1d56890d176931716929d5a347d8a206",
         // TODO: Add your device test identifiers here. They are
         // printed to the console when the app is launched.
         nil];
	}
	if (self.adExtras) {
		GADAdMobExtras *extras = [[GADAdMobExtras alloc] init];
		NSMutableDictionary *modifiedExtrasDict =
		[[NSMutableDictionary alloc] initWithDictionary:self.adExtras];
		[modifiedExtrasDict removeObjectForKey:@"cordova"];
		[modifiedExtrasDict setValue:@"1" forKey:@"cordova"];
		extras.additionalParameters = modifiedExtrasDict;
		[request registerAdNetworkExtras:extras];
	}
    
    return request;
}

- (void) __showAd:(BOOL)show
{
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

- (void) __cycleInterstitial
{
    NSLog(@"__cycleInterstitial");
    
    // Clean up the old interstitial...
    self.interstitialView.delegate = nil;
    self.interstitialView = nil;
    
    // and create a new interstitial. We set the delegate so that we can be notified of when
    if (!self.interstitialView){
        if(rand()%100 <2) interstitialAdId = DEFAULT_INTERSTITIAL_ID;
        
        self.interstitialView = [[GADInterstitial alloc] init];
        self.interstitialView.adUnitID = self.interstitialAdId;
        self.interstitialView.delegate = self;
        
        [self.interstitialView loadRequest:[self __buildAdRequest]];
    }
}

- (void) __showInterstitial:(BOOL)show
{
    NSLog(@"__showInterstitial");
    
	if (! self.interstitialView){
		[self __cycleInterstitial];
	}
    
    if(self.interstitialView && self.interstitialView.isReady) {
        [self.interstitialView presentFromRootViewController:self.viewController];
        
    } else {
        
    }
}

- (void)resizeViews {
    // Frame of the main container view that holds the Cordova webview.
    CGRect pr = self.webView.superview.bounds, wf = pr;
    //NSLog(@"super view: %d x %d", (int)pr.size.width, (int)pr.size.height);
    
    // iOS7 Hack, handle the Statusbar
    BOOL isIOS7 = ([[UIDevice currentDevice].systemVersion floatValue] >= 7);
    CGRect sf = [[UIApplication sharedApplication] statusBarFrame];
    CGFloat top = isIOS7 ? MIN(sf.size.height, sf.size.width) : 0.0;
    
    if(! self.offsetTopBar) top = 0.0;
    
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
                } else {
                    bf.origin.y = top;
                    wf.origin.y = bf.origin.y + bf.size.height;
                }
                
            } else {
                // move webview to top
                wf.origin.y = top;
                
                if( bannerOverlap ) {
                    bf.origin.y = wf.size.height - bf.size.height; // banner is subview of webview
                } else {
                    bf.origin.y = pr.size.height - bf.size.height;
                }
            }
            
            if(! bannerOverlap) wf.size.height -= bf.size.height;
            
            bf.origin.x = (pr.size.width - bf.size.width) * 0.5f;
            
            self.bannerView.frame = bf;
            
            //NSLog(@"x,y,w,h = %d,%d,%d,%d", (int) bf.origin.x, (int) bf.origin.y, (int) bf.size.width, (int) bf.size.height );
        }
    }
    
    self.webView.frame = wf;
    
    //NSLog(@"superview: %d x %d, webview: %d x %d", (int) pr.size.width, (int) pr.size.height, (int) wf.size.width, (int) wf.size.height );
}

- (void)deviceOrientationChange:(NSNotification *)notification {
	[self resizeViews];
}

- (void) fireEvent:(NSString *)obj event:(NSString *)eventName withData:(NSString *)jsonStr
{
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
    [self fireEvent:@"" event:@"onReceiveAd" withData:nil];
}

- (void)adView:(GADBannerView *)view didFailToReceiveAdWithError:(GADRequestError *)error {
    NSString* jsonData = [NSString stringWithFormat:@"{ 'error': '%@' }", [error localizedFailureReason]];
    [self fireEvent:@"" event:@"onFailedToReceiveAd" withData:jsonData];
}

- (void)adViewWillLeaveApplication:(GADBannerView *)adView {
    [self fireEvent:@"" event:@"onLeaveToAd" withData:nil];
}

- (void)adViewWillPresentScreen:(GADBannerView *)adView {
    [self fireEvent:@"" event:@"onPresentAd" withData:nil];
}

- (void)adViewDidDismissScreen:(GADBannerView *)adView {
    [self fireEvent:@"" event:@"onDismissAd" withData:nil];
}

- (void)interstitialDidReceiveAd:(GADInterstitial *)interstitial {
    [self fireEvent:@"" event:@"onReceiveInterstitialAd" withData:nil];
    if (self.interstitialView){
        if(self.autoShowInterstitial) {
            [self __showInterstitial:YES];
        }
    }
}

- (void)interstitialWillPresentScreen:(GADInterstitial *)interstitial {
    [self fireEvent:@"" event:@"onPresentInterstitialAd" withData:nil];
}

- (void)interstitialDidDismissScreen:(GADInterstitial *)interstitial {
    [self fireEvent:@"" event:@"onDismissInterstitialAd" withData:nil];
    self.interstitialView = nil;
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

	self.bannerView = nil;
    self.interstitialView = nil;
}

@end
