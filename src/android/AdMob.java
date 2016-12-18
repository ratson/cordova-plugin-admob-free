package name.ratson.cordova.admob;

import android.os.Bundle;
import android.provider.Settings;
import android.support.annotation.NonNull;
import android.util.Log;
import android.view.View;
import android.view.ViewGroup;
import android.widget.LinearLayout;
import android.widget.RelativeLayout;

import com.google.ads.mediation.admob.AdMobAdapter;
import com.google.android.gms.ads.AdRequest;
import com.google.android.gms.ads.AdView;
import com.google.android.gms.ads.InterstitialAd;
import com.google.android.gms.ads.MobileAds;
import com.google.android.gms.ads.reward.RewardedVideoAd;
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

import name.ratson.cordova.admob.banner.BannerListener;
import name.ratson.cordova.admob.interstitial.InterstitialListener;
import name.ratson.cordova.admob.rewardvideo.RewardVideoListener;

/**
 * This class represents the native implementation for the AdMob Cordova plugin.
 * This plugin can be used to request AdMob ads natively via the Google AdMob SDK.
 * The Google AdMob SDK is a dependency for this plugin.
 */
public class AdMob extends CordovaPlugin {
    /**
     * Common tag used for logging statements.
     */
    private static final String TAG = "AdMob";

    public final AdMobConfig config = new AdMobConfig();

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

    /**
     * RewardVideo
     */
    private RewardedVideoAd rewardedVideoAd;
    public boolean isRewardedVideoLoading = false;
    public final Object rewardedVideoLock = new Object();


    public boolean bannerVisible = false;
    private boolean isGpsAvailable = false;

    @Override
    public void initialize(CordovaInterface cordova, CordovaWebView webView) {
        super.initialize(cordova, webView);
        isGpsAvailable = (GoogleApiAvailability.getInstance().isGooglePlayServicesAvailable(cordova.getActivity()) == ConnectionResult.SUCCESS);
        Log.w(TAG, String.format("isGooglePlayServicesAvailable: %s", isGpsAvailable ? "true" : "false"));
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

        if (Actions.SET_OPTIONS.equals(action)) {
            JSONObject options = inputs.optJSONObject(0);
            result = executeSetOptions(options, callbackContext);

        } else if (Actions.CREATE_BANNER.equals(action)) {
            JSONObject options = inputs.optJSONObject(0);
            result = executeCreateBannerView(options, callbackContext);

        } else if (Actions.CREATE_INTERSTITIAL.equals(action)) {
            JSONObject options = inputs.optJSONObject(0);
            result = executeCreateInterstitialView(options, callbackContext);

        } else if (Actions.DESTROY_BANNER.equals(action)) {
            result = executeDestroyBannerView(callbackContext);

        } else if (Actions.REQUEST_INTERSTITIAL.equals(action)) {
            JSONObject options = inputs.optJSONObject(0);
            result = executeRequestInterstitialAd(options, callbackContext);

        } else if (Actions.REQUEST_AD.equals(action)) {
            JSONObject options = inputs.optJSONObject(0);
            result = executeRequestAd(options, callbackContext);

        } else if (Actions.SHOW_AD.equals(action)) {
            boolean show = inputs.optBoolean(0);
            result = executeShowAd(show, callbackContext);

        } else if (Actions.SHOW_INTERSTITIAL.equals(action)) {
            boolean show = inputs.optBoolean(0);
            result = executeShowInterstitialAd(show, callbackContext);

        } else if (Actions.CREATE_REWARD_VIDEO.equals(action)) {
            JSONObject options = inputs.optJSONObject(0);            
            result = executeCreateRewardVideo(options, callbackContext);

        } else if (Actions.SHOW_REWARD_VIDEO.equals(action)) {
            boolean show = inputs.optBoolean(0);
            result = executeShowRewardVideo(show, callbackContext);
        
        } else {
            Log.d(TAG, String.format("Invalid action passed: %s", action));
            result = new PluginResult(Status.INVALID_ACTION);
        }

        if (result != null) {
            callbackContext.sendPluginResult(result);
        }

        return true;
    }

    private PluginResult executeSetOptions(JSONObject options, CallbackContext callbackContext) {
        Log.w(TAG, "executeSetOptions");

        config.setOptions(options);

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
        config.setBannerOptions(options);

        cordova.getActivity().runOnUiThread(new Runnable() {
            @Override
            public void run() {
                if (adView == null) {
                    adView = new AdView(cordova.getActivity());
                    adView.setAdUnitId(config.getBannerAdUnitId());
                    adView.setAdSize(config.adSize);
                    adView.setAdListener(new BannerListener(AdMob.this));
                }
                if (adView.getParent() != null) {
                    ((ViewGroup) adView.getParent()).removeView(adView);
                }

                bannerVisible = false;
                adView.loadAd(buildAdRequest());

//                if (config.autoShowBanner) {
//                    executeShowAd(true, null);
//                }
                Log.w("banner", config.getBannerAdUnitId());

                callbackContext.success();
            }
        });

        return null;
    }

    private PluginResult executeDestroyBannerView(CallbackContext callbackContext) {
        Log.w(TAG, "executeDestroyBannerView");

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
        config.setInterstitialOptions(options);

        final CallbackContext delayCallback = callbackContext;
        cordova.getActivity().runOnUiThread(new Runnable() {
            @Override
            public void run() {
                clearInterstitial();
                interstitialAd = new InterstitialAd(cordova.getActivity());
                interstitialAd.setAdUnitId(config.getInterstitialAdUnitId());
                interstitialAd.setAdListener(new InterstitialListener(AdMob.this));
                Log.w("interstitial", config.getInterstitialAdUnitId());
                interstitialAd.loadAd(buildAdRequest());
                delayCallback.success();
            }
        });
        return null;
    }

    public void clearInterstitial() {
        if (interstitialAd != null) {
            interstitialAd.setAdListener(null);
            interstitialAd = null;
        }
    }

    private AdRequest buildAdRequest() {
        AdRequest.Builder builder = new AdRequest.Builder();
        if (config.isTesting) {
            builder = builder.addTestDevice(AdRequest.DEVICE_ID_EMULATOR).addTestDevice(getDeviceId());
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
                    Log.w(TAG, String.format("Caught JSON Exception: %s", exception.getMessage()));
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
        if ("yes".equals(config.forFamily)) {
            builder.setIsDesignedForFamilies(true);
        } else if ("no".equals(config.forFamily)) {
            builder.setIsDesignedForFamilies(false);
        }
        if ("yes".equals(config.forChild)) {
            builder.tagForChildDirectedTreatment(true);
        } else if ("no".equals(config.forChild)) {
            builder.tagForChildDirectedTreatment(false);
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
     * @param show The JSONArray representing input parameters.  This function
     *             expects the first object in the array to be a JSONObject with the
     *             input parameters.
     * @return A PluginResult representing whether or not an ad was requested
     * succcessfully.  Listen for onReceiveAd() and onFailedToReceiveAd()
     * callbacks to see if an ad was successfully retrieved.
     */
    public PluginResult executeShowAd(final boolean show, final CallbackContext callbackContext) {

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

    public PluginResult executeShowInterstitialAd(final boolean show, final CallbackContext callbackContext) {

        if (interstitialAd == null) {
            return new PluginResult(Status.ERROR, "interstitialAd is null, call createInterstitialView first.");
        }

        cordova.getActivity().runOnUiThread(new Runnable() {
            @Override
            public void run() {

                if (interstitialAd.isLoaded()) {
                    interstitialAd.show();
                    if (callbackContext != null) {
                        callbackContext.success();
                    }
                } else if (!config.autoShowInterstitial) {
                    if (callbackContext != null) {
                        callbackContext.error("Interstital not ready yet");
                    }
                }

            }
        });

        return null;
    }


    private PluginResult executeCreateRewardVideo(JSONObject options, CallbackContext callbackContext) {
        config.setRewardVideoOptions(options);

        final CallbackContext delayCallback = callbackContext;
        cordova.getActivity().runOnUiThread(new Runnable() {
            @Override
            public void run() {
                clearRewardedVideo();

                rewardedVideoAd = MobileAds.getRewardedVideoAdInstance(cordova.getActivity());
                rewardedVideoAd.setRewardedVideoAdListener(new RewardVideoListener(AdMob.this));
                Log.w("rewardedvideo", config.getRewardedVideoAdUnitId());

                synchronized (rewardedVideoLock) {
                    if (!isRewardedVideoLoading) {
                        isRewardedVideoLoading = true;
                        Bundle extras = new Bundle();
                        extras.putBoolean("_noRefresh", true);
                        AdRequest adRequest = new AdRequest.Builder()
                                .addNetworkExtrasBundle(AdMobAdapter.class, extras)
                                .build();
                        rewardedVideoAd.loadAd(config.getRewardedVideoAdUnitId(), adRequest);
                        delayCallback.success();
                    }
                }                
            }
        });
        return null;       
    }

    public void clearRewardedVideo() {
        if (rewardedVideoAd == null) {
            return;
        }
        rewardedVideoAd.setRewardedVideoAdListener(null);
        rewardedVideoAd = null;
    }

    public PluginResult executeShowRewardVideo(final boolean show, final CallbackContext callbackContext) {
        if (rewardedVideoAd == null) {
            return new PluginResult(Status.ERROR, "rewardedVideoAd is null, call createRewardVideo first.");
        }

        cordova.getActivity().runOnUiThread(new Runnable() {
            @Override
            public void run() {

                if(rewardedVideoAd instanceof RewardedVideoAd) {
                    RewardedVideoAd rvad = rewardedVideoAd;
                    if(rvad.isLoaded()){
                        rvad.show();
                    }
                }

                if (callbackContext != null) {
                    callbackContext.success();
                }
            }
        });

        return null;
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
        destroyAdView();
        clearInterstitial();
        if (adViewLayout != null) {
            ViewGroup parentView = (ViewGroup) adViewLayout.getParent();
            if (parentView != null) {
                parentView.removeView(adViewLayout);
            }
            adViewLayout = null;
        }
        super.onDestroy();
    }

    private void destroyAdView() {
        if (adView != null) {
            adView.destroy();
            adView = null;
        }
    }

    private View getWebView() {
        try {
            return (View) webView.getClass().getMethod("getView").invoke(webView);
        } catch (Exception e) {
            return (View) webView;
        }
    }

    @NonNull
    private String getDeviceId() {
        // This will request test ads on the emulator and deviceby passing this hashed device ID.
        String ANDROID_ID = Settings.Secure.getString(cordova.getActivity().getContentResolver(), Settings.Secure.ANDROID_ID);
        return md5(ANDROID_ID).toUpperCase();
    }

    private static String md5(final String s) {
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
