export default class numberFormat {
    static formatCurrency(value = 0, currency) {
        const currencies = {
            "EUR": "€",
            "GBP": "£",
            "USD": "$",
            "CNY": "¥",
            "MYR": "RM",
            "THB": "฿",
            "IDR": "Rp",
            "BDT": "৳",
            "INR": "₹"
        }
        currency = currencies[currency] ? currencies[currency] : ''

        return `${currency}${this.format(value, 2)}`
    }

    static formatDecimal(value) {
        return this.format(value, 2)
    }

    static format(value, digits = 0) {
        return value.toFixed(digits).replace(/(\d)(?=(\d{3})+(\.|$))/g, "$1,")
    }

    static nFormatter(num, digits = 2) {
      const si = [
        { value: 1, symbol: "" },
        { value: 1E3, symbol: "K" },
        { value: 1E6, symbol: "M" },
        { value: 1E9, symbol: "B" },
        { value: 1E12, symbol: "T" }
      ];
      const rx = /\.0+$|(\.[0-9]*[1-9])0+$/;
      let i;
      const absNum = Math.abs(num);
      for (i = si.length - 1; i > 0; i--) {
        if (absNum >= si[i].value) {
          break;
        }
      }
      return (num / si[i].value).toFixed(digits).replace(rx, "$1") + si[i].symbol;
    }
}

