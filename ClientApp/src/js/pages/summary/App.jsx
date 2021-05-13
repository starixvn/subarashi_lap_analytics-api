import React from 'react';
import { Responsive, WidthProvider } from 'react-grid-layout';

import Turnover from './components/chart/Turnover';
import OnlineCustomer from './components/chart/OnlineCustomer';
import LicenseeWinLoss from './components/chart/LicenseeWinLoss';

import Deposit from './components/normal/Deposit';
import Withdrawal from './components/normal/Withdrawal';
import PendingFunds from './components/normal/PendingFunds';
import CustomerBalance from './components/normal/CustomerBalance';
import RegisteredCustomer from './components/normal/RegisteredCustomer';
import RightPanel from './components/RightPanel';

import widgetLayout from '../../layouts/widgetLayout';
import languageApi from '../../lib/languageApi';
import { widgetKey, wgLayouts } from './lib/constants';
import localStorageHelper from '../../lib/localStorageHelper';
import { bindActionCreators } from 'redux'
import { ActionCreators } from '../../actions/index'
import { connect } from 'react-redux';
class SummaryDashboard extends React.Component {

  constructor(props) {
    super(props);

    localStorageHelper.clearLayoutSettingIfNewVersion();

    const langCode = window.dashboardLangCode || 'en';
    const permission = window.widgetPermission;

    if (!permission || !permission.length) {
      alert(languageApi.getRes(langCode, 'no_widget_permission'));
    }

    this.state = Object.assign({
      langCode: langCode,
      refresh: 0,
      selectedFilter: {
        currency: 0,
        baseCurrency: window.defaultBaseCurrency
      },
      isMounted: false,
    }, this._getLayoutSettings());;

    this._responsiveReactGridLayout = WidthProvider(Responsive);
    this.props.getCurrencyInfo();
  }
  _me = this;
  componentDidUpdate(prevProps, prevState) {
    if (this.state.isMounted && this.props.newState && this.props.newState !== prevProps.newState) {
      this.onChildChanged(this.props.newState);
    }
  }

  _renderWgItem(key) {
    const keyToWidget = {
      [widgetKey.ChartTurnover]: Turnover,
      [widgetKey.ChartLicenseeWL]: LicenseeWinLoss,
      [widgetKey.NormalDeposit]: Deposit,
      [widgetKey.ChartlOnlineCustomer]: OnlineCustomer,
      [widgetKey.NormalPendingFund]: PendingFunds,
      [widgetKey.NormalRegisterCustomer]: RegisteredCustomer,
      [widgetKey.NormalCustomerBalance]: CustomerBalance,
      [widgetKey.NormalWithdrawal]: Withdrawal
    };
    const Widget = keyToWidget[key];
    var itemStatus = this.state.wgStatus.find(x => x.key === key);

    return (<div key={key}>
      <Widget wgItemFromParent={itemStatus} callbackParent={(newState) => this.onChildChanged(newState)} filter={this.state.selectedFilter} refresh={this.state.refresh} langCode={this.state.langCode} />
    </div>);
  }

  _resetLayout() {
    if (this.state.isMounted) {
      localStorageHelper.clearWidgetCache();

      this.setState(this._getLayoutSettings());
    }
  }

  _getLayoutSettings() {
    const wgStatus = widgetLayout.getWidgetStatusSettings(window.widgetPermission, widgetKey);
    const userWgStatus = widgetLayout.mergeWidgetStatusSettingsToUserSettings(wgStatus);
    localStorageHelper.saveWidgetStatus(userWgStatus);

    const userWidgetLayouts = widgetLayout.getWidgetLayoutSettings(wgLayouts);
    localStorageHelper.saveWidgetLayout(userWidgetLayouts);

    return {
      wgStatus: userWgStatus,
      layouts: userWidgetLayouts
    }
  }

  onChildChanged(newState) {

    if (this.state.isMounted && newState.action == 'apply') {
      this.setState(pre => ({
        refresh: pre.refresh + 1,
        selectedFilter: newState.filter
      }))
    } else if (newState.action == 'reset') {
      this._resetLayout();
    } else if (newState.items && newState.items.length) {
      const syncResult = widgetLayout.syncLayoutWithNewStatus(newState.items, this.state.wgStatus, this.state.layouts, wgLayouts);

      this.setState({ layouts: syncResult.newLayouts, wgStatus: syncResult.newWidgetStatus });

      localStorageHelper.saveWidgetStatus(syncResult.newWidgetStatus);
      localStorageHelper.saveWidgetLayout(syncResult.newLayouts);
      localStorageHelper.saveRemovedWidgetLayout(syncResult.removedLayouts);
    }
  }

  onLayoutChange(layout, layouts) {
    if (this.state.isMounted) {
      localStorageHelper.saveWidgetLayout(layouts);
      this.setState({
        wgStatus: this.state.wgStatus,
        layouts: layouts
      });
    }
  }

  get isTouchScreen() {
    return 'ontouchstart' in window || navigator.maxTouchPoints
  }

  componentDidMount() {
    this.setState({ isMounted: true });
  }

  componentWillUnmount() {
    this.setState({ isMounted: false });
  }

  render() {
    const ResponsiveReactGridLayout = this._responsiveReactGridLayout;

    return (
      <React.Fragment>
        <RightPanel langCode={this.state.langCode} wgs={this.state.wgStatus} callbackParent={(x) => this.onChildChanged(x)} />
        <ResponsiveReactGridLayout className="layout" layouts={this.state.layouts} rowHeight={32} onLayoutChange={(layout, layouts) => this.onLayoutChange(layout, layouts)}
          breakpoints={{ md: 1280, sm: 768, xs: 480, xxs: 0 }} cols={{ md: 12, sm: 6, xs: 4, xxs: 2 }} isDraggable={!this.isTouchScreen}>
          {
            this.state.wgStatus.filter(i => i.enable).map((item) => this._renderWgItem(item.key))
          }
        </ResponsiveReactGridLayout>
      </React.Fragment>
    );
  };
}
function mapDispatchToProps(dispatch) {
  return bindActionCreators(ActionCreators, dispatch);
}

function mapStateToProps(state) {
  return Object.assign({}, state);
}
export default connect(mapStateToProps, mapDispatchToProps)(SummaryDashboard)
