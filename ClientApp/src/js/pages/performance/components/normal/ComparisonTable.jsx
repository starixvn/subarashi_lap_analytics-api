import React from 'react';

const renderHeader = header => {
  return header.map((_, index) => <th key={`table_comparison_header_${index}`} colSpan={_.colSpan || 1} rowSpan={_.rowSpan || 1}>{_.name}</th>)
}

const renderHeaders = headers => {
return headers.map((_, index) => <tr key={`table_comparison_row_header_${index}`}>{renderHeader(_)}</tr>)
}

const renderRows = (data, renderRow) => {
  return data.map((rowData, index) => renderRow(rowData, index));
}

const ComparisonTable = props => {
  const { headers, data, title, renderRow } = props;
  return (
    <div className='table-comparison'>
      <div className="table-comparison-title">
        {title}
      </div>
      <table className="table-comparison-body">
        <thead>
          {headers && renderHeaders(headers)}
        </thead>
        <tbody>
          {data && renderRow && renderRows(data, renderRow)}
        </tbody>
      </table>
    </div>
  );
}

export default React.memo(ComparisonTable);
