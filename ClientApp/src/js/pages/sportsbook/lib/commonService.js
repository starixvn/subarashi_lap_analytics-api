import React from 'react';
import languageApi from '../../../lib/languageApi';
import numberFormat from '../../../lib/numberFormat';
import { rowDataMapper } from './constants';
import { getCurrencyDisplayName } from '../../../lib/currencyHelper';

const getReportInTextTableHeader = (dimension, measurement) => [
  [
    { name: languageApi.getDefaultRes(dimension), rowSpan: 2 },
    { name: languageApi.getDefaultRes(measurement), colSpan: 2 },
    { name: languageApi.getDefaultRes('margin'), rowSpan: 2 },
  ],
  [
    { name: languageApi.getDefaultRes('value') },
    { name: '%' }
  ]
];

export const renderReportInTextTableRow = (rowData, index) => (<tr key={`table_comparison_row_${index}`}>
  <td><span>{rowData.name}</span></td>
  {rowData.data.map(_ => <td>
    {`${numberFormat.formatDecimal(_.value)}${_.isPercentUnit ? '%' : ''}`}
  </td>
  )}
</tr>)

export const reportInTextTableDataMapper = (data, dimension, measurement, displayNameMapper = null) => {
  const rows = data.map(_ => ({
    name: displayNameMapper ? displayNameMapper(_) : _.name,
    data: rowDataMapper.map(__ => __(_))
  }));

  const headers = getReportInTextTableHeader(dimension, measurement);
  return {
    headers,
    data: rows,
    renderRow: renderReportInTextTableRow
  }
}

export const currencyDisplayNameMapper = currencyObj => getCurrencyDisplayName(currencyObj, 'id', 'name');
export const getScrollTabStyle = () => {
  const { innerHeight } = window;
  return {
    maxHeight: (innerHeight - 200) + 'px'
  }
}
