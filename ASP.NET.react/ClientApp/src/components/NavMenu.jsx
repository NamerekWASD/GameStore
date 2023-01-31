import React, { useState } from 'react';
import { Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';
import { logout } from '../utils/ApiRequests';

const NavMenu = ({isAuthenticated}) => {
    
    const [collapsed, setCollapsed] = useState(true);

    function toggleNavbar() {
        setCollapsed(!collapsed);
    };

    const collapseBeforeStyle = {
        flexDirection: "row !important",
    }
    return (
        <header id='myHeader' className='header'>
            <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white box-shadow" light>
                <NavbarBrand tag={Link} to="/">ASP.NET.react</NavbarBrand>
                <NavbarToggler onClick={() => toggleNavbar()} className="mr-2" />
                <Collapse className="d-sm-inline-flex" isOpen={!collapsed} navbar style={collapseBeforeStyle}>
                    <ul className="navbar-nav flex-grow" style={{ flexGrow: 1 }}>
                        <NavItem>
                            <NavLink tag={Link} className="text-dark" to="/">Home</NavLink>
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
                                        <NavLink tag={Link} className="text-dark" to="/user-account/profile">Profile</NavLink>
                                    </NavItem>
                                    <NavItem>
                                        <a href={window.location} className="text-dark nav-link" onClick={() => logout()}>Logout</a>
                                    </NavItem>
                                </>
                                :
                                <>
                                    <NavItem>
                                        <NavLink tag={Link} className="text-dark" to="/user-account/login">Login</NavLink>
                                    </NavItem>
                                    <NavItem>
                                        <NavLink tag={Link} className="text-dark" to="/user-account/register">Register</NavLink>
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
