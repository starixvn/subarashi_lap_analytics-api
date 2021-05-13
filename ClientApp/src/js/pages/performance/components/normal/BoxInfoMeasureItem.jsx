import React from 'react';

export const BoxInfoMeasureItem = props => {
  const { measurement, totalValue, changeValue } = props;

  return (
    <div className='box-info-measure'>
      <div className="box-info-mesure-name">{measurement}</div>
      <div className="box-info-value-total">
        {totalValue}
      </div>
      <div className="box-info-value-change">
      <div className={changeValue < 0 ? "arrow-down" : "arrow-up"} />
        <span>{changeValue}{"%"}</span>
      </div>
    </div>
  );
}

export default React.memo(BoxInfoMeasureItem)
