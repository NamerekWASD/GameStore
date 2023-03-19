import React, { createRef, useEffect, useMemo, useRef, useState } from "react";
import './Profile.css';
import { Navigate, useNavigate } from "react-router-dom";
import { AppPaths } from "../../../utils/AppPaths";
import newUser from './newUser.png'
import Orders from "../order/Orders";
import { loadUserData } from "../../../utils/ApiRequests";
import { toast } from "react-toastify";

const Profile = ({ isAuthenticated, refreshAuth }) => {
    const navigate = useNavigate();
    
    const [user, setUser] = useState();
    const userName = createRef();
    const email = createRef();

    useEffect(() => {
        if(!isAuthenticated){
            navigate(AppPaths.authorization)
        }
        if (!user) {
            fetchData();
        }
    }, [user]);

    async function fetchData() {
        const response = await loadUserData(refreshAuth);
        if(!response) return;
        const responseBody = await response.json();
        setUser(responseBody);
    }

    const onSubmit = (e) => {
        e.preventDefault()
        setUser(prev => ({
            ...prev,
            userName: userName.current.value,
            email: email.current.value
        }))
        send()
    }
    async function send() {
        console.log(user);
        const response = await fetch(`api/account/update`, {
            method: 'POST',
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json",
            },
            body: JSON.stringify(user)
        })
        processResponse(response)
    }

    async function processResponse(response) {
        if (response.ok) {
            return true;
        }
        toast.error(await response.json());
        return false;
    }

    const option = {
        settings: "settings",
        orders: "orders",
    }
    const [optionType, setOptionType] = useState(option.settings);

    const handleTabClick = (e) => {
        const borderBottom = "border-bottom-0"
        const otherTabs = document.getElementsByClassName('nav-tab-item');
        Array.from(otherTabs).forEach(element => {
            element.classList.remove(borderBottom);
        });
        e.target.classList.add(borderBottom);
        setOptionType(e.target.value);

    }

    const memoOption = useMemo(() => renderOption(), [user, optionType]);
    function renderOption() {
        switch (optionType) {
            case option.settings:
                return (
                    <div>
                        <form className="row profile-form" onSubmit={onSubmit}>
                            <div className="p-3">
                                <div className="form-group">
                                    <label className="labels" htmlFor="fname">Ім'я</label>
                                    <input ref={userName}
                                        type="text" id="fname" name="fname"
                                        className="form-control rounded-0"
                                        defaultValue={user ? user.lastName : ''} onChange={(e) => e.target.value} />
                                </div>
                                <div className="form-group">
                                    <label className="labels" htmlFor="lname">Призвіще</label>
                                    <input ref={userName}
                                        type="text" id="lname" name="lname"
                                        className="form-control rounded-0"
                                        defaultValue={user ? user.firstName : ''} onChange={(e) => e.target.value} />
                                </div>
                                <div className="form-group required">
                                    <label className="labels" htmlFor="username">Ім'я користувача</label>
                                    <input ref={userName}
                                        type="text" id="username" name="username"
                                        className="form-control rounded-0" required minLength={3}
                                        defaultValue={user ? user.userName : ''} onChange={(e) => e.target.value} />
                                </div>
                                <div className="form-group required">
                                    <label className="labels" htmlFor="email">Електронна пошта</label>
                                    <input ref={email}
                                        type="email" id="email" name="email"
                                        className="form-control rounded-0" required disabled={user && user.provider}
                                        defaultValue={user ? user.email : ''} onChange={(e) => e.target.value} />
                                </div>
                                <div className="text-center">
                                    <input type="submit" className="btn btn-outline-success rounded-0 btn-75px" value='Зберегти зміни' />
                                </div>
                            </div>
                        </form>
                    </div>
                )
            case option.orders:
                return (
                    <Orders />
                )
            default:
                return (
                    <>
                    </>
                )
        }
    }

    return (
        <div id="profile" className="d-flex flex-row bg-white mx-auto my-5 flex-wrap justify-content-center" style={{ maxWidth: "1200px" }}>
            <div className="border-right flex-fill">
                <div className="d-flex flex-column h-100 align-items-center justify-content-center">
                    {
                        user ?
                            <>
                                <img width="130px" src={user.imageURL && user.imageURL.length === 0 ? newUser : user.imageURL} alt="User" />
                                <span className="text-black-50 mt-2">{user.userName}</span>
                            </>
                            : ''
                    }
                    <span> </span>
                </div>
            </div>
            <div className="border-right" style={{ minWidth: '40vw' }}>
                <ul className="nav nav-tabs gap-2 flex-nowrap">
                    <li className="nav-item ms-2">
                        <button className="btn btn-outline-dark rounded-0 border-bottom-0 nav-tab-item" onClick={handleTabClick} value={option.settings}>Налаштування</button>
                    </li>
                    <li className="nav-item">
                        <button className="btn btn-outline-dark rounded-0 nav-tab-item" onClick={handleTabClick} value={option.orders}>Мої замовлення</button>
                    </li>
                </ul>
                {memoOption}
            </div>
        </div>
    )
}
export default Profile;