import numberFormat from '../numberFormat';

describe('formatCurrency()', () => {
    test('exclude symbol when currency not found', () => {
        [null, undefined, '', 'WTF'].forEach(currency => {
            expect(numberFormat.formatCurrency(1234.1, currency)).toBe("1,234.10");
        });
    });

    test('add currency symbol', () => {
         const currencies = {
            "EUR": "€",
            "GBP": "£",
            "USD": "$",
            "CNY": "¥",
            "MYR": "RM",
            "THB": "฿",
            "IDR": "Rp"
        }

        Object.keys(currencies).forEach(currency => {
            expect(numberFormat.formatCurrency(1234.1, currency)).toBe(`${currencies[currency]}1,234.10`);
        });
    });

    test('no parameters: default zero', () => {
        expect(numberFormat.formatCurrency()).toBe("0.00");
    });
});

describe('formatDecimal()', () => {
    test('has default 2 decimal places', () => {
        expect(numberFormat.formatDecimal(1234.1)).toBe("1,234.10");
    });
});

describe('format()', () => {
    test('format an integer number with 3 decimal places', () => {
        expect(numberFormat.format(1234, 3)).toBe("1,234.000");
    });

    test('format a decimal number with 2 decimal places', () => {
        expect(numberFormat.format(1234.149, 2)).toBe("1,234.15");
    });

    test('format an integer number', () => {
        expect(numberFormat.format(1234.149)).toBe("1,234");
    });
});
