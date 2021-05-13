import React, { useMemo } from 'react';
import { Pie } from 'react-chartjs-2';
import numberFormat from '../../../lib/numberFormat';

const defaultColorBarChart = ['#2B465B', '#37C2CC', '#F15D21', '#4C6375', '#F1A837', '#208E9B', '#4C6568', '#4A863B', '#39AF93', '#5F684C', '#524C68'];
const displayPercentValue = value => {
  return value + '%';
}

const defaultHeightPieChart = 320;

const drawTextToChart = function () {
  var self = this,
    chart = this.chart,
    ctx = chart.ctx;

  var chartSize = Math.min(chart.width, chart.height);
  if (chartSize < 100) {
    return;
  }

  ctx.textAlign = "center";
  ctx.fillStyle = "#fff";
  ctx.save();
  ctx.shadowColor = "black";
  ctx.shadowBlur = 3;
  ctx.font = '11px "Helvetica Neue", Helvetica, Arial, sans-serif';
  var chartCircumference = chart.config.options.circumference;
  self.data.datasets.forEach((dataSet, datasetIndex) => {
    var meta = self.getDatasetMeta(datasetIndex);
    meta.data.forEach((element, index) => {
      var position = element.tooltipPosition();
      if (element._model.circumference > 0) {
        const segmentPercent = element._model.circumference / chartCircumference * 100;
        if (segmentPercent >= 3) {
          let value = dataSet.data[index];
          value = numberFormat.formatDecimal(value);
          ctx.fillText(displayPercentValue(value), position.x, position.y);
        }
      }
    });
  });

  ctx.restore();
}

const buildOptions = ({ title }) => {
  return {
    title: {
      display: true,
      position: "top",
      text: title,
      fontSize: 15,
      fontStyle: 'normal'
    },
    layout: {
      padding: {
        top: 10,
        bottom: 10,
        right: 60
      }
    },
    legend: {
      display: true,
      position: 'right',
      onHover: function (e) {
        e.target.style.cursor = 'pointer';
      },
      labels: {
        usePointStyle: true,
        boxWidth: 10,
        padding: 12
      }
    },
    responsive: true,
    hover: {
      animationDuration: 0
    },
    animation: {
      onComplete: drawTextToChart
    },
    maintainAspectRatio: false,
    tooltips: {
      callbacks: {
        title: function () {
          return '';
        },
        label: function (tooltipItem, data) {
          const { datasetIndex, index } = tooltipItem;
          const value = data.datasets[datasetIndex].data[index];
          const label = data.labels[index];
          return label + ' ' + displayPercentValue(value);
        }
      }
    },
  };
}

const buildData = (data, displayNameMapper)=> ({
  datasets: [
    {
      data: data.map(_ => _.distribution),
      backgroundColor: defaultColorBarChart
    }
  ],
  labels: data.map(_ => displayNameMapper ? displayNameMapper(_) : _.name)
})

const DistributionPieChart = ({ data, title, displayNameMapper, height }) => {
  const distributionData = useMemo(() => buildData(data, displayNameMapper), [data])
  const options = useMemo(() => buildOptions({ title }), [title])
  return (<Pie
    data={distributionData}
    options={options}
    height={height || defaultHeightPieChart}
  />)
}

export default React.memo(DistributionPieChart)
