import React from 'react';
import ReactDOM from 'react-dom';
import renderer from 'react-test-renderer';
import * as ReactGridLayout from 'react-grid-layout';
import DefaultLayout from '../../../layouts/DefaultLayout';

import { widgetKey, wgLayouts } from '../modules/constants';
import widgetLayout from '../../../modules/widgetLayout';
import localStorageHelper from '../../../lib/localStorageHelper';

import Turnover from '../components/chart/Turnover';
import OnlineCustomer from '../components/chart/OnlineCustomer';
import LicenseeWinLoss from '../components/chart/LicenseeWinLoss';

import Deposit from '../components/normal/Deposit';
import Withdrawal from '../components/normal/Withdrawal';
import PendingFunds from '../components/normal/PendingFunds';
import CustomerBalance from '../components/normal/CustomerBalance';
import RegisteredCustomer from '../components/normal/RegisteredCustomer';

jest.mock('react-dom');
jest.mock('react-grid-layout');
jest.mock('../../../layouts/DefaultLayout');
jest.mock('../../../lib/localStorageHelper');

jest.mock('../components/normal/Deposit');
jest.mock('../components/normal/Withdrawal');
jest.mock('../components/normal/PendingFunds');
jest.mock('../components/normal/CustomerBalance');
jest.mock('../components/normal/RegisteredCustomer');

jest.mock('../components/chart/Turnover');
jest.mock('../components/chart/OnlineCustomer');
jest.mock('../components/chart/LicenseeWinLoss');

const dashboardAppElement = (function injectDashboardAppElementToDocument() {
    let dashboardAppElement = document.createElement('div');
    dashboardAppElement.id = 'dashboad-app';
    document
        .body
        .appendChild(dashboardAppElement);
    return dashboardAppElement;
})();

const DashboardApp = require('../App.jsx').default;
const fakeWidgetLayout = wgLayouts;
const fakeWidgetStatus = [
    { key: 'c_turnover', enable: true },
    { key: 'c_licWL', enable: true },
    { key: 'n_reg_customer', enable: true },
    { key: 'c_online_customer', enable: true },
    { key: 'n_customer_balance', enable: true },
    { key: 'n_pending_funds', enable: true },
    { key: 'n_deposit', enable: true },
    { key: 'n_withdrawal', enable: true }
];

function getMockComponentRender(name) {
    return {
        render: function () {
            return (
                <div {...this.props} name={name}>
                    {this.props.children}
                </div>
            )
        }
    }
}

function findElementByName(component, name) {
    if (component._component) {
        return findElementByName(component._component, name);
    }

    if (component._currentElement && component._currentElement.props && component._currentElement.props.name === name) {
        return component._currentElement;
    }

    if (component._renderedComponent) {
        return findElementByName(component._renderedComponent, name);
    }

    if (component._renderedChildren) {
        for (let key in component._renderedChildren) {
            let found = findElementByName(component._renderedChildren[key], name);
            if (found) {
                return found;
            }
        }
    }

    return null;
}

beforeAll(() => {
    Deposit.mockImplementation(() => getMockComponentRender('Deposit'));
    OnlineCustomer.mockImplementation(() => getMockComponentRender('OnlineCustomer'));
    PendingFunds.mockImplementation(() => getMockComponentRender('PendingFunds'));
    RegisteredCustomer.mockImplementation(() => getMockComponentRender('RegisteredCustomer'));
    CustomerBalance.mockImplementation(() => getMockComponentRender('CustomerBalance'));
    Withdrawal.mockImplementation(() => getMockComponentRender('Withdrawal'));
    LicenseeWinLoss.mockImplementation(() => getMockComponentRender('LicenseeWinLoss'));
    Turnover.mockImplementation(() => getMockComponentRender('Turnover'));
    DefaultLayout.mockImplementation(() => getMockComponentRender('DefaultLayout'));
    ReactGridLayout
        .WidthProvider
        .mockImplementation(responsive => {
            expect(responsive).toBe(ReactGridLayout.Responsive);
            return props => (
                <div {...props} name="ResponsiveReactGridLayout">
                    {props.children}
                </div>
            );
        });
});

beforeEach(() => {
    window.widgetPermission = [
        'c_turnover',
        'c_licWL',
        'n_deposit',
        'c_online_customer',
        'n_pending_funds',
        'n_reg_customer',
        'n_customer_balance',
        'n_withdrawal'
    ];
    localStorageHelper.mockReset();
    localStorageHelper.getWidgetStatus.mockImplementation(() => fakeWidgetStatus);
    localStorageHelper.getWidgetLayout.mockImplementation(() => fakeWidgetLayout);
    localStorageHelper.getRemovedWidgetLayout.mockImplementation(() => ({}));
});

test('render DashboardApp to #dashboad-app', () => {
    expect(ReactDOM.render).toBeCalledWith((<DashboardApp />), dashboardAppElement);
});

describe('render correctly', () => {
    test('with english as default language', () => {
        window.dashboardLangCode = null;

        const component = renderer.create(<DashboardApp />);
        expect(component.toJSON()).toMatchSnapshot();
    });
    test('with language from global config', () => {
        window.dashboardLangCode = 'ch';

        const component = renderer.create(<DashboardApp />);
        expect(component.toJSON()).toMatchSnapshot();
    });
    test('with saved widget status data', () => {
        localStorageHelper.getWidgetLayout = jest.fn(() => fakeWidgetLayout);
        localStorageHelper.getWidgetStatus = jest.fn(() => [
            {
                key: widgetKey.NormalPendingFund,
                enable: true
            }, {
                key: widgetKey.ChartLicenseeWL,
                enable: false
            }
        ]);

        const component = renderer.create(<DashboardApp />);
        expect(component.toJSON()).toMatchSnapshot();
    });

    test('with saved widget layout data', () => {
        localStorageHelper.getWidgetLayout = jest.fn(() => ({
            lg: wgLayouts.lg,
            xs: wgLayouts.xs,
            md: [],
            sm: [],
            xxs: []
        }));

        const component = renderer.create(<DashboardApp />);
        expect(component.toJSON()).toMatchSnapshot();
    });
});

describe('constructor()', () => {
    test('merge widget permission to local then save', () => {
        const userSettings = [
            { key: 'c_turnover', enable: true },
            { key: 'n_deposit', enable: false },
            { key: 'dont have permission anymore, should be removed', enable: false },
        ];
        localStorageHelper.getWidgetStatus.mockImplementation(() => userSettings);
        window.widgetPermission = [
            'c_turnover',
            'c_licWL',
            'n_deposit'
        ];
        localStorageHelper.saveWidgetStatus.mockReset();
        const app = new DashboardApp();
        expect(localStorageHelper.saveWidgetStatus).toBeCalledWith([
            { key: 'c_turnover', enable: true },
            { key: 'c_licWL', enable: true },
            { key: 'n_deposit', enable: false },
        ]);
    });

    test('don\'t merge widget permission if no user settings', () => {
        localStorageHelper.saveWidgetStatus.mockReset();
        localStorageHelper.getWidgetStatus.mockReset();

        localStorageHelper.getWidgetStatus
            .mockImplementationOnce(() => null)
            .mockImplementationOnce(() => fakeWidgetStatus);
        window.widgetPermission = [
            'c_turnover',
            'c_licWL',
            'n_deposit'
        ];
        const app = new DashboardApp();
        expect(localStorageHelper.saveWidgetStatus).toBeCalledWith([
            { key: 'c_turnover', enable: true },
            { key: 'c_licWL', enable: true },
            { key: 'n_deposit', enable: true },
        ]);
    });

    test('remove widget if not support', () => {
        window.widgetPermission = [
            widgetKey.ChartTurnover,
            widgetKey.ChartLicenseeWL,
            'not support',
            'should be removed'
        ];
        localStorageHelper.saveWidgetStatus.mockReset();
        const app = new DashboardApp();
        expect(localStorageHelper.saveWidgetStatus).toBeCalledWith([
            { key: 'c_turnover', enable: true },
            { key: 'c_licWL', enable: true }
        ]);
    });

    test('clear layout setting if new version', () => {
        const app = new DashboardApp();
        expect(localStorageHelper.clearLayoutSettingIfNewVersion).toBeCalled();
    });

    test('alert when no permission', () => {
        [undefined, []].forEach(permission => {
            window.widgetPermission = permission;
            window.dashboardLangCode = 'en';
            window.alert = jest.fn();
            const app = new DashboardApp();
            expect(window.alert).toBeCalledWith('You do not have sufficient permissions to access this page');
        });
    });

    test('get widget layout then save', () => {
        const _getWidgetLayoutSettings = widgetLayout.getWidgetLayoutSettings;
        widgetLayout.getWidgetLayoutSettings = jest.fn(() => 'fake widget layout settings');
        localStorageHelper.saveWidgetLayout.mockReset();
        const app = new DashboardApp();

        expect(localStorageHelper.saveWidgetLayout).toBeCalledWith('fake widget layout settings');
        widgetLayout.getWidgetLayoutSettings = _getWidgetLayoutSettings;
    });
});

describe('_renderWgItem(key)', () => {
    [
        'c_turnover',
        'c_licWL',
        'n_deposit',
        'c_online_customer',
        'n_pending_funds',
        'n_reg_customer',
        'n_customer_balance',
        'n_withdrawal'
    ].forEach(key => {
        test(`return a correct element that match key: ${key}`, () => {
            const app = new DashboardApp();
            app.state = {
                selectedFilter: 'fake filter',
                refresh: 'fake refresh',
                langCode: 'fake code',
                wgStatus: [{
                    key: 'fake key', enable: false
                }, {
                    key: key, enable: true
                }]
            };
            const widget = renderer.create(
                <div>
                    {app._renderWgItem(key)}
                </div>
            );
            expect(widget.toJSON()).toMatchSnapshot();
        });
    });

    test('callbackParent is mapped to onChildChanged', () => {
        const app = new DashboardApp();
        app.state = { wgStatus: [] };
        app.onChildChanged = jest.fn();

        const widget = renderer.create(
            <div>
                {app._renderWgItem('n_customer_balance')}
            </div>
        );
        const widgetjson = widget.toJSON();
        const callbackParent = widgetjson
            .children[0]
            .children[0]
            .props.callbackParent;
        callbackParent('fake next state');
        expect(app.onChildChanged).toBeCalledWith('fake next state');
    });
});

describe('when get isTouchScreen', () => {
    const _window = global.window;

    afterEach(() => {
        global.window = _window;
    });

    test('if window has ontouchstart: return true', () => {
        const app = new DashboardApp();
        global.window.ontouchstart = null;
        global.navigator.maxTouchPoints = 0;

        expect(app.isTouchScreen).toBeTruthy();
    });

    test('if navigator has maxTouchPoints: return true', () => {
        const app = new DashboardApp();
        delete global.window;
        global.window = {};
        global.navigator.maxTouchPoints = 1;
        expect(app.isTouchScreen).toBeTruthy();
    });
});

describe('onChildChanged(newState)', () => {
    const oldState = {
        refresh: 123456
    };

    test('if action is apply: update state', () => {
        const app = new DashboardApp();
        const newState = {
            filter: 'fake new filter',
            items: [],
            action: 'apply'
        };
        app.setState = jest.fn();
        app.setState.mockReset();
        app.state.isMounted = true;
        app.onChildChanged(newState);

        const asyncSetState = app.setState.mock.calls[0][0];
        expect(asyncSetState(oldState)).toEqual({
            refresh: oldState.refresh + 1,
            selectedFilter: newState.filter
        });
    });

    test('if action is reset: reset layout', () => {
        const app = new DashboardApp();
        app.setState = jest.fn();
        app.state.isMounted = true;
        app._getLayoutSettings = jest.fn(() => 'get layout settings');
        localStorageHelper.clearWidgetCache = jest.fn();

        app.onChildChanged({ action: 'reset' });

        expect(localStorageHelper.clearWidgetCache).toBeCalled();
        expect(app.setState).toBeCalledWith('get layout settings');
    });

    test('if action is reset but component not mounted: do not reset layout', () => {
        const app = new DashboardApp();
        app.setState = jest.fn();
        app.state.isMounted = false;
        localStorageHelper.clearWidgetCache = jest.fn();

        app.onChildChanged({ action: 'reset' });

        expect(localStorageHelper.clearWidgetCache).not.toBeCalled();
        expect(app.setState).not.toBeCalled();
    });

    test('add widget', () => {
        const app = new DashboardApp();
        app.setState = jest.fn();
        const newState = {
            filter: 'fake new filter',
            items: [{
                key: 'n_reg_customer',
                enable: true
            }],
            action: 'other actions'
        };

        app.state = {
            wgStatus: [
                {
                    key: 'n_reg_customer',
                    enable: false
                }
            ]
        };
        const _addWidgetToLayout = widgetLayout._addWidgetToLayout;
        widgetLayout._addWidgetToLayout = jest.fn(
            (layouts, item) => item.lg.i === 'n_reg_customer' ? 'add' : '');
        app.onChildChanged(newState);

        expect(app.setState.mock.calls[0][0]()).toEqual({
            wgStatus: [
                {
                    key: 'n_reg_customer',
                    enable: true
                }
            ],
            layouts: 'add'
        });
        expect(localStorageHelper.saveWidgetStatus).toBeCalledWith([
            {
                key: 'n_reg_customer',
                enable: true
            }
        ])
        expect(localStorageHelper.saveWidgetLayout).toBeCalledWith('add');
        widgetLayout._addWidgetToLayout = _addWidgetToLayout;
    });

    test('remove widget', () => {
        const app = new DashboardApp();
        app.setState = jest.fn();
        const newState = {
            filter: 'fake new filter',
            items: [{
                key: 'n_reg_customer',
                enable: false
            }],
            action: 'other actions'
        };
        app.state = {
            wgStatus: [{ key: 'n_reg_customer', enable: true }]
        };
        const _removeWidgetFromLayout = widgetLayout._removeWidgetFromLayout;
        widgetLayout._removeWidgetFromLayout = jest.fn(
            (layouts, key) => key === 'n_reg_customer' ? 'remove' : '');
        app.onChildChanged(newState);

        expect(app.setState.mock.calls[0][0]()).toEqual({
            wgStatus: [{ key: 'n_reg_customer', enable: false }],
            layouts: 'remove'
        });
        expect(localStorageHelper.saveWidgetStatus).toBeCalledWith([
            { key: 'n_reg_customer', enable: false }
        ])
        expect(localStorageHelper.saveWidgetLayout).toBeCalledWith('remove');
        widgetLayout._removeWidgetFromLayout = _removeWidgetFromLayout;
    });

    test('no item: do nothing', () => {
        const app = new DashboardApp();
        app.setState = jest.fn();
        const newState = {
            filter: 'fake new filter',
            action: 'other actions'
        };

        [[], null, undefined].forEach(items => {
            newState.items = items;
            app.onChildChanged(newState);
        });

        expect(app.setState).not.toBeCalled();
    });

    test('status not change: dont change layout', () => {
        const app = new DashboardApp();
        app.setState = jest.fn();
        app.state.wgStatus = [{ key: 'no change', enable: false }];
        app.state.layouts = { say: 'fake layout should not be changed' };
        const newState = {
            filter: 'fake new filter',
            action: 'other actions',
            items: [{ key: 'no change', enable: false }]
        };

        app.onChildChanged(newState);

        expect(localStorageHelper.saveWidgetLayout).toBeCalledWith({ say: 'fake layout should not be changed' });
    });
});

test('save layout when layout change', () => {
    const app = new DashboardApp();
    app.setState = jest.fn();
    app.state.isMounted = true;
    app.state.wgStatus = 'fake widget status';

    localStorageHelper.saveWidgetLayout.mockReset();
    app.onLayoutChange('current layout', 'all layout settings');

    expect(localStorageHelper.saveWidgetLayout).toBeCalledWith('all layout settings');
    expect(app.setState).toBeCalledWith({
        wgStatus: 'fake widget status',
        layouts: 'all layout settings'
    });
});

test('component is unmounted: do not save layout when layout change', () => {
    const app = new DashboardApp();
    app.setState = jest.fn();
    app.state.isMounted = false;

    localStorageHelper.saveWidgetLayout.mockReset();
    app.onLayoutChange('current layout', 'all layout settings');

    expect(localStorageHelper.saveWidgetLayout).not.toBeCalled();
    expect(app.setState).not.toBeCalled();
});

test('componentDidMount(): set mount state true', () => {
    const app = new DashboardApp();
    app.setState = jest.fn();
    app.componentDidMount();
    expect(app.setState).toBeCalledWith({ isMounted: true });
});

test('componentWillUnmount(): set mount state false', () => {
    const app = new DashboardApp();
    app.setState = jest.fn();
    app.componentWillUnmount();
    expect(app.setState).toBeCalledWith({ isMounted: false });
});

test('DefaultLayout.callbackParent() is bound to onChildChanged()', () => {
    const component = renderer.create(<DashboardApp />);
    const instance = component.getInstance();
    const element = findElementByName(component, 'DefaultLayout');

    instance.onChildChanged = jest.fn();
    element
        .props
        .callbackParent('new state');
    expect(instance.onChildChanged).toBeCalledWith('new state');
});

test('ResponsiveReactGridLayout.onLayoutChange() is bound to onLayoutChange()', () => {
    const component = renderer.create(<DashboardApp />);
    const instance = component.getInstance();
    const element = findElementByName(component, 'ResponsiveReactGridLayout');

    instance.onLayoutChange = jest.fn();
    element
        .props
        .onLayoutChange('layout', 'layouts');
    expect(instance.onLayoutChange).toBeCalledWith('layout', 'layouts');
});
