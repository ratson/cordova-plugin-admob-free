Cordova AdMob plugin
====================

# Overview #
Show admob banner and full screen ad

[android, ios, wp8] [cordova cli] [xdk]

Requires admob account http://www.google.com/ads/admob/

Fix Admob SDK FPS issue: go to your AdMob account and disable text banners, leaving enabled only image banners. (provided by Cipriux)
(not both, but one of them)

This is open source cordova plugin.

You can see Plugins For Cordova in one page: http://cranberrygame.github.io?referrer=github

# Change log #
```c
2014.9.17
	supports SKYSCRAPER size (120x600, Tablets, ipad only)
	Added additional example (example/banner_position_size/index.html)
2014.9.18
	supports isTest
	supports other position: 'top-left', 'top-right', 'left', 'center', 'right', 'bottom-left', 'bottom-right' on android, wp8.
	supports SMART_BANNER resize when orientation changes on android.
2014.9.20
	supports isOverlap on android, ios, wp8
2014.9.24
	supports banner ad callback (onBannerAdLoaded)
	supports full screen ad callback (onFullScreenAdLoaded)
2014.10.3
	supports banner ad callback (onBannerAdPreloaded)
	supports full screen ad callback (onFullScreenAdPreloaded)
1.0.31
	Updated Admob SDK
		iOS 6.12.2
1.0.39
	Moved package name from com.cranberrygame.phonegap.plugin.ad.admob to com.cranberrygame.cordova.plugin.ad.admob
	Updated Admob SDK
		android com.google.playservices@21.0.0
		iOS GoogleMobileAdsSdkiOS-7.1.0
1.0.53
	Added isShowingBannerAd, isShowingFullScreenAd
	
To-Do:

	supports ios split mode
	supports wp8 split mode
	supports other position: 'top-left', 'top-right', 'left', 'center', 'right', 'bottom-left', 'bottom-right' on ios.
	supports banner reposition when orientation changes on ios.
	supports SMART_BANNER resize when orientation changes on ios.	
```
# Install plugin #

## Cordova cli ##
```c
cordova plugin add com.cranberrygame.cordova.plugin.ad.admob

cf)apple app store meta data

xport Compliance
	Have you added or made changes to encryption features since your last submission of this app? (No)

Advertising Identifier
	Does this app use the Advertising Identifier (IDFA)? (Yes)
	
	This app uses the Advertising Identifier to (select all that apply):
		Serve advertisements within the app (check)
	
	Limit Ad Tracking setting in iOS (check)	
	
Previous Purchase Restrictions
	Are you updating this app because of a significant usability issue or for a legal issue, such as an infringement claim? (No)
```

## Xdk ##
```c
XDK PORJECTS - your_xdk_project - CORDOVA 3.X HYBRID MOBILE APP SETTINGS - PLUGINS AND PERMISSIONS - Third Party Plugins - Add a Third Party Plugin - Get Plugin from the Web -

Name: admob
Plugin ID: com.cranberrygame.cordova.plugin.ad.admob
[v] Plugin is located in the Apache Cordova Plugins Registry

cf)apple app store meta data

xport Compliance
	Have you added or made changes to encryption features since your last submission of this app? (No)

Advertising Identifier
	Does this app use the Advertising Identifier (IDFA)? (Yes)
	
	This app uses the Advertising Identifier to (select all that apply):
		Serve advertisements within the app (check)
	
	Limit Ad Tracking setting in iOS (check)	
	
Previous Purchase Restrictions
	Are you updating this app because of a significant usability issue or for a legal issue, such as an infringement claim? (No)
```

## Phonegap build service (config.xml) ##
```c
<gap:plugin name="com.cranberrygame.cordova.plugin.ad.admob" source="plugins.cordova.io" />
```

## Construct2 ##
Download construct2 plugin: https://dl.dropboxusercontent.com/u/186681453/pluginsforcordova/admob/construct2.html
<br>
Now all the native plugins are installed automatically: https://plus.google.com/102658703990850475314/posts/XS5jjEApJYV
# Server setting #
```c
```
<img src="https://github.com/cranberrygame/cordova-plugin-ad-admob/blob/master/doc/ad_unit1.png"><br>
<img src="https://github.com/cranberrygame/cordova-plugin-ad-admob/blob/master/doc/ad_unit2.png"><br>
<img src="https://github.com/cranberrygame/cordova-plugin-ad-admob/blob/master/doc/ad_unit3.png"><br>
<img src="https://github.com/cranberrygame/cordova-plugin-ad-admob/blob/master/doc/ad_unit4.png"><br>
<img src="https://github.com/cranberrygame/cordova-plugin-ad-admob/blob/master/doc/ad_unit5.png"><br>
<img src="https://github.com/cranberrygame/cordova-plugin-ad-admob/blob/master/doc/ad_unit6.png"><br>
<img src="https://github.com/cranberrygame/cordova-plugin-ad-admob/blob/master/doc/ad_unit7.png"><br>
<img src="https://github.com/cranberrygame/cordova-plugin-ad-admob/blob/master/doc/ad_unit8.png"><br>

cf)submit for review for ios

Export Compliance
	Have you added or made changes to encryption features since your last submission of this app? (No)

콘텐츠 권한
	App에 타사 콘텐츠가 포함 또는 표시되거나, App에서 타사 콘텐츠에 액세스할 수 있습니까? (No)

Advertising Identifier
	Does this app use the Advertising Identifier (IDFA)? (Yes)
	
	This app uses the Advertising Identifier to (select all that apply):
		Serve advertisements within the app (check)
	
	Limit Ad Tracking setting in iOS (check)
	
# API #
```javascript
var adUnit = "REPLACE_THIS_WITH_YOUR_AD_UNIT";
var adUnitFullScreen = "REPLACE_THIS_WITH_YOUR_AD_UNIT";
var isOverlap = true; //true: overlap, false: split
var isTest = true;

/*
var adUnit;
var adUnitFullScreen;
var isOverlap = true; //true: overlap, false: split
var isTest = true;
//android
if (navigator.userAgent.match(/Android/i)) {
	adUnit = "REPLACE_THIS_WITH_YOUR_ANDROID_AD_UNIT";
	adUnitFullScreen = "REPLACE_THIS_WITH_YOUR_ANDROID_AD_UNIT";
}
//ios
else if (navigator.userAgent.match(/iPhone/i) || navigator.userAgent.match(/iPad/i)) {
	adUnit = "REPLACE_THIS_WITH_YOUR_IOS_AD_UNIT";
	adUnitFullScreen = "REPLACE_THIS_WITH_YOUR_IOS_AD_UNIT";
}
//wp8
else if( navigator.userAgent.match(/Windows Phone/i) ) {
	adUnit = "REPLACE_THIS_WITH_YOUR_WP8_AD_UNIT";
	adUnitFullScreen = "REPLACE_THIS_WITH_YOUR_WP8_AD_UNIT";
}
*/

document.addEventListener("deviceready", function(){
	//if no license key, 2% ad traffic share for dev support.
	//you can get free license key from https://play.google.com/store/apps/details?id=com.cranberrygame.pluginsforcordova
	//window.admob.setLicenseKey("yourEmailId@yourEmaildDamin.com", "yourFreeLicenseKey");

	window.admob.setUp(adUnit, adUnitFullScreen, isOverlap, isTest);

	//
	window.admob.onBannerAdPreloaded = function() {
		alert('onBannerAdPreloaded');
	};
	window.admob.onBannerAdLoaded = function() {
		alert('onBannerAdLoaded');
	};
	//
	window.admob.onFullScreenAdPreloaded = function() {
		alert('onFullScreenAdPreloaded');
	};
	window.admob.onFullScreenAdLoaded = function() {
		alert('onFullScreenAdLoaded');
	};
	window.admob.onFullScreenAdShown = function() {
		alert('onFullScreenAdShown');
	};
	window.admob.onFullScreenAdHidden = function() {
		alert('onFullScreenAdHidden');
	};
}, false);

window.admob.preloadBannerAd();
/*
position: 'top-left', 'top-center', 'top-right', 'left', 'center', 'right', 'bottom-left', 'bottom-center', 'bottom-right'
size: 	'BANNER' (320x50, Phones and Tablets)
		'LARGE_BANNER' (320x100, Phones and Tablets)
		'MEDIUM_RECTANGLE' (300x250, Phones and Tablets)
		'FULL_BANNER' (468x60, Tablets)
		'LEADERBOARD' (728x90, Tablets)
		'SKYSCRAPER' (120x600, Tablets, ipad only)
		'SMART_BANNER' (Auto size, Phones and Tablets, recommended)
*/
window.admob.showBannerAd('top-center', 'SMART_BANNER');
window.admob.showBannerAd('bottom-center', 'SMART_BANNER');
window.admob.reloadBannerAd();
window.admob.hideBannerAd();

window.admob.preloadFullScreenAd();
window.admob.showFullScreenAd();

alert(window.admob.isShowingBannerAd());//boolean: true or false
alert(window.admob.isShowingFullScreenAd());//boolean: true or false
```
# Examples #
<a href="https://github.com/cranberrygame/cordova-plugin-ad-admob/blob/master/example/basic/index.html">example/basic/index.html</a><br>
<a href="https://github.com/cranberrygame/cordova-plugin-ad-admob/blob/master/example/advanced/index.html">example/advanced/index.html</a>

# Test #

[![](http://img.youtube.com/vi/xXrVb8E8gMM/0.jpg)](https://www.youtube.com/watch?v=xXrVb8E8gMM&feature=youtu.be "Youtube")

You can also run following test apk.
https://dl.dropboxusercontent.com/u/186681453/pluginsforcordova/admob/apk.html

# Useful links #

Plugins For Cordova<br>
http://cranberrygame.github.io?referrer=github

# Credits #
