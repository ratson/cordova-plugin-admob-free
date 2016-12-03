package name.ratson.cordova.admob;

import org.json.JSONException;
import org.json.JSONObject;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.junit.runners.JUnit4;

import static org.junit.Assert.assertEquals;

@RunWith(JUnit4.class)
public class CordovaEventBuilderTest {
    @Test
    public void buildEvent() {
        String js = new CordovaEventBuilder("event").build();
        assertEquals(js, "javascript:cordova.fireDocumentEvent('event');");
    }

    @Test
    public void ignoreEmptyData() {
        String js = new CordovaEventBuilder("event").withData("").build();
        assertEquals(js, "javascript:cordova.fireDocumentEvent('event');");
    }

    @Test
    public void buildEventWithData() {
        String js = new CordovaEventBuilder("event").withData("{}").build();
        assertEquals(js, "javascript:cordova.fireDocumentEvent('event',{});");
    }

    @Test
    public void buildEventWithJSONObject() throws JSONException {
        JSONObject jsonObj = new JSONObject("{\"data\": 1}");
        assertEquals(jsonObj.toString(), "{\"data\":1}");
        CordovaEventBuilder builder = new CordovaEventBuilder("event").withData(jsonObj);
        String js = builder.build();
        assertEquals(js, "javascript:cordova.fireDocumentEvent('event',{\"data\":1});");


        jsonObj = new JSONObject();
        assertEquals(jsonObj.toString(), "{}");
        js = builder.withData(jsonObj).build();
        assertEquals(js, "javascript:cordova.fireDocumentEvent('event',{});");
    }
}
