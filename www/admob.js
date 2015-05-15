
module.exports = {
	_isShowingBannerAd: false,
	_isShowingFullScreenAd: false,
	//
	setLicenseKey: function(email, licenseKey) {
		var self = this;	
        cordova.exec(
            null,
            null,
            'Admob',
            'setLicenseKey',			
            [email, licenseKey]
        ); 
    },
	setUp: function(adUnit, adUnitFullScreen, isOverlap, isTest) {
		var self = this;	
        cordova.exec(
            function (result) {
				if (typeof result == "string") {
					if (result == "onBannerAdPreloaded") {
						if (self.onBannerAdPreloaded)
							self.onBannerAdPreloaded();
					}
					else if (result == "onBannerAdLoaded") {
						if (self.onBannerAdLoaded)
							self.onBannerAdLoaded();
					}
					//
					if (result == "onFullScreenAdPreloaded") {
						if (self.onFullScreenAdPreloaded)
							self.onFullScreenAdPreloaded();
					}
					else if (result == "onFullScreenAdLoaded") {
						if (self.onFullScreenAdLoaded)
							self.onFullScreenAdLoaded();
					}
					else if (result == "onFullScreenAdShown") {
						self._isShowingFullScreenAd = true;

						if (self.onFullScreenAdShown)
							self.onFullScreenAdShown();
					}
					else if (result == "onFullScreenAdHidden") {
						self._isShowingFullScreenAd = false;
						
						if (self.onFullScreenAdHidden)
							self.onFullScreenAdHidden();
						if (self.onFullScreenAdClosed)
							self.onFullScreenAdClosed(); //deprecated							
					}
				}
				else {
					//if (result["event"] == "onXXX") {
					//	//result["message"]
					//	if (self.onXXX)
					//		self.onXXX(result);
					//}
				}			
			}, 
			function (error) {
			},
            'Admob',
            'setUp',			
            [adUnit, adUnitFullScreen, isOverlap, isTest]
        ); 
    },
	preloadBannerAd: function() {
		var self = this;	
        cordova.exec(
            null,
            null,
            'Admob',
            'preloadBannerAd',
            []
        ); 
    },
    showBannerAd: function(position, size) {
		var self = this;	

		self._isShowingBannerAd = true;
	
        cordova.exec(
            null,
            null,
            'Admob',
            'showBannerAd',
            [position, size]
        ); 
    },
	reloadBannerAd: function() {
		var self = this;	
        cordova.exec(
            null,
            null,
            'Admob',
            'reloadBannerAd',
            []
        ); 
    },
    hideBannerAd: function() {
		var self = this;
		
		self._isShowingBannerAd = false;
		
        cordova.exec(
            null,
            null,
            'Admob',
            'hideBannerAd',
            []
        ); 
    },
	//
	preloadFullScreenAd: function() {
		var self = this;	
        cordova.exec(
            null,
            null,
            'Admob',
            'preloadFullScreenAd',
            []
        ); 
    },
    showFullScreenAd: function() {
		var self = this;
		cordova.exec(
            null,
            null,
            'Admob',
            'showFullScreenAd',
            []
        ); 
    },
	reloadFullScreenAd: function() { //deprecated
    },
	isShowingBannerAd: function() {
		return this._isShowingBannerAd;
	},
	isShowingFullScreenAd: function() {
		return this._isShowingFullScreenAd;
	},	
	onBannerAdPreloaded: null,
	onBannerAdLoaded: null,
	//
	onFullScreenAdPreloaded: null,
	onFullScreenAdLoaded: null,
	onFullScreenAdShown: null,
	onFullScreenAdHidden: null,
	onFullScreenAdClosed: null //deprecated	
};
