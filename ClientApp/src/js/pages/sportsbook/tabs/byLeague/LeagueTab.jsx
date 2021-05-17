import React, { useEffect, useState } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import languageApi from '../../../../lib/languageApi';
import QueryService from '../../lib/queryService';
import { Widget, WidgetContent } from '../../components/Widget';
import { defaultResponseData, dimension, measurement } from '../../lib/constants';
import { getScrollTabStyle } from '../../lib/commonService';
import withGA from '../../../../components/HOC/WithGA';

const LeagueTab = ({ globalFilter }) => {
  const [isSoccerLoading, setSoccerLoading] = useState(false);
  const [soccerData, setSoccerData] = useState(defaultResponseData);

  const [isBasketballLoading, setBasketballLoading] = useState(false);
  const [basketballData, setBasketballData] = useState(defaultResponseData);

  useEffect(() => {
    globalFilter && getDataByLeagueSoccer();
    globalFilter && getDataByLeagueBasketball();
  }, [globalFilter])

  const getDataByLeagueSoccer = () => {
    setSoccerLoading(true);
    QueryService.getDataByLeagueSoccer(globalFilter, response => {
      setSoccerLoading(false);
      setSoccerData(response)
    })
  }

  const getDataByLeagueBasketball = () => {
    setBasketballLoading(true);
    QueryService.getDataByLeagueBasketball(globalFilter, response => {
      setBasketballLoading(false);
      setBasketballData(response)
    })
  }

  const _renderSoccerSection = () => {
    return (<Widget title={languageApi.getDefaultRes('soccer')} isLoading={isSoccerLoading}>
      <WidgetContent data={soccerData.byTurnover} chartTitle={languageApi.getDefaultRes('distribution_of_turnover')} dimension={dimension.league} measurement={measurement.turnover} />
      <WidgetContent data={soccerData.byWinloss} chartTitle={languageApi.getDefaultRes('distribution_of_winloss')} dimension={dimension.league} measurement={measurement.winloss} />
    </Widget>)
  }

  const _renderBasketballSection = () => {
    return (<Widget title={languageApi.getDefaultRes('basketball')} isLoading={isBasketballLoading}>
      <WidgetContent data={basketballData.byTurnover} chartTitle={languageApi.getDefaultRes('distribution_of_turnover')} dimension={dimension.league} measurement={measurement.turnover} />
      <WidgetContent data={basketballData.byWinloss} chartTitle={languageApi.getDefaultRes('distribution_of_winloss')} dimension={dimension.league} measurement={measurement.winloss} />
    </Widget>)
  }

  return (globalFilter &&
    <React.Fragment>
      <div className="child-tab by-league-tab" style={getScrollTabStyle()}>
        {_renderSoccerSection()}
        {_renderBasketballSection()}
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

export default connect(mapStateToProps, mapDispatchToProps)(withGA(LeagueTab, 'Sportsbook/ByLeague'));
