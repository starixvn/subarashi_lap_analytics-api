import localStorageHelper from '../localStorageHelper';

describe('localStorageHelper', () => {
    const mockWindowProperty = (property, value) => {
        const {
            [property]: originalProperty
        } = window;
        delete window[property];
        beforeAll(() => {
            Object.defineProperty(window, property, {
                configurable: true,
                writable: true,
                value,
            });
        });
        afterAll(() => {
            window[property] = originalProperty;
        });
    };

    mockWindowProperty('localStorage', {
        setItem: jest.fn(),
        getItem: jest.fn(),
        removeItem: jest.fn(),
    });

    const runTestForSetItemWithThrowingError = (func) => {
        it('when error occur - window.alert should be called with correct message', () => {
            window.localStorage = {
                setItem: jest.fn(() => {
                    throw new Error('Error');
                })
            }
            window.alert = jest.fn();

            func();

            expect(window.alert).toHaveBeenCalledWith('To continue use dashboard. Please turn private browsing off!!');
        })
    }


    function fakeDashboardScript(scriptSrc) {
        var scriptElement = window.document.createElement("script");
        scriptElement.id = "dashboard-script";
        scriptElement.src = scriptSrc;
        window.document.body.appendChild(scriptElement);

        return scriptElement;
    }
    let originalLocalStorage = window.localStorage;

    beforeEach(() => {
        jest.resetAllMocks();
        global.alert = jest.fn(text => text)
    });

    afterEach(() => {
        window.localStorage = originalLocalStorage;
    });

    describe('clearLayoutSettingIfNewVersion()', () => {
        beforeEach(() => {
            window.localStorage = {
                setItem: jest.fn(),
                removeItem: jest.fn(),
                getItem: jest.fn()
            }
        })

        afterEach(() => {
            window.document.body.innerHTML = '';
        });

        test('no dashboard script: do nothing', () => {
            localStorageHelper.clearLayoutSettingIfNewVersion();
            expect(window.localStorage.getItem)
                .not
                .toHaveBeenCalled();
        });
        test('script version not match: do nothing', () => {
            fakeDashboardScript('something else i don\'t know');

            localStorageHelper.clearLayoutSettingIfNewVersion();
            expect(window.localStorage.getItem)
                .not
                .toBeCalled();
        });
        test('get cache version with correct setting key', () => {
            fakeDashboardScript('f**k.js?v=123');

            localStorageHelper.clearLayoutSettingIfNewVersion();

            expect(window.localStorage.getItem).toHaveBeenCalledWith('_LAP_Dashboard_Wg_Version');
        });

        [null, '', undefined, 'v=122'].forEach(cacheVersion => {
            test('dashboard script has different version with cache: set new version', () => {
                window.localStorage = {
                        setItem: jest.fn(),
                        removeItem: jest.fn(),
                        getItem: jest.fn()
                    }
                    // new version
                fakeDashboardScript('f**k.js?v=123456');

                localStorageHelper.clearLayoutSettingIfNewVersion();

                expect(window.localStorage.setItem).toHaveBeenCalledWith('_LAP_Dashboard_Wg_Version', 'v=123456');
                expect(window.localStorage.removeItem.mock.calls[0][0]).toBe('_LAP_Dashboard_Wg_Status');
                expect(window.localStorage.removeItem.mock.calls[1][0]).toBe('_LAP_Dashboard_Wg_Layout');
            });
        });

        test('dashboard script has same version with cache: do not set new version', () => {
            // new version
            window.localStorage = {
                getItem: jest.fn(() => 'v=123456'),
                setItem: jest.fn(),
                removeItem: jest.fn()
            }

            fakeDashboardScript('f**k.js?v=123456');

            localStorageHelper.clearLayoutSettingIfNewVersion();

            expect(window.localStorage.setItem).not.toHaveBeenCalled();
        });
    });

    describe('saveWidgetStatus(wgs)', () => {
        test('set item to localStorage with correct key', () => {
            window.localStorage = {
                setItem: jest.fn()
            }
            localStorageHelper.saveWidgetStatus('any-status');
            expect(window.localStorage.setItem).toHaveBeenCalledWith('_LAP_Dashboard_Wg_Status', JSON.stringify('any-status'));
        });

        runTestForSetItemWithThrowingError(() => localStorageHelper.saveWidgetStatus('any-status'));
    });


    describe('saveWidgetLayout(saveRemovedWidgetLayout)', () => {
        test('set item to localStorage with correct key', () => {
            const layoutConfigs = {
                any: 'layout-config'
            };
            window.localStorage = {
                setItem: jest.fn()
            }
            localStorageHelper.saveWidgetLayout(layoutConfigs);
            expect(window.localStorage.setItem).toHaveBeenCalledWith('_LAP_Dashboard_Wg_Layout', JSON.stringify(layoutConfigs));
        });

        runTestForSetItemWithThrowingError(() => localStorageHelper.saveWidgetLayout({}));
    });

    describe('saveRemovedWidgetLayout(layout)', () => {
        test('set item to localStorage with correct key', () => {
            const layoutConfigs = {
                any: 'layout-config'
            };
            window.localStorage = {
                setItem: jest.fn()
            }
            localStorageHelper.saveRemovedWidgetLayout(layoutConfigs);
            expect(window.localStorage.setItem).toHaveBeenCalledWith('_LAP_Dashboard_Wg_Layout_removed', JSON.stringify(layoutConfigs));
        });

        runTestForSetItemWithThrowingError(() => localStorageHelper.saveRemovedWidgetLayout({}));
    });

    describe('saveWidgetReloadCountdown(values)', () => {
        test('set item to localStorage with correct key', () => {
            const savedValue = {
                any: 'hellllo'
            };
            window.localStorage = {
                setItem: jest.fn()
            }
            localStorageHelper.saveWidgetReloadCountdown(savedValue);
            expect(window.localStorage.setItem).toHaveBeenCalledWith(
                '_LAP_Dashboard_Wg_Reload_Countdown',
                JSON.stringify(savedValue));
        });

        runTestForSetItemWithThrowingError(() => localStorageHelper.saveWidgetReloadCountdown({}));
    });

    describe('getWidgetStatus', () => {
        const widgetStatusCacheKey = "_LAP_Dashboard_Wg_Status";
        const returnedValue = {
            value: 'correctValue'
        };
        it('item has value - should return value correctly', () => {
            window.localStorage = {
                getItem: jest.fn((key) => {
                    return key === widgetStatusCacheKey ? JSON.stringify(returnedValue) : null
                })
            }

            const result = localStorageHelper.getWidgetStatus();

            expect(result).toEqual(returnedValue);
        })

        it('item is null - should return null', () => {
            window.localStorage = {
                getItem: jest.fn(() => {
                    return null
                })
            }

            const result = localStorageHelper.getWidgetStatus();

            expect(result).toEqual(null);
        })
    })

    describe('getWidgetLayout', () => {
        const widgetLayoutCacheKey = "_LAP_Dashboard_Wg_Layout";
        const returnedValue = {
            value: 'correctValue of widget layout'
        };
        it('item has value - should return value correctly', () => {
            window.localStorage = {
                getItem: jest.fn((key) => {
                    return key === widgetLayoutCacheKey ? JSON.stringify(returnedValue) : null
                })
            }

            const result = localStorageHelper.getWidgetLayout();

            expect(result).toEqual(returnedValue);
        })

        it('item is null - should return null', () => {
            window.localStorage = {
                getItem: jest.fn(() => {
                    return null
                })
            }

            const result = localStorageHelper.getWidgetLayout();

            expect(result).toEqual(null);
        })
    })

    describe('getRemovedWidgetLayout', () => {
        const widgetLayoutRemovedCacheKey = "_LAP_Dashboard_Wg_Layout_removed";
        const returnedValue = {
            value: 'correctValue of RemovedWidgetLayout'
        };
        it('item has value - should return value correctly', () => {
            window.localStorage = {
                getItem: jest.fn((key) => {
                    return key === widgetLayoutRemovedCacheKey ? JSON.stringify(returnedValue) : null
                })
            }

            const result = localStorageHelper.getRemovedWidgetLayout();

            expect(result).toEqual(returnedValue);
        })

        it('item is null - should return null', () => {
            window.localStorage = {
                getItem: jest.fn(() => {
                    return null
                })
            }

            const result = localStorageHelper.getRemovedWidgetLayout();

            expect(result).toEqual({});
        })
    })

    describe('getWidgetReloadCountdown', () => {
        const widgetReloadCountdownCacheKey = "_LAP_Dashboard_Wg_Reload_Countdown";
        const returnedValue = {
            value: 'correctValue of RemovedWidgetLayout'
        };
        it('item has value - should return value correctly', () => {
            window.localStorage = {
                getItem: jest.fn((key) => {
                    return key === widgetReloadCountdownCacheKey ? JSON.stringify(returnedValue) : null
                })
            }

            const result = localStorageHelper.getWidgetReloadCountdown();

            expect(result).toEqual(returnedValue);
        })

        it('item is null - should return null', () => {
            window.localStorage = {
                getItem: jest.fn(() => {
                    return null
                })
            }

            const result = localStorageHelper.getWidgetReloadCountdown();

            expect(result).toEqual({});
        })
    })
})