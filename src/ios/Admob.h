//Copyright (c) 2014 Sang Ki Kwon (Cranberrygame)
//Email: cranberrygame@yahoo.com
//Homepage: http://www.github.com/cranberrygame
//License: MIT (http://opensource.org/licenses/MIT)
#import <Foundation/Foundation.h>
#import <Cordova/CDV.h>
//
#import <GoogleMobileAds/GADBannerViewDelegate.h>
#import <GoogleMobileAds/GADInterstitialDelegate.h>
#import <UIKit/UIKit.h>

@protocol Plugin <NSObject>
- (UIWebView*) getWebView;
- (UIViewController*) getViewController;
- (id<CDVCommandDelegate>) getCommandDelegate;
- (NSString*) getCallbackIdKeepCallback;
@end

@protocol PluginDelegate <NSObject>
- (void) _setUp:(NSString *)adUnit anAdUnitFullScreen:(NSString *)adUnitFullScreen anIsOverlap:(BOOL)isOverlap anIsTest:(BOOL)isTest;
- (void) _setLicenseKey:(NSString *)email aLicenseKey:(NSString *)licenseKey;
- (void) _preloadBannerAd;
- (void) _showBannerAd:(NSString *)position aSize:(NSString *)size;
- (void) _reloadBannerAd;
- (void) _hideBannerAd;
- (void) _preloadFullScreenAd;
- (void) _showFullScreenAd;
@end

@interface Admob : CDVPlugin <Plugin, PluginDelegate>

@property NSString *callbackIdKeepCallback;

@property id<PluginDelegate> pluginDelegate;

- (void) setUp: (CDVInvokedUrlCommand*)command;
- (void) setLicenseKey: (CDVInvokedUrlCommand*)command;
- (void) preloadBannerAd: (CDVInvokedUrlCommand*)command;
- (void) showBannerAd: (CDVInvokedUrlCommand*)command;
- (void) reloadBannerAd: (CDVInvokedUrlCommand*)command;
- (void) hideBannerAd: (CDVInvokedUrlCommand*)command;
- (void) preloadFullScreenAd: (CDVInvokedUrlCommand*)command;
- (void) showFullScreenAd: (CDVInvokedUrlCommand*)command;

@end
