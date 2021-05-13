import React from 'react';
import localStorageHelper from '../lib/localStorageHelper'

export default class AutoReload extends React.Component {
    constructor(props) {
        super(props);

        const countdowns = localStorageHelper.getWidgetReloadCountdown();
        let countdown = countdowns[this.props.dataKey];

        if (!countdown) {
            countdown = this.props.defaultCountdown;
        }

        this.countdownValues = this.props.countdownValues;
        this.state = {
            countdown: countdown,
            currentCountdown: countdown,
            isShowSelection: false,
            isMounted: false
        };

    }

    componentDidMount() {
        this.setState({ isMounted: true }, () => {
            if (this.state.countdown > 0) {
                this._startTimer();
            }
        });

    }

    componentWillUnmount() {
        this.setState({ isMounted: false });
        this._stopTimer();
    }

    componentDidUpdate(prevProps, prevState) {
        if (!this.props.isLoading && prevProps.isLoading) {
            if (!prevState.currentCountdown) {
                this.setState({ currentCountdown: prevState.countdown });
            }
            if (!this.timerId) {
                this._startTimer();
            }
        }
    }

    _onTick() {
        if (this.state.isMounted && !this.props.isLoading) {
            let countdown = this.state.currentCountdown;

            if (countdown === 0) {
                this._stopTimer();
                this.props.reloadCallback();
            } else {
                this.setState({ currentCountdown: countdown - 1 });
            }
        }
    }

    _startTimer() {
        this.timerId = setInterval(() => this._onTick(), 1000);
    }

    _stopTimer() {
        if (this.timerId) {
            clearInterval(this.timerId);
            this.timerId = 0;
        }
    }

    _onCounterClick() {
        if (this.state.isMounted && !this.props.isLoading) {
            this.setState({ isShowSelection: !this.state.isShowSelection });
        }
    }

    _onSelectCounterValue(value) {
        if (this.state.isMounted) {
            this.setState({
                isShowSelection: false,
                countdown: value,
                currentCountdown: value
            });

            const countdowns = localStorageHelper.getWidgetReloadCountdown();
            countdowns[this.props.dataKey] = value;
            localStorageHelper.saveWidgetReloadCountdown(countdowns);
        }
    }

    render() {
        var className = "btn-refresh right border-" + (this.props.colorMode || "white");
        const selection = this.state.isShowSelection
            ? (
                <ul className="dropdown-timer">
                    {(this.countdownValues || []).map(value => (
                        <li key={value} onClick={() => this._onSelectCounterValue(value)}>{value}</li>
                    ))}
                </ul>
            )
            : null;

        return (
            <div className={className} onClick={() => this._onCounterClick()} disabled={!!this.props.isLoading}>
                <i className="icon-refresh"></i>
                <span className="currentCountdown">{this.state.currentCountdown + 's'}</span>
                <i className="icon-menu-down"></i>
                {selection}
            </div>
        );
    }
}
