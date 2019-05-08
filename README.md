# Cordova AdMob Plugin

A free, no ad-sharing version of Google AdMob plugin for Cordova.

## Status

I have been asking the interest about [funding this project](https://github.com/ratson/cordova-plugin-admob-free/issues/161) a while, got some encouraging feedback, finally have setup [a funding page](https://ratson.name/fund-admob-plus/).

Please use [admob-plus](https://github.com/admob-plus/admob-plus) if possible.

While my focus is `admob-plus`, I will keep maintaining `cordova-plugin-admob-free` until all exisiting features are available with the new plugin.

If you are earning more than USD$200 monthly from using this plugin, please consider [funding my work](https://ratson.name/fund-admob-plus/).

## Features

* **No Ad-Sharing**

  Unlike [some](https://github.com/appfeel/admob-google-cordova/blob/3f122f278a323a4bc9e580f400182a7bd690a346/src/android/AdMobAds.java#L569) [other](https://github.com/sunnycupertino/cordova-plugin-admob-simple/blob/a58846c1ea14188a4aef44381ccd28ffdcae3bfa/src/android/AdMob.java#L207) [plugins](https://github.com/floatinghotpot/cordova-admob-pro/wiki/License-Agreement#3-win-win-partnership), this plugin does not share your advertising revenue by randomly display developer's owned ads.

* **Fully Open Sourced**

  Except Google provided [AdMob SDKs](https://github.com/rehy/cordova-admob-sdk), every line of code are on Github. You don't [execute](https://github.com/admob-google/admob-cordova/blob/master/src/android/libs/admobadplugin.jar) [compiled](https://github.com/floatinghotpot/cordova-extension/blob/master/src/android/cordova-generic-ad.jar) [binary](https://github.com/floatinghotpot/cordova-extension/blob/master/src/ios/libCordovaGenericAd.a) without seeing the source code.

* **No Remote Control**

  Do not [send your application information to a remote server](https://github.com/floatinghotpot/cordova-admob-pro/issues/326) to control whether ad could be displayed. Therefore, you don't [lose revenue](https://github.com/ratson/cordova-plugin-admob-free/issues/354#issuecomment-482822202) because [some server bugs](https://github.com/ratson/cordova-plugin-admob-free/issues/354#issuecomment-482821951),

  NOTE(2018-03-17): The above issue links are broken due to the author removed the discussions, the fact is the `cordova-plugin-admobpro` is sending requests to http://adlic.rjfun.com/adlic with application information for controling ad display, and some users reported losting more revenue than advertised, so use it at your own risk.

### Compare to other projects

| Project                                                                                      | No Ad-Sharing                                                                                                                                  | Fully Open Sourced                                                                                                                                                                                           | No Remote Control                                                                                                                             |
| -------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ | --------------------------------------------------------------------------------------------------------------------------------------------- |
| [admob](https://github.com/admob-google/admob-cordova)                                       | Not Sure                                                                                                                                       | [❌](https://github.com/admob-google/admob-cordova/blob/master/src/android/libs/admobadplugin.jar) [❌](https://github.com/admob-google/admob-cordova/blob/master/src/ios/AdmobAPI.framework/AdmobAPI)       | Not Sure                                                                                                                                      |
| [cordova-admob](https://github.com/appfeel/admob-google-cordova)                             | [❌](https://github.com/appfeel/admob-google-cordova/blob/3f122f278a323a4bc9e580f400182a7bd690a346/src/android/AdMobAds.java#L569)             | ✅                                                                                                                                                                                                           | ✅                                                                                                                                            |
| [cordova-plugin-ad-admob](https://github.com/cranberrygame/cordova-plugin-ad-admob)          | [❌](https://github.com/cranberrygame/cordova-plugin-ad-admob/blob/7aaa397b19ab63579d6aa68fbf20ffdf795a15fc/src/android/AdMobPlugin.java#L330) | ✅                                                                                                                                                                                                           | ✅                                                                                                                                            |
| [cordova-plugin-admob-free](https://github.com/ratson/cordova-plugin-admob-free)             | ✅                                                                                                                                             | ✅                                                                                                                                                                                                           | ✅                                                                                                                                            |
| [cordova-plugin-admob-simple](https://github.com/sunnycupertino/cordova-plugin-admob-simple) | [❌](https://github.com/sunnycupertino/cordova-plugin-admob-simple/blob/a58846c1ea14188a4aef44381ccd28ffdcae3bfa/src/android/AdMob.java#L207)  | ✅                                                                                                                                                                                                           | [❌](https://github.com/sunnycupertino/cordova-plugin-admob-simple/blob/f7cc64e9e018f2146b2735b5ae8d3b780fa24f72/src/android/AdMob.java#L728) |
| [cordova-plugin-admobpro](https://github.com/floatinghotpot/cordova-admob-pro)               | [❌](https://github.com/floatinghotpot/cordova-admob-pro/wiki/License-Agreement#2-win-win-partnership)                                         | [❌](https://github.com/floatinghotpot/cordova-extension/blob/master/src/android/cordova-generic-ad.jar) [❌](https://github.com/floatinghotpot/cordova-extension/blob/master/src/ios/libCordovaGenericAd.a) | [❌](https://github.com/floatinghotpot/cordova-admob-pro/issues/326) [❌](https://github.com/floatinghotpot/cordova-admob-pro/issues/450)     |

Click ❌ to see the detail.

NOTE(2018-03-17): `cordova-plugin-admobpro` is using [`cordova-plugin-extension`](https://www.npmjs.com/package/cordova-plugin-extension) for its compiled code, the author removed the repository casusing the above broken links.
For those interested could download the npm tarball for investigation.

## Installation

```sh
cordova plugin add cordova-plugin-admob-free --save
```

Since the version 17 of play-services-ads and the 0.21.0 version of the plugin the ADMOB_APP_ID must be added to the AndroidManifest.xml. To install the plugin without errors and to insert the ADMOB_APP_ID to the manifest file automatically use the following code:

```sh
cordova plugin add cordova-plugin-admob-free --save --variable ADMOB_APP_ID="<YOUR_ANDROID_ADMOB_APP_ID_AS_FOUND_IN_ADMOB>"
```

Since version 7.42 of the Google AdMob Mobile Ads SDK for iOS, you must add the ADMOB_APP_ID to your `Info.plist`, which you can do by adding the folowing inside the `<platform name="ios">` section in your `config.xml` file:

```xml
<config-file target="*-Info.plist" parent="GADApplicationIdentifier">
    <string>YOUR_IOS_ADMOB_APP_ID_AS_FOUND_IN_ADMOB</string>
</config-file>
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

| Android Banner                           | Android Interstitial                           |
| ---------------------------------------- | ---------------------------------------------- |
| ![ScreenShot][banner-android-screenshot] | ![ScreenShot][interstitial-android-screenshot] |

| iOS Banner                           | iOS Interstitial                           |
| ------------------------------------ | ------------------------------------------ |
| ![ScreenShot][banner-ios-screenshot] | ![ScreenShot][interstitial-ios-screenshot] |

[banner-android-screenshot]: docs/screenshots/banner-android.jpg
[banner-ios-screenshot]: docs/screenshots/banner-ios.jpg
[interstitial-android-screenshot]: docs/screenshots/interstitial-android.jpg
[interstitial-ios-screenshot]: docs/screenshots/interstitial-ios.jpg

## API

See [documentation page](https://ratson.github.io/cordova-plugin-admob-free/identifiers.html).

## Customize Google Play Services versions (Android only)

The default `PLAY_SERVICES_VERSION` is set to `11.0.4`.
If you need a different version, edit `config.xml` with following,

```xml
<plugin name="cordova-admob-sdk" spec="~0.13.1">
    <variable name="PLAY_SERVICES_VERSION" value="11.6.0" />
</plugin>
```

Note that if you are adding these lines to an existing project, you need to remove both `admob-free` plugin and `android` platform and add them back again to affect the version number.

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
