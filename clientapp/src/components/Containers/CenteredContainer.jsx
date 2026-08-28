import React from 'react';
import './Container.css';

const CenteredContainer = ({ children }) => { // parent element usually needs position: relative
	return (
		<div className="centered-container">
			{children}
		</div>
	);
};

export default CenteredContainer;