import React, { Component } from 'react';
import languageApi from '../../../lib/languageApi';
import { bindActionCreators } from 'redux'
import { ActionCreators } from '../../../actions/index'
import { connect } from 'react-redux';
class RightPanel extends Component {
  constructor(props) {
    super(props);

    this.state = {
      isMounted: false,
      SelectedBaseCurrency: window.defaultBaseCurrency,
      SelectedCurrency: 0,
      isHidden: true,
      BaseCurrencyList: this.props.currencyInfo.BaseCurrency,
      wgs: this._bindWgCheckBox(props.wgs)
    };
  }

  _togglePanel() {
    this.setState(prevState => ({
      isHidden: !prevState.isHidden
    }));
  }

  componentDidUpdate(prevProps, prevState) {
    const { wgs } = this.props;

    if (this.state.isMounted && wgs && wgs !== prevProps.wgs) {
      this.setState({
        wgs: this._bindWgCheckBox(wgs)
      });
    }

  }

  componentDidMount() {
    this.setState({ isMounted: true });

  }

  componentWillUnmount() {
    this.setState({ isMounted: false });
  }

  _bindWgCheckBox(wgs) {
    var arrayToDictionary = {};
    wgs.forEach(x => arrayToDictionary[x.key] = x.enable);

    return arrayToDictionary;
  }

  _applyAction() {
    if (this.state.isMounted) {
      let baseCurrency = this.props.currencyInfo.BaseCurrency.find(x =>
        x == this.state.SelectedBaseCurrency
      );

      this.props.callbackParent({
        action: 'apply',
        filter: {
          currency: this.state.SelectedCurrency,
          baseCurrency: baseCurrency ? baseCurrency : ""
        }
      });
    }
  }

  _getNewStateOnChangeCurrency(selectedCurrency) {
    let newState = { SelectedCurrency: selectedCurrency };

    if (selectedCurrency == "0") {
      return Object.assign({}, newState, {
        SelectedBaseCurrency: this.props.currencyInfo.BaseCurrency[0]

      });
    }
    var selectedCurrencyName = this.props.currencyInfo.Currency.find(x => x.CurrencyId == selectedCurrency).Currency;

    if (this.props.currencyInfo.BaseCurrency.find(baseCurrency => baseCurrency == selectedCurrencyName)) {

      if (!this.props.currencyInfo.BaseCurrency.find(baseCurrency => baseCurrency == this.state.SelectedBaseCurrency)) {
        newState.SelectedBaseCurrency = selectedCurrencyName;
        newState.BaseCurrencyList = this.props.currencyInfo.BaseCurrency;

      }
      return newState;
    }


    return Object.assign({}, newState, {
      SelectedBaseCurrency: selectedCurrencyName,
      BaseCurrencyList: this.props.currencyInfo.BaseCurrency.concat(selectedCurrencyName)
    });
  }
  _onChangeCurrencyList(event) {
    var newState = this._getNewStateOnChangeCurrency(event.target.value);

    this.setState(newState);
  }

  _onChangeBaseCurrencyList(event) {
    this.setState({ SelectedBaseCurrency: event.target.value });
  }

  _onCheckWg(key) {
    if (this.state.isMounted) {
      this.props.callbackParent({
        action: 'wgSyncStatus',
        items: [{
          key: key,
          enable: !this.state.wgs[key]
        }]
      });
    }
  }

  _onResetLayout() {
    if (this.state.isMounted) {
      this.props.callbackParent({ action: 'reset' });
    }
  }

  render() {
    const isHidden = this.state.isHidden ? '' : ' is-expanded';

    const { langCode, wgs } = this.props;
    var realBaseCurrency = this.state.BaseCurrencyList.length > 0 ? this.state.BaseCurrencyList
      : this.props.currencyInfo.BaseCurrency;
    return (
      <div className={`dashboard__right-panel${isHidden}`}>
        <div className="right-panel-wrap">
          <span
            onClick={(e) => this._togglePanel(e)}
            className="icon-quickaccess js-icon-quickaccess">
            <i className="icon-settings"></i>
          </span>
          <div className="right-panel-content">
            <div className="currency">
              <label className="currency-label">{languageApi.getRes(langCode, "currency")}</label>
              <select
                ref="currencyDropdownList"
                onChange={(e) => this._onChangeCurrencyList(e)}
                value={this.state.SelectedCurrency}>
                {this.props.currencyInfo.Currency.map((item, i) => {
                  return <option key={i} value={item.CurrencyId}>{item.Currency}</option>
                })}
              </select>
            </div>
            <div className="currency">
              <label className="currency-label">{languageApi.getRes(langCode, "Based")}</label>
              <select
                ref="baseCurrencyDropdownList"
                onChange={(e) => this._onChangeBaseCurrencyList(e)}
                value={this.state.SelectedBaseCurrency}>
                {realBaseCurrency.map((item, i) => {
                  return <option key={i} value={item}>{item}</option>
                })}
              </select>
            </div>
            <a className="btn-submit" onClick={(e) => this._applyAction(e)} href="#">{languageApi.getRes(langCode, "apply")}</a>
            <div className="widgets-config">
              <div className="widgets-title">{languageApi.getRes(langCode, "Widgets")}</div>
              {wgs.map(item => (
                <p key={item.key} className="widget-item">
                  <input
                    onChange={(e) => this._onCheckWg(item.key)}
                    checked={!!this.state.wgs && this.state.wgs[item.key]}
                    className="filled-in"
                    type="checkbox"
                    id={item.key} />
                  <label htmlFor={item.key}>{languageApi.getRes(langCode, item.key)}</label>
                </p>
              ))}
            </div>
            <a className="btn-reset" onClick={() => this._onResetLayout()} href="#">{languageApi.getRes(langCode, "reset_layout")}</a>
          </div>
        </div>
        <div className="right-panel-overlay" onClick={(e) => this._togglePanel(e)}></div>
      </div>
    );
  }
}

function mapDispatchToProps(dispatch) {
  return bindActionCreators(ActionCreators, dispatch);
}

function mapStateToProps(state) {
  return Object.assign({}, state);
}
export default connect(mapStateToProps, mapDispatchToProps)(RightPanel)
