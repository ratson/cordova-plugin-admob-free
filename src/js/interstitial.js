import {buildEvents, exec, translateOptions} from './utils'

/**
 * Interstitial config object.
 * @typedef {Object} InterstitialConfig
 * @property {string} [id=TESTING_AD_ID] - Ad Unit ID
 * @property {boolean} [isTesting=false] - receiving test ad
 * @property {boolean} [autoShow=false] - auto show ad when loaded
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
   * @param {InterstitialConfig} opts - initial config.
   */
  constructor(opts) {
    this.config({
      ...opts,
    })
  }

  /**
   * Update config.
   * @param {InterstitialConfig} opts - new config.
   * @return {InterstitialConfig} updated config.
   */
  config(opts) {
    this._config = {
      ...this._config,
      ...opts,
    }
    return this._config
  }

  /**
   * @return {Promise}
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
   * @return {Promise}
   */
  show() {
    return exec('showInterstitialAd', [true])
  }

  /**
   * @return {Promise}
   */
  isReady() {
    return exec('isInterstitialReady', [])
  }
}

export {Interstitial}
