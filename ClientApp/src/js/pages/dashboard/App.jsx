import React, { Component, Suspense } from 'react';
import ReactDOM from 'react-dom';

import DefaultLayout from '../../layouts/DefaultLayout';
import languageApi from '../../lib/languageApi';
import { summary, performance, sportsbook } from './lib/constants';
import TabMenu from './components/TabMenu';

__webpack_public_path__ = document.getElementById("public-path").value;

const dynamicImports = {
  [summary]: () => import(/* webpackChunkName: "summary" */ '../summary/index'),
  [performance]: () => import(/* webpackChunkName: "performance" */ '../performance/index'),
  [sportsbook]: () => import(/* webpackChunkName: "sportsbook" */ '../sportsbook/index')
}

const tabPermissions = window.TabKeys || [];
const tabMenuList = tabPermissions.map(_ => ({
  key: _,
  name: languageApi.getDefaultRes(_),
  component: React.lazy(dynamicImports[_]),
  preLoad: dynamicImports[_]
}))
const preLoadComponents = () => {
  setTimeout(() => tabMenuList.forEach(_ => _.preLoad && _.preLoad()), 0)
}
export default class DashboardApp extends Component {
    constructor(props) {
        super(props);
        this.state = {
            selectedTabMenu: this._getDefaultTabKey()
        }
    }

    _getDefaultTabKey() {
      const tabKey = tabMenuList[0] ? tabMenuList[0].key : null;
      return tabKey;
    }

    componentDidMount() {
      preLoadComponents();
    }

    onClickTabMenu = (name) => {
        this.setState({ selectedTabMenu: name });
    }

    render() {
        const { selectedTabMenu } = this.state;
        const componentIndex = tabMenuList.findIndex(p => p.key === selectedTabMenu);
        const TabComponent = componentIndex === -1 ? null : tabMenuList[componentIndex].component;

        return (
            <div className="dashboard-container">
                <TabMenu selectedTabMenu={selectedTabMenu} onClickTabMenu={this.onClickTabMenu} tabMenuList={tabMenuList} />
                <DefaultLayout>
                    {
                        TabComponent && (
                            <Suspense fallback={<div>Loading...</div>}>
                                <TabComponent />
                            </Suspense>
                        )
                    }
                </DefaultLayout>
            </div>
        );
    };
}

ReactDOM.render(<DashboardApp />, document.getElementById("dashboard"));
