import React, { Component } from 'react';
import { Bar } from 'react-chartjs-2';
import numberFormat from '../../../../../lib/numberFormat';

const defaultColorBarChart = ['#208E9B'];
const defaultCalculateBarWidth = numberOfColumn => {
  if(numberOfColumn >= 6) {
    return  { barPercentage: 0.9, categoryPercentage: 0.5 }
  } else {
    return { barPercentage: 0.5, categoryPercentage: 0.6  }
  }
}

const buildBarChartDataSet = barChartData => {
  return barChartData.map((_, index) => {
    return {
      type: 'bar',
      label: _.name,
      data: _.dataSet,
      fill: false,
      backgroundColor: _.color || defaultColorBarChart[index],
      borderColor: _.color || defaultColorBarChart[index],
      hoverBackgroundColor: _.color || defaultColorBarChart[index],
      hoverBorderColor: _.color || defaultColorBarChart[index],
      yAxisID: 'y-axis-1'
    }
  });
}

const buildChartDataSet = barChartData => {
  const barChartDataSet = buildBarChartDataSet(barChartData);
  return {
    datasets: [...barChartDataSet]
  }
}

const drawTextToTopChart = function () {
  const self = this,
    chart = this.chart,
    ctx = chart.ctx;
  ctx.textAlign = "center";
  ctx.fillStyle = "#555";
  ctx.textAlign = "left";
  ctx.textBaseline = "middle";

  self.data.datasets.forEach(function (dataset, datasetIndex) {
    const meta = self.getDatasetMeta(datasetIndex);
    meta.data.forEach(function (element, elementIndex) {
      const value = dataset.data[elementIndex];
      const text = value + '%';
      const yOffset = value > 0 ? 6 : -7;
      const textWidth = ctx.measureText(text).width;
      const xLabel = element._model.x - textWidth/2;
      ctx.fillText(text, xLabel, element._model.y - yOffset);
    });
  });
}

const buildChartOption = ({ labels, barChartData, hideLegend }) => {
  const barWidthObj = defaultCalculateBarWidth(barChartData[0].dataSet.length);
  const leftAxisName = barChartData[0].name || '';

  return {
    onClick: function (e, item) {
      const activeElement = this.getElementAtEvent(e);
      if (activeElement && activeElement.length > 0 && activeElement[0]._datasetIndex) {
        const datasetIndex = activeElement[0]._datasetIndex - 1;
        const events = barChartData[datasetIndex].events;
        const eventClicks = events ? events.click : null;
        if (eventClicks && eventClicks.length > 0) {
          const colIndex = item[0]._index;
          const eventClick = eventClicks.find(_ => _.columnIndex === colIndex);
          eventClick && eventClick.callBack && eventClick.callBack();
        }
      }
    },
    onHover: (event, chartElement) => {
      event.target.style.cursor = chartElement[0] ? 'pointer' : 'default';
    },
    tooltips: {
      callbacks: {
        label: function (tooltipItem, data) {
          const dataSet = data.datasets[tooltipItem.datasetIndex];
          if (dataSet.type === 'bar') {
            return numberFormat.formatDecimal(dataSet.data[tooltipItem.index]);
          }
          return tooltipItem.value;
        }
      }
    },
    responsive: true,
    elements: {
      line: {
        fill: false
      }
    },
    hover: {
      animationDuration: 0
    },
    legend: {
      display: !hideLegend,
      position: 'bottom',
      labels: {
        filter: function (item) {
          return !item.hidden;
        },
        usePointStyle: true,
        boxWidth: 8,
        padding: 20
      },
      onClick: e => e.stopPropagation()
    },
    animation: {
      onComplete: drawTextToTopChart
    },
    layout: {
      padding: {
        top: 20,
        bottom: 20
      }
    },
    scales: {
      xAxes: [
        {
          ...barWidthObj,
          ...{
            display: true,
            gridLines: {
              display: false
            },
            labels: {
              show: true
            },
            labels: labels
          }
        }
      ],
      yAxes: [
        {
          type: 'linear',
          display: true,
          position: 'left',
          id: 'y-axis-1',
          labels: {
            show: true
          },
          scaleLabel: {
            display: true,
            fontSize: 13,
            fontFamily: "'Roboto'",
            labelString: leftAxisName
          },
          ticks: {
            beginAtZero: true,
            callback: function (value) {
              return numberFormat.nFormatter(value);
            }
          }
        }
      ]
    }
  };
}

export default class BarChart extends Component {
  constructor(props) {
    super(props);
  }

  _renderChart() {
    const { barChartData, labels, hideLegend } = this.props;
    const data = buildChartDataSet(barChartData);
    const options = buildChartOption({ labels, barChartData, hideLegend });
    return <Bar
      data={data}
      options={options}
    />
  }

  render() {
    const { header } = this.props;
    return (
      <div className="chart-container">
        {header && header()}
        {this._renderChart()}
      </div>
    );
  }
}
