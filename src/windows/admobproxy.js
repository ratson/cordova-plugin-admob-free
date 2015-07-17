
module.exports = {
	_loadedBannerAd: false,
	_loadedFullScreenAd: false,
	_isShowingBannerAd: false,
	_isShowingFullScreenAd: false,
	//
	setLicenseKey: function(successCallback, errorCallback, args) {
	    var email = args[0];
	    var licenseKey = args[1];	
		Com.Cranberrygame.Cordova.Plugin.Ad.AdMob.AdMobPlugin.setLicenseKey(email, licenseKey);		        
    },
	setUp: function(successCallback, errorCallback, args) {
		var adUnit = args[0];
		var fullScreenAdUnit = args[1];
		var isOverlap = args[2];
		var isTest = args[3];
		Com.Cranberrygame.Cordova.Plugin.Ad.AdMob.AdMobPlugin.setUp(adUnit, fullScreenAdUnit, isOverlap, isTest);
/*
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
			}
		}
		else {
			//if (result["event"] == "onXXX") {
			//	//result["message"]
			//	if (self.onXXX)
			//		self.onXXX(result);
			//}
		}	
*/	
    },
	preloadBannerAd: function(successCallback, errorCallback, args) {
		Com.Cranberrygame.Cordova.Plugin.Ad.AdMob.AdMobPlugin.preloadBannerAd(); 
    },
    showBannerAd: function(successCallback, errorCallback, args) {
	    var position = args[0];
	    var size = args[1];
		Com.Cranberrygame.Cordova.Plugin.Ad.AdMob.AdMobPlugin.showBannerAd(position, size);
    },
	reloadBannerAd: function(successCallback, errorCallback, args) {
		Com.Cranberrygame.Cordova.Plugin.Ad.AdMob.AdMobPlugin.reloadBannerAd();
    },
    hideBannerAd: function(successCallback, errorCallback, args) {
		Com.Cranberrygame.Cordova.Plugin.Ad.AdMob.AdMobPlugin.hideBannerAd();
    },
	//
	preloadFullScreenAd: function(successCallback, errorCallback, args) {
		Com.Cranberrygame.Cordova.Plugin.Ad.AdMob.AdMobPlugin.preloadFullScreenAd();
    },
    showFullScreenAd: function(successCallback, errorCallback, args) {
		Com.Cranberrygame.Cordova.Plugin.Ad.AdMob.AdMobPlugin.showFullScreenAd(); 
    },
/*	
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
*/	
	onBannerAdPreloaded: null,
	onBannerAdLoaded: null,
	onBannerAdShown: null,
	onBannerAdHidden: null,	
	//
	onFullScreenAdPreloaded: null,
	onFullScreenAdLoaded: null,
	onFullScreenAdShown: null,
	onFullScreenAdHidden: null	
};

//require("cordova/exec/proxy").add("AdMobPlugin", module.exports);
cordova.commandProxy.add("AdMobPlugin", module.exports);
