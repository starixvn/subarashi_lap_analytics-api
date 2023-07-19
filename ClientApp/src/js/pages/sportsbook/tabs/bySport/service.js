const sportOrder = {
  [1]: {
    [false]: 0, //Soccer
    [true]: 60 //Saba Soccer
  },
  [2]: {
    [false]: 10, //Basketball
    [true]: 70 //Saba Basketball
  },
  [43]: {
    [false]: 20, //E-Sports
    [true]: 80 //Saba E-Sports
  },
  [5]: {
    [false]: 30, //Tennis
    [true]: 90 //Saba Tennis
  },
  [50]: {
    [false]: 40 //Cricket
  },
  [-2]: {
    [false]: 50 //Parlay
  },
  [0]: {
    [false]: 900 //Others
  }
}

export const orderSports = sports => {
  const sortedData = [...sports];
  sortedData.sort((sport1, sport2) => {
    return sportOrder[sport1.id][sport1.isSaba] - sportOrder[sport2.id][sport2.isSaba];
  });
  return sortedData;
}
