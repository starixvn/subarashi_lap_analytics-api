import React, { Component, Fragment } from 'react';
import { Pie } from 'react-chartjs-2';

const defaultColorBarChart = ['#2B465B', '#37C2CC', '#F15D21', '#4C6375', '#F1A837', '#208E9B', '#4C6568', '#524C68', '#39AF93', '#5F684C', '#4A863B'];

const buildChartDataSet = (dataSet) => {
  return {
    datasets: [
      {
        data: dataSet.distributions,
        backgroundColor: defaultColorBarChart,
      }
    ],
    labels: dataSet.labels
  }
}

const legendClickCallback = (legendItemIndex, event) => {
  event = event || window.event;
  var target = event.target || event.srcElement;
  target = target.nodeName == "SPAN" ? target.parentNode : target;

  const keys = Object.keys(Chart.instances);
  document.querySelectorAll('.chartjs-render-monitor').forEach((chartItem,index)=>{
    var chart = Chart.instances[keys[index]];
    if (chart.config.type == "pie") {
      var meta = chart.getDatasetMeta(0);
      var item = meta.data[legendItemIndex];
      if (item.hidden == null || item.hidden == false) {
        item.hidden = true;
        target.classList.add('hidden');
      } else {
        item.hidden = false;
        target.classList.remove('hidden');
      }
      chart.update();
    }
  });
}

const buildPieChartOption = (title) => {
  return ({
    legend: {
      display: false
    },
    tooltips: {
      callbacks: {
        label: function (tooltipItem, data) {
          var index = tooltipItem.index;
          return data.labels[index] + ': ' + data.datasets[0].data[index].toFixed(2) + '%';
        }
      }
    },
    title: {
      display: true,
      position: "bottom",
      text: title
    },
    maintainAspectRatio: false,
    hover: { animationDuration: 0 },
    animation: {
      onProgress: function(animation) {
        const meta = this.chart.getDatasetMeta(0);
        if (meta.data && meta.data.length > 0) {
          var myLegendContainer = document.getElementById("custom-legend");
          if (!myLegendContainer.innerHTML || myLegendContainer.getElementsByTagName('li').length <= 0) {
            // generate HTML legend
            myLegendContainer.innerHTML = this.chart.generateLegend();
            // bind onClick event to all LI-tags of the legend
            var legendItems = myLegendContainer.getElementsByTagName('li');
            for (var i = 0; i < legendItems.length; i++) {
              legendItems[i].addEventListener("click", legendClickCallback.bind(this, i), false);
            }
          }
        } else {
          document.getElementById("custom-legend").innerHTML = null;
          return
        }
      },
      onComplete: function () {
        const self = this;
        const chart = this.chart;
        const ctx = chart.ctx;
        const chartSize = Math.min(chart.width, chart.height);
        if (chartSize < 100) {
          return;
        }
        ctx.textAlign = "center";
        ctx.fillStyle = "#fff";
        ctx.font = "13 Roboto";
        ctx.save();
        var chartCircumference = chart.config.options.circumference;
        self.data.datasets.forEach((dataset, datasetIndex) => {
          const meta = self.getDatasetMeta(datasetIndex);
          meta.data.forEach((element, index) => {
            const value = dataset.data[index];
            const position = element.tooltipPosition();
            if (element._model.circumference > 0) {
              const segmentPercent = element._model.circumference / chartCircumference * 100;
              const label = (segmentPercent < 5 && segmentPercent > -5) ? '' : value.toFixed(2) + '%';
              ctx.fillText(label, position.x, position.y);
            }
          });
        });
        ctx.restore();
      }
    }
  })
}

export default class PieChart extends Component {
  constructor(props) {
    super(props);
  }

  _renderChart() {
    const { pieChartData, labels } = this.props;

    return (
      <Fragment>
        <div className="flex-container">
          {
            pieChartData.map((_, index) => {
              const periodData = buildChartDataSet(_.dataSet);
              const options = buildPieChartOption(_.name);
              return (
                <div className={"pieChart"}>
                  <Pie
                    data={periodData}
                    options={options}
                    height={310}
                    width={null}
                  />
                </div>
              )
            })
          }
        </div>
        <div id="custom-legend" className="pieLegend"></div>
      </Fragment>
    )
  }

  render() {
    return (
      <div className="chart-container">
        {this._renderChart()}
      </div>
    );
  }
}
