import React, { useEffect, useState } from 'react';
import { Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';

const NavMenu = ({isAuthenticated, logout}) => {

    var collapsed = true;

    function toggleNavbar() {
        collapsed = !collapsed;
    };

    const collapseBeforeStyle = {
        flexDirection: "row !important",
    }

    return (
        <header>
            <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" container light>
                <NavbarBrand tag={Link} to="/">ASP.NET.react</NavbarBrand>
                <NavbarToggler onClick={toggleNavbar()} className="mr-2" />
                <Collapse className="d-sm-inline-flex" isOpen={!collapsed} navbar style={collapseBeforeStyle}>
                    <ul className="navbar-nav flex-grow" style={{ flexGrow: 1 }}>
                        <NavItem>
                            <NavLink tag={Link} className="text-dark" to="/">Home</NavLink>
                        </NavItem>
                        <NavItem>
                            <NavLink tag={Link} className="text-dark" to="/counter">Counter</NavLink>
                        </NavItem>
                        <NavItem>
                            <NavLink tag={Link} className="text-dark" to="/fetch-data">Fetch data</NavLink>
                        </NavItem>
                    </ul>
                    <ul className="navbar-nav flex-grow">
                        {
                            isAuthenticated ?
                                <>
                                    <NavItem>
                                        <NavLink tag={Link} className="text-dark" to="/account/profile">Profile</NavLink>
                                    </NavItem>
                                    <NavItem>
                                        <a href='#' className="text-dark nav-link" onClick={() => logout()}>Logout</a>
                                    </NavItem>
                                </>
                                :
                                <>
                                    <NavItem>
                                        <NavLink tag={Link} className="text-dark" to="/account/login">Login</NavLink>
                                    </NavItem>
                                    <NavItem>
                                        <NavLink tag={Link} className="text-dark" to="/account/register">Register</NavLink>
                                    </NavItem>
                                </>
                        }
                    </ul>
                </Collapse>
            </Navbar>
        </header>
    );
}
export default NavMenu;
