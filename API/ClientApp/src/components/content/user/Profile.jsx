import React, { createRef, useEffect, useMemo, useRef, useState } from "react";
import './Profile.css';
import { Navigate, useNavigate } from "react-router-dom";
import { AppPaths } from "../../../utils/AppPaths";
import newUser from './newUser.png'
import Orders from "../order/Orders";
import { loadUserData } from "../../../utils/ApiRequests";

const Profile = ({isAuthenticated}) => {
    const [user, setUser] = useState();
    const navigate = useNavigate();
    const userName = createRef();
    const email = createRef();

    const reportMessage = useRef();
    const searchParams = {
        'ReturnUrl': AppPaths.profile
    }

    useEffect(() => {
        if (!user) {
            fetchData();
        }
    }, [user]);

    async function fetchData() {
        const response = await loadUserData(navigate, true);
        if (!processResponse(response)) {
            return
        }
        setUser(await response.json());

    }

    const onSubmit = (e) => {
        e.preventDefault()
        user.userName = userName.current.value;
        user.email = email.current.value;
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
        const message = await response.json();
        reportMessage.style.color = 'red';
        reportMessage.innerText = message;
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
    function renderOption() {
        switch (optionType) {
            case option.settings:
                return (
                    <div>
                        <form className="row profile-form" onSubmit={onSubmit}>
                            <div className="p-3 py-5">
                                <div className="row mt-3">
                                    <div className="col-md-12">
                                        <label className="labels" htmlFor="username">Ім'я користувача</label>
                                        <input type="text" id="username"
                                            name="username"
                                            className="form-control"
                                            placeholder="Username" required minLength={3} 
                                            defaultValue={user ? user.userName : ''}/>
                                    </div>
                                </div>
                                <div className="row mt-3">
                                    <div className="col-md-12">
                                        <label className="labels" htmlFor="email">Електронна пошта</label>
                                        <input type="email" id="email"
                                            name="email"
                                            className="form-control" required 
                                            defaultValue={user ? user.email : ''}/>
                                    </div>
                                </div>
                                <div className="mt-5 text-center">
                                    <input type="submit" className="btn btn-outline-success rounded-0 w-100" value='Зберегти зміни' />
                                </div>
                            </div>
                        </form>
                    </div>
                )
            case option.orders:
                return (
                        <Orders/>
                )
            default:
                return (
                    <>
                    </>
                )
        }
    }
    const memoOption = useMemo(() => renderOption(), [user, optionType])
    return isAuthenticated !== false ? (
        <div className="d-flex flex-row bg-white m-5" >
            <div className="col border-right">
                <div className="d-flex flex-column align-items-center text-center p-3 py-5">
                    {user ?
                        <>
                            <img className="border border-3 border-dark mt-5" width="130px" src={user.imageURL.length === 0 ? newUser : user.imageURL} alt="User" />
                            <span className="text-black-50">{user.userName}</span>
                        </>
                        : ''}
                    <span> </span>
                </div>
            </div>
            <div className="col border-right">
                <ul className="nav nav-tabs gap-2">
                    <li className="nav-item ms-2">
                        <button className="btn btn-outline-dark rounded-0 border-bottom-0 nav-tab-item" onClick={handleTabClick} value={option.settings}>Налаштування</button>
                    </li>
                    <li className="nav-item">
                        <button className="btn btn-outline-dark rounded-0 nav-tab-item" onClick={handleTabClick} value={option.orders}>Мої замовлення</button>
                    </li>
                </ul>
                {memoOption}
                <div ref={reportMessage}></div>
            </div>
        </div>
    ) : (
        <>
            <Navigate to={AppPaths.login} searchParams={searchParams.ReturnUrl} />
        </>
    )
}
export default Profile;