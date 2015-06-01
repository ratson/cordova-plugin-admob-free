//Copyright (c) 2014 Sang Ki Kwon (Cranberrygame)
//Email: cranberrygame@yahoo.com
//Homepage: http://cranberrygame.github.io
//License: MIT (http://opensource.org/licenses/MIT)
////#import <GoogleMobileAds/GoogleMobileAds.h> //http://stackoverflow.com/questions/28572343/admob-sdk-ios-file-not-found
////#import <GoogleMobileAds/GADBannerView.h>
////#import <GoogleMobileAds/GADInterstitialDelegate.h>
//
#import "Admob.h"

@interface AdmobOverlap : NSObject <PluginDelegate, GADBannerViewDelegate, GADInterstitialDelegate>

@property id<Plugin> plugin;
//
@property NSString *bannerAdUnit;
@property NSString *fullScreenAdUnit;
@property BOOL isOverlap;
@property BOOL isTest;
//
@property NSString *bannerPreviousPosition;
@property NSString *bannerPreviousSize;
@property NSInteger lastOrientation;
//
@property BOOL bannerAdPreload;	
@property BOOL fullScreenAdPreload;	
//admob
@property GADBannerView *bannerView;
@property GADInterstitial *interstitialView;

- (id) initWithPlugin:(id<Plugin>)plugin_;

@end

