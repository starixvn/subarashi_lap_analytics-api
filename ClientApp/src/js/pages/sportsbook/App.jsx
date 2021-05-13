import React, { useState } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import FormFilter from './components/FormFilter';
import TabContainer from '../dashboard/components/TabContainer';
import { tabMenuKey } from './lib/constants';
import languageApi from '../../lib/languageApi';
import SportTab from './tabs/bySport/SportTab';
import LeagueTab from './tabs/byLeague/LeagueTab';
import BettypeTab from './tabs/byBettype/BettypeTab';
import CurrencyTab from './tabs/byCurrency/CurrencyTab';
import PlatformTab from './tabs/byPlatform/PlatformTab';
import { setGlobalFilter } from './actions/filterAction';

const tabList = [
  { key: tabMenuKey.bySport, name: languageApi.getDefaultRes('by_sport'), component: SportTab },
  { key: tabMenuKey.byLeague, name: languageApi.getDefaultRes('by_league'), component: LeagueTab },
  { key: tabMenuKey.byBettype, name: languageApi.getDefaultRes('by_bettype'), component: BettypeTab },
  { key: tabMenuKey.byCurrency, name: languageApi.getDefaultRes('by_currency'), component: CurrencyTab },
  { key: tabMenuKey.byPlatform, name: languageApi.getDefaultRes('by_platform'), component: PlatformTab }
];

const SportsbookDashboard = ({ setGlobalFilter }) => {
  const [selectedTabKey, setSelectedTabKey] = useState(tabMenuKey.bySport);

  const _onChangeTab = selectedTabKey => {
    setGlobalFilter(null);
    setSelectedTabKey(selectedTabKey);
  }

  return (
    <div className="sportsbook-container">
      <FormFilter />
      <TabContainer onChangeTab={_onChangeTab} selectedTabKey= {selectedTabKey} tabMenuList={tabList} />
    </div >
  );
}
function mapDispatchToProps(dispatch) {
  return bindActionCreators({
    setGlobalFilter
  }, dispatch);
}

function mapStateToProps(state) {
  return Object.assign({}, {});
}

export default connect(mapStateToProps, mapDispatchToProps)(SportsbookDashboard);
