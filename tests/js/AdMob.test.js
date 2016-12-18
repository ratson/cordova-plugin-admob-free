import _ from 'lodash'

const mockFn = jest.fn()
jest.mock('cordova/exec', () => mockFn)

const success = () => {}
const error = () => {}
const options = {adId: 'abc'}

const admob = require('../../src/js/AdMob')

test('AD_SIZE keys match values', () => {
  _.each(admob.AD_SIZE, (v, k) => {
    expect(k).toBe(v)
  })
})

test('setOptions() call correct native method', () => {
  admob.setOptions(options, success, error)
  expect(mockFn).toBeCalledWith(success, error, 'AdMob', 'setOptions', [options])
})

describe('Banner', () => {
  it('prepareBanner() call correct native method', () => {
    admob.prepareBanner(options, success, error)
    expect(mockFn).toBeCalledWith(success, error, 'AdMob', 'createBannerView', [options])
  })

  it('removeBanner() call correct native method', () => {
    admob.removeBanner(success, error)
    expect(mockFn).toBeCalledWith(success, error, 'AdMob', 'destroyBannerView', [])
  })

  it('showBanner() call correct native method', () => {
    admob.showBanner(success, error)
    expect(mockFn).toBeCalledWith(success, error, 'AdMob', 'showAd', [true])
  })

  it('hideBanner() call correct native method', () => {
    admob.hideBanner(success, error)
    expect(mockFn).toBeCalledWith(success, error, 'AdMob', 'showAd', [false])
  })
})

describe('Interstitial', () => {
  it('showInterstitial() call correct native method', () => {
    admob.showInterstitial(success, error)
    expect(mockFn).toBeCalledWith(success, error, 'AdMob', 'showInterstitialAd', [true])
  })
})

describe('Reward Video', () => {
  it('prepareRewardVideo() call correct native method', () => {
    admob.prepareRewardVideo(options, success, error)
    expect(mockFn).toBeCalledWith(success, error, 'AdMob', 'createRewardVideo', [options])
  })

  it('showRewardVideo() call correct native method', () => {
    admob.showRewardVideo(success, error)
    expect(mockFn).toBeCalledWith(success, error, 'AdMob', 'showRewardVideo', [true])
  })
})

describe('Old APIs', () => {
  it('createBannerView() should work', () => {
    admob.createBannerView(options, success, error)
    expect(mockFn).toBeCalledWith(success, error, 'AdMob', 'createBannerView', [options])
  })

  it('destroyBannerView() should work', () => {
    admob.destroyBannerView(options, success, error)
    expect(mockFn).toBeCalledWith(success, error, 'AdMob', 'destroyBannerView', [])
  })

  it('showAd() should work', () => {
    admob.showAd(false, success, error)
    expect(mockFn).toBeCalledWith(success, error, 'AdMob', 'showAd', [false])
    admob.showAd(true, success, error)
    expect(mockFn).toBeCalledWith(success, error, 'AdMob', 'showAd', [true])
  })

  it('showInterstitialAd() should work', () => {
    admob.showInterstitialAd(false, success, error)
    expect(mockFn).toBeCalledWith(success, error, 'AdMob', 'showInterstitialAd', [false])
    admob.showInterstitialAd(true, success, error)
    expect(mockFn).toBeCalledWith(success, error, 'AdMob', 'showInterstitialAd', [true])
  })
})
