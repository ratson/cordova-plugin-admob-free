//Copyright (c) 2014 Sang Ki Kwon (Cranberrygame)
//Email: cranberrygame@yahoo.com
//Homepage: http://www.github.com/cranberrygame
//License: MIT (http://opensource.org/licenses/MIT)
package com.cranberrygame.cordova.plugin.ad.admob;

import org.apache.cordova.CordovaPlugin;
import org.apache.cordova.PluginResult;
import org.apache.cordova.CallbackContext;
import org.json.JSONArray;
import org.json.JSONObject;
import org.json.JSONException;
import org.apache.cordova.CordovaInterface;
import org.apache.cordova.CordovaWebView;
import android.app.Activity;
import android.util.Log;
//
import com.google.android.gms.ads.AdRequest;
import com.google.android.gms.ads.AdSize;
import com.google.android.gms.ads.AdView;
import com.google.android.gms.ads.InterstitialAd;
import com.google.android.gms.ads.AdListener;
import android.view.View;
import android.view.ViewGroup;
import android.widget.RelativeLayout;
import android.os.Build;
import android.provider.Settings;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import android.os.Handler;

interface Plugin {
	public CordovaWebView getWebView();
	public CordovaInterface getCordova();
	public CallbackContext getCallbackContextKeepCallback();
}

interface PluginDelegate {
	public void _setUp(String adUnit, String adUnitFullScreen, boolean isOverlap, boolean isTest);
	public void _setLicenseKey(String email, String licenseKey);
	public void _preloadBannerAd();
	public void _showBannerAd(String position, String size);
	public void _reloadBannerAd();
	public void _hideBannerAd();
	public void _preloadFullScreenAd();
	public void _showFullScreenAd();
    public void onPause(boolean multitasking);
    public void onResume(boolean multitasking);
    public void onDestroy();
}

public class Admob extends CordovaPlugin implements PluginDelegate, Plugin {
	private static final String LOG_TAG = "Admob";	
	protected CallbackContext callbackContextKeepCallback;
	//
	protected PluginDelegate pluginDelegate;
	
    public void initialize(CordovaInterface cordova, CordovaWebView webView) {
		super.initialize(cordova, webView);
    }
	
	@Override
	public boolean execute(String action, JSONArray args, CallbackContext callbackContext) throws JSONException {

		if (action.equals("setUp")) {
			setUp(action, args, callbackContext);

			return true;
		}			
		else if (action.equals("setLicenseKey")) {
			setLicenseKey(action, args, callbackContext);

			return true;
		}			
		else if (action.equals("preloadBannerAd")) {
			preloadBannerAd(action, args, callbackContext);
			
			return true;
		}
		else if (action.equals("showBannerAd")) {
			showBannerAd(action, args, callbackContext);

			return true;
		}
		else if (action.equals("reloadBannerAd")) {
			reloadBannerAd(action, args, callbackContext);
			
			return true;
		}			
		else if (action.equals("hideBannerAd")) {
			hideBannerAd(action, args, callbackContext);
			
			return true;
		}
		else if (action.equals("preloadFullScreenAd")) {
			preloadFullScreenAd(action, args, callbackContext);
			
			return true;
		}
		else if (action.equals("showFullScreenAd")) {
			showFullScreenAd(action, args, callbackContext);
						
			return true;
		}
		
		return false; // Returning false results in a "MethodNotFound" error.
	}
	
	private void setUp(String action, JSONArray args, CallbackContext callbackContext) throws JSONException {
		//Activity activity=cordova.getActivity();
		//webView
		//args.length()
		//args.getString(0)
		//args.getString(1)
		//args.getInt(0)
		//args.getInt(1)
		//args.getBoolean(0)
		//args.getBoolean(1)
		//JSONObject json = args.optJSONObject(0);
		//json.optString("adUnit")
		//json.optString("adUnitFullScreen")
		//JSONObject inJson = json.optJSONObject("inJson");
		//final String adUnit = args.getString(0);
		//final String adUnitFullScreen = args.getString(1);				
		//final boolean isOverlap = args.getBoolean(2);				
		//final boolean isTest = args.getBoolean(3);				
		//Log.d(LOG_TAG, String.format("%s", adUnit));			
		//Log.d(LOG_TAG, String.format("%s", adUnitFullScreen));
		//Log.d(LOG_TAG, String.format("%b", isOverlap));
		//Log.d(LOG_TAG, String.format("%b", isTest));		
		final String adUnit = args.getString(0);
		final String adUnitFullScreen = args.getString(1);				
		final boolean isOverlap = args.getBoolean(2);				
		final boolean isTest = args.getBoolean(3);				
		Log.d(LOG_TAG, String.format("%s", adUnit));			
		Log.d(LOG_TAG, String.format("%s", adUnitFullScreen));
		Log.d(LOG_TAG, String.format("%b", isOverlap));
		Log.d(LOG_TAG, String.format("%b", isTest));
		
		callbackContextKeepCallback = callbackContext;
		
		if(isOverlap)
			pluginDelegate = new AdmobOverlap(this);
		else
			pluginDelegate = new AdmobSplit(this);
		
		cordova.getActivity().runOnUiThread(new Runnable() {
			@Override
			public void run() {
				_setUp(adUnit, adUnitFullScreen, isOverlap, isTest);
			}
		});
	}
	
	private void setLicenseKey(String action, JSONArray args, CallbackContext callbackContext) throws JSONException {
		final String email = args.getString(0);
		final String licenseKey = args.getString(1);				
		Log.d(LOG_TAG, String.format("%s", email));			
		Log.d(LOG_TAG, String.format("%s", licenseKey));
		
		cordova.getActivity().runOnUiThread(new Runnable() {
			@Override
			public void run() {
				_setLicenseKey(email, licenseKey);
			}
		});
	}
	
	private void preloadBannerAd(String action, JSONArray args, CallbackContext callbackContext) throws JSONException {
		cordova.getActivity().runOnUiThread(new Runnable(){
			@Override
			public void run() {
				_preloadBannerAd();
			}
		});
	}

	private void showBannerAd(String action, JSONArray args, CallbackContext callbackContext) throws JSONException {
		final String position = args.getString(0);
		final String size = args.getString(1);
		Log.d(LOG_TAG, String.format("%s", position));
		Log.d(LOG_TAG, String.format("%s", size));
	
		cordova.getActivity().runOnUiThread(new Runnable() {
			@Override
			public void run() {
				_showBannerAd(position, size);
			}
		});
	}

	private void reloadBannerAd(String action, JSONArray args, CallbackContext callbackContext) throws JSONException {
		cordova.getActivity().runOnUiThread(new Runnable(){
			@Override
			public void run() {
				_reloadBannerAd();
			}
		});
	}
	
	private void hideBannerAd(String action, JSONArray args, CallbackContext callbackContext) throws JSONException {
		cordova.getActivity().runOnUiThread(new Runnable(){
			@Override
			public void run() {
				_hideBannerAd();
			}
		});
	}

	private void preloadFullScreenAd(String action, JSONArray args, CallbackContext callbackContext) throws JSONException {
		cordova.getActivity().runOnUiThread(new Runnable(){
			@Override
			public void run() {
				_preloadFullScreenAd();
			}
		});
	}

	private void showFullScreenAd(String action, JSONArray args, CallbackContext callbackContext) throws JSONException {
		cordova.getActivity().runOnUiThread(new Runnable(){
			@Override
			public void run() {
				_showFullScreenAd();
			}
		});
	}
	
	//cranberrygame start: Plugin

	public CordovaWebView getWebView() {
		return webView;
	}

	public CordovaInterface getCordova() {
		return cordova;
	}

	public CallbackContext getCallbackContextKeepCallback() {
		return callbackContextKeepCallback;
	}

	//cranberrygame end: Plugin
	
	//cranberrygame start: AdmobPluginDelegate
	
	public void _setUp(String adUnit, String adUnitFullScreen, boolean isOverlap, boolean isTest) {
		pluginDelegate._setUp(adUnit, adUnitFullScreen, isOverlap, isTest);
	}

	public void _setLicenseKey(String email, String licenseKey) {
		pluginDelegate._setLicenseKey(email, licenseKey);
	}
	
	public void _preloadBannerAd() {
		pluginDelegate._preloadBannerAd();           	
	}
		
	public void _showBannerAd(String position, String size) {
		pluginDelegate._showBannerAd(position, size);		
	}
	
	public void _reloadBannerAd() {
		pluginDelegate._reloadBannerAd();
	}
	
	public void _hideBannerAd() {
		pluginDelegate._hideBannerAd();
	}
		
	public void _preloadFullScreenAd() {
		pluginDelegate._preloadFullScreenAd();
	}
	
	public void _showFullScreenAd() {
		pluginDelegate._showFullScreenAd();
	}

  	@Override
    public void onPause(boolean multitasking) {		
		pluginDelegate.onPause(multitasking);		
        super.onPause(multitasking);
    }
      
    @Override
    public void onResume(boolean multitasking) {
        pluginDelegate.onResume(multitasking);
        super.onResume(multitasking);
    }
  	
    @Override
    public void onDestroy() {
		pluginDelegate.onDestroy();		
        super.onDestroy();
    }
	
	//cranberrygame end: AdmobPluginDelegate
}
