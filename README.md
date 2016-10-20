## Cordova/Phonegap AdMob Plugin

The FASTEST and EASIEST TO USE Cordova Admob plugin for Android, iOS and Windows phone. We do not send your apps details to our servers. Pure OpenSource Admob code. Allows preloading and automatic loading of interstitials and banners plus more. Works with Cordova, Phonegap, Intel XDK/Crosswalk, Ionic, Meteor and more.

## CONTENTS
1. [DESCRIPTION](#description)
2. [QUICK START](#quick-start)
3. [CODING DETAILS - Load interstitial first and show later](#coding-details-load-interstitial-first-and-show-later)
4. [CODING DETAILS - Show interstitial when it's loaded](#coding-details-show-interstitial-when-it-is-loaded)
5. [VIDEO DEMO](#video-demo)
6. [ECLIPSE NOTES](#eclipse-notes)
7. [ANDROID STUDIO NOTES](#android-studio-notes)
8. [CAUTION](#caution)

## DESCRIPTION

A Cordova-Phonegap plugin that allows you to integrate Google Admob ads into your iOS or Android app and display banners or interstitials. Cordova allows you to build cross platform HTML 5 and Javascript apps without having to rewrite in Objective C or Java.

## QUICK START

- Create your app

```bash
cordova create hallo com.example.hallo HalloWorld

cd hallo

cordova platform add android
```

- Add the plugin
```bash
cordova plugin add cordova-plugin-admob-simple
```
OR
```bash
cordova plugin add https://github.com/sunnycupertino/cordova-plugin-admob-simple
```


## CODING DETAILS Load interstitial first and show later

- Add the following javascript functions, put in your own ad code, play with the variables if you want.

- Call initAd() from onDeviceReady()
```javascript
//initialize the goodies
function initAd(){
        if ( window.plugins && window.plugins.AdMob ) {
            var ad_units = {
                ios : {
                    banner: 'ca-app-pub-xxxxxxxxxxx/xxxxxxxxxxx',		//PUT ADMOB ADCODE HERE
                    interstitial: 'ca-app-pub-xxxxxxxxxxx/xxxxxxxxxxx'	//PUT ADMOB ADCODE HERE
                },
                android : {
                    banner: 'ca-app-pub-xxxxxxxxxxx/xxxxxxxxxxx',		//PUT ADMOB ADCODE HERE
                    interstitial: 'ca-app-pub-xxxxxxxxxxx/xxxxxxxxxxx'	//PUT ADMOB ADCODE HERE
                }
            };
            var admobid = ( /(android)/i.test(navigator.userAgent) ) ? ad_units.android : ad_units.ios;

            window.plugins.AdMob.setOptions( {
                publisherId: admobid.banner,
                interstitialAdId: admobid.interstitial,
                adSize: window.plugins.AdMob.AD_SIZE.SMART_BANNER,	//use SMART_BANNER, BANNER, LARGE_BANNER, IAB_MRECT, IAB_BANNER, IAB_LEADERBOARD
                bannerAtTop: false, // set to true, to put banner at top
                overlap: true, // banner will overlap webview 
                offsetTopBar: false, // set to true to avoid ios7 status bar overlap
                isTesting: false, // receiving test ad
                autoShow: false // auto show interstitial ad when loaded
            });

            registerAdEvents();
            window.plugins.AdMob.createInterstitialView();	//get the interstitials ready to be shown
            window.plugins.AdMob.requestInterstitialAd();

        } else {
            //alert( 'admob plugin not ready' );
        }
}
//functions to allow you to know when ads are shown, etc.
function registerAdEvents() {
        document.addEventListener('onReceiveAd', function(){});
        document.addEventListener('onFailedToReceiveAd', function(data){});
        document.addEventListener('onPresentAd', function(){});
        document.addEventListener('onDismissAd', function(){ });
        document.addEventListener('onLeaveToAd', function(){ });
        document.addEventListener('onReceiveInterstitialAd', function(){ });
        document.addEventListener('onPresentInterstitialAd', function(){ });
        document.addEventListener('onDismissInterstitialAd', function(){
        	window.plugins.AdMob.createInterstitialView();			//REMOVE THESE 2 LINES IF USING AUTOSHOW
            window.plugins.AdMob.requestInterstitialAd();			//get the next one ready only after the current one is closed
        });
    }

```
- Add the following 2 functions and call them when you want ads to show
```javascript
//display the banner
function showBannerFunc(){
	window.plugins.AdMob.createBannerView();
}
//display the interstitial
function showInterstitialFunc(){
	window.plugins.AdMob.showInterstitialAd();
}
```
- To close the banner
```javascript
    window.plugins.AdMob.destroyBannerView();
```

## CODING DETAILS Show interstitial when it is loaded

- Add the following javascript functions, put in your own ad code, play with the variables if you want.

- Call initAd() from onDeviceReady()
```javascript
//initialize the goodies
function initAd(){
        if ( window.plugins && window.plugins.AdMob ) {
            var ad_units = {
                ios : {
                    banner: 'ca-app-pub-xxxxxxxxxxx/xxxxxxxxxxx',		//PUT ADMOB ADCODE HERE
                    interstitial: 'ca-app-pub-xxxxxxxxxxx/xxxxxxxxxxx'	//PUT ADMOB ADCODE HERE
                },
                android : {
                    banner: 'ca-app-pub-xxxxxxxxxxx/xxxxxxxxxxx',		//PUT ADMOB ADCODE HERE
                    interstitial: 'ca-app-pub-xxxxxxxxxxx/xxxxxxxxxxx'	//PUT ADMOB ADCODE HERE
                }
            };
            var admobid = ( /(android)/i.test(navigator.userAgent) ) ? ad_units.android : ad_units.ios;

            window.plugins.AdMob.setOptions( {
                publisherId: admobid.banner,
                interstitialAdId: admobid.interstitial,
                adSize: window.plugins.AdMob.AD_SIZE.SMART_BANNER,	//use SMART_BANNER, BANNER, LARGE_BANNER, IAB_MRECT, IAB_BANNER, IAB_LEADERBOARD
                bannerAtTop: false, // set to true, to put banner at top
                overlap: true, // banner will overlap webview
                offsetTopBar: false, // set to true to avoid ios7 status bar overlap
                isTesting: false, // receiving test ad
                autoShow: true // auto show interstitial ad when loaded
            });

            registerAdEvents();
        } else {
            //alert( 'admob plugin not ready' );
        }
}
//functions to allow you to know when ads are shown, etc.
function registerAdEvents() {
        document.addEventListener('onReceiveAd', function(){});
        document.addEventListener('onFailedToReceiveAd', function(data){});
        document.addEventListener('onPresentAd', function(){});
        document.addEventListener('onDismissAd', function(){ });
        document.addEventListener('onLeaveToAd', function(){ });
        document.addEventListener('onReceiveInterstitialAd', function(){ });
        document.addEventListener('onPresentInterstitialAd', function(){ });
        document.addEventListener('onDismissInterstitialAd', function(){ });
    }

```
- Add the following 2 functions and call them when you want ads to show
```javascript
//display the banner
function showBannerFunc(){
	window.plugins.AdMob.createBannerView();
}
//display the interstitial
function showInterstitialFunc(){
	window.plugins.AdMob.createInterstitialView();	//get the interstitials ready to be shown and show when it's loaded.
	window.plugins.AdMob.requestInterstitialAd();
}
```
- To close the banner
```javascript
    window.plugins.AdMob.destroyBannerView();
```

## VIDEO DEMO

* Watch the video below to see a tutorial on how to install the Cordova Admob Plugin with Android Studio:

[![Video](https://github.com/sunnycupertino/cordova-plugin-admob-simple/raw/master/docs/cordova-admob-plugin-video.jpg)](https://youtu.be/esHGZxmXOMg)

## ECLIPSE NOTES

- Import your cordova project into eclipse, make sure you import the folder 'platforms/android', not the base folder of the project.

- Copy the google-play-services.jar into the libs folder.

- Add the following line to the manifest file, just before the ending application tag
```javascript
<meta-data android:name="com.google.android.gms.version" android:value="8487000" />
```
- If your play services is a different version, then use the right value above. The console will warn you when you try run it if it's wrong. 
   
## ANDROID STUDIO NOTES

- Import your Cordova project with File->new->import project

- Make sure you import the folder 'platforms/android' or 'platforms/ios', not the base folder of the project.

- Now you have to launch the sdk manager and download and install the following files located under "extras" (if you don't have them already): 
	Android support repository, Google play services, Google repository.

## CAUTION
- Do not click your own ads or google could cancel all your accounts. They have automatic systems checking for this. For testing use the  'isTesting: true' javascript variable in the code below.

- Do not make apps that allow people to download youtube movies, this is against their terms of service also. They will find out.

- Do not make apps that embed youtube movies in a way that is not allowed.

Credits to Liming Xie

## The MIT License (MIT)

Copyright (c) 2016 Sunny Cupertino,

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
DISCLAIMER. TWO PERCENT OF THE AD REQUESTS GO TO THE DEVS.


![track](http://goo.gl/tkH4rZ)
