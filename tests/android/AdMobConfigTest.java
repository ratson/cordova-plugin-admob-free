package name.ratson.cordova.admob;

import com.google.android.gms.ads.AdSize;

import org.json.JSONException;
import org.json.JSONObject;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.junit.runners.JUnit4;

import static org.junit.Assert.assertEquals;

@RunWith(JUnit4.class)
public class AdMobConfigTest {
    @Test
    public void canSetBannerUnitId() throws JSONException {
        AdMobConfig config = new AdMobConfig();
        assertEquals(config.getBannerAdUnitId(), "");

        config.setOptions(new JSONObject("{\"publisherId\": \"bannerAdUnitId\"}"));
        assertEquals(config.getBannerAdUnitId(), "bannerAdUnitId");
    }

    @Test
    public void canSetInterstitialAdUnitId() throws JSONException {
        AdMobConfig config = new AdMobConfig();
        assertEquals(config.getInterstitialAdUnitId(), "");

        config.setOptions(new JSONObject("{\"interstitialAdId\": \"interstitialAdUnitId\"}"));
        assertEquals(config.getInterstitialAdUnitId(), "interstitialAdUnitId");
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

    @Test
    public void canSetAdUnitIds() throws JSONException {
        AdMobConfig config = new AdMobConfig();
        assertEquals(config.getBannerAdUnitId(), "ca-app-pub-3940256099942544/6300978111");
        assertEquals(config.getInterstitialAdUnitId(), "ca-app-pub-3940256099942544/1033173712");

        config.setOptions(new JSONObject("{\"publisherId\": \"banner-id\"}"));
        assertEquals(config.getBannerAdUnitId(), "banner-id");

        config.setOptions(new JSONObject("{\"interstitialAdId\": \"interstitial-Id\"}"));
        assertEquals(config.getInterstitialAdUnitId(), "interstitial-Id");
    }
}
