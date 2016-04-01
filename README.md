# Cordova AdMob Plugin

A free, no ad-sharing version of Google AdMob plugin for Cordova.

## Features

- **No Ad-Sharing**

  Unlike [some](https://github.com/appfeel/admob-google-cordova/blob/master/src/android/AdMobAds.java#L270) [other](https://github.com/sunnycupertino/cordova-plugin-admob-simple/blob/master/src/android/AdMob.java#L194) [plugins](https://github.com/floatinghotpot/cordova-admob-pro/wiki/License-Agreement#2-win-win-partnership), this plugin does not share your advertising revenue by randomly display developer's owned ads.

- **Fully Open Sourced**

  Except the provided [AdMob SDKs](https://github.com/sunnycupertino/cordova-admob-sdklibs), every line of code are on Github. You don't execute [compiled](https://github.com/floatinghotpot/cordova-extension/blob/master/src/android/cordova-generic-ad.jar) [binary](https://github.com/floatinghotpot/cordova-extension/blob/master/src/ios/libCordovaGenericAd.a) without seeing the source code.

- **No Remote Control**

  Do not [send your application information to a remote server](https://github.com/floatinghotpot/cordova-admob-pro/issues/326) to control whether ad could be displayed.

## Installation

```bash
cordova plugin add cordova-plugin-admob-free
```

## Usage

### 1. Go to [AdMob portal](https://www.google.com/admob/), create Ad Unit ID for your banner and interstitial.

### 2. Define configiration for differrent platforms.

```javascript
var admobid = {};
if ( /(android)/i.test(navigator.userAgent) ) {  // for android & amazon-fireos
  admobid = {
    banner: 'ca-app-pub-xxx/xxx',
    interstitial: 'ca-app-pub-xxx/xxx',
  };
} else if ( /(ipod|iphone|ipad)/i.test(navigator.userAgent) ) {  // for ios
  admobid = {
    banner: 'ca-app-pub-xxx/xxx',
    interstitial: 'ca-app-pub-xxx/xxx',
  };
} else {  // for windows phone
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
  autoShow: false,  // auto show interstitial ad when loaded
});
```

### 4. Display advertisements

#### Banner Ad

```javascript
// Create banner
AdMob.createBannerView();

// Close the banner
AdMob.destroyBannerView();
```

#### Interstitial Ad

```javascript
// preppare and load ad resource in background, e.g. at begining of game level
AdMob.prepareInterstitial({
  adId: admobid.interstitial,
  autoShow: false,
});

// show the interstitial later, e.g. at end of game level
AdMob.showInterstitial();
```

## Screenshots

Android Banner                 |  Android Interstitial
-------------------------------|--------------------------------------------
![ScreenShot](docs/android.jpg) | ![ScreenShot](docs/android_interstitial.jpg)

iPhone Banner                  |  iPhone Interstitial
-------------------------------|--------------------------------------------
![ScreenShot](docs/iphone.jpg) | ![ScreenShot](docs/iphone_interstitial.jpg)

## API

Methods:
```javascript
// set default value for other methods
AdMob.setOptions(options, success, fail);

// use banner
AdMob.createBannerView();
AdMob.destroyBannerView();

// use interstitial
AdMob.prepareInterstitial(adId/options, success, fail);
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
document.addEventListener('onLeaveToAd', function() { );
document.addEventListener('onReceiveInterstitialAd', function() {});
document.addEventListener('onPresentInterstitialAd', function() {});
document.addEventListener('onDismissInterstitialAd', function() {});
```

## Status

This plugin is forked from [cordova-plugin-admob-simple](https://github.com/sunnycupertino/cordova-plugin-admob-simple) and removed the ad-sharing related code. All APIs are remaining the same as the original fork.

For Android, there are also some enhanced options. If anyone wants them on iOS, please open an issue and let me know.

If you find this plugin useful, please [star it on Github](https://github.com/ratson/cordova-plugin-admob-free).

## Contributing

You can use this Cordova plugin for free. You can contribute to this project in many ways:

* [Reporting issues](https://github.com/ratson/cordova-plugin-admob-free/issues).
* Patching and bug fixing, especially when submitted with test code. [Open a pull request](https://github.com/ratson/cordova-plugin-admob-free/pulls).
* Other enhancements.

Help with documentation is always appreciated and can be done via pull requests.

## Credits

Thanks for the [cordova-plugin-admob-simple](https://github.com/sunnycupertino/cordova-plugin-admob-simple) author for forking the original project [cordova-plugin-admob](https://github.com/floatinghotpot/cordova-plugin-admob) to [make it functional](https://github.com/sunnycupertino/cordova-plugin-admob-simple/issues/1) and open source it.

Screenshots are copied from [cordova-admob-pro](https://github.com/floatinghotpot/cordova-admob-pro)

## Disclaimer

This is NOT an official Google product. It is just a community-driven project, which use the Google AdMob SDKs.

## License

[MIT](LICENSE)
