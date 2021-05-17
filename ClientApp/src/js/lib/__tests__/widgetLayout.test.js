import widgetLayout from '../../layouts/widgetLayout';
import localStorageHelper from '../localStorageHelper';
import { wgLayouts } from '../constants';

jest.mock('../localStorageHelper')

describe('removeWidgetFromLayout(layouts, key)', () => {
    const layouts = {
        a:[{ i: 'a' }, { i: 'bb' }, { i: 'ccc' }],
        b:[{ i: 'a' }, { i: 'bb' }, { i: 'ccc' }],
        c:[{ i: 'a' }, { i: 'bb' }, { i: 'ccc' }],
    };

    test('key is found: remove widget from all breakpoints', () => {
        const newLayout = widgetLayout._removeWidgetFromLayout(layouts, 'bb');
        expect(newLayout).toEqual({
            a:[{ i: 'a' }, { i: 'ccc' }],
            b:[{ i: 'a' }, { i: 'ccc' }],
            c:[{ i: 'a' }, { i: 'ccc' }],
        });
    });

    test('key is nout found: return layouts', () => {
        const newLayout = widgetLayout._removeWidgetFromLayout(layouts, 'not found');
        expect(newLayout).toEqual(layouts);
    });

    test('no layouts: return empty object', () => {
        const newLayout = widgetLayout._removeWidgetFromLayout(null, 'anykey');
        expect(newLayout).toEqual({});
    });
});

describe('_addWidgetToLayout(layouts, _widgetLayout)', () => {
    test('add given layout to layout settings', () => {
        const layouts = {
            lg:[{ i: 'aaa' }, { i: 'bb' }, { i: 'c' }],
            md:[{ i: 'aaa' }, { i: 'bb' }, { i: 'c' }],
            sm:[{ i: 'aaa' }, { i: 'bb' }, { i: 'c' }],
            xs:[{ i: 'aaa' }, { i: 'bb' }, { i: 'c' }],
            xxs:[{ i: 'aaa' }, { i: 'bb' }, { i: 'c' }]
        };
        const _widgetLayout = {
            lg: { i: 'n_reg_customer', a:1, b:2 },
            md: { i: 'n_reg_customer', a:1, b:2 },
            sm: { i: 'n_reg_customer', a:1, b:2 },
            xs: { i: 'n_reg_customer', a:1, b:2 },
            xxs: { i: 'n_reg_customer', a:1, b:2 }
        };

        const newLayouts = widgetLayout._addWidgetToLayout(layouts, _widgetLayout);
        expect(newLayouts).toEqual({
            lg:[{ i: 'aaa' }, { i: 'bb' }, { i: 'c' }, { i: 'n_reg_customer', a:1, b:2 }],
            md:[{ i: 'aaa' }, { i: 'bb' }, { i: 'c' }, { i: 'n_reg_customer', a:1, b:2 }],
            sm:[{ i: 'aaa' }, { i: 'bb' }, { i: 'c' }, { i: 'n_reg_customer', a:1, b:2 }],
            xs:[{ i: 'aaa' }, { i: 'bb' }, { i: 'c' }, { i: 'n_reg_customer', a:1, b:2 }],
            xxs:[{ i: 'aaa' }, { i: 'bb' }, { i: 'c' },{ i: 'n_reg_customer', a:1, b:2 }]
        });
    });

    test('invalid widget layout: do not add', () => {
        const layouts = {
            lg:[{ i: 'aaa' }, { i: 'bb' }, { i: 'c' }],
            md:[{ i: 'aaa' }, { i: 'bb' }, { i: 'c' }],
            sm:[{ i: 'aaa' }, { i: 'bb' }, { i: 'c' }],
            xs:[{ i: 'aaa' }, { i: 'bb' }, { i: 'c' }],
            xxs:[{ i: 'aaa' }, { i: 'bb' }, { i: 'c' }]
        };
        const invalidWidgetLayout = {
            lg:null,
            md:'i dont know',
            sm:null,
            xs:null,
            xxs:'something else'
        };

        const newLayouts = widgetLayout._addWidgetToLayout(layouts, invalidWidgetLayout);
        expect(newLayouts).toBe(layouts);
    });
});

describe('getWidgetLayoutSettings()', () => {
    const widgetIsEnabledFilter = w => enabledWidgets.indexOf(w.i) >= 0;
    const enabledWidgets = ['c_turnover', 'c_online_customer', 'n_deposit', 'n_withdrawal'];
    localStorageHelper.getWidgetStatus.mockImplementation(() => [
        {key: 'c_turnover', enable: true},
        {key: 'c_licWL',enable: false},
        {key: 'n_reg_customer',enable: false},
        {key: 'c_online_customer',enable: true},
        {key: 'n_customer_balance',enable: false},
        {key: 'n_pending_funds',enable: false},
        {key: 'n_deposit',enable: true},
        {key: 'n_withdrawal',enable: true}
    ]);

    test('don\'t have local settings: store default settings but remove disabled items', () => {
        localStorageHelper.getWidgetLayout.mockImplementation(() => null);
        localStorageHelper.saveWidgetLayout.mockReset();
        const actual = widgetLayout.getWidgetLayoutSettings({});
        const expected = {
            lg: wgLayouts.lg.filter(widgetIsEnabledFilter),
            md: wgLayouts.md.filter(widgetIsEnabledFilter),
            sm: wgLayouts.sm.filter(widgetIsEnabledFilter),
            xs: wgLayouts.xs.filter(widgetIsEnabledFilter),
            xxs:wgLayouts.xxs.filter(widgetIsEnabledFilter),
        };

        expect(actual).toEqual(expected);
    });

    test('return local settings but remove disabled items', () => {
        const emptyLayouts = {lg:[], md:[], sm:[], xs:[], xxs:[]};
        let userLayouts = widgetLayout._addWidgetToLayout(emptyLayouts, widgetLayout._getWidgetLayoutByKey(wgLayouts, 'c_turnover'));
        userLayouts = widgetLayout._addWidgetToLayout(userLayouts, widgetLayout._getWidgetLayoutByKey(wgLayouts, 'n_reg_customer'));

        localStorageHelper.getWidgetLayout.mockImplementation(() => userLayouts);
        localStorageHelper.saveWidgetLayout.mockReset();
        const actual = widgetLayout.getWidgetLayoutSettings();
        let expected = widgetLayout._addWidgetToLayout(emptyLayouts, widgetLayout._getWidgetLayoutByKey(wgLayouts, 'c_turnover'));
        expected = widgetLayout._addWidgetToLayout(expected, widgetLayout._getWidgetLayoutByKey(wgLayouts, 'c_online_customer'));
        expected = widgetLayout._addWidgetToLayout(expected, widgetLayout._getWidgetLayoutByKey(wgLayouts, 'n_deposit'));
        expected = widgetLayout._addWidgetToLayout(expected, widgetLayout._getWidgetLayoutByKey(wgLayouts, 'n_withdrawal'));

        expect(actual).toEqual(expected);
    });
});
