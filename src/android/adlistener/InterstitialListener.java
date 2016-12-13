package name.ratson.cordova.admob.adlistener;

import android.util.Log;

import name.ratson.cordova.admob.AdMob;

public class InterstitialListener extends BaseAdListener {
    public InterstitialListener(AdMob adMob) {
        super(adMob);
    }

    @Override
    String getAdType() {
        return "interstitial";
    }

    @Override
    public void onAdLoaded() {
        Log.w("AdMob", "InterstitialAdLoaded");
        this.fireAdEvent("onReceiveInterstitialAd");

        if (this.adMob.config.autoShowInterstitial) {
            this.adMob.executeShowInterstitialAd(true, null);
        }
    }

    @Override
    public void onAdOpened() {
        this.fireAdEvent("onPresentInterstitialAd");
    }

    @Override
    public void onAdClosed() {
        this.fireAdEvent("onDismissInterstitialAd");
        this.adMob.clearInterstitial();
    }
}
