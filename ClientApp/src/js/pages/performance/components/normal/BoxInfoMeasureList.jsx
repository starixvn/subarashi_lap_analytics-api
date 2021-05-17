import React from 'react';
import BoxInfoMeasureItem from './BoxInfoMeasureItem';
import { defaultData } from '../../lib/constants'
import numberFormat from '../../../../lib/numberFormat';

export const BoxInfoMeasureList = props => {
  const { data, currencyUnit } = props;
  return (
    <div className='box-info-measure-list'>
      {data && data.map(_ => {
        const measurement = defaultData.measurements.find(__ => __.id === _.measurementId);
        const totalValue = measurement.id !== 3 ? `${currencyUnit} ${numberFormat.nFormatter(_.total)}` : `${_.total}%`;
        return <BoxInfoMeasureItem
        key={'box_info_measure_' + _.measurementId}
        currencyUnit={currencyUnit}
        measurement={measurement.name}
        totalValue={totalValue}
        changeValue={_.changePercent}
      />})
      }
    </div>
  );
}

export default React.memo(BoxInfoMeasureList);
