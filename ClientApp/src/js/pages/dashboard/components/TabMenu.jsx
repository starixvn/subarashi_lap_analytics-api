import React from 'react';
import { isActiveTabMenu } from '../lib/service';
import TabMenuItem from './TabMenuItem';

export const TabMenu = (props) => {
    const { selectedTabMenu, onClickTabMenu, tabMenuList } = props;
    const selectedTabMenuItem = tabMenuList.find(p => p.key === selectedTabMenu);

    return (
        <ul className="tab-menu">
            {
                tabMenuList.map(item => {
                    return (
                        <TabMenuItem key={item.key} item={item}
                            isActive={isActiveTabMenu(selectedTabMenuItem, item)}
                            onClickTabMenu={onClickTabMenu} />
                    )
                })
            }
        </ul>
    )
}

export default React.memo(TabMenu);
