# Cordova AdMob Plugin

A free, no ad-sharing version of Google AdMob plugin for Cordova.

## Features

- **No Ad-Sharing**

  Unlike [some](https://github.com/appfeel/admob-google-cordova/blob/3f122f278a323a4bc9e580f400182a7bd690a346/src/android/AdMobAds.java#L569) [other](https://github.com/sunnycupertino/cordova-plugin-admob-simple/blob/a58846c1ea14188a4aef44381ccd28ffdcae3bfa/src/android/AdMob.java#L207) [plugins](https://github.com/floatinghotpot/cordova-admob-pro/wiki/License-Agreement#2-win-win-partnership), this plugin does not share your advertising revenue by randomly display developer's owned ads.

- **Fully Open Sourced**

  Except Google provided [AdMob SDKs](https://github.com/rehy/cordova-admob-sdk), every line of code are on Github. You don't [execute](https://github.com/admob-google/admob-cordova/blob/master/src/android/libs/admobadplugin.jar) [compiled](https://github.com/floatinghotpot/cordova-extension/blob/master/src/android/cordova-generic-ad.jar) [binary](https://github.com/floatinghotpot/cordova-extension/blob/master/src/ios/libCordovaGenericAd.a) without seeing the source code.

- **No Remote Control**

  Do not [send your application information to a remote server](https://github.com/floatinghotpot/cordova-admob-pro/issues/326) to control whether ad could be displayed. Therefore, you don't [lose revenue](https://github.com/floatinghotpot/cordova-admob-pro/issues/544) because [some server bugs](https://github.com/floatinghotpot/cordova-admob-pro/issues/450#issuecomment-244837346),

### Compare to other projects

  Project | No Ad-Sharing | Fully Open Sourced | No Remote Control
  --------|---------------|--------------------|-------------------
  [admob](https://github.com/admob-google/admob-cordova) | Not Sure  | [❌](https://github.com/admob-google/admob-cordova/blob/master/src/android/libs/admobadplugin.jar) [❌](https://github.com/admob-google/admob-cordova/blob/master/src/ios/AdmobAPI.framework/AdmobAPI) | Not Sure
  [cordova-admob](https://github.com/appfeel/admob-google-cordova) | [❌](https://github.com/appfeel/admob-google-cordova/blob/3f122f278a323a4bc9e580f400182a7bd690a346/src/android/AdMobAds.java#L569) | ✅ | ✅
  [cordova-plugin-ad-admob](https://github.com/cranberrygame/cordova-plugin-ad-admob) | [❌](https://github.com/cranberrygame/cordova-plugin-ad-admob/blob/7aaa397b19ab63579d6aa68fbf20ffdf795a15fc/src/android/AdMobPlugin.java#L330) | ✅ | ✅
  [cordova-plugin-admob-free](https://github.com/ratson/cordova-plugin-admob-free) | ✅ | ✅ | ✅
  [cordova-plugin-admob-simple](https://github.com/sunnycupertino/cordova-plugin-admob-simple) | [❌](https://github.com/sunnycupertino/cordova-plugin-admob-simple/blob/a58846c1ea14188a4aef44381ccd28ffdcae3bfa/src/android/AdMob.java#L207) | ✅ | [❌](https://github.com/sunnycupertino/cordova-plugin-admob-simple/blob/f7cc64e9e018f2146b2735b5ae8d3b780fa24f72/src/android/AdMob.java#L728)
  [cordova-plugin-admobpro](https://github.com/floatinghotpot/cordova-admob-pro) | [❌](https://github.com/floatinghotpot/cordova-admob-pro/wiki/License-Agreement#2-win-win-partnership) | [❌](https://github.com/floatinghotpot/cordova-extension/blob/master/src/android/cordova-generic-ad.jar) [❌](https://github.com/floatinghotpot/cordova-extension/blob/master/src/ios/libCordovaGenericAd.a)  | [❌](https://github.com/floatinghotpot/cordova-admob-pro/issues/326) [❌](https://github.com/floatinghotpot/cordova-admob-pro/issues/450)

Click ❌ to see the detail.

## Installation

```sh
cordova plugin add cordova-plugin-admob-free --save
```

Note that `cordova plugin add [GIT_URL]` is not supported.

## Usage

### 1. Create Ad Unit ID for your banner and interstitial.

Go to the [AdMob portal](https://www.google.com/admob/) and add your app (if you haven't done so already), once your app is added to your AdMob account, create a new ad unit for it.

### 2. Display advertisements

#### [Banner Ad](https://ratson.github.io/cordova-plugin-admob-free/variable/index.html#static-variable-banner)

#### [Interstitial Ad](https://ratson.github.io/cordova-plugin-admob-free/variable/index.html#static-variable-interstitial)

#### [Reward Video Ad](https://ratson.github.io/cordova-plugin-admob-free/variable/index.html#static-variable-rewardvideo)

### 3. Profit

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

See [documentation page](https://ratson.github.io/cordova-plugin-admob-free/identifiers.html).


## Status

This plugin is forked from [cordova-plugin-admob-simple](https://github.com/sunnycupertino/cordova-plugin-admob-simple) and removed the ad-sharing related code.

The code for Android has almost all rewritten to be more maintainable, plus some extra features.

For iOS, while it is working, I have less interest in writing Objective-C code these days. If someone is willing to help, feel free to send a pull request, I will review it.


## Contributing

You can use this Cordova plugin for free. You can contribute to this project in many ways:

* [Reporting issues](https://github.com/ratson/cordova-plugin-admob-free/issues).
* Patching and bug fixing, especially when submitted with test code. [Open a pull request](https://github.com/ratson/cordova-plugin-admob-free/pulls).
* Other enhancements.

Help with documentation is always appreciated and can be done via pull requests.

Read [Contributing Guide](https://ratson.github.io/cordova-plugin-admob-free/manual/tutorial.html#contributing-guide) to learn how to contribute.

### Ionic Support

While the Ionic community have provided [an Ionic Native Plugin](https://ionicframework.com/docs/native/admob-free/), plugin users need more examples and tutorials.

As I ([@ratson](https://github.com/ratson)) don't use Ionic myself, it would be great if some experienced Ionic developers could help answering questions or come up with more examples. HELP WANTED HERE.


## Credits

Thanks for the [cordova-plugin-admob-simple](https://github.com/sunnycupertino/cordova-plugin-admob-simple) author for forking the original project [cordova-plugin-admob](https://github.com/floatinghotpot/cordova-plugin-admob) to [make it functional](https://github.com/sunnycupertino/cordova-plugin-admob-simple/issues/1) and open source it.

Screenshots are copied from [cordova-admob-pro](https://github.com/floatinghotpot/cordova-admob-pro).

## Disclaimer

This is NOT an official Google product. It is just a community-driven project, which use the Google AdMob SDKs.

## License

[MIT](LICENSE)
