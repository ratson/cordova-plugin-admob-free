package name.ratson.cordova.admob;

import org.json.JSONObject;

public class CordovaEventBuilder {
    private String eventName;
    private String jsonData;

    public CordovaEventBuilder(String eventName) {
        this.eventName = eventName;
    }

    public CordovaEventBuilder withData(String data) {
        this.jsonData = data;
        return this;
    }

    public CordovaEventBuilder withData(JSONObject jsonObj) {
        if (jsonObj == null) {
            return withData("");
        }
        return withData(jsonObj.toString());
    }

    public String build() {
        StringBuilder js = new StringBuilder();
        js.append("javascript:cordova.fireDocumentEvent('");
        js.append(eventName);
        js.append("'");
        if (jsonData != null && !"".equals(jsonData)) {
            js.append(",");
            js.append(jsonData);
        }
        js.append(");");
        return js.toString();
    }
}
