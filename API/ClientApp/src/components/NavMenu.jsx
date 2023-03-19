import React, { useEffect, useRef, useState } from 'react';
import { Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink, DropdownMenu, DropdownItem, Dropdown, DropdownToggle } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';
import { loadGenres, logout } from '../utils/ApiRequests';
import cart from './../static/shopping-cart.svg';
import $ from 'jquery';
import { AppPaths } from '../utils/AppPaths';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSearch } from '@fortawesome/free-solid-svg-icons';
import GameDropList from './content/game/GameDropList';

export function setItemsCount(count) {
    const element = $("#shopping-cart-counter");
    const value = count - +element.text();
    const color = value <= 0 ? 'red' : 'green';
    const responsiveElement = $(`<span></span>`).text(value).css({ "position": "absolute", 'color': color, 'font-weight': 800 });
    element.text(count);
    element.append(responsiveElement);
    responsiveElement.animate({ 'right': '-25px', 'opacity': 0 }, 800, function () {
        responsiveElement.remove();
    })
}


const NavMenu = ({ isAuthenticated, refreshAuth }) => {
    const [collapsed, setCollapsed] = useState(true);
    const [dropdownOpen, setDropdownOpen] = useState(false);
    const [genres, setGenres] = useState([]);
    const [searchQuery, setSearchQuery] = useState("");
    const counter = useRef(null);
    const searchField = useRef(null);

    const toggle = () => setDropdownOpen((prevState) => !prevState);

    useEffect(() => {
        if (localStorage.games && +counter.current.textContent == 0) {
            var items = JSON.parse(localStorage.games);
            setItemsCount(items.length);
        }
    }, []);

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
    const handleSubmit = (e) => {
        e.preventDefault()
        setSearchQuery(searchField.current.value);
    }
    const refresh = () => {
        setSearchQuery('')
        searchField.current.value = '';
    }

    return (
        <header id='myHeader' className='header'>
            <Navbar className="navbar-expand-sm box-shadow p-0">
                <NavbarBrand className='my-0 p-0' tag={Link} to="/">Game Store</NavbarBrand>
                <NavbarToggler onClick={() => toggleNavbar()} className="mr-2 rounded-0" />
                <Collapse className="d-sm-inline-flex" isOpen={!collapsed} navbar style={collapseBeforeStyle}>
                    <ul className="navbar-nav flex-grow justify-content-end flex-fill align-content-center">
                        <NavItem className='ms-1'>
                            <Dropdown isOpen={dropdownOpen} toggle={toggle} direction='down' className='h-100'>
                                <DropdownToggle className='rounded-0 h-100'>Каталог</DropdownToggle>
                                <DropdownMenu className='rounded-0 p-0'>
                                    <DropdownItem>
                                        <NavItem>
                                            <NavLink tag={Link} className="text-dark nav-link" to={AppPaths.gameCatalog}>Усі ігри</NavLink>
                                        </NavItem>
                                    </DropdownItem>
                                    <DropdownItem header>Жанри</DropdownItem>
                                    {
                                        genres.length !== 0 ? genres.map(genre => {
                                            return (
                                                <DropdownItem key={genre.id}>
                                                    <NavItem>
                                                        <NavLink tag={Link} className="text-dark" to={AppPaths.gameCatalog + '?' + new URLSearchParams([['genre', genre.name], ['id', genre.id]]).toString()}>{genre.name}</NavLink>
                                                    </NavItem>
                                                </DropdownItem>
                                            )
                                        }) : ''
                                    }
                                </DropdownMenu>
                            </Dropdown>
                        </NavItem>
                        <NavItem className='flex-fill ms-3'>
                            <form className='h-100'  onSubmit={handleSubmit}>
                                <FontAwesomeIcon icon={faSearch} />
                                <input ref={searchField} className='h-100 border-0 no-outline bg-transparent w-50 fw-bold' type="text" placeholder='Пошук...' onChange={(e) => e.target.value} />
                                <GameDropList searchQuery={searchQuery} refresh={refresh} />
                            </form>
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
                                        <NavLink tag={Link} className="text-dark" to={AppPaths.profile}>Профіль</NavLink>
                                    </NavItem>
                                    <NavItem>
                                        <span className="text-dark nav-link pointer" onClick={() => logout(refreshAuth)}>Вийти з аккаунту</span>
                                    </NavItem>
                                </>
                                :
                                <>
                                    <NavItem>
                                        <NavLink tag={Link} className="text-dark" to={AppPaths.authorization}>Авторизуватись</NavLink>
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