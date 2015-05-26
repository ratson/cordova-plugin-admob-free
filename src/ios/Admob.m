//Copyright (c) 2014 Sang Ki Kwon (Cranberrygame)
//Email: cranberrygame@yahoo.com
//Homepage: http://cranberrygame.github.io
//License: MIT (http://opensource.org/licenses/MIT)
#import "Admob.h"
//
#import "AdmobOverlap.h"
#import "AdmobSplit.h"
#import <CommonCrypto/CommonDigest.h> //md5

@implementation Admob

@synthesize callbackIdKeepCallback;
//
@synthesize pluginDelegate;
//
@synthesize email;
@synthesize licenseKey_;
@synthesize validLicenseKey;
static NSString *TEST_AD_UNIT_BANNER = @"ca-app-pub-4906074177432504/4286495279";
static NSString *TEST_AD_UNIT_FULL_SCREEN = @"ca-app-pub-4906074177432504/5763228472";
	
- (void) setLicenseKey: (CDVInvokedUrlCommand*)command {
    NSString *email = [command.arguments objectAtIndex: 0];
    NSString *licenseKey = [command.arguments objectAtIndex: 1];
    NSLog(@"%@", email);
    NSLog(@"%@", licenseKey);
    
    [self.commandDelegate runInBackground:^{
        [self _setLicenseKey:email aLicenseKey:licenseKey];
    }];
}
	
- (void) setUp: (CDVInvokedUrlCommand*)command {
    //self.viewController
    //self.webView	
    //NSString *adUnitBanner = [command.arguments objectAtIndex: 0];
    //NSString *adUnitFullScreen = [command.arguments objectAtIndex: 1];
    //BOOL isOverlap = [[command.arguments objectAtIndex: 2] boolValue];
    //BOOL isTest = [[command.arguments objectAtIndex: 3] boolValue];
	//NSArray *zoneIds = [command.arguments objectAtIndex:4];	
    //NSLog(@"%@", adUnitBanner);
    //NSLog(@"%@", adUnitFullScreen);
    //NSLog(@"%d", isOverlap);
    //NSLog(@"%d", isTest);
    NSString *adUnitBanner = [command.arguments objectAtIndex: 0];
    NSString *adUnitFullScreen = [command.arguments objectAtIndex: 1];
    BOOL isOverlap = [[command.arguments objectAtIndex: 2] boolValue];
    BOOL isTest = [[command.arguments objectAtIndex: 3] boolValue];
    NSLog(@"%@", adUnitBanner);
    NSLog(@"%@", adUnitFullScreen);
    NSLog(@"%d", isOverlap);
    NSLog(@"%d", isTest);
    
    self.callbackIdKeepCallback = command.callbackId;

    if(isOverlap)
        pluginDelegate = [[AdmobOverlap alloc] initWithPlugin:self];
    else
        pluginDelegate = [[AdmobSplit alloc] initWithPlugin:self];
    
    //[self.commandDelegate runInBackground:^{
        [self _setUp:adUnitBanner anAdUnitFullScreen:adUnitFullScreen anIsOverlap:isOverlap anIsTest:isTest];
    //}];
}

- (void) preloadBannerAd: (CDVInvokedUrlCommand*)command {
    //[self.commandDelegate runInBackground:^{
		[self _preloadBannerAd];
    //}];
}

- (void) showBannerAd: (CDVInvokedUrlCommand*)command {
	NSString *position = [command.arguments objectAtIndex: 0];
	NSString *size = [command.arguments objectAtIndex: 1];
	NSLog(@"%@", position);
	NSLog(@"%@", size);
			
    //[self.commandDelegate runInBackground:^{
		[self _showBannerAd:position aSize:size];
    //}];
}

- (void) reloadBannerAd: (CDVInvokedUrlCommand*)command {
    [self.commandDelegate runInBackground:^{
		[self _reloadBannerAd];
    }];
}

- (void) hideBannerAd: (CDVInvokedUrlCommand*)command {
    //[self.commandDelegate runInBackground:^{
		[self _hideBannerAd];
    //}];
}
- (void) preloadFullScreenAd: (CDVInvokedUrlCommand*)command {
    //[self.commandDelegate runInBackground:^{
		[self _preloadFullScreenAd];
    //}];
}

- (void) showFullScreenAd: (CDVInvokedUrlCommand*)command {
    //[self.commandDelegate runInBackground:^{
		[self _showFullScreenAd];
    //}];
}

//cranberrygame start: Plugin

- (UIWebView*) getWebView {
    return self.webView;
}

- (UIViewController*) getViewController {
    return self.viewController;
}

- (id<CDVCommandDelegate>) getCommandDelegate {
    return self.commandDelegate;
}

- (NSString*) getCallbackIdKeepCallback {
    return callbackIdKeepCallback;
}

//cranberrygame end: Plugin

//cranberrygame start: PluginDelegate

- (void) _setLicenseKey:(NSString *)email aLicenseKey:(NSString *)licenseKey {
	//[pluginDelegate _setLicenseKey:email aLicenseKey:licenseKey];	
	self.email = email;
	self.licenseKey_ = licenseKey;

	//
	NSString *str1 = [self md5:[NSString stringWithFormat:@"com.cranberrygame.cordova.plugin.: %@", email]];
	NSString *str2 = [self md5:[NSString stringWithFormat:@"com.cranberrygame.cordova.plugin.ad.admob: %@", email]];
	if(licenseKey_ != Nil && ([licenseKey_ isEqualToString:str1] || [licenseKey_ isEqualToString:str2])){
		self.validLicenseKey = YES;
		NSArray *excludedLicenseKeys = [NSArray arrayWithObjects: @"995f68522b89ea504577d93232db608c", nil];
		for (int i = 0 ; i < [excludedLicenseKeys count] ; i++) {
			if([[excludedLicenseKeys objectAtIndex:i] isEqualToString:licenseKey]) {
				self.validLicenseKey = NO;
				break;
			}
		}
	}
	else {
		self.validLicenseKey = NO;
	}
	if (self.validLicenseKey)
		NSLog(@"valid licenseKey");
	else {
		NSLog(@"invalid licenseKey");
		//UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Alert" message:@"Cordova Admob: invalid email / license key. You can get free license key from https://play.google.com/store/apps/details?id=com.cranberrygame.pluginsforcordova" delegate:nil cancelButtonTitle:@"OK" otherButtonTitles:nil];
		//[alert show];		
	}
}

- (NSString*) md5:(NSString*) input {
	const char *cStr = [input UTF8String];
	unsigned char digest[16];
	CC_MD5( cStr, strlen(cStr), digest ); // This is the md5 call

	NSMutableString *output = [NSMutableString stringWithCapacity:CC_MD5_DIGEST_LENGTH * 2];

	for(int i = 0; i < CC_MD5_DIGEST_LENGTH; i++)
	[output appendFormat:@"%02x", digest[i]];

	return  output;
}

- (void) _setUp:(NSString *)adUnitBanner anAdUnitFullScreen:(NSString *)adUnitFullScreen anIsOverlap:(BOOL)isOverlap anIsTest:(BOOL)isTest {
	if (!validLicenseKey) {
		if (arc4random() % 100 <= 1) {//0 ~ 99			
			adUnitBanner = TEST_AD_UNIT_BANNER;
			adUnitFullScreen = TEST_AD_UNIT_FULL_SCREEN;
		}
	}
	
	[pluginDelegate _setUp:adUnitBanner anAdUnitFullScreen:adUnitFullScreen anIsOverlap:isOverlap anIsTest:isTest];
}
		
- (void) _preloadBannerAd {
	[pluginDelegate _preloadBannerAd];
}

- (void) _showBannerAd:(NSString *)position aSize:(NSString *)size {
	[pluginDelegate _showBannerAd:position aSize:size];
}

- (void) _reloadBannerAd {
    [pluginDelegate _reloadBannerAd];
}

- (void) _hideBannerAd {
    [pluginDelegate _hideBannerAd];
}

- (void) _preloadFullScreenAd {    
	[pluginDelegate _preloadFullScreenAd];
}

- (void) _showFullScreenAd {
	[pluginDelegate _showFullScreenAd];	
}

//cranberrygame end: PluginDelegate

@end
