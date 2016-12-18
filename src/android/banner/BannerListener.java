package name.ratson.cordova.admob.banner;

import android.util.Log;

import name.ratson.cordova.admob.AdMob;
import name.ratson.cordova.admob.AbstractAdListener;

public class BannerListener extends AbstractAdListener {
    public BannerListener(AdMob adMob) {
        super(adMob);
    }

    @Override
    public String getAdType() {
        return "banner";
    }

    @Override
    public void onAdLoaded() {
        Log.w("AdMob", "BannerAdLoaded");
        if (this.adMob.config.autoShowBanner && !this.adMob.bannerVisible) {
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
