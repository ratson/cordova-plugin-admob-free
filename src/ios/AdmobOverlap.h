//Copyright (c) 2014 Sang Ki Kwon (Cranberrygame)
//Email: cranberrygame@yahoo.com
//Homepage: http://www.github.com/cranberrygame
//License: MIT (http://opensource.org/licenses/MIT)
#import <Foundation/Foundation.h>
#import <Cordova/CDV.h>
//
#import <GoogleMobileAds/GADBannerView.h>
#import <GoogleMobileAds/GADInterstitialDelegate.h>
#import <UIKit/UIKit.h>
//
#import "Admob.h"

@interface AdmobOverlap : NSObject <PluginDelegate, GADBannerViewDelegate, GADInterstitialDelegate>

@property id<Plugin> plugin;
//
@property NSString *adUnit;
@property NSString *adUnitFullScreen;
@property BOOL isOverlap;
@property BOOL isTest;
//
@property NSString *email;
@property NSString *licenseKey;
@property NSString *TEST_AD_UNIT;
@property NSString *TEST_AD_UNIT_FULL_SCREEN;
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

