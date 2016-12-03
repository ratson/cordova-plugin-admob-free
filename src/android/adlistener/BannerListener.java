package name.ratson.cordova.admob.adlistener;

import android.util.Log;

import name.ratson.cordova.admob.AdMob;

public class BannerListener extends BaseAdListener {
    public BannerListener(AdMob adMob) {
        super(adMob);
    }

    @Override
    String getAdType() {
        return "banner";
    }

    @Override
    public void onAdLoaded() {
        Log.w("AdMob", "BannerAdLoaded");
        if (this.adMob.autoShowBanner && !this.adMob.bannerVisible) {
            this.adMob.executeShowAd(true, null);
        }
        super.fireAdEvent("onReceiveAd");
    }

    @Override
    public void onAdOpened() {
        super.fireAdEvent("onPresentAd");
    }

    @Override
    public void onAdClosed() {
        super.fireAdEvent("onDismissAd");
    }
}
