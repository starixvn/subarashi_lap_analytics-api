
const getLastQuarter = currentTime => {
  const { month, year } = currentTime;
  const currentQuater = Math.ceil((month + 1) / 3);
  const lastQuarter = currentQuater - 1 || 4;
  return {
    quarter: lastQuarter,
    year: lastQuarter === 4 ? year - 1 : year
  }
}

export const getDistanceBetweenPeriod = ({ from, to }) => {
  const distance = 12 * (to.year - from.year) + (to.month - from.month);
  return distance;
}

export const addMonths = (date, numberOfMonth) => {
  const newDate = new Date(date.setMonth(date.getMonth() + numberOfMonth));
  return newDate;
}

export const defaultFormatFunc = (month, year) => {
  return `${month < 10 ? '0' + month : month}/${year}`
}

export const getPreviousPeriod = currentPeriod => {
  const { from } = currentPeriod;
  const distance = getDistanceBetweenPeriod(currentPeriod);

  let date = new Date(from.year, from.month);
  date = addMonths(date, -1);
  const prevPeriodTo = {
    month: date.getMonth(),
    year: date.getFullYear(),
  }

  date = addMonths(date, -distance);
  const prevPeriodFrom = {
    month: date.getMonth(),
    year: date.getFullYear()
  }

  return {
    from: prevPeriodFrom,
    to: prevPeriodTo
  }
}

export const getPreviousYear = currentPeriod => {
  const { from, to } = currentPeriod;

  const prevPeriodFrom = {
    month: from.month,
    year: from.year - 1
  }
  const prevPeriodTo = {
    month: to.month,
    year: to.year - 1
  }

  return {
    from: prevPeriodFrom,
    to: prevPeriodTo
  }
}

export const getPeriodByNumberOfLastMonths = (currentTime, numberOfMonth) => {
  const { month, year } = currentTime;

  let date = new Date(year, month);
  date = addMonths(date, -1);
  const to = {
    month: date.getMonth(),
    year: date.getFullYear()
  }

  date = addMonths(date, -numberOfMonth + 1);
  const from = {
    month: date.getMonth(),
    year: date.getFullYear()
  }

  return { from, to }
}

export const getPeriodByLastQuarter = currentTime => {
  const { quarter, year } = getLastQuarter(currentTime);
  const to = {
    month: quarter * 3 - 1,
    year
  }
  const from = {
    month: to.month - 2,
    year
  }
  return {
    from,
    to
  }
}

export const convertDateToString = date => {
  return typeof date === "string" ? date : date.format("MM/DD/YYYY");
}

