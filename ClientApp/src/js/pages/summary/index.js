import React, { Component } from 'react';
import SummaryDashboard from './App'
import { Provider } from 'react-redux'
import { createStore, applyMiddleware, compose } from 'redux'
import { getMiddleware  } from '../../lib/middleware'
import reducer from '../../reducers/index'

const defaultState ={
  currencyInfo: {
    Currency: [],
    BaseCurrency: []
  }
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
        <SummaryDashboard />
      </Provider>
    );
  }
}
