import React from 'react';

export const TabMenuItem = (props) => {
    const { item, isActive, onClickTabMenu } = props;

    return (
        <li key={item.key}>
            <a href="#!" className={isActive ? 'active' : ''} onClick={() => onClickTabMenu(item.key)}>
                {item.name}
            </a>
        </li>
    )
}

export default React.memo(TabMenuItem);