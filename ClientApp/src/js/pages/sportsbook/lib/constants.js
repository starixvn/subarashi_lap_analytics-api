import moment from 'moment';

export const tabMenuKey = {
  bySport: 'bySport',
  byLeague: 'byLeague',
  byBettype: 'byBettype',
  byCurrency: 'byCurrency',
  byPlatform: 'byPlatform'
}

export const dimension = {
  sport: 'sport',
  league: 'league',
  bettype: 'bettype',
  currency: 'currency',
  platform: 'platform'
}

export const measurement = {
  turnover: 'turnover',
  winloss: 'winloss'
}

export const apiUrl = {
  bySport: '/api/sportsbook/sport',
  byLeagueSoccer: '/api/sportsbook/league/soccer',
  byLeagueBasketball: '/api/sportsbook/league/basketball',
  byBettypeSoccer: '/api/sportsbook/bettype/soccer',
  byBettypeBasketball: '/api/sportsbook/bettype/basketball',
  byCurrency: '/api/sportsbook/currency',
  byPlatform: '/api/sportsbook/platform'
}

export const defaultResponseData = { byTurnover: [], byWinloss:[] };

const dataAvailableFrom = '01/01/2021';
const minDate = window.minDate || dataAvailableFrom;

export const dateConstraint = {
  limitedMonthRange: 3,
  minDate: new Date(dataAvailableFrom) > new Date(minDate) ? dataAvailableFrom : minDate,
  maxDate: window.twoDaysBefore
}

export const rowDataMapper = [
  data => ({
    value: data.value,
    isPercentUnit: false
  }),
  data => ({
    value: data.distribution,
    isPercentUnit: true
  }),
  data => ({
    value: data.margin,
    isPercentUnit: true
  })
]
