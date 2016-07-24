var admobid = {}
if (/(android)/i.test(navigator.userAgent)) {  // for android & amazon-fireos
  admobid = {
    banner: 'ca-app-pub-3940256099942544/6300978111',
    interstitial: 'ca-app-pub-3940256099942544/1033173712',
  }
} else if (/(ipod|iphone|ipad)/i.test(navigator.userAgent)) {  // for ios
  admobid = {
    banner: 'ca-app-pub-3940256099942544/2934735716',
    interstitial: 'ca-app-pub-3940256099942544/4411468910',
  }
}

document.addEventListener('deviceready', function() {
  AdMob.setOptions({
    isTesting: true,
  })

  AdMob.prepareInterstitial({
    adId: admobid.interstitial,
    autoShow: false,
  })

  document.getElementById('showAd').disabled = true
  document.getElementById('showAd').onclick = function() {
    AdMob.showInterstitial()
  }
}, false)

document.addEventListener('onFailedToReceiveAd', function(event) {
  console.log(event)
})

document.addEventListener('onReceiveInterstitialAd', function(event) {
  console.log(event)
  document.getElementById('showAd').disabled = false
})

document.addEventListener('onDismissInterstitialAd', function(event) {
  console.log(event)

  AdMob.prepareInterstitial({
    adId: admobid.interstitial,
    autoShow: false,
  })
})
