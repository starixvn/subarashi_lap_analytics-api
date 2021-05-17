import React from 'react';

const ValueChangeIcon = props => {
  function getClassName(value) {
    if(value > 0)
      return 'arrow-up';
    if(value < 0)
      return 'arrow-down';
    return 'rec-equal'
  };
  const className = getClassName(props.value);
  return (
    <div className={className}></div>
  );
}

export default React.memo(ValueChangeIcon)
