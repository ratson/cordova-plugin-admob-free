package name.ratson.cordova.admob.interstitial;

import android.util.Log;

import name.ratson.cordova.admob.AdMob;
import name.ratson.cordova.admob.AbstractAdListener;

class InterstitialListener extends AbstractAdListener {
    private final InterstitialExecutor interstitialExecutor;

    InterstitialListener(AdMob adMob, InterstitialExecutor interstitialExecutor) {
        super(adMob);
        this.interstitialExecutor = interstitialExecutor;
    }

    @Override
    public String getAdType() {
        return "interstitial";
    }

    @Override
    public void onAdLoaded() {
        Log.w("AdMob", "InterstitialAdLoaded");
        this.fireAdEvent("onReceiveInterstitialAd");

        if (this.adMob.config.autoShowInterstitial) {
            interstitialExecutor.showAd(true, null);
        }
    }

    @Override
    public void onAdOpened() {
        this.fireAdEvent("onPresentInterstitialAd");
    }

    @Override
    public void onAdClosed() {
        this.fireAdEvent("onDismissInterstitialAd");
        interstitialExecutor.destroy();
    }
}
