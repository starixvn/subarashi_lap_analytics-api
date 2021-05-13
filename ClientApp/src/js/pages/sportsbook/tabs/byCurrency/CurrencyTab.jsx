import React, { useEffect, useState } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import languageApi from '../../../../lib/languageApi';
import QueryService from '../../lib/queryService';
import { Widget, WidgetContent } from '../../components/Widget';
import { defaultResponseData, dimension, measurement } from '../../lib/constants';
import { getScrollTabStyle, currencyDisplayNameMapper } from '../../lib/commonService';
import withGA from '../../../../components/HOC/WithGA';

const CurrencyTab = ({ globalFilter }) => {
  const [isLoading, setLoading] = useState(false);
  const [data, setData] = useState(defaultResponseData);

  useEffect(() => {
    if (globalFilter) {
      setLoading(true)
      QueryService.getDataByCurrency(globalFilter, response => {
        setLoading(false);
        setData(response);
      })
    }
  }, [globalFilter])

  return (globalFilter &&
    <React.Fragment>
      {<div className="child-tab by-currency-tab" style={getScrollTabStyle()}>
        <Widget title={languageApi.getDefaultRes('report_by_turnover')} isLoading={isLoading}>
          <WidgetContent data={data.byTurnover}
            displayNameMapper={currencyDisplayNameMapper}
            chartTitle={`${languageApi.getDefaultRes('top_5_currencies')} - ${languageApi.getDefaultRes('distribution_by_turnover')}`}
            dimension={dimension.currency}
            measurement={measurement.turnover} />
        </Widget>
        <Widget title={languageApi.getDefaultRes('report_by_winloss')} isLoading={isLoading}>
          <WidgetContent data={data.byWinloss}
            displayNameMapper={currencyDisplayNameMapper}
            chartTitle={`${languageApi.getDefaultRes('top_5_currencies')} - ${languageApi.getDefaultRes('distribution_by_winloss')}`}
            dimension={dimension.currency}
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

export default connect(mapStateToProps, mapDispatchToProps)(withGA(CurrencyTab, 'Sportsbook/ByCurrency' ));
