## INSTRUCTIONS

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

## CODING DETAILS

- Add the following javascript functions and call them from onDeviceReady()
```javascript

	function initAd(){

	    if ( window.plugins && window.plugins.AdMob ) {
	        var ad_units = {
	            ios : {
	                banner: 'ca-app-pub-4789158063632032/7680949608',
	                interstitial: 'ca-app-pub-4789158063632032/4587882405'
	            },
	            android : {
	                banner: 'ca-app-pub-4789158063632032/7680949608',
	                interstitial: 'ca-app-pub-4789158063632032/4587882405'
	            }
	        };
	        var admobid = ( /(android)/i.test(navigator.userAgent) ) ? ad_units.android : ad_units.ios;
	        window.plugins.AdMob.setOptions( {
	            publisherId: admobid.banner,
	            interstitialAdId: admobid.interstitial,
	            bannerAtTop: false, // set to true, to put banner at top
	            overlap: false, // set to true, to allow banner overlap webview
	            offsetTopBar: false, // set to true to avoid ios7 status bar overlap
	            isTesting: false, // receiving test ad
	            autoShow: false // auto show interstitial ad when loaded
	        });
	    } else {
	        //alert( 'admob plugin not ready' );
	    }
	}

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
- Add the following 2 functions and call them when you want ads
```javascript

	function showAdsFunc(){
	
		//alert("show ads");
		window.plugins.AdMob.createBannerView();
	}

	function showInterstitialFunc(){
	
	    //alert("interstitial");
	    window.plugins.AdMob.createInterstitialView();      
	    window.plugins.AdMob.requestInterstitialAd();	//don't need this line if autoshow is true
	}

```

- If not using autoshow, then need the following, or something like it when you want to display the interstitial
```javascript
    document.addEventListener('onReceiveInterstitialAd', function(){window.plugins.AdMob.showInterstitialAd() });
```
- To close the banner
```javascript
    window.plugins.AdMob.destroyBannerView();
```

## ECLIPSE STUFF

- Import your cordova project into eclipse, make sure you import the folder 'platforms/android', not the base folder of the project.

- Copy the google-play-services.jar into the libs folder.

- Add the following line to the manifest file, just before the ending application tag
```javascript
	<meta-data android:name="com.google.android.gms.version" android:value="8487000" />
```
- If your play services is a different version, then use the right value above. The console will warn you when you try run it if it's wrong. 
   
## ANDROID STUDIO STUFF

- Import your Cordova project with File->new->import project

- Make sure you import the folder 'platforms/android', not the base folder of the project.

- Now you have to launch the sdk manager and download and install the following files located under "extras" (if you don't have them already): 
	Android support repository, Google play services, Google repository.

- Restart android studio and open the build gradle file. You must modify your build.gradle file to look like this under dependencies:

	dependencies {
	    compile 'com.google.android.gms:play-services:8.4.0'
	 }
 
- If you use a different version of play services, put the correct version.

- And finally syncronise your project (the button to the left of the AVD manager).




Disclaimer: This code is free to use how you want as it's opensource. Two percent of the ad requests go to the developers.
