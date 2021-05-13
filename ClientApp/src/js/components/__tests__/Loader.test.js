import React from 'react';
import renderer from 'react-test-renderer';
import Loader from '../Loader';
import { widgetKey } from '../../lib/constants';

test('renders the Loader component', () => {
    const component = renderer.create(<Loader/>);
    let tree = component.toJSON();
    expect(tree).toMatchSnapshot();
});

test('renders the Loader component when isLoading = true', () => {
    const component = renderer.create(<Loader isLoading="true"/>);
    let tree = component.toJSON();
    expect(tree).toMatchSnapshot();
});

test('base normal widgets: loader white', () => {
    [
        widgetKey.NormalCustomerBalance,
        widgetKey.NormalPendingFund,
        widgetKey.NormalDeposit,
        widgetKey.NormalWithdrawal
    ].forEach(key => {
        const component = renderer.create(<Loader widgetKey={key}/>);
        let tree = component.toJSON();
        expect(tree).toMatchSnapshot();
    });
});
