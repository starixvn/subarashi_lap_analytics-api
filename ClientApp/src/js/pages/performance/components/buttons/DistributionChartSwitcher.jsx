import React from 'react';
import {ChartMode} from '../../enums/enum';
import languageApi from '../../../../lib/languageApi';

const DistributionChartSwitcher = ({ value, onSelectChart }) => {
    return(
        <div className="flex-container button-group">
            <button className={"radius-left" + (value == ChartMode.PIE_CHART ? " selected" : "")} onClick={() => onSelectChart(ChartMode.PIE_CHART)}>{languageApi.getDefaultRes('pie_chart')}</button>
            <button className={value == ChartMode.COLUMN_CHART ? "selected" : ""} onClick={() => onSelectChart(ChartMode.COLUMN_CHART)}>{languageApi.getDefaultRes('column_chart')}</button>
            <button className={"radius-right" + (value == ChartMode.REPORT_TEXT ? " selected" : "")} onClick={() => onSelectChart(ChartMode.REPORT_TEXT)}>{languageApi.getDefaultRes('report_in_text')}</button>
        </div>
    )
}

export default DistributionChartSwitcher;