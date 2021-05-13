export const getCurrentTime = () => {
  const now = new Date();
  return {
    month: now.getMonth(),
    year: now.getFullYear()
  }
}

export const dashboardLangCode = window.dashboardLangCode || 'en';

export const langCodes = {
  english: 'en',
  simplifiedChinese: 'cs'
}

export const tabMenuKey = {
    summary: 'summary',
    performance: 'performance',
    sportsbook: 'sportsbook'
}

export const CurrenyListApiUrl = "GetCurrencyInfo";

export const widgetKey = {
    NormalCustomerBalance: 'n_customer_balance',
    NormalPendingFund: 'n_pending_funds',
    NormalDeposit: 'n_deposit',
    NormalWithdrawal: 'n_withdrawal'
};
