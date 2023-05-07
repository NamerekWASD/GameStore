import React, { useEffect, useMemo, useState } from "react";
import { useNavigate } from "react-router-dom";
import { AppPaths } from "../../../utils/AppPaths";
import newUser from './newUser.png'
import Orders from "../order/Orders";
import { GetUserData } from "../../../utils/ApiRequests";
import { toast } from "react-toastify";

const Profile = ({ isAuthenticated, refreshAuth }) => {
    const navigate = useNavigate();

    const [user, setUser] = useState();

    useEffect(() => {
        if (isAuthenticated === false) {
            navigate(AppPaths.authorization)
        }
    }, [navigate, isAuthenticated]);

    useEffect(() => {
        if (!user) {
            GetUserData(refreshAuth).then(response => {
                if (!response) return;
                return response.json();
            })
                .then(result => {
                    setUser(result);
                });
        }
    }, [refreshAuth]);

    const onSubmit = (e) => {
        e.preventDefault();
        const requestInfo = `api/account/update`;
        const requestInit = {
            method: 'POST',
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json",
            },
            body: JSON.stringify(user)
        };

        fetch(requestInfo, requestInit).then(response => {
            processResponse(response)
        })
    }

    async function processResponse(response) {
        if (response.ok) {
            toast.success(await response.json())
            return true;
        }
        toast.error(await response.text());
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

    const renderOption = () => {
        switch (optionType) {
            case option.settings:
                return (
                    <div>
                        <form className="row profile-form" onSubmit={onSubmit}>
                            <div className="p-3">
                                <div className="form-group">
                                    <label className="labels" htmlFor="fname">Ім'я</label>
                                    <input
                                        type="text" id="fname" name="fname"
                                        className="form-control rounded-0"
                                        defaultValue={user ? user.firstName : ''} onChange={(e) => setUser(prevData => ({ ...prevData, firstName: e.target.value }))} />
                                </div>
                                <div className="form-group">
                                    <label className="labels" htmlFor="lname">Призвіще</label>
                                    <input
                                        type="text" id="lname" name="lname"
                                        className="form-control rounded-0"
                                        defaultValue={user ? user.lastName : ''} onChange={(e) => setUser(prevData => ({ ...prevData, lastName: e.target.value }))} />
                                </div>
                                <div className="form-group required">
                                    <label className="labels" htmlFor="username">Ім'я користувача</label>
                                    <input
                                        type="text" id="username" name="username"
                                        className="form-control rounded-0" required minLength={3}
                                        defaultValue={user ? user.userName : ''} onChange={(e) => setUser(prevData => ({ ...prevData, userName: e.target.value }))} />
                                </div>
                                <div className="form-group required">
                                    <label className="labels" htmlFor="email">Електронна пошта</label>
                                    <input
                                        type="email" id="email" name="email"
                                        className="form-control rounded-0" required disabled={user && user.provider}
                                        defaultValue={user ? user.email : ''} onChange={(e) => setUser(prevData => ({ ...prevData, email: e.target.value }))} />
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
    const memoOption = useMemo(renderOption, [user, optionType, option]);

    return (
        <div id="profile" className="d-flex flex-row bg-white mx-auto my-5 flex-wrap justify-content-center" style={{ maxWidth: "1200px" }}>
            <div className="border-right flex-fill">
                <div className="d-flex flex-column h-100 align-items-center justify-content-center">
                    {
                        user ?
                            <>
                                <img width="130px" src={!user.imageURL || user.imageURL.length !== 0 ? newUser : user.imageURL} alt="User" />
                                <span className="text-black-50 mt-2">{user.userName}</span>
                            </>
                            : ''
                    }
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
                <div style={{minHeight: '50vh'}}>
                    {memoOption}
                </div>
            </div>
        </div>
    )
}
export default Profile;