import React, { useEffect, useRef, useState } from 'react';
import { Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink, DropdownMenu, DropdownItem, Dropdown, DropdownToggle } from 'reactstrap';
import { Link, useNavigate } from 'react-router-dom';
import './NavMenu.css';
import { GetGenres, logout } from '../utils/ApiRequests';
import cart from './../static/shopping-cart1.svg';
import $ from 'jquery';
import { AppPaths } from '../utils/AppPaths';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSearch } from '@fortawesome/free-solid-svg-icons';
import GameDropList from './content/game/GameDropList';
import { useScrollPosition } from '@n8tb1t/use-scroll-position';

const timeout = { current: undefined }

export function setItemsCount(count) {
    const element = $("#shopping-cart-counter");
    const color = count - +element.text() <= 0 ? 'red' : 'green';
    element.text(count);
    const parent = element.closest(".nav-link");
    parent.addClass(`highlight-${color}`);
    if (typeof timeout.current !== "undefined") {
        clearTimeout(timeout.current);
    }
    timeout.current = setTimeout(() => {
        parent.removeClass(`highlight-${color}`);
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

    const toggleDropDown = () => {
        setDropdownOpen((prevState) => !prevState)
    };

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
            setTimeout(() => {
                formAnimation.classList.remove('focused');
                searchField.current.value = '';
                setSearchQuery('')
            }, 100)
        });

        GetGenres().then(result => setGenres(result));
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
            if (!isVisible && dropdownOpen) toggleDropDown();
            setHeaderStyle(shouldBeStyle);
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

    const searchGames = (e) => {
        if(e.target.value.length < 3) {
            setSearchQuery('');
            return;
        }
        setSearchQuery(e.target.value);
    }

    return (
        <header id='myHeader' className='header' style={{ ...headerStyle }}>
            <Navbar className="navbar-expand-sm box-shadow p-0">
                <NavbarBrand className='my-0 p-0' tag={Link} to="/">Game Store</NavbarBrand>
                <NavbarToggler onClick={() => toggleNavbar()} className="mr-2 rounded-0" />
                <Collapse className="d-sm-inline-flex" isOpen={!collapsed} navbar style={collapseBeforeStyle}>
                    <ul className={"navbar-nav flex-grow justify-content-end flex-fill align-content-center " + (!collapsed && "gap-2")}>
                        <NavItem>
                            <Dropdown isOpen={dropdownOpen} toggle={toggleDropDown} direction='down' className='h-100 z-index-top'>
                                <DropdownToggle className='rounded-0 h-100 fs-5'>Каталог</DropdownToggle>
                                <DropdownMenu className='rounded-0 p-0'>
                                    <DropdownItem>
                                        <NavItem>
                                            <NavLink tag={Link} className="nav-link" to={AppPaths.gameCatalog}>Усі ігри</NavLink>
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
                        <NavItem className='formAnimation-container'>
                            <form className='h-100 px-3 formAnimation' onSubmit={navigateToSearch}>
                                <FontAwesomeIcon icon={faSearch} className="pe-3" color='gray' />
                                <input id='myInput' ref={searchField} className='h-100 border-0 no-outline fw-bold bg-transparent'
                                    type="text" placeholder='Пошук...' autoComplete='off'
                                    onChange={searchGames} />
                            </form>
                            <GameDropList searchQuery={searchQuery} refresh={refresh} isVisible={headerStyle && headerStyle.visibility === 'visible'} />
                        </NavItem>

                        <div className='flex-fill'>

                        </div>
                        <NavItem className='position-relative'>
                            <NavLink tag={Link} to="cart" className='counter-container p-1'>
                                <img src={cart} alt="" width={42} height={42} />
                                <div ref={counter} id='shopping-cart-counter' className='counter'>0</div>
                            </NavLink>
                        </NavItem>
                        {
                            isAuthenticated ?
                                <>
                                    <NavItem>
                                        <NavLink tag={Link} to={AppPaths.profile} className='d-flex'><span className='my-auto'>Профіль</span></NavLink>
                                    </NavItem>
                                    <NavItem>
                                        <div className="nav-link pointer d-flex" onClick={() => logout(refreshAuth)}>
                                            <span className='my-auto'>Вийти з аккаунту</span>
                                        </div>
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