import { buildEvents, exec, translateOptions } from './utils'

/**
 * Interstitial config object.
 * @typedef {BaseConfig} InterstitialConfig
 */

const events = buildEvents('interstitial', [
  'LOAD',
  'LOAD_FAIL',
  'OPEN',
  'CLOSE',
  'EXIT_APP',
])

/**
 * @protected
 * @desc
 * See usage in {@link interstitial}.
 */
class Interstitial {
  static events = events

  /**
   * @protected
   * @param {InterstitialConfig} opts - Initial config.
   */
  constructor(opts) {
    this.config({
      ...opts,
    })
  }

  /**
   * Update config.
   *
   * @param {InterstitialConfig} opts - New config.
   * @returns {InterstitialConfig} Updated config.
   */
  config(opts) {
    this._config = {
      ...this._config,
      ...opts,
    }
    return this._config
  }

  /**
   * @returns {Promise}
   */
  prepare() {
    const options = {
      interstitialAdId: this._config.id,
      ...this._config,
    }
    delete options.id
    return exec('prepareInterstitial', [translateOptions(options)])
  }

  /**
   * @returns {Promise}
   */
  show() {
    return exec('showInterstitialAd', [true])
  }

  /**
   * @returns {Promise}
   */
  isReady() {
    return exec('isInterstitialReady', [])
  }
}

export { Interstitial }
