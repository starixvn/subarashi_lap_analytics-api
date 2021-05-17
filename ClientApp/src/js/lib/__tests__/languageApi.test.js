import languageApi from '../languageApi';

test('getRes(langcode,resKey) return correct result with correct langcode', () => {
    expect(languageApi.getRes("cs", "licensee_winloss_month_todate")).toBe("投注输赢(当月累计)");
});

test('getRes(langcode,resKey) return english result with incorrect langcode', () => {
    expect(languageApi.getRes("abc", "c_turnover")).toBe("Turnover by Platform");
});