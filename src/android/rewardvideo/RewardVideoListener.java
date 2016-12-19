package name.ratson.cordova.admob.rewardvideo;

import com.google.android.gms.ads.reward.RewardItem;
import com.google.android.gms.ads.reward.RewardedVideoAdListener;

import org.json.JSONException;
import org.json.JSONObject;

import android.util.Log;

import name.ratson.cordova.admob.AdMob;
import name.ratson.cordova.admob.AbstractAdListener;

public class RewardVideoListener extends AbstractAdListener implements RewardedVideoAdListener {
    private final RewardVideoExecutor rewardVideoExecutor;

    public RewardVideoListener(AdMob adMob, RewardVideoExecutor rewardVideoExecutor) {
        super(adMob);
        this.rewardVideoExecutor = rewardVideoExecutor;
    }

    @Override
    public String getAdType() {
        return "rewardvideo";
    }

    @Override
    public void onRewardedVideoAdFailedToLoad(int errorCode) {
        synchronized (this.rewardVideoExecutor.rewardedVideoLock) {
            this.rewardVideoExecutor.isRewardedVideoLoading = false;
        }

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
    public void onRewardedVideoAdLeftApplication() {
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

    @Override
    public void onRewardedVideoAdLoaded() {
        synchronized (this.rewardVideoExecutor.rewardedVideoLock) {
            this.rewardVideoExecutor.isRewardedVideoLoading = false;
        }
        Log.w("AdMob", "RewardedVideoAdLoaded");
        this.fireAdEvent("onReceiveRewardVideoAd");

        if (this.adMob.config.autoShowRewardVideo) {
            this.rewardVideoExecutor.showAd(true, null);
        }
    }

    @Override
    public void onRewardedVideoAdOpened() {
        this.fireAdEvent("onPresentRewardVideoAd");
    }

    @Override
    public void onRewardedVideoStarted() {
        this.fireAdEvent("onPresentStartedRewardVideoAd");
    }

    @Override
    public void onRewardedVideoAdClosed() {
        this.fireAdEvent("onDismissRewardVideoAd");
        this.rewardVideoExecutor.clearAd();
    }

    @Override
    public void onRewarded(RewardItem reward) {
        JSONObject data = new JSONObject();
        try {
            data.put("adType", this.getAdType());
            data.put("rewardType", reward.getType());
            data.put("rewardAmount", reward.getAmount());
        } catch (JSONException e) {
            e.printStackTrace();
            this.fireAdEvent("onRewardedVideo");
            return;
        }

        this.fireAdEvent("onRewardedVideo", data);
    }
}
