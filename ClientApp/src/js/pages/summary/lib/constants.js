import { widgetKey as commonWidgetKey } from '../../../lib/constants';

export const widgetKey = {
    ...commonWidgetKey,
    ChartTurnover: 'c_turnover',
    ChartLicenseeWL: 'c_licWL',
    NormalRegisterCustomer: 'n_reg_customer',
    ChartlOnlineCustomer: 'c_online_customer'
};

export const widgetApiUrl = {
    ChartTurnover: '/api/TurnoverByPlatform/GetData',
    ChartLicenseeWL: '/api/LicenseeWinLossByPlatform/GetData',
    NormalRegisterCustomer: 'GetRegisteredUserOnline',
    ChartlOnlineCustomer: 'GetOnlineCustomers',
    NormalCustomerBalance: '/api/CustomerBalance/GetData',
    NormalPendingFund: '/api/PendingFund/GetData',
    NormalDeposit: '/api/TodayDeposit/GetData',
    NormalWithdrawal: '/api/TodayWithdrawal/GetData'
};

export const wgLayouts = {
    md: [{ i: widgetKey.ChartTurnover, x: 0, y: 0, w: 6, h: 11, minW: 3, minH: 9 },
        { i: widgetKey.ChartLicenseeWL, x: 6, y: 0, w: 6, h: 11, minW: 3, minH: 9 },
        { i: widgetKey.NormalRegisterCustomer, x: 0, y: 11, w: 6, h: 6, minW: 4, minH: 6 },
        { i: widgetKey.ChartlOnlineCustomer, x: 6, y: 11, w: 6, h: 6, minW: 4, minH: 5 },
        { i: widgetKey.NormalCustomerBalance, x: 0, y: 17, w: 3, h: 6, minW: 3, minH: 3 },
        { i: widgetKey.NormalPendingFund, x: 3, y: 17, w: 3, h: 6, minW: 3, minH: 3 },
        { i: widgetKey.NormalDeposit, x: 6, y: 17, w: 3, h: 6, minW: 3, minH: 3 },
        { i: widgetKey.NormalWithdrawal, x: 9, y: 17, w: 3, h: 6, minW: 3, minH: 3 }
    ],
    sm: [{ i: widgetKey.ChartTurnover, x: 0, y: 0, w: 6, h: 9, isResizable: false },
        { i: widgetKey.ChartLicenseeWL, x: 0, y: 9, w: 6, h: 9, isResizable: false },
        { i: widgetKey.NormalRegisterCustomer, x: 0, y: 18, w: 3, h: 6, isResizable: false },
        { i: widgetKey.ChartlOnlineCustomer, x: 3, y: 18, w: 3, h: 6, isResizable: false },
        { i: widgetKey.NormalCustomerBalance, x: 0, y: 24, w: 3, h: 3, isResizable: false },
        { i: widgetKey.NormalPendingFund, x: 3, y: 24, w: 3, h: 3, isResizable: false },
        { i: widgetKey.NormalDeposit, x: 0, y: 27, w: 3, h: 3, isResizable: false },
        { i: widgetKey.NormalWithdrawal, x: 3, y: 27, w: 3, h: 3, isResizable: false }
    ],
    xs: [{ i: widgetKey.ChartTurnover, x: 0, y: 0, w: 4, h: 9, isResizable: false },
        { i: widgetKey.ChartLicenseeWL, x: 0, y: 9, w: 4, h: 9, isResizable: false },
        { i: widgetKey.NormalRegisterCustomer, x: 0, y: 18, w: 4, h: 6, isResizable: false },
        { i: widgetKey.ChartlOnlineCustomer, x: 0, y: 24, w: 4, h: 5, isResizable: false },
        { i: widgetKey.NormalCustomerBalance, x: 0, y: 29, w: 2, h: 3, isResizable: false },
        { i: widgetKey.NormalPendingFund, x: 2, y: 29, w: 2, h: 3, isResizable: false },
        { i: widgetKey.NormalDeposit, x: 0, y: 32, w: 2, h: 3, isResizable: false },
        { i: widgetKey.NormalWithdrawal, x: 2, y: 32, w: 2, h: 3, isResizable: false }
    ],

    xxs: [{ i: widgetKey.ChartTurnover, x: 0, y: 0, w: 4, h: 8, isResizable: false },
        { i: widgetKey.ChartLicenseeWL, x: 0, y: 8, w: 4, h: 8, isResizable: false },
        { i: widgetKey.NormalRegisterCustomer, x: 0, y: 16, w: 4, h: 6, isResizable: false },
        { i: widgetKey.ChartlOnlineCustomer, x: 0, y: 22, w: 4, h: 4, isResizable: false },
        { i: widgetKey.NormalCustomerBalance, x: 0, y: 26, w: 4, h: 3, isResizable: false },
        { i: widgetKey.NormalPendingFund, x: 0, y: 29, w: 4, h: 3, isResizable: false },
        { i: widgetKey.NormalDeposit, x: 0, y: 32, w: 4, h: 3, isResizable: false },
        { i: widgetKey.NormalWithdrawal, x: 0, y: 35, w: 4, h: 3, isResizable: false }
    ]

};
