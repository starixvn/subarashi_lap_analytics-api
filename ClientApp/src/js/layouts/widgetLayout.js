import localStorageHelper from '../lib/localStorageHelper';

export default class widgetLayout {
    static getWidgetStatusSettings(permissions = [], widgetKey) {
        const defaultKeys = Object.keys(widgetKey).map(key => widgetKey[key]);
        const wgStatus = [];

        permissions.forEach(key => {
            if (defaultKeys.indexOf(key) >= 0) {
                wgStatus.push({
                    key: key,
                    enable: true
                });
            }
        });

        return wgStatus;
    }

    static mergeWidgetStatusSettingsToUserSettings(widgetStatus) {
        const userWgStatus = localStorageHelper.getWidgetStatus();

        return userWgStatus
            ? widgetStatus.map(widget => Object.assign({}, widget, userWgStatus.find(w => w.key === widget.key)))
            : widgetStatus;
    }

    static getWidgetLayoutSettings(wgLayouts) {
        let userWidgetLayouts = localStorageHelper.getWidgetLayout();

        if (!userWidgetLayouts) {
            userWidgetLayouts = widgetLayout._createEmptyLayout(wgLayouts);
        }
        const widgetStatus = localStorageHelper.getWidgetStatus();
        let layouts = widgetLayout._createEmptyLayout(wgLayouts);

        widgetStatus.forEach(status => {
            if(status.enable){
                let _widgetLayout =
                    widgetLayout._getWidgetLayoutByKey(userWidgetLayouts, status.key) ||
                    widgetLayout._getWidgetLayoutByKey(wgLayouts, status.key);
                layouts = widgetLayout._addWidgetToLayout(layouts, _widgetLayout);
            }
        });

        return layouts;
    }

    static syncLayoutWithNewStatus(newStatus, widgetStatus, _widgetLayouts, wgLayouts) {
        let newWidgetStatus = [...widgetStatus];
        let newLayouts = Object.assign({}, _widgetLayouts);
        let removedLayouts = localStorageHelper.getRemovedWidgetLayout();
        const me = widgetLayout;

        newStatus.forEach(widgetItem => {
            let status = newWidgetStatus.find(w => w.key === widgetItem.key);
            if(status.enable === widgetItem.enable){
                return;
            }
            status.enable = widgetItem.enable;

            if (status.enable) {
                const addedItem = removedLayouts[widgetItem.key] || me._getWidgetLayoutByKey(wgLayouts, widgetItem.key);
                newLayouts = me._addWidgetToLayout(newLayouts, addedItem);
                removedLayouts[widgetItem.key] = null;
            } else {
                const remmovedItem = me._getWidgetLayoutByKey(newLayouts, widgetItem.key);
                newLayouts = me._removeWidgetFromLayout(newLayouts, widgetItem.key);
                removedLayouts[widgetItem.key] = remmovedItem;
            }
        });

        return {newWidgetStatus, newLayouts, removedLayouts};
    }

    static _removeWidgetFromLayout(layouts, key) {
        const filter = w => w.i !== key;
        const newLayout = {};

        if (layouts) {
            Object.keys(layouts).forEach(breakpoint => {
                newLayout[breakpoint] = layouts[breakpoint].filter(filter);
            });
        }

        return newLayout;
    }

    static _addWidgetToLayout(layouts, _widgetLayout) {
        const newLayouts = {};
        const breakpoints = Object.keys(layouts);
        let isValid = true;

        breakpoints .forEach(breakpoint => {
            if(!_widgetLayout[breakpoint]) {
                return (isValid = false);
            }
            newLayouts[breakpoint] = [...layouts[breakpoint]];
            newLayouts[breakpoint].push(_widgetLayout[breakpoint]);
        });

        return isValid ? newLayouts : layouts;
    }

    static _getWidgetLayoutByKey(layouts, key) {
        const _widgetLayout = {};
        let isValid = true;

        Object.keys(layouts).forEach(breakpoint => {
            _widgetLayout[breakpoint] = layouts[breakpoint].find( w => w.i === key);
            if(!_widgetLayout[breakpoint] ){
                isValid = false;
            }
        });

        return isValid ? _widgetLayout : null;
    }

    static _createEmptyLayout(wgLayouts){
        const layouts = {};
        Object.keys(wgLayouts).forEach(breakpoint => layouts[breakpoint] = []);
        return layouts;
    }
}
