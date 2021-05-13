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
  '92': 'HID'
}
