import React, { Component } from 'react';
import { Container } from 'reactstrap';
import NavMenu from './NavMenu';

const Layout = (props) => {

  return (
    <div>
      <NavMenu isAuthenticated={props.isAuthenticated} logout={props.logout}/>
      <Container>
        {props.children}
      </Container>
    </div>
  );
}
export default Layout;
