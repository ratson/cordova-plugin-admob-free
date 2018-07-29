import corodvaExec from 'cordova/exec'
import isUndefined from 'lodash/isUndefined'

/**
 * Base config object.
 * @typedef {Object} BaseConfig
 * @property {string} [id=TESTING_AD_ID] - Ad Unit ID
 * @property {boolean} [isTesting=false] - Receiving test ad
 * @property {boolean} [autoShow=true] - Auto show ad when loaded
 *
 * @property {boolean|null} [forChild=null]
 * Child-directed setting.
 * Default is not calling `tagForChildDirectedTreatment`.
 * Set to `true` for `tagForChildDirectedTreatment(true)`.
 * Set to `false` for `tagForChildDirectedTreatment(false)`.
 * @see https://firebase.google.com/docs/admob/android/targeting#child-directed_setting
 *
 * @property {boolean|null} [forFamily=null]
 * Designed for Families setting.
 * Android-only.
 * Default is not calling `setIsDesignedForFamilies`.
 * Set to `true` for `setIsDesignedForFamilies(true)`.
 * Set to `false` for `setIsDesignedForFamilies(false)`.
 * @see https://firebase.google.com/docs/admob/android/targeting#designed_for_families_setting
 *
 * @property {Array<number>|null} [location=null]
 * Location targeting.
 * It accept an array in the form of `[latitude, longitude]`.
 * Android-only.
 * Default is not calling `setLatitude` and `setLongitude`.
 */

/**
 * @ignore
 */
export function exec(method, args) {
  return new Promise((resolve, reject) => {
    corodvaExec(resolve, reject, 'AdMob', method, args)
  })
}

/**
 * @ignore
 */
export function isFunction(x) {
  return typeof x === 'function'
}

function isString(x) {
  return typeof x === 'string'
}

/**
 * @ignore
 */
export function wrapCallbacks(p, successCallback, failureCallback) {
  if (isFunction(successCallback)) {
    p = p.then(successCallback) // eslint-disable-line no-param-reassign
  }
  if (isFunction(failureCallback)) {
    p = p.catch(failureCallback) // eslint-disable-line no-param-reassign
  }
  return p
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

/**
 * @ignore
 */
export function translateOptions(options) {
  /* eslint-disable no-console */
  const opts = {}
  if (!isUndefined(options.forChild)) {
    opts.forChild = boolean2string(options.forChild)
    if (isString(options.forChild)) {
      console.warn(
        '`forChild` will not accept string in future, pass boolean instead',
      )
    }
  }
  if (!isUndefined(options.forFamily)) {
    opts.forFamily = boolean2string(options.forFamily)
    if (isString(options.forFamily)) {
      console.warn(
        '`forFamily` will not accept string in future, pass boolean instead',
      )
    }
  }
  /* eslint-enable no-console */
  return {
    ...options,
    ...opts,
    // TODO update native implementation using `size`
    adSize: options.size,
  }
}

/**
 * @ignore
 */
export function buildEvents(adType, eventKeys) {
  return eventKeys.reduce((acc, eventKey) => {
    acc[eventKey] = `admob.${adType}.events.${eventKey}`
    return acc
  }, {})
}
