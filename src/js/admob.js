import exec from 'cordova/exec'

import { wrapCallbacks, translateOptions } from './utils'

import { Banner } from './banner'
import { Interstitial } from './interstitial'
import { RewardVideo } from './reward-video'

/**
 * @type {Banner}
 * @since 0.6
 * @emits {admob.banner.events.LOAD}
 * @emits {admob.banner.events.LOAD_FAIL}
 * @emits {admob.banner.events.OPEN}
 * @emits {admob.banner.events.CLOSE}
 * @emits {admob.banner.events.EXIT_APP}
 * @example
 * admob.banner.config({
 *  id: 'ca-app-pub-xxx/xxx',
 * })
 *
 * // Create banner
 * admob.banner.prepare()
 *
 * // Show the banner
 * admob.banner.show()
 *
 * // Hide the banner
 * admob.banner.hide()
 *
 * // Remove the banner
 * admob.banner.remove()
 */
export const banner = new Banner()

/**
 * @type {Interstitial}
 * @since 0.6
 * @emits {admob.interstitial.events.LOAD}
 * @emits {admob.interstitial.events.LOAD_FAIL}
 * @emits {admob.interstitial.events.OPEN}
 * @emits {admob.interstitial.events.CLOSE}
 * @emits {admob.interstitial.events.EXIT_APP}
 * @example
 * admob.interstitial.config({
 *  id: 'ca-app-pub-xxx/xxx',
 * })
 *
 * admob.interstitial.prepare()
 *
 * admob.interstitial.show()
 */
export const interstitial = new Interstitial()

/**
 * @external {cordova-admob-mediation} https://github.com/rehy/cordova-admob-mediation
 */
/**
 * Reward video requires mediation SDK to be installed,
 * see {@link cordova-admob-mediation} for list of supported networks.
 * @type {RewardVideo}
 * @since 0.6
 * @emits {admob.rewardvideo.events.LOAD}
 * @emits {admob.rewardvideo.events.LOAD_FAIL}
 * @emits {admob.rewardvideo.events.OPEN}
 * @emits {admob.rewardvideo.events.CLOSE}
 * @emits {admob.rewardvideo.events.EXIT_APP}
 * @emits {admob.rewardvideo.events.START}
 * @emits {admob.rewardvideo.events.REWARD}
 * @example
 * admob.rewardvideo.config({
 *  id: 'ca-app-pub-xxx/xxx',
 * })
 *
 * admob.rewardvideo.prepare()
 *
 * admob.rewardvideo.show()
 */
export const rewardvideo = new RewardVideo()

// Old APIs

/**
 * Set options.
 *
 * @deprecated since version 0.6
 * @param {Object} options
 * @param {string} options.publisherId
 * @param {string} options.interstitialAdId
 *
 * @param {boolean} [options.bannerAtTop=false]
 * Set to true, to put banner at top.
 * @param {boolean} [options.overlap=true]
 * Set to true, to allow banner overlap webview.
 * @param {boolean} [options.offsetTopBar=false]
 * Set to true to avoid ios7 status bar overlap.
 * @param {boolean} [options.isTesting=false]    Receiving test ad.
 * @param {boolean} [options.autoShow=true]
 * Auto show interstitial ad when loaded.
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
    Object.keys(options).forEach(k => {
      switch (k) {
        case 'publisherId':
          banner._config.id = options[k]
          break
        case 'bannerAtTop':
        case 'overlap':
        case 'offsetTopBar':
          banner._config[k] = options[k]
          break
        case 'interstitialAdId':
          interstitial._config.id = options[k]
          break
        case 'rewardVideoId':
          rewardvideo._config.id = options[k]
          break
        case 'isTesting':
        case 'autoShow':
          banner._config[k] = options[k]
          interstitial._config[k] = options[k]
          rewardvideo._config[k] = options[k]
          break
        default:
      }
    })
    exec(successCallback, failureCallback, 'AdMob', 'setOptions', [
      translateOptions(options),
    ])
  } else if (typeof failureCallback === 'function') {
    failureCallback('options should be specified.')
  }
}

/**
 * Ad sizes.
 * @constant
 * @type {BANNER_SIZE}
 * @deprecated since version 0.6
 */
export const AD_SIZE = Banner.sizes

/* eslint-disable no-console */
/**
 * @deprecated since version 0.6
 */
export function createBannerView(
  options = {},
  successCallback,
  failureCallback,
) {
  console.warn('Use admob.banner.prepare() instead.')
  exec(successCallback, failureCallback, 'AdMob', 'createBannerView', [
    translateOptions(options),
  ])
}

/**
 * @deprecated since version 0.6
 */
export function destroyBannerView(options, successCallback, failureCallback) {
  console.warn('Use admob.banner.remove() instead.')
  exec(successCallback, failureCallback, 'AdMob', 'destroyBannerView', [])
}

/**
 * @deprecated since version 0.6
 */
export function showAd(show = true, successCallback, failureCallback) {
  console.warn('Use admob.banner.show() and admob.banner.hide() instead.')
  exec(successCallback, failureCallback, 'AdMob', 'showAd', [show])
}

/**
 * @deprecated since version 0.6
 */
export function createInterstitialView(
  options,
  successCallback,
  failureCallback,
) {
  console.warn(
    'Use admob.interstitial.prepare() instead, it will do both createInterstitialView() and requestInterstitialAd().',
  )
  exec(successCallback, failureCallback, 'AdMob', 'createInterstitialView', [
    translateOptions(options),
  ])
}

/**
 * @deprecated since version 0.6
 */
export function requestInterstitialAd(
  options = {},
  successCallback,
  failureCallback,
) {
  console.warn(
    'Use admob.interstitial.prepare() instead, it will do both createInterstitialView() and requestInterstitialAd().',
  )
  exec(successCallback, failureCallback, 'AdMob', 'requestInterstitialAd', [
    translateOptions(options),
  ])
}

/**
 * @deprecated since version 0.6
 */
export function prepareInterstitial(
  options = {},
  successCallback,
  failureCallback,
) {
  console.warn('Use admob.interstitial.prepare() instead.')
  exec(successCallback, failureCallback, 'AdMob', 'prepareInterstitial', [
    translateOptions(options),
  ])
}

/**
 * @deprecated since version 0.6
 */
export function showInterstitial(successCallback, failureCallback) {
  console.warn('Use admob.interstitial.show() instead.')
  wrapCallbacks(interstitial.show(), successCallback, failureCallback)
}

/**
 * @deprecated since version 0.6
 */
export function showInterstitialAd(
  show = true,
  successCallback,
  failureCallback,
) {
  console.warn('Use admob.interstitial.show() instead.')
  exec(successCallback, failureCallback, 'AdMob', 'showInterstitialAd', [show])
}
/* eslint-enable no-console */
