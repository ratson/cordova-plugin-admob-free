/* eslint-disable no-console */
import exec from 'cordova/exec'

/**
 * Ad sizes.
 * @constant
 * @type {Object}
 */
export const AD_SIZE = {
  BANNER: 'BANNER',
  IAB_BANNER: 'IAB_BANNER',
  IAB_LEADERBOARD: 'IAB_LEADERBOARD',
  IAB_MRECT: 'IAB_MRECT',
  LARGE_BANNER: 'LARGE_BANNER',
  SMART_BANNER: 'SMART_BANNER',
  FLUID: 'FLUID',
  // android-only
  FULL_BANNER: 'FULL_BANNER',
  LEADERBOARD: 'LEADERBOARD',
  MEDIUM_RECTANGLE: 'MEDIUM_RECTANGLE',
  SEARCH: 'SEARCH',
  WIDE_SKYSCRAPER: 'WIDE_SKYSCRAPER',
}

function boolean2string(x) {
  if (x === null) {
    return ''
  }
  if (x === true) {
    return 'yes'
  }
  if (x === false) {
    return 'no'
  }
  return x
}

function isUndefined(x) {
  return typeof x === 'undefined'
}

function translateOptions(options) {
  const opts = {}
  if (!isUndefined(options.forChild)) {
    opts.forChild = boolean2string(options.forChild)
    if (typeof options.forChild === 'string') {
      console.warn('`forChild` will not accept string in future, pass boolean instead')
    }
  }
  if (!isUndefined(options.forFamily)) {
    opts.forFamily = boolean2string(options.forFamily)
    if (typeof options.forFamily === 'string') {
      console.warn('`forFamily` will not accept string in future, pass boolean instead')
    }
  }
  return Object.assign({}, options, opts)
}
// export for testing
export const _translateOptions = translateOptions

/**
 *
 * @param {Object} options
 * @param {string} options.publisherId
 * @param {string} options.interstitialAdId
 *
 * @param {boolean} [options.bannerAtTop=false]    Set to true, to put banner at top
 * @param {boolean} [options.overlap=true]   Set to true, to allow banner overlap webview
 * @param {boolean} [options.offsetTopBar=false]    Set to true to avoid ios7 status bar overlap
 * @param {boolean} [options.isTesting=false]    Receiving test ad
 * @param {boolean} [options.autoShow=false]    Auto show interstitial ad when loaded
 *
 * @param {boolean|null} [options.forChild=null]
 * Default is not calling `tagForChildDirectedTreatment`.
 * Set to "true" for `tagForChildDirectedTreatment(true)`.
 * Set to "false" for `tagForChildDirectedTreatment(false)`.
 *
 * @param {boolean|null} [options.forFamily=null]
 * Android-only.
 * Default is not calling `setIsDesignedForFamilies`.
 * Set to "true" for `setIsDesignedForFamilies(true)`.
 * Set to "false" for `setIsDesignedForFamilies(false)`.
 *
 * @param {function()} [successCallback]
 * @param {function()} [failureCallback]
 */
export function setOptions(options, successCallback, failureCallback) {
  if (typeof options === 'object') {
    exec(successCallback, failureCallback, 'AdMob', 'setOptions', [translateOptions(options)])
  } else if (typeof failureCallback === 'function') {
    failureCallback('options should be specified.')
  }
}

export function prepareBanner(options = {}, successCallback, failureCallback) {
  exec(successCallback, failureCallback, 'AdMob', 'createBannerView', [translateOptions(options)])
}

export function removeBanner(successCallback, failureCallback) {
  exec(successCallback, failureCallback, 'AdMob', 'destroyBannerView', [])
}

export function showBanner(successCallback, failureCallback) {
  exec(successCallback, failureCallback, 'AdMob', 'showAd', [true])
}

export function hideBanner(successCallback, failureCallback) {
  exec(successCallback, failureCallback, 'AdMob', 'showAd', [false])
}

export function prepareInterstitial(args, successCallback, failureCallback) {
  createInterstitialView(args, successCallback, failureCallback)
  requestInterstitialAd(args, successCallback, failureCallback)
}

export function showInterstitial(successCallback, failureCallback) {
  exec(successCallback, failureCallback, 'AdMob', 'showInterstitialAd', [true])
}

export function prepareRewardVideo(options, successCallback, failureCallback) {
  exec(successCallback, failureCallback, 'AdMob', 'createRewardVideo', [translateOptions(options)])
}

export function showRewardVideo(successCallback, failureCallback) {
  exec(successCallback, failureCallback, 'AdMob', 'showRewardVideo', [true])
}

// Old APIs

export const createBannerView = (...args) => {
  console.warn('Use prepareBanner() instead.')
  prepareBanner(...args)
}

export function destroyBannerView(options = {}, successCallback, failureCallback) {
  console.warn('Use removeBanner() instead.')
  removeBanner(successCallback, failureCallback)
}

export function createInterstitialView(options, successCallback, failureCallback) {
  console.warn('Use prepareInterstitial() instead, it will do both createInterstitialView() and requestInterstitialAd().')
  exec(successCallback, failureCallback, 'AdMob', 'createInterstitialView', [translateOptions(options)])
}

export function requestInterstitialAd(options = {}, successCallback, failureCallback) {
  console.warn('Use prepareInterstitial() instead, it will do both createInterstitialView() and requestInterstitialAd().')
  exec(successCallback, failureCallback, 'AdMob', 'requestInterstitialAd', [translateOptions(options)])
}

export function showAd(show = true, successCallback, failureCallback) {
  console.warn('Use showBanner() and hideBanner() instead.')
  exec(successCallback, failureCallback, 'AdMob', 'showAd', [show])
}

export function showInterstitialAd(show = true, successCallback, failureCallback) {
  console.warn('Use showInterstitial() instead.')
  exec(successCallback, failureCallback, 'AdMob', 'showInterstitialAd', [show])
}
