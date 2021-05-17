import React, { PureComponent } from 'react';
import TabMenu from './TabMenu';

class TabContainer extends PureComponent {
  constructor(props) {
    super(props);
  }

  _onClickTabMenu = tabKey => {
    this.props.onChangeTab(tabKey);
  }

  _getCurrentTab() {
    const { selectedTabKey, tabMenuList} = this.props;
    const item = tabMenuList.find(_ => _.key === selectedTabKey);
    return item ? item.component : null;
  }

  render() {
    const CurrentTab = this._getCurrentTab();
    const { selectedTabKey, tabMenuList } = this.props;
    return (
      <div className="tab-container">
        <TabMenu selectedTabMenu={selectedTabKey} onClickTabMenu={this._onClickTabMenu} tabMenuList={tabMenuList} />
        {CurrentTab && <CurrentTab />}
      </div>
    );
  }
}

export default TabContainer;
