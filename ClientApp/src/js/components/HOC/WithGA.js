import React, { useEffect } from 'react';

const withGA = (Component, name) => {
  return props => {
    useEffect(() => {
      window.logViewTab && window.logViewTab(name)
    }, [])
    return <Component {...props} />
  }
}

export default withGA;
