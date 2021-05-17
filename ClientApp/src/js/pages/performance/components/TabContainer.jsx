import React, { Suspense } from 'react';
import TabMenu from '../../dashboard/components/TabMenu';
import { tabMenuKey } from '../lib/constants';
import OverviewTab from '../tabs/overview/OverviewTab';
import CurrencyTab from '../tabs/currency/CurrencyTab';
import ProductTab from '../tabs/product/ProductTab';
import BettypeTab from '../tabs/bettype/BettypeTab';
import PlatformTab from '../tabs/platform/PlatformTab';
import languageApi from '../../../lib/languageApi';

const tabMenuList = [
  { key: tabMenuKey.overview, name: languageApi.getDefaultRes('overview'), component: OverviewTab },
  { key: tabMenuKey.currency, name: languageApi.getDefaultRes('currency'), component: CurrencyTab },
  { key: tabMenuKey.product, name: languageApi.getDefaultRes('product'), component: ProductTab },
  { key: tabMenuKey.bettype, name: languageApi.getDefaultRes('bettype'), component: BettypeTab },
  { key: tabMenuKey.platform, name: languageApi.getDefaultRes('platform'), component: PlatformTab }
];

class TabContainer extends React.PureComponent {
  constructor(props) {
    super(props);
  }

  _onClickTabMenu = tabKey => {
    this.props.onChangeTab(tabKey);
  }

  _getCurrentTab() {
    const { selectedTabKey } = this.props;
    const item = tabMenuList.find(_ => _.key === selectedTabKey);
    return item ? item.component : null;
  }

  render() {
    const CurrentTab = this._getCurrentTab();
    const { selectedTabKey } = this.props;
    return (
      <div className="tab-container">
        <TabMenu selectedTabMenu={selectedTabKey} onClickTabMenu={this._onClickTabMenu} tabMenuList={tabMenuList} />
        {CurrentTab && <CurrentTab />}
      </div>
    );
  }
}

export default TabContainer;
