import React, { PureComponent } from 'react';
import BarChart from './base/BarChart';

export default class OverallChart extends PureComponent {
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

  render() {
    const { style } = this.state;
    const { title, overallData, dataMapper } = this.props;
    const { labels, barChartData } = dataMapper(overallData);

    return (
      <div className="info-zone-container" ref={this.refSelf} style={style}>
        <div className="info-zone-header">
          {title}
        </div>
        <div className="info-zone-body">
          <BarChart
            barChartData={barChartData}
            labels={labels}
            hideLegend={true}
          />
        </div>
      </div>
    );
  }
}
