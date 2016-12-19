package name.ratson.cordova.admob.interstitial;

import android.util.Log;

import com.google.android.gms.ads.AdListener;

import org.json.JSONException;
import org.json.JSONObject;

import name.ratson.cordova.admob.AbstractExecutor;

class InterstitialListener extends AdListener {
    private final InterstitialExecutor executor;

    InterstitialListener(InterstitialExecutor executor) {
        this.executor = executor;
    }

    @Override
    public void onAdFailedToLoad(int errorCode) {
        JSONObject data = new JSONObject();
        try {
            data.put("error", errorCode);
            data.put("reason", AbstractExecutor.getErrorReason(errorCode));
            data.put("adType", executor.getAdType());
        } catch (JSONException e) {
            e.printStackTrace();
        }
        executor.fireAdEvent("onFailedToReceiveAd", data);
    }

    @Override
    public void onAdLeftApplication() {
        JSONObject data = new JSONObject();
        try {
            data.put("adType", executor.getAdType());
        } catch (JSONException e) {
            e.printStackTrace();
        }
        executor.fireAdEvent("onLeaveToAd", data);
    }

    @Override
    public void onAdLoaded() {
        Log.w("AdMob", "InterstitialAdLoaded");
        executor.fireAdEvent("onReceiveInterstitialAd");

        if (executor.shouldAutoShow()) {
            executor.showAd(true, null);
        }
    }

    @Override
    public void onAdOpened() {
        executor.fireAdEvent("onPresentInterstitialAd");
    }

    @Override
    public void onAdClosed() {
        executor.fireAdEvent("onDismissInterstitialAd");
        executor.destroy();
    }
}
