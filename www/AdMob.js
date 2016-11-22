var argscheck = require('cordova/argscheck');
var exec = require('cordova/exec');
var AdMob = {};

AdMob.AD_SIZE = {
  BANNER: 'BANNER',
  IAB_BANNER: 'IAB_BANNER',
  IAB_LEADERBOARD: 'IAB_LEADERBOARD',
  IAB_MRECT: 'IAB_MRECT',
  LARGE_BANNER: 'LARGE_BANNER',
  SMART_BANNER: 'SMART_BANNER',
  FLUID: 'FLUID',
  // android-only
  FULL_BANNER: 'FULL_BANNER',
  // LARGE_BANNER: 'LARGE_BANNER', // See previous entry
  LEADERBOARD: 'LEADERBOARD',
  MEDIUM_RECTANGLE: 'MEDIUM_RECTANGLE',
  SEARCH: 'SEARCH',
  WIDE_SKYSCRAPER: 'WIDE_SKYSCRAPER',
}

AdMob.setOptions = function(options, successCallback, failureCallback) {
  if (typeof options === 'object') {
    cordova.exec( successCallback, failureCallback, 'AdMob', 'setOptions', [ options ] )
  } else {
    if (typeof failureCallback === 'function') {
      failureCallback('options should be specified.')
    }
  }
}

AdMob.createBannerView = function(options, successCallback, failureCallback) {
  if (typeof options == 'undefined') {
    options = {};
  }
  cordova.exec( successCallback, failureCallback, 'AdMob', 'createBannerView', [ options ] )
}

AdMob.createInterstitialView = function(options, successCallback, failureCallback) {
  cordova.exec( successCallback, failureCallback, 'AdMob', 'createInterstitialView', [ options ] )
}

AdMob.destroyBannerView = function(options, successCallback, failureCallback) {
  if (typeof options == 'undefined') {
    options = {};
  }
  cordova.exec( successCallback, failureCallback, 'AdMob', 'destroyBannerView', [] )
}

AdMob.requestInterstitialAd = function(options, successCallback, failureCallback) {
  if (typeof options == 'undefined') {
    options = {};
  }
  cordova.exec( successCallback, failureCallback, 'AdMob', 'requestInterstitialAd', [ options ] )
}

AdMob.showAd = function(show, successCallback, failureCallback) {
  if (typeof options == 'undefined') {
    options = true;
  }
  cordova.exec( successCallback, failureCallback, 'AdMob', 'showAd', [ show ] )
}

AdMob.showInterstitialAd = function(show, successCallback, failureCallback) {
  if (typeof options == 'undefined') {
    options = true;
  }
  cordova.exec( successCallback, failureCallback, 'AdMob', 'showInterstitialAd', [ show ] )
}

// emulate cordova-admob-pro interface

AdMob.prepareInterstitial = function(args, successCallback, failureCallback) {
  AdMob.createInterstitialView(args, successCallback, failureCallback)
  AdMob.requestInterstitialAd(args, successCallback, failureCallback)
}

AdMob.showInterstitial = function(successCallback, failureCallback) {
  AdMob.showInterstitialAd(true, successCallback, failureCallback)
}

module.exports = AdMob;
