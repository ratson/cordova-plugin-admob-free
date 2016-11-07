package name.ratson.cordova.admob;

import com.google.android.gms.ads.AdSize;

import org.json.JSONException;
import org.json.JSONObject;
import org.junit.Test;

import static org.junit.Assert.assertEquals;

public class AdMobConfigTest {
    @Test
    public void canSetBannerUnitId() throws JSONException {
        AdMobConfig config = new AdMobConfig();
        assertEquals(config.bannerAdUnitId, "");

        config.setOptions(new JSONObject("{\"publisherId\": \"bannerAdUnitId\"}"));
        assertEquals(config.bannerAdUnitId, "bannerAdUnitId");
    }

    @Test
    public void canSetInterstitialAdUnitId() throws JSONException {
        AdMobConfig config = new AdMobConfig();
        assertEquals(config.interstitialAdUnitId, "");

        config.setOptions(new JSONObject("{\"interstitialAdId\": \"interstitialAdUnitId\"}"));
        assertEquals(config.interstitialAdUnitId, "interstitialAdUnitId");
    }

    @Test
    public void canSetAdSize() throws JSONException {
        AdMobConfig config = new AdMobConfig();
        assertEquals(config.adSize, AdSize.SMART_BANNER);

        config.setOptions(new JSONObject("{\"adSize\": \"BANNER\"}"));
        assertEquals(config.adSize, AdSize.BANNER);

        config.setOptions(new JSONObject("{\"adSize\": \"FULL_BANNER\"}"));
        assertEquals(config.adSize, AdSize.FULL_BANNER);

        config.setOptions(new JSONObject("{\"adSize\": \"LARGE_BANNER\"}"));
        assertEquals(config.adSize, AdSize.LARGE_BANNER);

        config.setOptions(new JSONObject("{\"adSize\": \"LEADERBOARD\"}"));
        assertEquals(config.adSize, AdSize.LEADERBOARD);

        config.setOptions(new JSONObject("{\"adSize\": \"MEDIUM_RECTANGLE\"}"));
        assertEquals(config.adSize, AdSize.MEDIUM_RECTANGLE);

        config.setOptions(new JSONObject("{\"adSize\": \"WIDE_SKYSCRAPER\"}"));
        assertEquals(config.adSize, AdSize.WIDE_SKYSCRAPER);

        config.setOptions(new JSONObject("{\"adSize\": \"FLUID\"}"));
        assertEquals(config.adSize, AdSize.FLUID);

        config.setOptions(new JSONObject("{\"adSize\": \"SEARCH\"}"));
        assertEquals(config.adSize, AdSize.SEARCH);

        // backward-compatibility
        config.setOptions(new JSONObject("{\"adSize\": \"IAB_BANNER\"}"));
        assertEquals(config.adSize, AdSize.FULL_BANNER);

        config.setOptions(new JSONObject("{\"adSize\": \"IAB_MRECT\"}"));
        assertEquals(config.adSize, AdSize.MEDIUM_RECTANGLE);

        config.setOptions(new JSONObject("{\"adSize\": \"IAB_LEADERBOARD\"}"));
        assertEquals(config.adSize, AdSize.LEADERBOARD);
    }
}
