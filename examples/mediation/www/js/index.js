document.addEventListener('deviceready', function() {
  admob.rewardvideo.config({
    // id: '<FILL_THIS>',
    isTesting: true,
    autoShow: true,
  })
  admob.rewardvideo.prepare()

  document.getElementById('showAd').disabled = true
  document.getElementById('showAd').onclick = function() {
    admob.rewardvideo.show()
  }
}, false)

document.addEventListener('admob.rewardvideo.events.LOAD_FAIL', function(event) {
  console.log(event)
})

document.addEventListener('admob.rewardvideo.events.LOAD', function(event) {
  console.log(event)
  document.getElementById('showAd').disabled = false
})

document.addEventListener('admob.rewardvideo.events.CLOSE', function(event) {
  console.log(event)

  admob.rewardvideo.prepare()
})
