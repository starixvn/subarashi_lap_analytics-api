import React from 'react';
import { Provider } from 'react-redux';
import { createStore, applyMiddleware, compose } from 'redux';
import { getMiddleware  } from '../../lib/middleware';
import reducer from './reducers/index';
import SportsbookDashboard from './App';

const defaultState = {
  currencyInfo: {
    BaseCurrency: []
  },
  dateRange: {
    fromDate: window.twoDaysBefore,
    toDate: window.twoDaysBefore
  },
  selectedBaseCurrency: window.defaultBaseCurrency,
  globalFilter: null
};

const configureStore = (initialState) => {
  const middleware = getMiddleware();
  const enhancer = compose(
    applyMiddleware(
      ...middleware
    )
  );
  return createStore(reducer, initialState, enhancer);
}

const App = () => {
  const store = configureStore(defaultState);
  return (
    <Provider store={store}>
      <SportsbookDashboard />
    </Provider>
  );
}

export default App;

