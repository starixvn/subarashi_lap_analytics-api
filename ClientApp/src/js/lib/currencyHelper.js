export const getCurrencyDisplayName = (currency, fieldId, fieldName) => {
  const displayName = _currenyDisplayNameMapper[currency[fieldId]] || currency[fieldName];
  return displayName;
};

const _currenyDisplayNameMapper = {
  '20': 'UUS',
  '51': 'INH',
  '70': 'MKK',
  '75': 'PTU',
  '76': 'PS',
  '77': 'PTH',
  '92': 'HID',
  '125': 'INH',
  '137': 'DOGE',
  '138': 'ELON',
  '139': 'SHIB',
  '142': 'USDT',
  '143': 'USDC',
  '146': 'ZEON',
  '150': 'PEOPLE',
  '153': 'LOOKS',
  '154': 'XPRT',
  '156': 'FLOW',
  '157': 'SAND',
  '158': 'AVAX',
  '160': 'CAKE'
};
