import argscheck from 'cordova/argscheck'
import exec from 'cordova/exec'

export const AD_SIZE = {
  SMART_BANNER: 'SMART_BANNER',
  BANNER: 'BANNER',
  IAB_MRECT: 'IAB_MRECT',
  IAB_BANNER: 'IAB_BANNER',
  IAB_LEADERBOARD: 'IAB_LEADERBOARD',
}

export const setOptions = (options, successCallback, failureCallback) => {
  if (typeof options === 'object') {
    cordova.exec( successCallback, failureCallback, 'AdMob', 'setOptions', [ options ] )
  } else {
    if (typeof failureCallback === 'function') {
      failureCallback('options should be specified.')
    }
  }
}

export const createBannerView = (options = {}, successCallback, failureCallback) => {
  cordova.exec( successCallback, failureCallback, 'AdMob', 'createBannerView', [ options ] )
}

export const createInterstitialView = (options, successCallback, failureCallback) => {
  cordova.exec( successCallback, failureCallback, 'AdMob', 'createInterstitialView', [ options ] )
}

export const destroyBannerView = (options = {}, successCallback, failureCallback) => {
  cordova.exec( successCallback, failureCallback, 'AdMob', 'destroyBannerView', [] )
}

export const requestInterstitialAd = (options = {}, successCallback, failureCallback) => {
  cordova.exec( successCallback, failureCallback, 'AdMob', 'requestInterstitialAd', [ options ] )
}

export const showAd = (show = true, successCallback, failureCallback) => {
  cordova.exec( successCallback, failureCallback, 'AdMob', 'showAd', [ show ] )
}

export const showInterstitialAd = (show = true, successCallback, failureCallback) => {
  cordova.exec( successCallback, failureCallback, 'AdMob', 'showInterstitialAd', [ show ] )
}
