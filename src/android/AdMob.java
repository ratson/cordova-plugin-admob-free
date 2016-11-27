package name.ratson.cordova.admob;

import android.os.Bundle;
import android.provider.Settings;
import android.util.Log;
import android.view.View;
import android.view.ViewGroup;
import android.widget.LinearLayout;
import android.widget.RelativeLayout;

import com.google.ads.mediation.admob.AdMobAdapter;
import com.google.android.gms.ads.AdListener;
import com.google.android.gms.ads.AdRequest;
import com.google.android.gms.ads.AdView;
import com.google.android.gms.ads.InterstitialAd;
import com.google.android.gms.ads.MobileAds;
import com.google.android.gms.common.ConnectionResult;
import com.google.android.gms.common.GoogleApiAvailability;

import org.apache.cordova.CallbackContext;
import org.apache.cordova.CordovaInterface;
import org.apache.cordova.CordovaPlugin;
import org.apache.cordova.CordovaWebView;
import org.apache.cordova.PluginResult;
import org.apache.cordova.PluginResult.Status;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.util.Iterator;

/**
 * This class represents the native implementation for the AdMob Cordova plugin.
 * This plugin can be used to request AdMob ads natively via the Google AdMob SDK.
 * The Google AdMob SDK is a dependency for this plugin.
 */
public class AdMob extends CordovaPlugin {
    /**
     * Common tag used for logging statements.
     */
    private static final String LOGTAG = "AdMob";

    private static final AdMobConfig config = new AdMobConfig();

    /**
     * Cordova Actions.
     */
    private static final String ACTION_SET_OPTIONS = "setOptions";

    private static final String ACTION_CREATE_BANNER_VIEW = "createBannerView";
    private static final String ACTION_DESTROY_BANNER_VIEW = "destroyBannerView";
    private static final String ACTION_REQUEST_AD = "requestAd";
    private static final String ACTION_SHOW_AD = "showAd";

    private static final String ACTION_CREATE_INTERSTITIAL_VIEW = "createInterstitialView";
    private static final String ACTION_REQUEST_INTERSTITIAL_AD = "requestInterstitialAd";
    private static final String ACTION_SHOW_INTERSTITIAL_AD = "showInterstitialAd";

    private ViewGroup parentView;

    private boolean bannerShow = true;

    /**
     * The adView to display to the user.
     */
    private AdView adView;
    /**
     * if want banner view overlap webview, we will need this layout
     */
    private RelativeLayout adViewLayout = null;

    /**
     * The interstitial ad to display to the user.
     */
    private InterstitialAd interstitialAd;

    private boolean autoShowBanner = true;
    private boolean autoShowInterstitial = true;
    private boolean autoShowInterstitialTemp = false;        //if people call it when it's not ready

    private boolean bannerVisible = false;
    private boolean isGpsAvailable = false;

    @Override
    public void initialize(CordovaInterface cordova, CordovaWebView webView) {
        super.initialize(cordova, webView);
        isGpsAvailable = (GoogleApiAvailability.getInstance().isGooglePlayServicesAvailable(cordova.getActivity()) == ConnectionResult.SUCCESS);
    }

    /**
     * This is the main method for the AdMob plugin.  All API calls go through here.
     * This method determines the action, and executes the appropriate call.
     *
     * @param action          The action that the plugin should execute.
     * @param inputs          The input parameters for the action.
     * @param callbackContext The callback context.
     * @return A PluginResult representing the result of the provided action.  A
     * status of INVALID_ACTION is returned if the action is not recognized.
     */
    @Override
    public boolean execute(String action, JSONArray inputs, CallbackContext callbackContext) throws JSONException {
        PluginResult result = null;

        if (ACTION_SET_OPTIONS.equals(action)) {
            JSONObject options = inputs.optJSONObject(0);
            result = executeSetOptions(options, callbackContext);

        } else if (ACTION_CREATE_BANNER_VIEW.equals(action)) {
            JSONObject options = inputs.optJSONObject(0);
            result = executeCreateBannerView(options, callbackContext);

        } else if (ACTION_CREATE_INTERSTITIAL_VIEW.equals(action)) {
            JSONObject options = inputs.optJSONObject(0);
            result = executeCreateInterstitialView(options, callbackContext);

        } else if (ACTION_DESTROY_BANNER_VIEW.equals(action)) {
            result = executeDestroyBannerView(callbackContext);

        } else if (ACTION_REQUEST_INTERSTITIAL_AD.equals(action)) {
            JSONObject options = inputs.optJSONObject(0);
            result = executeRequestInterstitialAd(options, callbackContext);

        } else if (ACTION_REQUEST_AD.equals(action)) {
            JSONObject options = inputs.optJSONObject(0);
            result = executeRequestAd(options, callbackContext);

        } else if (ACTION_SHOW_AD.equals(action)) {
            boolean show = inputs.optBoolean(0);
            result = executeShowAd(show, callbackContext);

        } else if (ACTION_SHOW_INTERSTITIAL_AD.equals(action)) {
            boolean show = inputs.optBoolean(0);
            result = executeShowInterstitialAd(show, callbackContext);

        } else {
            Log.d(LOGTAG, String.format("Invalid action passed: %s", action));
            result = new PluginResult(Status.INVALID_ACTION);
        }

        if (result != null) {
            callbackContext.sendPluginResult(result);
        }

        return true;
    }

    private PluginResult executeSetOptions(JSONObject options, CallbackContext callbackContext) {
        Log.w(LOGTAG, "executeSetOptions");

        config.setOptions(options);

        MobileAds.initialize(getApplicationContext(), config.appId);

        callbackContext.success();
        return null;
    }

    /**
     * Parses the create banner view input parameters and runs the create banner
     * view action on the UI thread.  If this request is successful, the developer
     * should make the requestAd call to request an ad for the banner.
     *
     * @param options The JSONArray representing input parameters.  This function
     *                expects the first object in the array to be a JSONObject with the
     *                input parameters.
     * @return A PluginResult representing whether or not the banner was created
     * successfully.
     */
    private PluginResult executeCreateBannerView(JSONObject options, final CallbackContext callbackContext) {
        config.setOptions(options);
        autoShowBanner = config.autoShow;

        cordova.getActivity().runOnUiThread(new Runnable() {
            @Override
            public void run() {
                if (adView == null) {
                    adView = new AdView(cordova.getActivity());
                    adView.setAdUnitId(config.bannerAdUnitId);
                    adView.setAdSize(config.adSize);
                    adView.setAdListener(new BannerListener());
                }
                if (adView.getParent() != null) {
                    ((ViewGroup) adView.getParent()).removeView(adView);
                }

                bannerVisible = false;
                adView.loadAd(buildAdRequest());

                //if(autoShowBanner) {
                // executeShowAd(true, null);
                //}

                callbackContext.success();
            }
        });

        return null;
    }

    private PluginResult executeDestroyBannerView(CallbackContext callbackContext) {
        Log.w(LOGTAG, "executeDestroyBannerView");

        final CallbackContext delayCallback = callbackContext;
        cordova.getActivity().runOnUiThread(new Runnable() {
            @Override
            public void run() {
                if (adView != null) {
                    ViewGroup parentView = (ViewGroup) adView.getParent();
                    if (parentView != null) {
                        parentView.removeView(adView);
                    }
                    adView.destroy();
                    adView = null;
                }
                bannerVisible = false;
                delayCallback.success();
            }
        });

        return null;
    }

    /**
     * Parses the create interstitial view input parameters and runs the create interstitial
     * view action on the UI thread.  If this request is successful, the developer
     * should make the requestAd call to request an ad for the banner.
     *
     * @param options The JSONArray representing input parameters.  This function
     *                expects the first object in the array to be a JSONObject with the
     *                input parameters.
     * @return A PluginResult representing whether or not the banner was created
     * successfully.
     */
    private PluginResult executeCreateInterstitialView(JSONObject options, CallbackContext callbackContext) {
        config.setOptions(options);
        autoShowInterstitial = config.autoShow;

        final CallbackContext delayCallback = callbackContext;
        cordova.getActivity().runOnUiThread(new Runnable() {
            @Override
            public void run() {
                clearInterstitial();
                interstitialAd = new InterstitialAd(cordova.getActivity());
                interstitialAd.setAdUnitId(config.interstitialAdUnitId);
                interstitialAd.setAdListener(new InterstitialListener());
                interstitialAd.loadAd(buildAdRequest());
                delayCallback.success();
            }
        });
        return null;
    }

    private void clearInterstitial() {
        if (interstitialAd == null) {
            return;
        }
        interstitialAd.setAdListener(null);
        interstitialAd = null;
    }

    private AdRequest buildAdRequest() {
        AdRequest.Builder builder = new AdRequest.Builder();
        if (config.isTesting) {
            // This will request test ads on the emulator and deviceby passing this hashed device ID.
            String ANDROID_ID = Settings.Secure.getString(cordova.getActivity().getContentResolver(), android.provider.Settings.Secure.ANDROID_ID);
            String deviceId = md5(ANDROID_ID).toUpperCase();
            builder = builder.addTestDevice(deviceId).addTestDevice(AdRequest.DEVICE_ID_EMULATOR);
        }

        if (config.testDeviceList != null) {
            Iterator<String> iterator = config.testDeviceList.iterator();
            while (iterator.hasNext()) {
                builder = builder.addTestDevice(iterator.next());
            }
        }

        Bundle bundle = new Bundle();
        bundle.putInt("cordova", 1);
        if (config.adExtras != null) {
            Iterator<String> it = config.adExtras.keys();
            while (it.hasNext()) {
                String key = it.next();
                try {
                    bundle.putString(key, config.adExtras.get(key).toString());
                } catch (JSONException exception) {
                    Log.w(LOGTAG, String.format("Caught JSON Exception: %s", exception.getMessage()));
                }
            }
        }
        builder = builder.addNetworkExtrasBundle(AdMobAdapter.class, bundle);

        if (config.gender != null) {
            if ("male".compareToIgnoreCase(config.gender) != 0) {
                builder.setGender(AdRequest.GENDER_MALE);
            } else if ("female".compareToIgnoreCase(config.gender) != 0) {
                builder.setGender(AdRequest.GENDER_FEMALE);
            } else {
                builder.setGender(AdRequest.GENDER_UNKNOWN);
            }
        }
        if (config.location != null) {
            builder.setLocation(config.location);
        }
        if (config.forFamily != null) {
            builder.setIsDesignedForFamilies(true);
        }
        if (config.forChild != null) {
            builder.tagForChildDirectedTreatment(true);
        }
        if (config.contentURL != null) {
            builder.setContentUrl(config.contentURL);
        }

        return builder.build();
    }

    /**
     * Parses the request ad input parameters and runs the request ad action on
     * the UI thread.
     *
     * @param options The JSONArray representing input parameters.  This function
     *                expects the first object in the array to be a JSONObject with the
     *                input parameters.
     * @return A PluginResult representing whether or not an ad was requested
     * succcessfully.  Listen for onReceiveAd() and onFailedToReceiveAd()
     * callbacks to see if an ad was successfully retrieved.
     */
    private PluginResult executeRequestAd(JSONObject options, CallbackContext callbackContext) {
        config.setOptions(options);

        if (adView == null) {
            callbackContext.error("adView is null, call createBannerView first");
            return null;
        }

        final CallbackContext delayCallback = callbackContext;
        cordova.getActivity().runOnUiThread(new Runnable() {
            @Override
            public void run() {
                adView.loadAd(buildAdRequest());

                delayCallback.success();
            }
        });

        return null;
    }

    private PluginResult executeRequestInterstitialAd(JSONObject options, CallbackContext callbackContext) {
        config.setOptions(options);

        if (adView == null) {
            callbackContext.error("interstitialAd is null, call createInterstitialView first");
            return null;
        }

        final CallbackContext delayCallback = callbackContext;
        cordova.getActivity().runOnUiThread(new Runnable() {
            @Override
            public void run() {
                interstitialAd.loadAd(buildAdRequest());

                delayCallback.success();
            }
        });

        return null;
    }

    /**
     * Parses the show ad input parameters and runs the show ad action on
     * the UI thread.
     *
     * @param options The JSONArray representing input parameters.  This function
     *                expects the first object in the array to be a JSONObject with the
     *                input parameters.
     * @return A PluginResult representing whether or not an ad was requested
     * succcessfully.  Listen for onReceiveAd() and onFailedToReceiveAd()
     * callbacks to see if an ad was successfully retrieved.
     */
    private PluginResult executeShowAd(final boolean show, final CallbackContext callbackContext) {

        bannerShow = show;

        if (adView == null) {
            return new PluginResult(Status.ERROR, "adView is null, call createBannerView first.");
        }

        cordova.getActivity().runOnUiThread(new Runnable() {
            @Override
            public void run() {
                if (bannerVisible == bannerShow) {
                    // no change
                } else if (bannerShow) {
                    if (adView.getParent() != null) {
                        ((ViewGroup) adView.getParent()).removeView(adView);
                    }
                    if (config.bannerOverlap) {
                        RelativeLayout.LayoutParams params2 = new RelativeLayout.LayoutParams(
                                RelativeLayout.LayoutParams.MATCH_PARENT,
                                RelativeLayout.LayoutParams.WRAP_CONTENT);
                        params2.addRule(config.bannerAtTop ? RelativeLayout.ALIGN_PARENT_TOP : RelativeLayout.ALIGN_PARENT_BOTTOM);

                        if (adViewLayout == null) {
                            adViewLayout = new RelativeLayout(cordova.getActivity());
                            RelativeLayout.LayoutParams params = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.MATCH_PARENT, RelativeLayout.LayoutParams.MATCH_PARENT);
                            try {
                                ((ViewGroup) (((View) webView.getClass().getMethod("getView").invoke(webView)).getParent())).addView(adViewLayout, params);
                            } catch (Exception e) {
                                ((ViewGroup) webView).addView(adViewLayout, params);
                            }
                        }

                        adViewLayout.addView(adView, params2);
                        adViewLayout.bringToFront();
                    } else {
                        ViewGroup wvParentView = (ViewGroup) getWebView().getParent();
                        if (parentView == null) {
                            parentView = new LinearLayout(webView.getContext());
                        }
                        if (wvParentView != null && wvParentView != parentView) {
                            wvParentView.removeView(getWebView());
                            ((LinearLayout) parentView).setOrientation(LinearLayout.VERTICAL);
                            parentView.setLayoutParams(new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.MATCH_PARENT, 0.0F));
                            getWebView().setLayoutParams(new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.MATCH_PARENT, 1.0F));
                            parentView.addView(getWebView());
                            cordova.getActivity().setContentView(parentView);
                        }


                        if (config.bannerAtTop) {
                            parentView.addView(adView, 0);
                        } else {
                            parentView.addView(adView);
                        }
                        parentView.bringToFront();
                        parentView.requestLayout();
                        parentView.requestFocus();
                    }

                    adView.setVisibility(View.VISIBLE);
                    bannerVisible = true;

                } else {
                    adView.setVisibility(View.GONE);
                    bannerVisible = false;
                }

                if (callbackContext != null) {
                    callbackContext.success();
                }
            }
        });

        return null;
    }

    private PluginResult executeShowInterstitialAd(final boolean show, final CallbackContext callbackContext) {

        if (interstitialAd == null) {
            return new PluginResult(Status.ERROR, "interstitialAd is null, call createInterstitialView first.");
        }

        cordova.getActivity().runOnUiThread(new Runnable() {
            @Override
            public void run() {

                if (interstitialAd.isLoaded()) {
                    interstitialAd.show();
                } else {
                    Log.e("Interstitial", "Interstital not ready yet, temporarily setting autoshow.");
                    autoShowInterstitialTemp = true;
                }

                if (callbackContext != null) {
                    callbackContext.success();
                }
            }
        });

        return null;
    }


    /**
     * This class implements the AdMob ad listener events.  It forwards the events
     * to the JavaScript layer.  To listen for these events, use:
     * <p>
     * document.addEventListener('onReceiveAd', function());
     * document.addEventListener('onFailedToReceiveAd', function(data){});
     * document.addEventListener('onPresentAd', function());
     * document.addEventListener('onDismissAd', function());
     * document.addEventListener('onLeaveToAd', function());
     */
    abstract class BasicListener extends AdListener {
        abstract String getAdType();

        protected void fireAdEvent(String eventName) {
            String js = new CordovaEventBuilder(eventName).build();
            webView.loadUrl(js);
        }

        protected void fireAdEvent(String eventName, JSONObject data) {
            String js = new CordovaEventBuilder(eventName).withData(data).build();
            webView.loadUrl(js);
        }

        @Override
        public void onAdFailedToLoad(int errorCode) {
            JSONObject data = new JSONObject();
            try {
                data.put("error", errorCode);
                data.put("reason", getErrorReason(errorCode));
                data.put("adType", this.getAdType());
            } catch (JSONException e) {
                e.printStackTrace();
                this.fireAdEvent("onFailedToReceiveAd");
                return;
            }
            this.fireAdEvent("onFailedToReceiveAd", data);
        }

        @Override
        public void onAdLeftApplication() {
            JSONObject data = new JSONObject();
            try {
                data.put("adType", this.getAdType());
            } catch (JSONException e) {
                e.printStackTrace();
                this.fireAdEvent("onLeaveToAd");
                return;
            }
            this.fireAdEvent("onLeaveToAd", data);
        }
    }

    private class BannerListener extends BasicListener {
        @Override
        String getAdType() {
            return "banner";
        }

        @Override
        public void onAdLoaded() {
            Log.w("AdMob", "BannerAdLoaded");
            if (autoShowBanner && !bannerVisible) {
                executeShowAd(true, null);
            }
            this.fireAdEvent("onReceiveAd");
        }

        @Override
        public void onAdOpened() {
            this.fireAdEvent("onPresentAd");
        }

        @Override
        public void onAdClosed() {
            this.fireAdEvent("onDismissAd");
        }

    }

    private class InterstitialListener extends BasicListener {
        @Override
        String getAdType() {
            return "interstitial";
        }

        @Override
        public void onAdLoaded() {
            Log.w("AdMob", "InterstitialAdLoaded");
            this.fireAdEvent("onReceiveInterstitialAd");

            if (autoShowInterstitial) {
                executeShowInterstitialAd(true, null);
            } else if (autoShowInterstitialTemp) {
                executeShowInterstitialAd(true, null);
                autoShowInterstitialTemp = false;
            }
        }

        @Override
        public void onAdOpened() {
            this.fireAdEvent("onPresentInterstitialAd");
        }

        @Override
        public void onAdClosed() {
            this.fireAdEvent("onDismissInterstitialAd");
            clearInterstitial();
        }

    }

    @Override
    public void onPause(boolean multitasking) {
        if (adView != null) {
            adView.pause();
        }
        super.onPause(multitasking);
    }

    @Override
    public void onResume(boolean multitasking) {
        super.onResume(multitasking);
        isGpsAvailable = (GoogleApiAvailability.getInstance().isGooglePlayServicesAvailable(cordova.getActivity()) == ConnectionResult.SUCCESS);
        if (adView != null) {
            adView.resume();
        }
    }

    @Override
    public void onDestroy() {
        if (adView != null) {
            adView.destroy();
            adView = null;
        }
        if (adViewLayout != null) {
            ViewGroup parentView = (ViewGroup) adViewLayout.getParent();
            if (parentView != null) {
                parentView.removeView(adViewLayout);
            }
            adViewLayout = null;
        }
        super.onDestroy();
    }

    private View getWebView() {
        try {
            return (View) webView.getClass().getMethod("getView").invoke(webView);
        } catch (Exception e) {
            return (View) webView;
        }
    }

    /**
     * Gets a string error reason from an error code.
     */
    public String getErrorReason(int errorCode) {
        String errorReason = "";
        switch (errorCode) {
            case AdRequest.ERROR_CODE_INTERNAL_ERROR:
                errorReason = "Internal error";
                break;
            case AdRequest.ERROR_CODE_INVALID_REQUEST:
                errorReason = "Invalid request";
                break;
            case AdRequest.ERROR_CODE_NETWORK_ERROR:
                errorReason = "Network Error";
                break;
            case AdRequest.ERROR_CODE_NO_FILL:
                errorReason = "No fill";
                break;
        }
        return errorReason;
    }

    public static final String md5(final String s) {
        try {
            MessageDigest digest = java.security.MessageDigest.getInstance("MD5");
            digest.update(s.getBytes());
            byte messageDigest[] = digest.digest();
            StringBuffer hexString = new StringBuffer();
            for (int i = 0; i < messageDigest.length; i++) {
                String h = Integer.toHexString(0xFF & messageDigest[i]);
                while (h.length() < 2)
                    h = "0" + h;
                hexString.append(h);
            }
            return hexString.toString();

        } catch (NoSuchAlgorithmException e) {
        }
        return "";
    }
}
