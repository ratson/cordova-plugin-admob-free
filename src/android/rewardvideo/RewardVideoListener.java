package name.ratson.cordova.admob.rewardvideo;

import android.util.Log;

import com.google.android.gms.ads.reward.RewardItem;
import com.google.android.gms.ads.reward.RewardedVideoAdListener;

import org.json.JSONException;
import org.json.JSONObject;

import name.ratson.cordova.admob.AbstractExecutor;

class RewardVideoListener implements RewardedVideoAdListener {
    private final RewardVideoExecutor executor;

    RewardVideoListener(RewardVideoExecutor executor) {
        this.executor = executor;
    }

    @Override
    public void onRewardedVideoAdFailedToLoad(int errorCode) {
        synchronized (executor.rewardedVideoLock) {
            executor.isRewardedVideoLoading = false;
        }

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
    public void onRewardedVideoAdLeftApplication() {
        JSONObject data = new JSONObject();
        try {
            data.put("adType", executor.getAdType());
        } catch (JSONException e) {
            e.printStackTrace();
        }
        executor.fireAdEvent("onLeaveToAd", data);
    }

    @Override
    public void onRewardedVideoAdLoaded() {
        synchronized (executor.rewardedVideoLock) {
            executor.isRewardedVideoLoading = false;
        }
        Log.w("AdMob", "RewardedVideoAdLoaded");
        executor.fireAdEvent("onReceiveRewardVideoAd");

        if (executor.shouldAutoShow()) {
            executor.showAd(true, null);
        }
    }

    @Override
    public void onRewardedVideoAdOpened() {
        executor.fireAdEvent("onPresentRewardVideoAd");
    }

    @Override
    public void onRewardedVideoStarted() {
        executor.fireAdEvent("onPresentStartedRewardVideoAd");
    }

    @Override
    public void onRewardedVideoAdClosed() {
        executor.fireAdEvent("onDismissRewardVideoAd");
        executor.clearAd();
    }

    @Override
    public void onRewarded(RewardItem reward) {
        JSONObject data = new JSONObject();
        try {
            data.put("adType", executor.getAdType());
            data.put("rewardType", reward.getType());
            data.put("rewardAmount", reward.getAmount());
        } catch (JSONException e) {
            e.printStackTrace();
        }
        executor.fireAdEvent("onRewardedVideo", data);
    }
}
