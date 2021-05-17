import React, { useEffect, useState } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import languageApi from '../../../../lib/languageApi';
import QueryService from '../../lib/queryService';
import { Widget, WidgetContent } from '../../components/Widget';
import { defaultResponseData, dimension, measurement } from '../../lib/constants';
import { distributionChartTitle } from './constants';
import { getScrollTabStyle } from '../../lib/commonService';
import withGA from '../../../../components/HOC/WithGA';

const BettypeTab = ({ globalFilter }) => {
  const [isSoccerLoading, setSoccerLoading] = useState(false);
  const [soccerData, setSoccerData] = useState(defaultResponseData);

  const [isSabaSoccerLoading, setSabaSoccerLoading] = useState(false);
  const [sabaSoccerData, setSabaSoccerData] = useState(defaultResponseData);

  const [isBasketballLoading, setBasketballLoading] = useState(false);
  const [basketballData, setBasketballData] = useState(defaultResponseData);

  const [isSabaBasketballLoading, setSabaBasketballLoading] = useState(false);
  const [sabaBasketballData, setSabaBasketballData] = useState(defaultResponseData);

  useEffect(() => {
    globalFilter && getDataByLeagueSoccer();
    globalFilter && getDataByLeagueBasketball();
    globalFilter && getDataByLeagueSabaSoccer();
    globalFilter && getDataByLeagueSabaBasketball();

  }, [globalFilter])

  const getDataByLeagueSoccer = () => {
    setSoccerLoading(true);
    QueryService.getDataByBettypeSoccer(globalFilter, response => {
      setSoccerLoading(false);
      setSoccerData(response)
    })
  }

  const getDataByLeagueBasketball = () => {
    setBasketballLoading(true);
    QueryService.getDataByBettypeBasketball(globalFilter, response => {
      setBasketballLoading(false);
      setBasketballData(response)
    })
  }

  const getDataByLeagueSabaSoccer = () => {
    setSabaSoccerLoading(true);
    QueryService.getDataByBettypeSoccer({ ...globalFilter, isSaba: true }, response => {
      setSabaSoccerLoading(false);
      setSabaSoccerData(response)
    })
  }

  const getDataByLeagueSabaBasketball = () => {
    setSabaBasketballLoading(true);
    QueryService.getDataByBettypeBasketball({ ...globalFilter, isSaba: true }, response => {
      setSabaBasketballLoading(false);
      setSabaBasketballData(response)
    })
  }

  const _renderSoccerSection = () => {
    return (<Widget title={languageApi.getDefaultRes('soccer')} isLoading={isSoccerLoading}>
      <WidgetContent data={soccerData.byTurnover} chartTitle={distributionChartTitle.turnover} dimension={dimension.bettype} measurement={measurement.turnover} />
      <WidgetContent data={soccerData.byWinloss}  chartTitle={distributionChartTitle.winloss} dimension={dimension.bettype} measurement={measurement.winloss} />
    </Widget>)
  }

  const _renderBasketballSection = () => {
    return (<Widget title={languageApi.getDefaultRes('basketball')} isLoading={isBasketballLoading}>
      <WidgetContent data={basketballData.byTurnover} chartTitle={distributionChartTitle.turnover} dimension={dimension.bettype} measurement={measurement.turnover} />
      <WidgetContent data={basketballData.byWinloss} chartTitle={distributionChartTitle.winloss} dimension={dimension.bettype} measurement={measurement.winloss} />
    </Widget>)
  }

  const _renderSabaSoccerSection = () => {
    return (<Widget title={languageApi.getDefaultRes('saba_soccer')} isLoading={isSabaSoccerLoading}>
      <WidgetContent data={sabaSoccerData.byTurnover} chartTitle={distributionChartTitle.turnover}  dimension={dimension.bettype} measurement={measurement.turnover} />
      <WidgetContent data={sabaSoccerData.byWinloss} chartTitle={distributionChartTitle.winloss}  dimension={dimension.bettype} measurement={measurement.winloss} />
    </Widget>)
  }

  const _renderSabaBasketballSection = () => {
    return (<Widget title={languageApi.getDefaultRes('saba_basketball')} isLoading={isSabaBasketballLoading}>
      <WidgetContent data={sabaBasketballData.byTurnover} chartTitle={distributionChartTitle.turnover}  dimension={dimension.bettype} measurement={measurement.turnover} />
      <WidgetContent data={sabaBasketballData.byWinloss} chartTitle={distributionChartTitle.winloss}  dimension={dimension.bettype} measurement={measurement.winloss} />
    </Widget>)
  }

  return (globalFilter &&
    <React.Fragment>
      <div className="child-tab by-bettype-tab" style={getScrollTabStyle()}>
        {_renderSoccerSection()}
        {_renderBasketballSection()}
        {_renderSabaSoccerSection()}
        {_renderSabaBasketballSection()}
      </div>
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

export default connect(mapStateToProps, mapDispatchToProps)(withGA(BettypeTab, 'Sportsbook/ByBettype'));
