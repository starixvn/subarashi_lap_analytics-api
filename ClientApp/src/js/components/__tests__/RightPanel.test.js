import React from 'react';
import renderer from 'react-test-renderer';
import RightPanel from '../RightPanel';
import widgetApi from '../../modules/widgetApi';
import * as constants from '../../modules/constants';

jest.mock('../../modules/widgetApi');

describe('RightPanel', () => {
    const baseCurrencyList = constants.baseCurrencyList;
    const fakeProps = {
        wgs: [{
            key: "abc",
            enable: true
        },
        {
            key: "def",
            enable: true
        }
        ],
        langCode: 'en',
        callbackParent: jest.fn()
    };
    const fakeResponse = {
        data: {
            Currency: [{
                CurrencyId: 1,
                Currency: "USD",

            },
            {
                CurrencyId: 20,
                Currency: "UUS",

            }],
            BaseCurrency: baseCurrencyList
        }
    };

    function findElement(component, selector) {
        if (component._component) {
            return findElement(component._component, selector);
        }

        if (component._currentElement) {
            const element = component._currentElement;

            if (element.ref === selector) {
                return element;
            }

            if (selector[0] === '.' && element.props && element.props.className) {
                const reg = new RegExp(selector.substring(1));
                if (component._currentElement.props.className.match(reg)) {
                    return element;
                }
            }
        }

        if (component._renderedComponent) {
            return findElement(component._renderedComponent, selector);
        }

        if (component._renderedChildren) {
            for (let key in component._renderedChildren) {
                let found = findElement(component._renderedChildren[key], selector);
                if (found) {
                    return found;
                }
            }
        }

        return null;
    }

    describe('constructor', () => {
        it('default based currency value not in based currency list - selectedBasedCurrency should be fisrt value', () => {
            const { defaultBasedCurrency } = constants;
            constants.defaultBasedCurrency = "NotInBasedList";
            const instance = new RightPanel({...fakeProps});
            const expectedResult = constants.baseCurrencyList[0].CurrencyId;

            const result = instance.state.SelectedBaseCurrency;

            expect(result).toBe(expectedResult);

            constants.defaultBasedCurrency = defaultBasedCurrency;
        })

        it('default based currency value is in based currency list - selectedBasedCurrency should be correct value', () => {
            const { defaultBasedCurrency } = constants;
            constants.defaultBasedCurrency = "USD";
            const instance = new RightPanel({...fakeProps});
            const expectedResult = constants.baseCurrencyList.find(x => x.Currency === constants.defaultBasedCurrency).CurrencyId;

            const result = instance.state.SelectedBaseCurrency;

            expect(result).toBe(expectedResult);

            constants.defaultBasedCurrency = defaultBasedCurrency;
        })
    })

    test('renders correctly', () => {
        widgetApi
            .fetchData
            .mockImplementation(() => Promise.resolve(fakeResponse));
        const component = renderer.create(<RightPanel {...fakeProps} />);
        let tree = component.toJSON();

        expect(tree).toMatchSnapshot();
    });

    test('hidden state change: re-render', () => {
        widgetApi
            .fetchData
            .mockImplementation(() => Promise.resolve(fakeResponse));
        const component = renderer.create(< RightPanel {...fakeProps
        }
        />);

        component
            .getInstance()
            .setState({
                isHidden: false
            });

        let tree = component.toJSON(); expect(tree).toMatchSnapshot();
    });

    test('componentDidMount: set mount state true', () => {
        const component = new RightPanel(fakeProps)
        component.setState = jest.fn();
        component.componentDidMount();
        expect(component.setState.mock.calls[0][0].isMounted).toBeTruthy();
    });

    test('componentDidMount: fetch data with given props', () => {
        const promise = Promise.resolve(fakeResponse);
        widgetApi
            .fetchData
            .mockImplementation(() => promise);
        const component = renderer.create(< RightPanel {...fakeProps
        }
        />);

        promise.then(() => expect(component.toJSON()).toMatchSnapshot());
    });

    test('componentWillUnmount: set mount status false', () => {
        const component = new RightPanel(fakeProps);
        component.setState = jest.fn();

        component.componentWillUnmount();

        expect(component.setState).toBeCalledWith({
            isMounted: false
        });
    });

    test('bindWgCheckBox(wgs): return mapped items from given parameter', () => {
        const component = new RightPanel(fakeProps);

        const result = component._bindWgCheckBox([{
            key: 'a',
            enable: true
        }, {
            key: 'b',
            enable: false
        }]);

        expect(result).toEqual({
            a: true,
            b: false
        });
    });

    describe('_fetchData()', () => {
        test('fetch data by criteria', () => {
            const promise = Promise.resolve();
            promise.then = () => promise;
            widgetApi
                .fetchData
                .mockImplementation(() => promise);

            const component = new RightPanel(fakeProps);

            component._fetchData();

            expect(widgetApi.fetchData).toBeCalledWith({
                Url: 'GetCurrencyInfo',
                IsCrossSite: false
            });
        });

        test('write log when fetch data fail', () => {
            // assign
            const component = new RightPanel(fakeProps);
            const promise = Promise.resolve();
            promise.then = () => promise;
            promise.catch = c => {
                c('should write log when fail');
                return promise;
            };

            widgetApi
                .fetchData
                .mockImplementation(() => promise);

            const _log = console.log;
            console.log = jest.fn();

            //  action
            component._fetchData(fakeProps);

            // assert
            expect(console.log).toBeCalledWith('error:', 'should write log when fail');
            console.log = _log;
        });

        test('update state', () => {
            const response = {
                data: {
                    Currency: [1, 2, 3]
                }
            };
            const promise = Promise.resolve(response);
            widgetApi.fetchData.mockImplementation(() => promise);
            const component = new RightPanel(fakeProps);

            component.setState = jest.fn();

            component._fetchData();

            return promise.then(() => {
                expect(component.setState).toBeCalledWith({
                    isMounted: true,
                    CurrencyList: response.data.Currency
                });
            });
        });

        test('insert option "All" at index 0', () => {
            const response = {
                data: {
                    Currency: [1, 2, 3]
                }
            };
            const promise = Promise.resolve(response);
            widgetApi.fetchData.mockImplementation(() => promise);
            const component = new RightPanel(fakeProps);
            component.setState = jest.fn();

            component._fetchData();

            return promise.then(() => {
                expect(response.data.Currency).toEqual([{
                    Currency: "All",
                    CurrencyId: "0"
                },
                    1,
                    2,
                    3
                ]);
            });
        });
    });

    describe('_togglePanel()', () => {
        [true, false].forEach(isHidden => {
            test('toggle hidden status', () => {
                const component = new RightPanel({
                    wgs: []
                });
                component.setState = jest.fn();
                component.state = {
                    isHidden: isHidden
                };

                component._togglePanel();
                const result = component.setState.mock.calls[0][0](component.state);
                expect(result).toEqual({
                    isHidden: !isHidden
                });
            });
        });
    });

    describe('event listeners', () => {
        beforeEach(() => {
            widgetApi.fetchData.mockReset();
            fakeProps.callbackParent.mockReset();
            widgetApi.fetchData.mockImplementation(() => Promise.resolve(fakeResponse));
        });

        test('on currency list change - Select All: update state', () => {

            const component = renderer.create(<RightPanel {...fakeProps} />);
            const componentInstance = component.getInstance();
            const element = findElement(component, 'currencyDropdownList');
            componentInstance.setState = jest.fn();

            element.props.onChange({
                target: {
                    value: '0'
                }
            });

            expect(componentInstance.setState).toBeCalledWith({
                SelectedCurrency: '0',
                SelectedBaseCurrency: baseCurrencyList[0].CurrencyId,
                BaseCurrencyList: baseCurrencyList
            });
        });

        test('on currency list change - specific currency: update state', () => {

            const component = renderer.create(< RightPanel {...fakeProps} />);
            const componentInstance = component.getInstance();
            const element = findElement(component, 'currencyDropdownList');

            componentInstance.setState({
                CurrencyList: fakeResponse.data.Currency
            });
            componentInstance.setState = jest.fn();

            element.props.onChange({
                target: {
                    value: '20'
                }
            });

            expect(componentInstance.setState).toBeCalledWith({
                SelectedCurrency: '20',
                SelectedBaseCurrency: '20',
                BaseCurrencyList: baseCurrencyList.concat({ Currency: "UUS", CurrencyId: 20 })
            });
        });

        test('on currency list change - specific base currency: update state', () => {

            const component = renderer.create(<RightPanel {...fakeProps} />);
            const componentInstance = component.getInstance();
            const element = findElement(component, 'currencyDropdownList');

            componentInstance.setState = jest.fn();

            element.props.onChange({
                target: {
                    value: '6'
                }
            });

            expect(componentInstance.setState).toBeCalledWith({
                SelectedCurrency: '6',
                BaseCurrencyList: baseCurrencyList
            });

        });

        test('on currency list change - specific base currency and selected base currency = another specific currency: update state', () => {

            const component = renderer.create(<RightPanel {...fakeProps} />);
            const componentInstance = component.getInstance();
            const element = findElement(component, 'currencyDropdownList');

            componentInstance.setState({
                SelectedBaseCurrency: 20
            })
            componentInstance.setState = jest.fn();

            element.props.onChange({
                target: {
                    value: '6'
                }
            });

            expect(componentInstance.setState).toBeCalledWith({
                SelectedCurrency: '6',
                SelectedBaseCurrency: '6',
                BaseCurrencyList: baseCurrencyList
            });

        });


        test('on base currency list change: update state', () => {
            const component = renderer.create(< RightPanel {...fakeProps} />);
            const componentInstance = component.getInstance();
            const element = findElement(component, 'baseCurrencyDropdownList');

            componentInstance.setState = jest.fn(); element.props.onChange({
                target: {
                    value: 'fake currency value'
                }
            });

            expect(componentInstance.setState).toBeCalledWith({
                SelectedBaseCurrency: 'fake currency value'
            });
        });

        describe('on submit button click', () => {
            test('component is unmounted then do nothing', () => {
                const component = renderer.create(< RightPanel {...fakeProps} />);
                const componentInstance = component.getInstance();
                const element = findElement(component, '.btn-submit');

                componentInstance.setState({
                    isMounted: false
                }); fakeProps.callbackParent.mockReset(); element.props.onClick();

                expect(fakeProps.callbackParent).not.toBeCalled();
            });

            test('component is mounted - selectedBaseCurrency is in Base CurrencyList - baseCurrency name is passed to callbackParent()', () => {
                const component = renderer.create(< RightPanel {...fakeProps} />);
                const componentInstance = component.getInstance();
                const element = findElement(component, '.btn-submit');

                componentInstance.setState({
                    isMounted: true,
                    SelectedCurrency: '6',
                    SelectedBaseCurrency: '6'
                });

                element.props.onClick();

                expect(fakeProps.callbackParent).toBeCalledWith({
                    action: 'apply',
                    filter: {
                        currency: '6',
                        baseCurrency: 'EUR'
                    }
                });
            });
        });

        test('component is mounted - selectedBaseCurrency is not in Base CurrencyList - empty string is passed to callbackParent()', () => {
            const component = renderer.create(< RightPanel {...fakeProps} />);
            const componentInstance = component.getInstance();
            const element = findElement(component, '.btn-submit');

            componentInstance.setState({
                isMounted: true,
                SelectedCurrency: '20',
                SelectedBaseCurrency: '20'
            });
            element.props.onClick();

            expect(fakeProps.callbackParent).toBeCalledWith({
                action: 'apply',
                filter: {
                    currency: '20',
                    baseCurrency: ''
                }
            });
        });
    });

    test('on quickaccess click: toggle panel', () => {
        const component = renderer.create(< RightPanel {...fakeProps} />);
        const componentInstance = component.getInstance();
        const element = findElement(component, '.icon-quickaccess');

        componentInstance._togglePanel = jest.fn();
        element.props.onClick('fake event');

        expect(componentInstance._togglePanel).toBeCalledWith('fake event');
    });

    test('right panel overlay click: toggle panel', () => {
        const component = renderer.create(<RightPanel {...fakeProps} />);
        const componentInstance = component.getInstance();
        const element = findElement(component, '.right-panel-overlay');

        componentInstance._togglePanel = jest.fn();
        element.props.onClick('fake event');

        expect(componentInstance._togglePanel).toBeCalledWith('fake event');
    });

    describe('on widget item check status change', () => {
        test('state is unmounted: do nothing', () => {
            fakeProps.callbackParent.mockReset();
            const component = renderer.create(<RightPanel {...fakeProps} />);
            const componentInstance = component.getInstance();
            const element = findElement(component, '.filled-in');

            componentInstance.setState({
                isMounted: false
            });

            element.props.onChange()

            expect(componentInstance.props.callbackParent).not.toBeCalled();
        });

        test('state is mounted: call parent callback and update state', () => {
            fakeProps.callbackParent.mockReset();
            const component = renderer.create(< RightPanel {...fakeProps} />);
            const componentInstance = component.getInstance();
            const element = findElement(component, '.filled-in');

            componentInstance.setState({
                isMounted: true
            });
            element.props.onChange();

            expect(componentInstance.props.callbackParent).toBeCalledWith({
                action: 'wgSyncStatus',
                items: [{
                    key: 'abc',
                    enable: false
                }]
            });
        });
    });

    describe('on reset button click', () => {
        test('state is unmounted: do nothing', () => {
            fakeProps.callbackParent.mockReset();
            const component = renderer.create(< RightPanel {...fakeProps} />);
            const componentInstance = component.getInstance();
            const element = findElement(component, '.btn-reset');

            componentInstance.setState({
                isMounted: false
            }, () => {

                element.props.onClick();
            });

            expect(componentInstance.props.callbackParent).not.toBeCalled();

        });

        test('state is mounted: call parent callback with action reset', () => {
            const component = renderer.create(< RightPanel {...fakeProps} />);
            const componentInstance = component.getInstance();
            const element = findElement(component, '.btn-reset');

            componentInstance.setState({
                isMounted: true
            });
            element.props.onClick();

            expect(componentInstance.props.callbackParent).toBeCalledWith({
                action: 'reset'
            });
        });
    });

    describe('componentDidUpdate(nextProps)', () => {
        test('component unmounted: do nothing', () => {
            const component = new RightPanel(fakeProps);
            component.setState = jest.fn();
            component.componentDidUpdate(fakeProps, { isMounted: false });

            expect(component.setState).not.toBeCalled();
        });

        test('mounted: bind widget status', () => {
            const wgs = [{
                key: "abcxxx",
                enable: true
            },
                {
                    key: "defxxx",
                    enable: false
                }
            ];
            const props = Object.assign({}, fakeProps, { wgs });
            const component = new RightPanel(props);
            component.setState = jest.fn();

            component.componentDidUpdate(fakeProps, { isMounted: true, wgs: { 'abc': true }});

            expect(component.setState).toBeCalledWith({
                wgs: {
                    "abcxxx": true,
                    "defxxx": false
                }
            });
        });
    });
})
