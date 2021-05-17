import React from 'react';
import renderer from 'react-test-renderer';
import DefaultLayout from '../DefaultLayout';
import widgetApi from '../../lib/widgetApi';

jest.mock('../../lib/widgetApi');
const fakeResponse = {
  data: {
    Currency: [{
      CurrencyId: 1,
      Currency: "USD",

    },
    {
      CurrencyId: "20",
      Currency: "UUS",

    }
    ],
    BaseCurrency: []
  }
};
test('renders the DefaultLayout component', () => {
    widgetApi
    .fetchData
    .mockImplementation(() => Promise.resolve(fakeResponse));
    const wgStatus = [
        { key: "abc", enable: true }
    ];

    const component = renderer.create(
        <DefaultLayout langCode="en" initialWgStatus={wgStatus}>
        </DefaultLayout>
    );
    let tree = component.toJSON();
    expect(tree).toMatchSnapshot();
});
