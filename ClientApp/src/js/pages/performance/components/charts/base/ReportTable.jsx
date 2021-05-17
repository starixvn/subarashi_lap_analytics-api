import React from 'react';
import languageApi from '../../../../../lib/languageApi';

const renderHeaders = headers => {
  return headers.map((_, index) => <th key={`table_report_row_header_${index}`}>{_}</th>)
}

const renderRows = (data) => {
  return data.map((rowData, index) => 
    <tr key={index}>
      <td>{rowData.name}</td>
      <td style={{textAlign: 'center'}}>{rowData.dataSet[0] + '%'}</td>
      <td style={{textAlign: 'center'}}>{rowData.dataSet[1] + '%'}</td>
    </tr>
  );
}

const ReportTable = props => {
    const {reportTableData, tabKey} = props;
    const headers = [languageApi.getDefaultRes(tabKey), languageApi.getDefaultRes('previous_period'), languageApi.getDefaultRes('current_period')];
    return (
      <div className='table-report'>
      <div className='table-comparison'>
        <table className="table-comparison-body">
          <thead>
            <tr>
              {renderHeaders(headers)}
            </tr>
          </thead>
          <tbody>
              {renderRows(reportTableData)}
          </tbody>
        </table>
      </div>
      </div>
    );
}

export default ReportTable
