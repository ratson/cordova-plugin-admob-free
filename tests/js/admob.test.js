import _ from 'lodash'

const mockFn = jest.fn()
jest.mock('cordova/exec', () => mockFn)

const success = () => {}
const error = () => {}
const options = { isTesting: true }

const admob = require('../../src/js/admob')

test('AD_SIZE keys match values', () => {
  _.each(admob.AD_SIZE, (v, k) => {
    expect(k).toBe(v)
  })
})

test('setOptions() call correct native method', () => {
  admob.setOptions(options, success, error)
  expect(mockFn).toBeCalledWith(success, error, 'AdMob', 'setOptions', [
    options,
  ])
})

describe('Banner', () => {
  it('prepare() call correct native method', () => {
    admob.banner.config(options)
    admob.banner.prepare()
    expect(mockFn).toBeCalledWith(
      expect.any(Function),
      expect.any(Function),
      'AdMob',
      'createBannerView',
      [expect.objectContaining(options)],
    )
  })

  it('remove() call correct native method', () => {
    admob.banner.remove(success, error)
    expect(mockFn).toBeCalledWith(
      expect.any(Function),
      expect.any(Function),
      'AdMob',
      'destroyBannerView',
      [],
    )
  })

  it('show() call correct native method', () => {
    admob.banner.show(success, error)
    expect(mockFn).toBeCalledWith(
      expect.any(Function),
      expect.any(Function),
      'AdMob',
      'showAd',
      [true],
    )
  })

  it('hide() call correct native method', () => {
    admob.banner.hide(success, error)
    expect(mockFn).toBeCalledWith(
      expect.any(Function),
      expect.any(Function),
      'AdMob',
      'showAd',
      [false],
    )
  })
})

describe('Interstitial', () => {
  it('prepare() call correct native method', () => {
    admob.interstitial.prepare(options, success, error)
    expect(mockFn).toBeCalledWith(
      expect.any(Function),
      expect.any(Function),
      'AdMob',
      'prepareInterstitial',
      [options],
    )
  })

  it('show() call correct native method', () => {
    admob.interstitial.show(success, error)
    expect(mockFn).toBeCalledWith(
      expect.any(Function),
      expect.any(Function),
      'AdMob',
      'showInterstitialAd',
      [true],
    )
  })
})

describe('Reward Video', () => {
  it('prepare() call correct native method', () => {
    admob.rewardvideo.prepare(options, success, error)
    expect(mockFn).toBeCalledWith(
      expect.any(Function),
      expect.any(Function),
      'AdMob',
      'createRewardVideo',
      [options],
    )
  })

  it('show() call correct native method', () => {
    admob.rewardvideo.show(success, error)
    expect(mockFn).toBeCalledWith(
      expect.any(Function),
      expect.any(Function),
      'AdMob',
      'showRewardVideo',
      [true],
    )
  })
})

describe('Old APIs', () => {
  it('createBannerView() should work', () => {
    admob.createBannerView(options, success, error)
    expect(mockFn).toBeCalledWith(
      expect.any(Function),
      expect.any(Function),
      'AdMob',
      'createBannerView',
      [expect.objectContaining(options)],
    )
  })

  it('destroyBannerView() should work', () => {
    admob.destroyBannerView(options, success, error)
    expect(mockFn).toBeCalledWith(
      expect.any(Function),
      expect.any(Function),
      'AdMob',
      'destroyBannerView',
      [],
    )
  })

  it('showAd() should work', () => {
    admob.showAd(false, success, error)
    expect(mockFn).toBeCalledWith(success, error, 'AdMob', 'showAd', [false])
    admob.showAd(true, success, error)
    expect(mockFn).toBeCalledWith(success, error, 'AdMob', 'showAd', [true])
  })

  it('prepareInterstitial() should work', () => {
    admob.prepareInterstitial(options, success, error)
    expect(mockFn).toBeCalledWith(
      expect.any(Function),
      expect.any(Function),
      'AdMob',
      'prepareInterstitial',
      [expect.objectContaining(options)],
    )
  })

  it('showInterstitial() should work', () => {
    admob.showInterstitial(success, error)
    expect(mockFn).toBeCalledWith(
      expect.any(Function),
      expect.any(Function),
      'AdMob',
      'showInterstitialAd',
      [true],
    )
  })

  it('showInterstitialAd() should work', () => {
    admob.showInterstitialAd(false, success, error)
    expect(
      mockFn,
    ).toBeCalledWith(success, error, 'AdMob', 'showInterstitialAd', [false])
    admob.showInterstitialAd(true, success, error)
    expect(
      mockFn,
    ).toBeCalledWith(success, error, 'AdMob', 'showInterstitialAd', [true])
  })
})
