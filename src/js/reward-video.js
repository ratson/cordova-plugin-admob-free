import {buildEvents, exec, translateOptions} from './utils'

/**
 * Reward Video config object.
 * @typedef {Object} RewardVideoConfig
 * @property {string} [id=TESTING_AD_ID] - Ad Unit ID
 * @property {boolean} [isTesting=false] - receiving test ad
 * @property {boolean} [autoShow=false] - auto show ad when loaded
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
   * @param {RewardVideoConfig} opts - initial config.
   */
  constructor(opts) {
    this.config({
      ...opts,
    })
  }

  /**
   * Update config.
   * @param {RewardVideoConfig} opts - new config.
   * @return {RewardVideoConfig} updated config.
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
      rewardVideoId: this._config.id,
      ...this._config,
    }
    delete options.id
    return exec('createRewardVideo', [translateOptions(options)])
  }

  /**
   * @return {Promise}
   */
  show() {
    return exec('showRewardVideo', [true])
  }

  /**
   * @return {Promise}
   */
  isReady() {
    return exec('isRewardVideoReady', [])
  }
}

export {RewardVideo}
