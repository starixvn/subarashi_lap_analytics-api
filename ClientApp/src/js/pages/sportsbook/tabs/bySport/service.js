const sportOrder = {
  [1]: {
    [false]: 0, //Soccer
    [true]: 50 //Saba Soccer
  },
  [2]: {
    [false]: 10, //Basketball
    [true]: 60 //Saba Basketball
  },
  [43]: {
    [false]: 20 //E-Sports
  },
  [5]: {
    [false]: 30 //Tennis
  },
  [-2]: {
    [false]: 40 //Parlay
  },
  [0]: {
    [false]: 70 //Others
  }
}

export const orderSports = sports => {
  const sortedData = [...sports];
  sortedData.sort((sport1, sport2) => {
    return sportOrder[sport1.id][sport1.isSaba] - sportOrder[sport2.id][sport2.isSaba];
  });
  return sortedData;
}
