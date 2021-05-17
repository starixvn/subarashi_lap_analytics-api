import React, { useEffect, useState } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import languageApi from '../../../../lib/languageApi';
import QueryService from '../../lib/queryService';
import { Widget, WidgetContent } from '../../components/Widget';
import { defaultResponseData, dimension, measurement } from '../../lib/constants';
import { getScrollTabStyle } from '../../lib/commonService';
import withGA from '../../../../components/HOC/WithGA';

const PlatformTab = ({ globalFilter }) => {
  const [isLoading, setLoading] = useState(false);
  const [data, setData] = useState(defaultResponseData);

  useEffect(() => {
    if (globalFilter) {
      setLoading(true)
      QueryService.getDataByPlatform(globalFilter, response => {
        setLoading(false);
        setData(response)
      })
    }
  }, [globalFilter])

  return (globalFilter &&
    <React.Fragment>
      {<div className="child-tab by-platform-tab" style={getScrollTabStyle()}>
        <Widget title={languageApi.getDefaultRes('report_by_turnover')} isLoading={isLoading}>
          <WidgetContent data={data.byTurnover}
            chartTitle={`${languageApi.getDefaultRes('top_5_platforms')} - ${languageApi.getDefaultRes('distribution_by_turnover')}`}
            dimension={dimension.platform}
            measurement={measurement.turnover} />
        </Widget>
        <Widget title={languageApi.getDefaultRes('report_by_winloss')} isLoading={isLoading}>
          <WidgetContent data={data.byWinloss}
            chartTitle={`${languageApi.getDefaultRes('top_5_platforms')} - ${languageApi.getDefaultRes('distribution_by_winloss')}`}
            dimension={dimension.platform}
            measurement={measurement.winloss} />
        </Widget>
      </div>
      }
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

export default connect(mapStateToProps, mapDispatchToProps)(withGA(PlatformTab, 'Sportsbook/ByPlatform'));
