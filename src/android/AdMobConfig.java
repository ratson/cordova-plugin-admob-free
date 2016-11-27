package name.ratson.cordova.admob;

import android.location.Location;

import com.google.android.gms.ads.AdSize;

import org.json.JSONArray;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.List;

public class AdMobConfig {
    /* options */
    private static final String OPT_APP_ID = "appId";
    private static final String OPT_PUBLISHER_ID = "publisherId";
    private static final String OPT_INTERSTITIAL_AD_ID = "interstitialAdId";
    private static final String OPT_AD_SIZE = "adSize";
    private static final String OPT_BANNER_AT_TOP = "bannerAtTop";
    private static final String OPT_OVERLAP = "overlap";
    private static final String OPT_OFFSET_TOPBAR = "offsetTopBar";
    private static final String OPT_IS_TESTING = "isTesting";
    private static final String OPT_AD_EXTRAS = "adExtras";
    private static final String OPT_AUTO_SHOW = "autoShow";

    public static final String OPT_TEST_DEVICES = "testDevices";

    private static final String OPT_LOCATION = "location";

    private static final String OPT_GENDER = "gender";
    private static final String OPT_FORCHILD = "forChild";
    private static final String OPT_FORFAMILY = "forFamily";
    private static final String OPT_CONTENTURL = "contentUrl";
    private static final String OPT_EXCLUDE = "exclude";

    public boolean isTesting = false;
    public JSONObject adExtras = null;
    public boolean autoShow = true;

    public String gender = null;
    public String forChild = null;
    public String forFamily = null;
    public String contentURL = null;
    public JSONArray exclude = null;

    public List<String> testDeviceList = null;

    public Location location = null;

    // app
    public String appId = "";
    
    // banner
    public String bannerAdUnitId = "";
    public AdSize adSize = AdSize.SMART_BANNER;
    /**
     * Whether or not the ad should be positioned at top or bottom of screen.
     */
    public boolean bannerAtTop = false;
    /**
     * Whether or not the banner will overlap the webview instead of push it up or down
     */
    public boolean bannerOverlap = false;
    public boolean offsetTopBar = false;

    // interstial
    public String interstitialAdUnitId = "";

    public void setOptions(JSONObject options) {
        if (options == null) {
            return;
        }

        if (options.has(OPT_APP_ID)) {
          this.appId = options.optString(OPT_APP_ID);
        }
        if (options.has(OPT_PUBLISHER_ID)) {
            this.bannerAdUnitId = options.optString(OPT_PUBLISHER_ID);
        }
        if (options.has(OPT_INTERSTITIAL_AD_ID)) {
            this.interstitialAdUnitId = options.optString(OPT_INTERSTITIAL_AD_ID);
        }
        if (options.has(OPT_AD_SIZE)) {
            this.adSize = adSizeFromString(options.optString(OPT_AD_SIZE));
        }
        if (options.has(OPT_BANNER_AT_TOP)) {
            this.bannerAtTop = options.optBoolean(OPT_BANNER_AT_TOP);
        }
        if (options.has(OPT_OVERLAP)) {
            this.bannerOverlap = options.optBoolean(OPT_OVERLAP);
        }
        if (options.has(OPT_OFFSET_TOPBAR)) {
            this.offsetTopBar = options.optBoolean(OPT_OFFSET_TOPBAR);
        }
        if (options.has(OPT_IS_TESTING)) {
            this.isTesting = options.optBoolean(OPT_IS_TESTING);
        }
        if (options.has(OPT_AD_EXTRAS)) {
            this.adExtras = options.optJSONObject(OPT_AD_EXTRAS);
        }
        if (options.has(OPT_AUTO_SHOW)) {
            this.autoShow = options.optBoolean(OPT_AUTO_SHOW);
        }

        if (options.has(OPT_LOCATION)) {
            JSONArray location = options.optJSONArray(OPT_LOCATION);
            if (location != null) {
                this.location = new Location("dummyprovider");
                this.location.setLatitude(location.optDouble(0, 0.0));
                this.location.setLongitude(location.optDouble(1, 0));
            }
        }

        if (options.has(OPT_GENDER)) {
            gender = options.optString(OPT_GENDER);
        }
        if (options.has(OPT_FORCHILD)) {
            forChild = options.optString(OPT_FORCHILD);
        }
        if (options.has(OPT_FORFAMILY)) {
            forFamily = options.optString(OPT_FORFAMILY);
        }
        if (options.has(OPT_CONTENTURL)) {
            contentURL = options.optString(OPT_CONTENTURL);
        }
        if (options.has(OPT_EXCLUDE)) {
            exclude = options.optJSONArray(OPT_EXCLUDE);
        }

        if (options.has(OPT_TEST_DEVICES)) {
            JSONArray testDevices = options.optJSONArray(OPT_TEST_DEVICES);
            if (testDevices != null) {
                testDeviceList = new ArrayList<String>();
                for (int i = 0; i < testDevices.length(); i++) {
                    testDeviceList.add(testDevices.optString(i));
                }
            }
        }
    }

    /**
     * Gets an AdSize object from the string size passed in from JavaScript.
     * Returns null if an improper string is provided.
     *
     * @param size The string size representing an ad format constant.
     * @return An AdSize object used to create a banner.
     */
    private static AdSize adSizeFromString(String size) {
        if ("BANNER".equals(size)) {
            return AdSize.BANNER;
        } else if ("FULL_BANNER".equals(size)) {
            return AdSize.FULL_BANNER;
        } else if ("LARGE_BANNER".equals(size)) {
            return AdSize.LARGE_BANNER;
        } else if ("LEADERBOARD".equals(size)) {
            return AdSize.LEADERBOARD;
        } else if ("MEDIUM_RECTANGLE".equals(size)) {
            return AdSize.MEDIUM_RECTANGLE;
        } else if ("WIDE_SKYSCRAPER".equals(size)) {
            return AdSize.WIDE_SKYSCRAPER;
        } else if ("SMART_BANNER".equals(size)) {
            return AdSize.SMART_BANNER;
        } else if ("FLUID".equals(size)) {
            return AdSize.FLUID;
        } else if ("SEARCH".equals(size)) {
            return AdSize.SEARCH;
        } else if ("IAB_BANNER".equals(size)) {
            return AdSize.FULL_BANNER;
        } else if ("IAB_MRECT".equals(size)) {
            return AdSize.MEDIUM_RECTANGLE;
        } else if ("IAB_LEADERBOARD".equals(size)) {
            return AdSize.LEADERBOARD;
        }
        return null;
    }
}
