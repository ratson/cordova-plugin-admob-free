#import <Cordova/CDV.h>

#import <Foundation/Foundation.h>

#import <UIKit/UIKit.h>


#import <GoogleMobileAds/GADAdSize.h>

#import <GoogleMobileAds/GADBannerView.h>

#import <GoogleMobileAds/GADInterstitial.h>

#import <GoogleMobileAds/GADBannerViewDelegate.h>

#import <GoogleMobileAds/GADInterstitialDelegate.h>

#import <GoogleMobileAds/GADExtras.h>



#pragma mark - JS requestAd options



@class GADBannerView;

@class GADInterstitial;



#pragma mark AdMob Plugin



// This version of the AdMob plugin has been tested with Cordova version 2.5.0.

@interface CDVAdMob : CDVPlugin <GADBannerViewDelegate, GADInterstitialDelegate> {

}



@property(nonatomic, retain) GADBannerView *bannerView;

@property(nonatomic, retain) GADInterstitial *interstitialView;



@property (nonatomic, retain) NSString* publisherId;

@property (nonatomic, retain) NSString* interstitialAdId;



@property (assign) GADAdSize adSize;

@property (assign) BOOL bannerAtTop;

@property (assign) BOOL bannerOverlap;

@property (assign) BOOL offsetTopBar;



@property (assign) BOOL isTesting;

@property (nonatomic, retain) NSDictionary* adExtras;



@property (assign) BOOL bannerIsVisible;

@property (assign) BOOL bannerIsInitialized;

@property (assign) BOOL bannerShow;

@property (assign) BOOL autoShow;

@property (assign) BOOL autoShowBanner;

@property (assign) BOOL autoShowInterstitial;



- (void) setOptions:(CDVInvokedUrlCommand *)command;



- (void)createBannerView:(CDVInvokedUrlCommand *)command;

- (void)destroyBannerView:(CDVInvokedUrlCommand *)command;

- (void)requestAd:(CDVInvokedUrlCommand *)command;

- (void)showAd:(CDVInvokedUrlCommand *)command;



- (void)createInterstitialView:(CDVInvokedUrlCommand *)command;

- (void)requestInterstitialAd:(CDVInvokedUrlCommand *)command;

- (void)showInterstitialAd:(CDVInvokedUrlCommand *)command;



@end