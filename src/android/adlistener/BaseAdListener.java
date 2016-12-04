package name.ratson.cordova.admob.adlistener;

import com.google.android.gms.ads.AdListener;
import com.google.android.gms.ads.AdRequest;

import org.json.JSONException;
import org.json.JSONObject;

import name.ratson.cordova.admob.AdMob;
import name.ratson.cordova.admob.CordovaEventBuilder;

/**
 * This class implements the AdMob ad listener events.  It forwards the events
 * to the JavaScript layer.  To listen for these events, use:
 * <p>
 * document.addEventListener('onReceiveAd', function());
 * document.addEventListener('onFailedToReceiveAd', function(data){});
 * document.addEventListener('onPresentAd', function());
 * document.addEventListener('onDismissAd', function());
 * document.addEventListener('onLeaveToAd', function());
 */
public abstract class BaseAdListener extends AdListener {
    protected AdMob adMob;

    public BaseAdListener(AdMob adMob) {
        this.adMob = adMob;
    }

    abstract String getAdType();

    protected void fireAdEvent(String eventName) {
        String js = new CordovaEventBuilder(eventName).build();
        adMob.webView.loadUrl(js);
    }

    protected void fireAdEvent(String eventName, JSONObject data) {
        String js = new CordovaEventBuilder(eventName).withData(data).build();
        adMob.webView.loadUrl(js);
    }

    @Override
    public void onAdFailedToLoad(int errorCode) {
        JSONObject data = new JSONObject();
        try {
            data.put("error", errorCode);
            data.put("reason", this.getErrorReason(errorCode));
            data.put("adType", this.getAdType());
        } catch (JSONException e) {
            e.printStackTrace();
            this.fireAdEvent("onFailedToReceiveAd");
            return;
        }
        this.fireAdEvent("onFailedToReceiveAd", data);
    }

    @Override
    public void onAdLeftApplication() {
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

    /**
     * Gets a string error reason from an error code.
     */
    public String getErrorReason(int errorCode) {
        String errorReason = "";
        switch (errorCode) {
            case AdRequest.ERROR_CODE_INTERNAL_ERROR:
                errorReason = "Internal error";
                break;
            case AdRequest.ERROR_CODE_INVALID_REQUEST:
                errorReason = "Invalid request";
                break;
            case AdRequest.ERROR_CODE_NETWORK_ERROR:
                errorReason = "Network Error";
                break;
            case AdRequest.ERROR_CODE_NO_FILL:
                errorReason = "No fill";
                break;
        }
        return errorReason;
    }
}
