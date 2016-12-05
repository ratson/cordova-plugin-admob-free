package name.ratson.cordova.admob;

import android.util.Log;

import com.google.android.gms.ads.AdSize;

import org.json.JSONException;
import org.json.JSONObject;
import org.junit.Before;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.powermock.api.mockito.PowerMockito;
import org.powermock.core.classloader.annotations.PrepareForTest;
import org.powermock.modules.junit4.PowerMockRunner;

import static org.junit.Assert.assertEquals;

@RunWith(PowerMockRunner.class)
@PrepareForTest({Log.class})
public class AdMobConfigTest {
    @Before
    public void setup() {
        PowerMockito.mockStatic(Log.class);
    }

    @Test
    public void canSetBannerUnitId() throws JSONException {
        AdMobConfig config = new AdMobConfig();
        assertEquals("ca-app-pub-3940256099942544/6300978111", config.getBannerAdUnitId());

        config.setOptions(new JSONObject("{\"publisherId\": \"banner-id\"}"));
        assertEquals("banner-id", config.getBannerAdUnitId());
    }

    @Test
    public void canSetInterstitialAdUnitId() throws JSONException {
        AdMobConfig config = new AdMobConfig();
        assertEquals("ca-app-pub-3940256099942544/1033173712", config.getInterstitialAdUnitId());

        config.setOptions(new JSONObject("{\"interstitialAdId\": \"interstitial-id\"}"));
        assertEquals("interstitial-id", config.getInterstitialAdUnitId());
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
    public void canSetAutoShow() throws JSONException {
        AdMobConfig config = new AdMobConfig();
        assertEquals(true, config.autoShow);
        assertEquals(true, config.autoShowBanner);
        assertEquals(true, config.autoShowInterstitial);

        config.setOptions(new JSONObject("{\"autoShow\": false}"));
        assertEquals(false, config.autoShow);
        assertEquals(true, config.autoShowBanner);
        assertEquals(true, config.autoShowInterstitial);

        config.setOptions(new JSONObject("{\"autoShow\": true}"));
        assertEquals(true, config.autoShow);
        assertEquals(true, config.autoShowBanner);
        assertEquals(true, config.autoShowInterstitial);

        config.setBannerOptions(new JSONObject("{\"autoShow\": false}"));
        assertEquals(true, config.autoShow);
        assertEquals(false, config.autoShowBanner);
        assertEquals(true, config.autoShowInterstitial);

        config.setBannerOptions(new JSONObject("{\"autoShowBanner\": true}"));
        assertEquals(true, config.autoShow);
        assertEquals(true, config.autoShowBanner);
        assertEquals(true, config.autoShowInterstitial);

        config.setInterstitialOptions(new JSONObject("{\"autoShow\": false}"));
        assertEquals(true, config.autoShow);
        assertEquals(true, config.autoShowBanner);
        assertEquals(false, config.autoShowInterstitial);
    }
}
