import React, { Component } from 'react';
import { Bar } from 'react-chartjs-2';

const defaultColorBarChart = ['#2B465B', '#37C2CC', '#F15D21', '#4C6375', '#F1A837', '#208E9B', '#4C6568', '#524C68', '#39AF93', '#5F684C', '#4A863B'];
const defaultTooltip = value => {
  return value + '%';
}
const defaultDisplayAxisY = value => {
  return value + '%';
}

const defaultLlimitAxisY = {
  suggestedMax: 100,
}
const heightText = 10;

const buildChartDataSet = stackedChartData => {
  return {
    datasets: stackedChartData.map((_, index) => {
      return {
        type: 'bar',
        label: _.name,
        data: _.dataSet,
        fill: false,
        backgroundColor: _.color || defaultColorBarChart[index],
        borderColor: _.color || defaultColorBarChart[index],
        hoverBackgroundColor: _.color || defaultColorBarChart[index],
        hoverBorderColor: _.color || defaultColorBarChart[index],
      }
    })
  };
}

const drawTextToTopChart = function () {
  const self = this,
    chart = this.chart,
    ctx = chart.ctx;
  ctx.textAlign = "center";
  ctx.fillStyle = "#FFFFFF";
  ctx.textAlign = "left";
  ctx.textBaseline = "middle";

  self.data.datasets.forEach(function (dataset, datasetIndex) {
    const meta = self.getDatasetMeta(datasetIndex);
    !meta.hidden && meta.data.forEach(function (element, elementIndex) {
      const value = dataset.data[elementIndex];
      if (value !== 0) {
        const text = defaultTooltip(value);
        const textWidth = ctx.measureText(text).width;
        const height = element.height();
        if (Math.abs(height) <= heightText) {
          return null;
        }
        const xLabel = element._model.x - textWidth / 3;
        const yLabel = element._model.y + height / 2;
        ctx.fillText(text, xLabel, yLabel);
      }
      return null;
    });
  });
}

const buildChartOption = ({ labels, limitAxisY }) => {
  limitAxisY = limitAxisY || defaultLlimitAxisY;

  return {
    tooltips: {
      callbacks: {
        title: function() {
          return '';
        },
        label: function (tooltipItem, data) {
          const dataSet = data.datasets[tooltipItem.datasetIndex];
          const label = dataSet.label;
          return label + ' ' + defaultTooltip(tooltipItem.value);
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
      display: true,
      position: 'bottom',
      onHover: function(e) {
        e.target.style.cursor = 'pointer';
      },
      labels: {
        usePointStyle: true,
        boxWidth: 8,
        padding: 20
      }
    },
    animation: {
      onComplete: drawTextToTopChart
    },
    layout: {
      padding: {
        top: 30,
        bottom: 20
      }
    },
    scales: {
      xAxes: [
        {
          ...{ barPercentage: 0.4 },
          ...{
            display: true,
            gridLines: {
              display: false
            },
            labels: {
              show: true
            },
            labels: labels,
            stacked: true
          }
        }
      ],
      yAxes: [
        {
          stacked: true,
          ticks: {
            ...{
              beginAtZero: true,
              callback: function (value) {
                return defaultDisplayAxisY(value);
              }
            },
            ...limitAxisY
          }
        }
      ]
    }
  };
}

export default class StackedBarChart extends Component {
  constructor(props) {
    super(props);
  }

  _renderChart() {
    const { stackedChartData, labels, limitAxisY } = this.props;
    const data = buildChartDataSet(stackedChartData);
    const options = buildChartOption({ labels, limitAxisY });
    return <Bar
      data={data}
      options={options}
    />
  }

  render() {
    return (
      <div className="chart-container">
        {this._renderChart()}
      </div>
    );
  }
}
