
module.exports = {
	_loadedBannerAd: false,
	_loadedFullScreenAd: false,
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
						self._loadedBannerAd = true;
						
						if (self.onBannerAdLoaded)
							self.onBannerAdLoaded();
					}
					else if (result == "onBannerAdShown") {
						self._loadedBannerAd = false;
						self._isShowingBannerAd = true;
					
						if (self.onBannerAdShown)
							self.onBannerAdShown();
					}
					else if (result == "onBannerAdHidden") {
						self._isShowingBannerAd = false;
					
						 if (self.onBannerAdHidden)
							self.onBannerAdHidden();
					}
					//
					else if (result == "onFullScreenAdPreloaded") {
						if (self.onFullScreenAdPreloaded)
							self.onFullScreenAdPreloaded();
					}
					else if (result == "onFullScreenAdLoaded") {
						self._loadedFullScreenAd = true;
						
						if (self.onFullScreenAdLoaded)
							self.onFullScreenAdLoaded();
					}
					else if (result == "onFullScreenAdShown") {
						self._loadedFullScreenAd = false;					
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
	loadedBannerAd: function() {
		return this._loadedBannerAd;
	},
	loadedFullScreenAd: function() {
		return this._loadedFullScreenAd;
	},	
	isShowingBannerAd: function() {
		return this._isShowingBannerAd;
	},
	isShowingFullScreenAd: function() {
		return this._isShowingFullScreenAd;
	},	
	onBannerAdPreloaded: null,
	onBannerAdLoaded: null,
	onBannerAdShown: null,
	onBannerAdHidden: null,	
	//
	onFullScreenAdPreloaded: null,
	onFullScreenAdLoaded: null,
	onFullScreenAdShown: null,
	onFullScreenAdHidden: null,
	onFullScreenAdClosed: null //deprecated	
};
