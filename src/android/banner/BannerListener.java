package name.ratson.cordova.admob.banner;

import android.util.Log;

import name.ratson.cordova.admob.AdMob;
import name.ratson.cordova.admob.AbstractAdListener;

class BannerListener extends AbstractAdListener {
    private final BannerExecutor bannerExecutor;

    BannerListener(AdMob plugin, BannerExecutor bannerExecutor) {
        super(plugin);
        this.bannerExecutor = bannerExecutor;
    }

    @Override
    public String getAdType() {
        return "banner";
    }

    @Override
    public void onAdLoaded() {
        Log.w("AdMob", "BannerAdLoaded");
        if (this.bannerExecutor.config.autoShowBanner && !this.bannerExecutor.bannerVisible) {
            this.bannerExecutor.showAd(true, null);
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
