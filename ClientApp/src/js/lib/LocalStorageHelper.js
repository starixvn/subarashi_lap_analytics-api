const widgetVersionCacheKey = "_LAP_Dashboard_Wg_Version";
const widgetStatusCacheKey = "_LAP_Dashboard_Wg_Status";
const widgetLayoutCacheKey = "_LAP_Dashboard_Wg_Layout";
const widgetLayoutRemovedCacheKey = "_LAP_Dashboard_Wg_Layout_removed";
const widgetReloadCountdownCacheKey = "_LAP_Dashboard_Wg_Reload_Countdown";

export default class localStorageHelper {
    static clearLayoutSettingIfNewVersion() {
        var scriptElement = document.getElementById("dashboard-script");
        if (!scriptElement) {
            return;
        }

        var jsVersions = scriptElement.src.match(/v=\d*/);
        if (!jsVersions) {
            return;
        }

        var newVersion = jsVersions[0];
        var cacheVersion = window.localStorage.getItem(widgetVersionCacheKey);
        if (newVersion !== cacheVersion) {
            window.localStorage.setItem(widgetVersionCacheKey, newVersion);
            localStorageHelper.clearWidgetCache();
        }
    }
    static clearWidgetCache() {
        window.localStorage.removeItem(widgetStatusCacheKey);
        window.localStorage.removeItem(widgetLayoutCacheKey);
        window.localStorage.removeItem(widgetLayoutRemovedCacheKey);
    }
    static saveWidgetStatus(wgs) {
        try {
            window.localStorage.setItem(widgetStatusCacheKey, JSON.stringify(wgs))
        } catch (error) {
            alert("To continue use dashboard. Please turn private browsing off!!")
        }
    }
    static getWidgetStatus() {
        var item = window.localStorage.getItem(widgetStatusCacheKey)
        return item && item !== "undefined" && item !== "null" ? JSON.parse(item) : null;
    }
    static saveWidgetLayout(layout) {
        try {
            window.localStorage.setItem(widgetLayoutCacheKey, JSON.stringify(layout))
        } catch (error) {
            alert("To continue use dashboard. Please turn private browsing off!!")
        }
    }
    static getWidgetLayout() {
        var item = window.localStorage.getItem(widgetLayoutCacheKey)
        return item && item !== "undefined" && item !== "null" ? JSON.parse(item) : null;
    }
    static saveRemovedWidgetLayout(layout) {
        try {
            window.localStorage.setItem(widgetLayoutRemovedCacheKey, JSON.stringify(layout))
        } catch (error) {
            alert("To continue use dashboard. Please turn private browsing off!!");
        }
    }
    static getRemovedWidgetLayout() {
        var item = window.localStorage.getItem(widgetLayoutRemovedCacheKey)
        return item && item !== "undefined" && item !== "null" ? JSON.parse(item) : {};
    }
    static getWidgetReloadCountdown() {
        var item = window.localStorage.getItem(widgetReloadCountdownCacheKey)
        return item && item !== "undefined" && item !== "null" ? JSON.parse(item) : {};
    }
    static saveWidgetReloadCountdown(values) {
        try {
            window.localStorage.setItem(widgetReloadCountdownCacheKey, JSON.stringify(values))
        } catch (error) {
          window.alert("To continue use dashboard. Please turn private browsing off!!");
        }
    }
}
