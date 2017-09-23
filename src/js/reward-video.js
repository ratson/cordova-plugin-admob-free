import { buildEvents, exec, translateOptions } from './utils'

/**
 * Reward Video config object.
 * @typedef {BaseConfig} RewardVideoConfig
 */

const events = buildEvents('rewardvideo', [
  'LOAD',
  'LOAD_FAIL',
  'OPEN',
  'CLOSE',
  'EXIT_APP',
  'START',
  'REWARD',
])

/**
 * See usage in {@link rewardvideo}.
 * @protected
 */
class RewardVideo {
  static events = events

  /**
   * @protected
   * @param {RewardVideoConfig} opts - Initial config.
   */
  constructor(opts) {
    this.config({
      ...opts,
    })
  }

  /**
   * Update config.
   *
   * @param {RewardVideoConfig} opts - New config.
   * @returns {RewardVideoConfig} Updated config.
   */
  config(opts) {
    this._config = {
      ...this._config,
      ...opts,
    }
    return this._config
  }

  /**
   * @returns {Promise} Excutaion result promise.
   */
  prepare() {
    const options = {
      rewardVideoId: this._config.id,
      ...this._config,
    }
    delete options.id
    return exec('createRewardVideo', [translateOptions(options)])
  }

  /**
   * @returns {Promise} Excutaion result promise.
   */
  show() {
    return exec('showRewardVideo', [true])
  }

  /**
   * @returns {Promise} Excutaion result promise.
   */
  isReady() {
    return exec('isRewardVideoReady', [])
  }
}

export { RewardVideo }
