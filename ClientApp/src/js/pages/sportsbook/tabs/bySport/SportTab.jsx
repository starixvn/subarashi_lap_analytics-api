import React, { useEffect, useMemo, useState } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import languageApi from '../../../../lib/languageApi';
import QueryService from '../../lib/queryService';
import { Widget, WidgetContent } from '../../components/Widget';
import { defaultResponseData, dimension, measurement } from '../../lib/constants';
import { getScrollTabStyle } from '../../lib/commonService';
import { orderSports } from './service';
import withGA from '../../../../components/HOC/WithGA';

const SportTab = ({ globalFilter }) => {
  const [isLoading, setLoading] = useState(false);
  const [data, setData] = useState(defaultResponseData);

  useEffect(() => {
    if (globalFilter) {
      setLoading(true)
      QueryService.getDataBySport(globalFilter, response => {
        setLoading(false);
        setData(response)
      })
    }
  }, [globalFilter])

  const distributionTurnoverData = useMemo(() => orderSports(data.byTurnover), [data])
  const distributionWinlossData = useMemo(() => orderSports(data.byWinloss), [data])

  return (globalFilter && data &&
    <React.Fragment>
      {<div className="child-tab by-sport-tab" style={getScrollTabStyle()}>
          <Widget title={languageApi.getDefaultRes('report_by_turnover')} isLoading={isLoading}>
            <WidgetContent data={data.byTurnover} distributionData={distributionTurnoverData} chartTitle={languageApi.getDefaultRes('distribution_of_turnover')} dimension={dimension.sport} measurement={measurement.turnover} />
          </Widget>
          <Widget title={languageApi.getDefaultRes('report_by_winloss')} isLoading={isLoading}>
           <WidgetContent data={data.byWinloss} distributionData={distributionWinlossData} chartTitle={languageApi.getDefaultRes('distribution_of_winloss')} dimension={dimension.sport} measurement={measurement.winloss} />
          </Widget>
        </div>}
    </React.Fragment>
  );
}

function mapDispatchToProps(dispatch) {
  return bindActionCreators({
  }, dispatch);
}

function mapStateToProps(state) {
  const { globalFilter } = state;
  return Object.assign({}, {
    globalFilter
  });
}

export default connect(mapStateToProps, mapDispatchToProps)(withGA(SportTab, 'Sportsbook/BySport'));
