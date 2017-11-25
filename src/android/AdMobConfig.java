package name.ratson.cordova.admob;

import android.location.Location;
import android.util.Log;

import com.google.android.gms.ads.AdSize;

import org.json.JSONArray;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.List;

public class AdMobConfig {
    /* options */
    private static final String OPT_PUBLISHER_ID = "publisherId";
    private static final String OPT_INTERSTITIAL_AD_ID = "interstitialAdId";
    private static final String OPT_REWARD_VIDEO_ID = "rewardVideoId";
    private static final String OPT_AD_SIZE = "adSize";
    private static final String OPT_BANNER_AT_TOP = "bannerAtTop";
    private static final String OPT_OVERLAP = "overlap";
    private static final String OPT_OFFSET_TOPBAR = "offsetTopBar";
    private static final String OPT_IS_TESTING = "isTesting";
    private static final String OPT_AD_EXTRAS = "adExtras";

    public static final String OPT_AUTO_SHOW = "autoShow";
    private static final String OPT_AUTO_SHOW_BANNER = "autoShowBanner";
    public static final String OPT_AUTO_SHOW_INTERSTITIAL = "autoShowInterstitial";

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
    public boolean autoShowBanner = true;
    public boolean autoShowInterstitial = true;
    public boolean autoShowRewardVideo = false;

    public String gender = null;
    public String forChild = null;
    public String forFamily = null;
    public String contentURL = null;
    public JSONArray exclude = null;

    public List<String> testDeviceList = null;

    public Location location = null;

    // Banner
    private static final String TEST_BANNER_ID = "ca-app-pub-3940256099942544/6300978111";
    private String bannerAdUnitId = "";
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

    // Interstial
    private static final String TEST_INTERSTITIAL_ID = "ca-app-pub-3940256099942544/1033173712";
    private String interstitialAdUnitId = "";

    // Reward Video
    private static final String TEST_REWARDED_VIDEO_ID = "ca-app-pub-3940256099942544/5224354917";
    private String rewardVideoId = "";


    public void setOptions(JSONObject options) {
        if (options == null) {
            return;
        }

        if (options.has(OPT_PUBLISHER_ID)) {
            this.bannerAdUnitId = options.optString(OPT_PUBLISHER_ID);
        }
        if (options.has(OPT_INTERSTITIAL_AD_ID)) {
            this.interstitialAdUnitId = options.optString(OPT_INTERSTITIAL_AD_ID);
        }
        if (options.has(OPT_REWARD_VIDEO_ID)) {
            this.rewardVideoId = options.optString(OPT_REWARD_VIDEO_ID);
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
        if (options.has(OPT_AUTO_SHOW_BANNER)) {
            this.autoShowBanner = options.optBoolean(OPT_AUTO_SHOW_BANNER);
        }
        if (options.has(OPT_AUTO_SHOW_INTERSTITIAL)) {
            this.autoShowInterstitial = options.optBoolean(OPT_AUTO_SHOW_INTERSTITIAL);
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

    public void setBannerOptions(JSONObject options) {
        try {
            this.autoShowBanner = (Boolean) options.remove(OPT_AUTO_SHOW);
        } catch (NullPointerException ignored) {
        }
        this.setOptions(options);
    }

    public void setInterstitialOptions(JSONObject options) {
        try {
            this.autoShowInterstitial = (Boolean) options.remove(OPT_AUTO_SHOW);
        } catch (NullPointerException ignored) {
        }
        this.setOptions(options);
    }

    public void setRewardVideoOptions(JSONObject options) {
        try {
            this.autoShowRewardVideo = (Boolean) options.remove(OPT_AUTO_SHOW);
        } catch (NullPointerException ignored) {
        }
        this.setOptions(options);
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

    public String getBannerAdUnitId() {
        if (isEmptyAdUnitId(bannerAdUnitId)) {
            // in case the user does not enter their own publisher id
            Log.e("banner", "Please put your AdMob id into the javascript code. Test ad is used.");
            return TEST_BANNER_ID;
        }
        return bannerAdUnitId;
    }

    public String getInterstitialAdUnitId() {
        if (isEmptyAdUnitId(interstitialAdUnitId)) {
            // in case the user does not enter their own publisher id
            Log.e("interstitial", "Please put your AdMob id into the javascript code. Test ad is used.");
            return TEST_INTERSTITIAL_ID;
        }
        return interstitialAdUnitId;
    }

    public String getRewardedVideoAdUnitId() {
        if (isEmptyAdUnitId(rewardVideoId)) {
            // in case the user does not enter their own publisher id
            Log.e("rewardedvideo", "Please put your AdMob id into the javascript code. Test ad is used.");
            return TEST_REWARDED_VIDEO_ID;
        }
        return rewardVideoId;
    }

    private static boolean isEmptyAdUnitId(String adId) {
        return adId.length() == 0 || adId.indexOf("xxxx") > 0;
    }
}
