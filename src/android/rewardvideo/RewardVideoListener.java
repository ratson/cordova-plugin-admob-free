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
        executor.fireAdEvent("admob.rewardvideo.events.LOAD_FAIL", data);
    }

    @Override
    public void onRewardedVideoAdLeftApplication() {
        JSONObject data = new JSONObject();
        try {
            data.put("adType", executor.getAdType());
        } catch (JSONException e) {
            e.printStackTrace();
        }
        executor.fireAdEvent("admob.rewardvideo.events.EXIT_APP", data);
    }

    @Override
    public void onRewardedVideoAdLoaded() {
        synchronized (executor.rewardedVideoLock) {
            executor.isRewardedVideoLoading = false;
        }
        Log.w("AdMob", "RewardedVideoAdLoaded");
        executor.fireAdEvent("admob.rewardvideo.events.LOAD");

        if (executor.shouldAutoShow()) {
            executor.showAd(true, null);
        }
    }

    @Override
    public void onRewardedVideoAdOpened() {
        executor.fireAdEvent("admob.rewardvideo.events.OPEN");
    }

    @Override
    public void onRewardedVideoStarted() {
        executor.fireAdEvent("admob.rewardvideo.events.START");
    }

    @Override
    public void onRewardedVideoAdClosed() {
        executor.fireAdEvent("admob.rewardvideo.events.CLOSE");
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
        executor.fireAdEvent("admob.rewardvideo.events.REWARD", data);
    }

    public void onRewardedVideoCompleted() {
        executor.fireAdEvent("admob.rewardvideo.events.COMPLETE");
    }
}
