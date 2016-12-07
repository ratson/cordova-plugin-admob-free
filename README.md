# Cordova AdMob Plugin

A free, no ad-sharing version of Google AdMob plugin for Cordova.

## Features

- **No Ad-Sharing**

  Unlike [some](https://github.com/appfeel/admob-google-cordova/blob/3f122f278a323a4bc9e580f400182a7bd690a346/src/android/AdMobAds.java#L569) [other](https://github.com/sunnycupertino/cordova-plugin-admob-simple/blob/a58846c1ea14188a4aef44381ccd28ffdcae3bfa/src/android/AdMob.java#L207) [plugins](https://github.com/floatinghotpot/cordova-admob-pro/wiki/License-Agreement#2-win-win-partnership), this plugin does not share your advertising revenue by randomly display developer's owned ads.

- **Fully Open Sourced**

  Except Google provided [AdMob SDKs](https://github.com/ratson/cordova-plugin-admob-free/tree/master/sdk), every line of code are on Github. You don't [execute](https://github.com/admob-google/admob-cordova/blob/master/src/android/libs/admobadplugin.jar) [compiled](https://github.com/floatinghotpot/cordova-extension/blob/master/src/android/cordova-generic-ad.jar) [binary](https://github.com/floatinghotpot/cordova-extension/blob/master/src/ios/libCordovaGenericAd.a) without seeing the source code.

- **No Remote Control**

  Do not [send your application information to a remote server](https://github.com/floatinghotpot/cordova-admob-pro/issues/326) to control whether ad could be displayed. Therefore, you don't lose revenue because [some server bugs](https://github.com/floatinghotpot/cordova-admob-pro/issues/450#issuecomment-244837346),

### Compare to other projects

  Project | No Ad-Sharing | Fully Open Sourced | No Remote Control
  --------|---------------|--------------------|-------------------
  [admob](https://github.com/admob-google/admob-cordova) | Not Sure  | [❌](https://github.com/admob-google/admob-cordova/blob/master/src/android/libs/admobadplugin.jar) [❌](https://github.com/admob-google/admob-cordova/blob/master/src/ios/AdmobAPI.framework/AdmobAPI) | Not Sure
  [cordova-admob](https://github.com/appfeel/admob-google-cordova) | [❌](https://github.com/appfeel/admob-google-cordova/blob/3f122f278a323a4bc9e580f400182a7bd690a346/src/android/AdMobAds.java#L569) | ✅ | ✅
  [cordova-plugin-ad-admob](https://github.com/cranberrygame/cordova-plugin-ad-admob) | [❌](https://github.com/cranberrygame/cordova-plugin-ad-admob/blob/7aaa397b19ab63579d6aa68fbf20ffdf795a15fc/src/android/AdMobPlugin.java#L330) | ✅ | ✅
  [cordova-plugin-admob-free](https://github.com/ratson/cordova-plugin-admob-free) | ✅ | ✅ | ✅
  [cordova-plugin-admob-simple](https://github.com/sunnycupertino/cordova-plugin-admob-simple) | [❌](https://github.com/sunnycupertino/cordova-plugin-admob-simple/blob/a58846c1ea14188a4aef44381ccd28ffdcae3bfa/src/android/AdMob.java#L207) | ✅ | ✅
  [cordova-plugin-admobpro](https://github.com/floatinghotpot/cordova-admob-pro) | [❌](https://github.com/floatinghotpot/cordova-admob-pro/wiki/License-Agreement#2-win-win-partnership) | [❌](https://github.com/floatinghotpot/cordova-extension/blob/master/src/android/cordova-generic-ad.jar) [❌](https://github.com/floatinghotpot/cordova-extension/blob/master/src/ios/libCordovaGenericAd.a)  | [❌](https://github.com/floatinghotpot/cordova-admob-pro/issues/326) [❌](https://github.com/floatinghotpot/cordova-admob-pro/issues/450)

Click ❌ to see the detail.

## Installation

```bash
cordova plugin add cordova-plugin-admob-free --save
```

## Usage

### 1. Create Ad Unit ID for your banner and interstitial.

Go to [AdMob portal](https://www.google.com/admob/), click "Monetize a new app" button to create new ad unit.

### 2. Define configuration for different platforms.

```javascript
var admobid = {};
if ( /(android)/i.test(navigator.userAgent) ) {  // for android & amazon-fireos
  admobid = {
    banner: 'ca-app-pub-xxx/xxx',
    interstitial: 'ca-app-pub-xxx/xxx',
  };
} else {  // for ios
  admobid = {
    banner: 'ca-app-pub-xxx/xxx',
    interstitial: 'ca-app-pub-xxx/xxx',
  };
}
```

### 3. Set options

```javascript
AdMob.setOptions({
  publisherId: admobid.banner,
  interstitialAdId: admobid.interstitial,
  bannerAtTop: false,  // set to true, to put banner at top
  overlap: true,  // set to true, to allow banner overlap webview
  offsetTopBar: false,  // set to true to avoid ios7 status bar overlap
  isTesting: false,  // receiving test ad
  autoShow: false  // auto show interstitial ad when loaded
});
```

### 4. Display advertisements

#### Banner Ad

```javascript
// Create banner
AdMob.createBannerView();

// Close the banner
AdMob.destroyBannerView();

// Hide the banner
AdMob.showAd(false);

// Show the banner
AdMob.showAd(true);
```

#### Interstitial Ad

```javascript
// prepare and load ad resource in background, e.g. at the beginning of game level
AdMob.prepareInterstitial({
  interstitialId: admobid.interstitial,
  autoShow: false
});

// show the interstitial later, e.g. at end of game level
AdMob.showInterstitial();
```

### 5. Profit

If you find this plugin useful, please [star it on Github](https://github.com/ratson/cordova-plugin-admob-free).

## Screenshots

Android Banner                                  |  Android Interstitial
------------------------------------------------|--------------------------------------------
![ScreenShot][banner-android-screenshot]        | ![ScreenShot][interstitial-android-screenshot]

iOS Banner                                      |  iOS Interstitial
------------------------------------------------|--------------------------------------------
![ScreenShot][banner-ios-screenshot]            | ![ScreenShot][interstitial-ios-screenshot]


[banner-android-screenshot]: docs/screenshots/banner-android.jpg
[banner-ios-screenshot]: docs/screenshots/banner-ios.jpg
[interstitial-android-screenshot]: docs/screenshots/interstitial-android.jpg
[interstitial-ios-screenshot]: docs/screenshots/interstitial-ios.jpg


## API

For more details, refer to [documentation page](https://ratson.github.io/cordova-plugin-admob-free/).

Methods:
```javascript
// set default value for other methods
AdMob.setOptions(options, success, fail);

// use banner
AdMob.createBannerView();
AdMob.destroyBannerView();
AdMob.showAd();

// use interstitial
AdMob.prepareInterstitial(options, success, fail);
AdMob.showInterstitial();
// low-level methods
AdMob.createInterstitialView();
AdMob.requestInterstitialAd();
AdMob.showInterstitialAd();
```

Events:
```javascript
document.addEventListener('onReceiveAd', function() {});
document.addEventListener('onFailedToReceiveAd', function(data) {});
document.addEventListener('onPresentAd', function() {});
document.addEventListener('onDismissAd', function() {});
document.addEventListener('onLeaveToAd', function() {});
document.addEventListener('onReceiveInterstitialAd', function() {});
document.addEventListener('onPresentInterstitialAd', function() {});
document.addEventListener('onDismissInterstitialAd', function() {});
```

## Status

This plugin is forked from [cordova-plugin-admob-simple](https://github.com/sunnycupertino/cordova-plugin-admob-simple) and removed the ad-sharing related code. All APIs are remaining the same as the original fork.

The code for Android has almost all rewritten to be more maintainable, plus some extra features.

For iOS, while it is working, I have less interest in writing Objective-C code these days. If someone is will to help, feel free to send a pull request, I will review it.

## Contributing

You can use this Cordova plugin for free. You can contribute to this project in many ways:

* [Reporting issues](https://github.com/ratson/cordova-plugin-admob-free/issues).
* Patching and bug fixing, especially when submitted with test code. [Open a pull request](https://github.com/ratson/cordova-plugin-admob-free/pulls).
* Other enhancements.

Help with documentation is always appreciated and can be done via pull requests.

## Credits

Thanks for the [cordova-plugin-admob-simple](https://github.com/sunnycupertino/cordova-plugin-admob-simple) author for forking the original project [cordova-plugin-admob](https://github.com/floatinghotpot/cordova-plugin-admob) to [make it functional](https://github.com/sunnycupertino/cordova-plugin-admob-simple/issues/1) and open source it.

Screenshots are copied from [cordova-admob-pro](https://github.com/floatinghotpot/cordova-admob-pro).

## Disclaimer

This is NOT an official Google product. It is just a community-driven project, which use the Google AdMob SDKs.

## License

[MIT](LICENSE)
