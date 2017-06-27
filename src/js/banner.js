import { buildEvents, exec, translateOptions } from './utils'

/**
 * Banner config object.
 * @typedef {BaseConfig} BannerConfig
 * @property {boolean} [bannerAtTop=false] - set to true, to put banner at top
 * @property {boolean} [overlap=true] - set to true, to allow banner overlap webview
 * @property {boolean} [offsetTopBar=false] - set to true to avoid ios7 status bar overlap
 * @property {string} [size=SMART_BANNER] - {@link BANNER_SIZE}
 */

const events = buildEvents('banner', [
  'LOAD',
  'LOAD_FAIL',
  'OPEN',
  'CLOSE',
  'EXIT_APP',
])

/**
 * @typedef {Object} BANNER_SIZE
 * @property {string} BANNER - BANNER
 * @property {string} IAB_BANNER - IAB_BANNER
 * @property {string} IAB_LEADERBOARD - IAB_LEADERBOARD
 * @property {string} IAB_MRECT - IAB_MRECT
 * @property {string} LARGE_BANNER - LARGE_BANNER
 * @property {string} SMART_BANNER - SMART_BANNER
 * @property {string} FLUID - FLUID
 */
/**
 * @constant
 * @type {Object}
 */
const sizes = {
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

/**
 * @protected
 * @desc
 * See usage in {@link banner}.
 */
class Banner {
  static events = events

  /**
   * Banner sizes
   * @type {BANNER_SIZE}
   */
  static sizes = sizes

  /**
   * @protected
   * @param {BannerConfig} opts - initial config.
   */
  constructor(opts) {
    this.config({
      size: sizes.SMART_BANNER,
      ...opts,
    })
  }

  /**
   * Update config.
   * @param {BannerConfig} opts - new config.
   * @return {BannerConfig} updated config.
   */
  config(opts) {
    this._config = {
      ...this._config,
      ...opts,
    }
    return this._config
  }

  /**
   * Create banner.
   * @return {Promise}
   */
  prepare() {
    const options = {
      publisherId: this._config.id,
      ...this._config,
    }
    delete options.id
    return exec('createBannerView', [translateOptions(options)])
  }

  /**
   * Show the banner.
   * @return {Promise}
   */
  show() {
    return exec('showAd', [true])
  }

  /**
   * Hide the banner.
   * @return {Promise}
   */
  hide() {
    return exec('showAd', [false])
  }

  /**
   * Remove the banner.
   * @return {Promise}
   */
  remove() {
    return exec('destroyBannerView', [])
  }
}

export { Banner }
