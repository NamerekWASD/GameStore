import React from 'react';
import './Container.css';

const CenteredContainer = ({ children }) => { // батькіський елемент має бути position relative в більшості випадках
	return (
		<div className="centered-container">
			{children}
		</div>
	);
};

export default CenteredContainer;