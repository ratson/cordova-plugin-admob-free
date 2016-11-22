var argscheck = require('cordova/argscheck');
var exec = require('cordova/exec');

exports.AD_SIZE = {
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

exports.setOptions = function(options, successCallback, failureCallback) {
  if (typeof options === 'object') {
    cordova.exec( successCallback, failureCallback, 'AdMob', 'setOptions', [ options ] )
  } else {
    if (typeof failureCallback === 'function') {
      failureCallback('options should be specified.')
    }
  }
}

exports.createBannerView = function(options, successCallback, failureCallback) {
  if (typeof options == 'undefined') {
    options = {};
  }
  cordova.exec( successCallback, failureCallback, 'AdMob', 'createBannerView', [ options ] )
}

exports.createInterstitialView = function(options, successCallback, failureCallback) {
  cordova.exec( successCallback, failureCallback, 'AdMob', 'createInterstitialView', [ options ] )
}

exports.destroyBannerView = function(options, successCallback, failureCallback) {
  if (typeof options == 'undefined') {
    options = {};
  }
  cordova.exec( successCallback, failureCallback, 'AdMob', 'destroyBannerView', [] )
}

exports.requestInterstitialAd = function(options, successCallback, failureCallback) {
  if (typeof options == 'undefined') {
    options = {};
  }
  cordova.exec( successCallback, failureCallback, 'AdMob', 'requestInterstitialAd', [ options ] )
}

exports.showAd = function(show, successCallback, failureCallback) {
  if (typeof options == 'undefined') {
    options = true;
  }
  cordova.exec( successCallback, failureCallback, 'AdMob', 'showAd', [ show ] )
}

exports.showInterstitialAd = function(show, successCallback, failureCallback) {
  if (typeof options == 'undefined') {
    options = true;
  }
  cordova.exec( successCallback, failureCallback, 'AdMob', 'showInterstitialAd', [ show ] )
}

// emulate cordova-admob-pro interface

exports.prepareInterstitial = function(args, successCallback, failureCallback) {
  createInterstitialView(args, successCallback, failureCallback)
  requestInterstitialAd(args, successCallback, failureCallback)
}

exports.showInterstitial = function(successCallback, failureCallback) {
  showInterstitialAd(true, successCallback, failureCallback)
}
