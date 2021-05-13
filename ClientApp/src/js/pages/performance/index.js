import React, { Component } from 'react';
import PerformanceDashboard from './App';
import { defaultData } from './lib/constants';
import { Provider } from 'react-redux';
import { createStore, applyMiddleware, compose } from 'redux';
import { getMiddleware  } from '../../lib/middleware';
import reducer from './reducers/index';

const defaultState = {
  isLoading: false,
  currencyInfo: {
    Currency: [],
    BaseCurrency: []
  },
  selectedMeasurement: defaultData.measurements[0],
  selectedBaseCurrency: window.defaultBaseCurrency,
  analyticData: {},
  selectedComparePeriods: defaultData.getComparePeriods(),
};

function configureStore(initialState) {
  const middleware = getMiddleware();
  const enhancer = compose(
    applyMiddleware(
      ...middleware
    ),
  );
  return createStore(reducer, initialState, enhancer);
}

const store = configureStore(defaultState);

export default class App extends Component {
  render() {
    return (
      <Provider store={store}>
        <PerformanceDashboard />
      </Provider>
    );
  }
}

