import axios from 'axios';
import widgetApi from '../widgetApi';

jest.mock('axios');

describe('fetchData(param)', () => {
    describe('Data of param is null', () => {
        afterEach(() => {
            axios
                .get
                .mockReset();
        });

        test('axios.get function should be called with correct parameter', () => {
            // assign
            window.HostUrl = '/lap/SessionIsAlive';
            const param = { Data: null, Url: 'test' };

            // action
            widgetApi.fetchData(param);

            // assert
            expect(axios.get).toBeCalledWith('/lap/test', { params: {} });
        });
    });

    describe('Not cross site: ', () => {
        const fakeParams = {
            Data: {
                any: 123.456
            },
            Url: '/fake/url',
            IsCrossSite: false
        };

        afterEach(() => {
            axios
                .get
                .mockReset();
        });
    });

    describe('cross site', () => {
        const fakeParams = {
            Data: {
                any: 456.789
            },
            Url: '/fake/url',
            IsCrossSite: true
        };
        const fakeResponse = {
            data: {
                Token: 'abc xyz',
                ApiUrl: '/fake/api'
            }
        };
        const _getAuthToken = widgetApi._getAuthToken;
        afterEach(() => {
            widgetApi._getAuthToken = _getAuthToken;
            axios
                .get
                .mockReset();
        });

        test('get auth token', () => {
            // assign
            widgetApi._getAuthToken = jest.fn(() => Promise.resolve(fakeResponse));

            // action
            widgetApi.fetchData(fakeParams);

            // assert
            expect(widgetApi._getAuthToken).toBeCalledWith(fakeParams.Url, 1);
        });
        test('after get auth token get data by given url', () => {
            // assign
            const promise = Promise.resolve(fakeResponse);
            widgetApi._getAuthToken = jest.fn(() => promise);

            // action
            widgetApi.fetchData(fakeParams);

            promise.then(() => {
                const params = Object.assign({
                    'X-Auth-Token': 'abc xyz'
                }, fakeParams.Data);

                expect(axios.get).toBeCalledWith('/fake/api/fake/url', { params: params });
            });
        });
    })
});
