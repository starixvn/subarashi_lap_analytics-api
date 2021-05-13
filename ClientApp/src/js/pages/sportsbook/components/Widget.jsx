import React from 'react';
import Loader from '../../../components/Loader';
import ReportInTextTable from './ReportInTextTable';
import DistributionPieChart from './DistributionPieChart';
import { reportInTextTableDataMapper } from '../lib/commonService';

const Widget = React.memo(({ title, isLoading, children }) => {
  return (
    <div className="widget">
      <Loader isLoading={isLoading} />
      <div className="widget-title">
        {title}
      </div>
      <div className="widget-body">
        {children}
      </div>
    </div>
  )
})

const WidgetContent = React.memo(({ data, distributionData, chartTitle, dimension, measurement, displayNameMapper }) => {
  const reportInTextProps = React.useMemo(() => data ? reportInTextTableDataMapper(data, dimension, measurement, displayNameMapper) : null, [data])
  return (
    <div className="widget-content-container">
      <div className='widget-content-left'>
        <ReportInTextTable {...reportInTextProps} />
      </div>
      <div className='widget-content-right chart-container'>
        <DistributionPieChart data={distributionData || data} displayNameMapper={displayNameMapper} title={chartTitle || ''} />
      </div>
    </div>
  )
})

export {
  Widget, WidgetContent
}
