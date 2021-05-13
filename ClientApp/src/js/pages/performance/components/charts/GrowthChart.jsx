import React, { PureComponent } from 'react';
import MixBarLineChart from './base/MixBarLineChart';
import languageApi from '../../../../lib/languageApi';

export default class GrowthChart extends PureComponent {
  constructor(props) {
    super(props);
    this.state = {
      style: {}
    };
    this.refSelf = React.createRef();
  }

  componentDidMount() {
    const { getStyle } = this.props;
    getStyle && this.setState({
      style: getStyle(this.refSelf)
    });
  }

  _renderHeaderChart = () => {
    const { showBackIcon, backToDefaultChart } = this.props;
    return <div className="chart-title">
      {showBackIcon ? <div className="back-icon" onClick={backToDefaultChart}>
        <i className="arrow-left"></i>
        {languageApi.getDefaultRes('go_back')}
      </div> : null}
    </div>;
  }

  render() {
    const { title, lineChartData, barChartData, labels, leftAxisName, autoScale, calculateBarWidth } = this.props;
    const { style } = this.state;

    return (
      <div className="info-zone-container" ref={this.refSelf} style={style}>
        <div className="info-zone-header">
          {title}
          </div>
        <div className="info-zone-body">
        <MixBarLineChart
            header={this._renderHeaderChart}
            lineChartData={lineChartData}
            barChartData={barChartData}
            leftAxisName={leftAxisName}
            rightAxisName={`${languageApi.getDefaultRes('growth_rate')} (%)`}
            labels={labels}
            autoScale={autoScale}
            calculateBarWidth={calculateBarWidth}
          />
        </div>
      </div>
    );
  }
}
