package name.ratson.cordova.admob;

import com.google.android.gms.ads.AdRequest;

import org.json.JSONObject;

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
public abstract class AbstractExecutor {
    protected AdMob plugin;

    public AbstractExecutor(AdMob plugin) {
        this.plugin = plugin;
    }

    /**
     * Gets a string error reason from an error code.
     */
    public static String getErrorReason(int errorCode) {
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

    public abstract String getAdType();

    public abstract void destroy();

    public void fireAdEvent(String eventName) {
        String js = new CordovaEventBuilder(eventName).build();
        loadJS(js);
    }

    public void fireAdEvent(String eventName, JSONObject data) {
        String js = new CordovaEventBuilder(eventName).withData(data).build();
        loadJS(js);
    }

    private void loadJS(String js) {
        plugin.webView.loadUrl(js);
    }
}
