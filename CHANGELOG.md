# Releases

## Next

## 0.26.0 (2019-03-30)

* Update `cordova-admob-sdk` to v0.24.1.

## 0.25.0 (2018-12-20)

* Update `cordova-admob-sdk` to v0.22.0.

## 0.24.0 (2018-12-07)

* Fix iOS banner position.

## 0.23.0 (2018-11-20)

* Update `cordova-admob-sdk` to v0.21.0.

## 0.22.0 (2018-10-27)

* Display test ads in Test Lab.
* Update `cordova-admob-sdk` to v0.20.2.

## 0.21.0 (2018-10-10)

* Make plugin available for Ads SDK v17.0.0. ([@paulstelzer](https://github.com/paulstelzer) in [#287](https://github.com/ratson/cordova-plugin-admob-free/pull/287))

## 0.20.0 (2018-10-06)

* Update `cordova-admob-sdk` to v0.20.0.

## 0.19.0 (2018-09-27)

* Emit COMPLETE event for Rewarded Video ad. ([@Montoya](https://github.com/Montoya) in [#281](https://github.com/ratson/cordova-plugin-admob-free/pull/281))
* Update `cordova-admob-sdk` to v0.19.0.
* Build with `babel` v7.


## 0.18.0 (2018-08-20)

* Update `cordova-admob-sdk` to v0.18.0.


## 0.17.4 (2018-07-30)

* Fix mismatch doc. ([@charlesverge](https://github.com/charlesverge) in [#267](https://github.com/ratson/cordova-plugin-admob-free/pull/267))


## 0.17.3 (2018-07-14)

* Fix banner locations on the iPhone X for top banners. ([@tennist](https://github.com/tennist) in [#262](https://github.com/ratson/cordova-plugin-admob-free/pull/262))

## 0.17.2 (2018-06-25)

* Remove unused import. ([@jcesarmobile](https://github.com/jcesarmobile) in [#254](https://github.com/ratson/cordova-plugin-admob-free/pull/254))
* Revert [#234](https://github.com/ratson/cordova-plugin-admob-free/pull/234))

## 0.17.1 (2018-06-16)

* Fix iPhone X & iOs11 issues. ([@tennist](https://github.com/tennist) in [#232](https://github.com/ratson/cordova-plugin-admob-free/pull/232))
* Fix onRewardedVideoCompleted error. ([@Penny13692018](https://github.com/Penny13692018) in [#234](https://github.com/ratson/cordova-plugin-admob-free/pull/234))

## 0.17.0 (2018-05-22)

* Update `cordova-admob-sdk` to v0.17.0. ([@rafaellop](https://github.com/rafaellop) in [#219](https://github.com/ratson/cordova-plugin-admob-free/pull/219))

## 0.16.1 (2018-05-13)

* Fix for iPhone X status bar improperly refreshed ([@rafaellop](https://github.com/rafaellop) in [#218](https://github.com/ratson/cordova-plugin-admob-free/pull/218))

## 0.16.0 (2018-04-27)

* Update `cordova-admob-sdk` to v0.16.0.
* Fix not showing production ads on iOS ([@MBuchalik](https://github.com/MBuchalik) in [#208](https://github.com/ratson/cordova-plugin-admob-free/pull/208))
* Fix crashes on Android ([@tominou](https://github.com/tominou) in [#201](https://github.com/ratson/cordova-plugin-admob-free/pull/201))

## 0.15.0 (2018-03-25)

* Update `cordova-admob-sdk` to v0.15.1.
* Implement `onRewardedVideoCompleted()` callback for Android.
* Add space for non-overlapping bottom banners on iPhone X ([@MBuchalik](https://github.com/MBuchalik) in [#186](https://github.com/ratson/cordova-plugin-admob-free/pull/186))
* Add filenames to src attr to copy source-files. ([@srikanth-wgl](https://github.com/srikanth-wgl) in [#189](https://github.com/ratson/cordova-plugin-admob-free/pull/189))

## 0.14.0 (2018-02-09)

### Breaking

* Update `cordova-admob-sdk` to v0.15.0.


## 0.13.0 (2017-12-10)

### Breaking

* Update `cordova-admob-sdk` to v0.13.1.
  Now `cordova-android >=6.3.0` is required.

## 0.12.1 (2017-11-25)

### Fixed

* Update Rewarded Video Test Ad ID. ([@nicolasmoreira](https://github.com/nicolasmoreira) in [#125](https://github.com/ratson/cordova-plugin-admob-free/pull/125))

## 0.12.0 (2017-11-19)

### Breaking

* Update Google AdMob SDK for iOS to v7.26.0.

### Fixed

* Fix banner size bug.

## 0.11.0 (2017-09-23)

### Breaking

* Update Google AdMob SDK for iOS to v7.24.0.
* Update cordova-promise-polyfill to v0.0.2.

## 0.10.0 (2017-07-31)

### Breaking

* Update Google AdMob SDK for iOS to v7.21.0.

### Fixed

* Avoid webview redraws the background. ([@warcry2000](https://github.com/wf9a5m75) in [#95](https://github.com/ratson/cordova-plugin-admob-free/pull/95))

## 0.9.0 (2017-05-07)

### Breaking

* Update Google AdMob SDK for iOS to v7.20.0.

## 0.8.0 (2017-03-22)

### Breaking

* Update Google AdMob SDK for iOS to v7.19.0.

## 0.7.0 (2017-02-21)

### Added

* Add availability methods for interstitials and reward videos. ([@vintage](https://github.com/vintage) in [#54](https://github.com/ratson/cordova-plugin-admob-free/pull/54))

### Breaking

* Update Google AdMob SDK for iOS to v7.17.0.

## 0.6.1 (2017-01-25)

### Added

* Document events.

### Fixed

* Fix iOS InterstitialAd does not call failure callback. ([@becvert](https://github.com/becvert) in [#47](https://github.com/ratson/cordova-plugin-admob-free/pull/47))

## 0.6.0 (2017-01-16)

Introduce whole new set of API, which focus on consistency and
conciseness. All old API should just work as before, but emit warnings,
it is strongly recommended to migrate to new API and give feedback.

### New API

* `window.admob` is introduced to replace `window.AdMob` and `window.plugins.AdMob`. Developer should no longer need to worry about missed upper-case causing `Admob is not defined` error.

* `admob.banner.prepare()` is replacing `createBannerView()`.

* `admob.banner.show()` and `admob.banner.hide()` are replacing `showAd()`.

* `admob.banner.remove()` is replacing `destroyBannerView()`.

* `admob.interstitial.prepare()` is replacing `prepareInterstitial()`

* `admob.interstitial.show()` is replacing `showInterstitialAd()`

### Breaking

* Update Google AdMob SDK for iOS to v7.16.0.

### Internal

* `prepareInterstitial` is now implemented natively, which should fix some race condition bugs.

### Added

* Support Rewarded Video. ([@warcry2000](https://github.com/warcry2000) in [#35](https://github.com/ratson/cordova-plugin-admob-free/pull/35))
* Promise APIs.
* Events now have namespace.
* Add [mediation example](https://github.com/ratson/cordova-plugin-admob-free/tree/master/examples/mediation)

## 0.5.1 (2016-12-08)

### Fixed

* Add more Babel transforms for more compatible code.

## 0.5.0 (2016-12-08)

### Breaking

* Drop support for WP8.
* Update Google AdMob SDK for iOS to v7.15.0.

### Added

* [Documentation page](https://ratson.github.io/cordova-plugin-admob-free/) is available.
* Support Child-directed setting. ([@Venkat-TTapp](https://github.com/Venkat-TTapp) in [#32](https://github.com/ratson/cordova-plugin-admob-free/issues/32))

### Fixed

* Babel generated more compatible code.
* Separate autoShow for banner and interstitial. ([@oangelo](https://github.com/oangelo) in [#27](https://github.com/ratson/cordova-plugin-admob-free/issues/27))

## 0.4.1 (2016-11-22)

### Fixed

* Remove duplicated `LARGE_BANNER`. ([@warcry2000](https://github.com/warcry2000) in [#28](https://github.com/ratson/cordova-plugin-admob-free/issues/28))

## 0.4.0 (2016-11-07)

### Breaking

* Drop support for Cordova < 4.
* Update Google AdMob SDK for iOS to v7.14.0.

### Added

* Add `FLUID` ad size support.

## 0.3.1 (2016-10-26)

### Fixed

* Fix banner focus for Android. ([@raulpopi](https://github.com/raulpopi) in [#19](https://github.com/ratson/cordova-plugin-admob-free/issues/19))

## 0.3.0 (2016-10-23)

### Added

* Add `LARGE_BANNER` support.

### Fixed

* Fix displaying test ads on iOS devices. ([@vintage](https://github.com/vintage) in [#17](https://github.com/ratson/cordova-plugin-admob-free/pull/17))
