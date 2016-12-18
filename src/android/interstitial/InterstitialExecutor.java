package name.ratson.cordova.admob.interstitial;

import android.util.Log;

import com.google.android.gms.ads.InterstitialAd;

import org.apache.cordova.CallbackContext;
import org.apache.cordova.CordovaInterface;
import org.apache.cordova.PluginResult;
import org.json.JSONObject;

import name.ratson.cordova.admob.AdMob;
import name.ratson.cordova.admob.AdMobConfig;

public class InterstitialExecutor {
    private final AdMob adMob;

    /**
     * The interstitial ad to display to the user.
     */
    private InterstitialAd interstitialAd;

    public InterstitialExecutor(AdMob adMob) {
        this.adMob = adMob;
    }

    /**
     * Parses the createAd interstitial view input parameters and runs the createAd interstitial
     * view action on the UI thread.  If this request is successful, the developer
     * should make the requestAd call to request an ad for the banner.
     *
     * @param options The JSONArray representing input parameters.  This function
     *                expects the first object in the array to be a JSONObject with the
     *                input parameters.
     * @return A PluginResult representing whether or not the banner was created
     * successfully.
     */
    public PluginResult createAd(JSONObject options, CallbackContext callbackContext) {
        AdMobConfig config = adMob.config;
        CordovaInterface cordova = adMob.cordova;

        config.setInterstitialOptions(options);

        final CallbackContext delayCallback = callbackContext;
        cordova.getActivity().runOnUiThread(new Runnable() {
            @Override
            public void run() {
                AdMobConfig config = adMob.config;
                CordovaInterface cordova = adMob.cordova;

                clearAd();
                interstitialAd = new InterstitialAd(cordova.getActivity());
                interstitialAd.setAdUnitId(config.getInterstitialAdUnitId());
                interstitialAd.setAdListener(new InterstitialListener(adMob, InterstitialExecutor.this));
                Log.w("interstitial", config.getInterstitialAdUnitId());
                interstitialAd.loadAd(adMob.buildAdRequest());
                delayCallback.success();
            }
        });
        return null;
    }

    public void clearAd() {
        if (interstitialAd != null) {
            interstitialAd.setAdListener(null);
            interstitialAd = null;
        }
    }

    public PluginResult requestAd(JSONObject options, CallbackContext callbackContext) {
        AdMobConfig config = adMob.config;
        CordovaInterface cordova = adMob.cordova;

        config.setOptions(options);

        if (interstitialAd == null) {
            callbackContext.error("interstitialAd is null, call createInterstitialView first");
            return null;
        }

        final CallbackContext delayCallback = callbackContext;
        cordova.getActivity().runOnUiThread(new Runnable() {
            @Override
            public void run() {
                interstitialAd.loadAd(adMob.buildAdRequest());

                delayCallback.success();
            }
        });

        return null;
    }

    public PluginResult showAd(final boolean show, final CallbackContext callbackContext) {
        if (interstitialAd == null) {
            return new PluginResult(PluginResult.Status.ERROR, "interstitialAd is null, call createInterstitialView first.");
        }
        CordovaInterface cordova = adMob.cordova;

        cordova.getActivity().runOnUiThread(new Runnable() {
            @Override
            public void run() {
                AdMobConfig config = adMob.config;

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
}
