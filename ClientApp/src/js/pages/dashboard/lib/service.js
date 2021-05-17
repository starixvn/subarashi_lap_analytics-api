export const isActiveTabMenu = (selectedTabMenu, tabMenuItem) => {
    return selectedTabMenu && selectedTabMenu.key === tabMenuItem.key;
}
