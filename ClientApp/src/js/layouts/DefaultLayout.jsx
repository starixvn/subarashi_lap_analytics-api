import React from 'react';

export default class DefaultLayout extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <div className="dashboard__container">
                {this.props.children}
            </div>
        );
    }
}
