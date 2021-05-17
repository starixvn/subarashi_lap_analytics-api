
import thunkMiddleware from 'redux-thunk';
import { createLogger } from 'redux-logger';

export const getMiddleware = function() {
  let middleware = [thunkMiddleware];
  if(process.env.NODE_ENV === 'development') {
    const __DEV__ = '__DEV__';
    const loggerMiddleware = createLogger({ predicate: (getState, action) => __DEV__ });
    middleware = [...middleware, loggerMiddleware];
  }
  return middleware;
}
