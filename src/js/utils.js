import corodvaExec from 'cordova/exec'
import isUndefined from 'lodash/isUndefined'

/**
 * Base config object.
 * @typedef {Object} BaseConfig
 * @property {string} [id=TESTING_AD_ID] - Ad Unit ID
 * @property {boolean} [isTesting=false] - receiving test ad
 * @property {boolean} [autoShow=false] - auto show ad when loaded
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
    p = p.then(successCallback)
  }
  if (isFunction(failureCallback)) {
    p = p.catch(failureCallback)
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
      console.warn('`forChild` will not accept string in future, pass boolean instead')
    }
  }
  if (!isUndefined(options.forFamily)) {
    opts.forFamily = boolean2string(options.forFamily)
    if (isString(options.forFamily)) {
      console.warn('`forFamily` will not accept string in future, pass boolean instead')
    }
  }
  return {
    ...options,
    ...opts,
  }
}

/**
 * @ignore
 */
export function buildEvents(adType, eventKeys) {
  return eventKeys.reduce((r, eventKey) => {
    r[eventKey] = `admob.${adType}.events.${eventKey}`
    return r
  }, {})
}
