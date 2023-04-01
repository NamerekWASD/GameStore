import React, { useEffect, useRef, useState } from 'react';
import { Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink, DropdownMenu, DropdownItem, Dropdown, DropdownToggle } from 'reactstrap';
import { Link, useNavigate } from 'react-router-dom';
import './NavMenu.css';
import { loadGenres, logout } from '../utils/ApiRequests';
import cart from './../static/shopping-cart.svg';
import $ from 'jquery';
import { AppPaths } from '../utils/AppPaths';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faClose, faSearch } from '@fortawesome/free-solid-svg-icons';
import GameDropList from './content/game/GameDropList';
import { useScrollPosition } from '@n8tb1t/use-scroll-position';

const timeout = { current: undefined }

export function setItemsCount(count) {
    const element = $("#shopping-cart-counter");
    const color = count - +element.text() <= 0 ? 'red' : 'green';
    element.text(count);
    element.addClass(`highlight-${color}`);
    if (typeof timeout.current !== "undefined") {
        clearTimeout(timeout.current);
    }
    timeout.current = setTimeout(() => {
        element.removeClass(`highlight-${color}`);
    }, 500);
}


const NavMenu = ({ isAuthenticated, refreshAuth }) => {
    const navigate = useNavigate();
    const [headerStyle, setHeaderStyle] = useState({
        visibility: 'visible',
        transition: 'all 200ms ease-in'
    })
    const [collapsed, setCollapsed] = useState(true);
    const [dropdownOpen, setDropdownOpen] = useState(false);
    const [genres, setGenres] = useState([]);
    const [searchQuery, setSearchQuery] = useState("");
    const counter = useRef(null);
    const searchField = useRef(null);

    const toggle = () => setDropdownOpen((prevState) => !prevState);

    useEffect(() => {
        if (localStorage.games && +counter.current.textContent === 0) {
            var items = JSON.parse(localStorage.games);
            setItemsCount(items.length);
        }

        const myInput = document.getElementById('myInput');
        const formAnimation = myInput.parentElement;

        myInput.addEventListener('focus', () => {
            formAnimation.classList.add('focused');
        });

        myInput.addEventListener('blur', () => {
            formAnimation.classList.remove('focused');
            searchField.current.value = '';
            setSearchQuery('')
        });
    }, []);

    useEffect(() => {
        loadGenres().then(result => setGenres(result));
    }, []);
    function toggleNavbar() {
        setCollapsed(!collapsed);
    };

    const collapseBeforeStyle = {
        flexDirection: "row !important",
    }
    const refresh = () => {
        setSearchQuery('')
        searchField.current.value = '';
    }

    useScrollPosition(
        ({ prevPos, currPos }) => {
            const isVisible = currPos.y > prevPos.y
            const shouldBeStyle = {
                visibility: isVisible ? 'visible' : 'hidden',
                transition: `all 200ms ${isVisible ? 'ease-in' : 'ease-out'}`,
                transform: isVisible ? 'none' : 'translate(0, -100%)'
            }

            if (JSON.stringify(shouldBeStyle) === JSON.stringify(headerStyle)) return

            setHeaderStyle(shouldBeStyle)
        },
        [headerStyle],
        false,
        false,
        100
    )

    
    const navigateToSearch = (e) => {
        e.preventDefault();
        navigate(AppPaths.gameSearch + '?' + new URLSearchParams([["search", searchQuery]]))
        refresh();
    }
    return (
        <header id='myHeader' className='header' style={{ ...headerStyle }}>
            <Navbar className="navbar-expand-sm box-shadow p-0">
                <NavbarBrand className='my-0 p-0' tag={Link} to="/">Game Store</NavbarBrand>
                <NavbarToggler onClick={() => toggleNavbar()} className="mr-2 rounded-0" />
                <Collapse className="d-sm-inline-flex" isOpen={!collapsed} navbar style={collapseBeforeStyle}>
                    <ul className="navbar-nav flex-grow justify-content-end flex-fill align-content-center">
                        <NavItem className='ms-1'>
                            <Dropdown isOpen={dropdownOpen} toggle={toggle} direction='down' className='h-100 z-index-top'>
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
                                                        <NavLink tag={Link}
                                                            to={AppPaths.gameCatalog + '?' +
                                                                new URLSearchParams([['genre', genre.name], ['id', genre.id]]).toString()}>
                                                            {genre.name}
                                                        </NavLink>
                                                    </NavItem>
                                                </DropdownItem>
                                            )
                                        }) : ''
                                    }
                                </DropdownMenu>
                            </Dropdown>
                        </NavItem>
                        <NavItem className='me-auto'>
                            <form className='h-100 px-3 formAnimation' onSubmit={navigateToSearch}>
                                <FontAwesomeIcon icon={faSearch} className="pe-3" />
                                <input id='myInput' ref={searchField} className='h-100 border-0 no-outline fw-bold bg-transparent'
                                    type="text" placeholder='Пошук...'
                                    onChange={(e) => setSearchQuery(e.target.value)}/>
                                <FontAwesomeIcon className='pointer ms-3' size={'xl'} icon={faClose} onClick={refresh} />
                                <GameDropList searchQuery={searchQuery} refresh={refresh} isVisible={headerStyle && headerStyle.visibility === 'visible'} />

                            </form>
                        </NavItem>
                        <NavItem>
                            <NavLink tag={Link} to="cart"><img src={cart} alt="" />
                                <div ref={counter} id='shopping-cart-counter' className='counter'>0</div>
                            </NavLink>
                        </NavItem>
                        {
                            isAuthenticated ?
                                <>
                                    <NavItem>
                                        <NavLink tag={Link} to={AppPaths.profile}>Профіль</NavLink>
                                    </NavItem>
                                    <NavItem>
                                        <span className="text-dark nav-link pointer" onClick={() => logout(refreshAuth)}>Вийти з аккаунту</span>
                                    </NavItem>
                                </>
                                :
                                <>
                                    <NavItem>
                                        <NavLink tag={Link} to={AppPaths.authorization}>Авторизуватись</NavLink>
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