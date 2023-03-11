import React, { useEffect, useRef, useState } from 'react';
import { Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink, DropdownMenu, DropdownItem, Dropdown, DropdownToggle } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';
import { loadGenres, logout } from '../utils/ApiRequests';
import cart from './../static/shopping-cart.svg';

const NavMenu = ({isAuthenticated}) => {
    const [collapsed, setCollapsed] = useState(true);
    const [dropdownOpen, setDropdownOpen] = useState(false);
    const [genres, setGenres] = useState([]);
    const counter = useRef(null);


    const toggle = () => setDropdownOpen((prevState) => !prevState);

    useEffect(() => {
        if (localStorage.games && +counter.current.textContent == 0) {
            var items = JSON.parse(localStorage.games);
            counter.current.textContent = items.length;
        }
    }, [counter]);

    useEffect(() => {
        if (genres.length === 0) {
            loadGenres().then(result => setGenres(result));
        }
    }, [genres]);
    function toggleNavbar() {
        setCollapsed(!collapsed);
    };

    const collapseBeforeStyle = {
        flexDirection: "row !important",
    }
    return (
        <header id='myHeader' className='header'>
            <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white box-shadow p-0" light>
                <NavbarBrand className='my-auto' tag={Link} to="/">Game Store</NavbarBrand>
                <NavbarToggler onClick={() => toggleNavbar()} className="mr-2 rounded-0" />
                <Collapse className="d-sm-inline-flex" isOpen={!collapsed} navbar style={collapseBeforeStyle}>
                    <ul className="navbar-nav flex-grow justify-content-end flex-fill align-content-center">
                        <NavItem className='flex-fill'>
                            <Dropdown className='h-100' isOpen={dropdownOpen} toggle={toggle} direction='down'>
                                <DropdownToggle className='rounded-0 h-100'>Каталог</DropdownToggle>
                                <DropdownMenu className='rounded-0 p-0'>
                                    <DropdownItem>
                                        <NavItem>
                                            <NavLink tag={Link} className="text-dark nav-link" to="game/search">Усі ігри</NavLink>
                                        </NavItem>
                                    </DropdownItem>
                                    <DropdownItem header>Жанри</DropdownItem>
                                    {
                                        genres.length !== 0 ? genres.map(genre => {
                                            return (
                                                <DropdownItem key={genre.id}>
                                                    <NavItem>
                                                        <NavLink tag={Link} className="text-dark" to={"/game/search?" + new URLSearchParams([['genre', genre.name], ['id', genre.id]]).toString()}>{genre.name}</NavLink>
                                                    </NavItem>
                                                </DropdownItem>
                                            )
                                        }) : ''
                                    }
                                </DropdownMenu>
                            </Dropdown>
                        </NavItem>
                        <NavItem>
                            <NavLink tag={Link} className="text-dark" to="cart"><img src={cart} alt="" />
                                <div ref={counter} id='shopping-cart-counter' className='counter'>0</div>
                            </NavLink>
                        </NavItem>
                        {
                            isAuthenticated ?
                                <>
                                    <NavItem>
                                        <NavLink tag={Link} className="text-dark" to="/account/profile">Профіль</NavLink>
                                    </NavItem>
                                    <NavItem>
                                        <a href={window.location} className="text-dark nav-link" onClick={() => logout()}>Вийти з аккаунту</a>
                                    </NavItem>
                                </>
                                :
                                <>
                                    <NavItem>
                                        <NavLink tag={Link} className="text-dark" to="/account/login">Увійти</NavLink>
                                    </NavItem>
                                    <NavItem>
                                        <NavLink tag={Link} className="text-dark" to="/account/register">Зареєструватись</NavLink>
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