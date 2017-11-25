# Releases

## Next


## 0.12.1 (2017-11-25)

### Fixed

* Update Rewarded Video Test Ad ID.  ([@nicolasmoreira](https://github.com/nicolasmoreira) in [#125](https://github.com/ratson/cordova-plugin-admob-free/pull/125))


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
