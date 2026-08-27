import React, { useEffect, useMemo, useRef, useState } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import { AppPaths } from "../../../utils/AppPaths";
import Orders from "../order/Orders";
import { GetUserData } from "../../../utils/ApiRequests";
import { toast } from "react-toastify";
import { useTranslation } from 'react-i18next';
import LoadingCircle from "../../../utils/LoadingCircle";

const Profile = ({ isAuthenticated, refreshAuth }) => {
    const navigate = useNavigate();
    const location = useLocation();
    const { t } = useTranslation();

    const [user, setUser] = useState();
    const [loading, setLoading] = useState(true);
    const [saving, setSaving] = useState(false);

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
                    setLoading(false);
                })
                .catch(() => {
                    setLoading(false);
                    toast.error(t('profile.loadError'));
                });
        }
    }, [refreshAuth]);

    const onSubmit = (e) => {
        e.preventDefault();
        setSaving(true);
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
        }).finally(() => setSaving(false))
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
    const optionType = location.pathname === AppPaths.accountOrders ? option.orders : option.settings;
    const tabOrder = [option.settings, option.orders];
    const settingsTabRef = useRef(null);
    const ordersTabRef = useRef(null);
    const tabRefs = { [option.settings]: settingsTabRef, [option.orders]: ordersTabRef };

    const handleTabClick = (value) => {
        navigate(value === option.orders ? AppPaths.accountOrders : AppPaths.profile);
    }

    const handleTabsKeyDown = (e) => {
        if (e.key !== 'ArrowLeft' && e.key !== 'ArrowRight') return;
        const currentIndex = tabOrder.indexOf(optionType);
        const nextIndex = e.key === 'ArrowRight'
            ? (currentIndex + 1) % tabOrder.length
            : (currentIndex - 1 + tabOrder.length) % tabOrder.length;
        const nextTab = tabOrder[nextIndex];
        handleTabClick(nextTab);
        tabRefs[nextTab].current?.focus();
    }

    const renderOption = () => {
        switch (optionType) {
            case option.settings:
                return (
                    <div>
                        <form className="row profile-form" onSubmit={onSubmit}>
                            <div className="profile-form-names">
                                <div className="form-group">
                                    <label className="labels" htmlFor="fname">{t('profile.firstName')}</label>
                                    <input
                                        type="text" id="fname" name="fname"
                                        className="form-control rounded-0"
                                        value={user?.firstName ?? ''} onChange={(e) => setUser(prevData => ({ ...prevData, firstName: e.target.value }))} />
                                </div>
                                <div className="form-group">
                                    <label className="labels" htmlFor="lname">{t('profile.lastName')}</label>
                                    <input
                                        type="text" id="lname" name="lname"
                                        className="form-control rounded-0"
                                        value={user?.lastName ?? ''} onChange={(e) => setUser(prevData => ({ ...prevData, lastName: e.target.value }))} />
                                </div>
                            </div>
                            <div className="form-group required">
                                <label className="labels" htmlFor="username">{t('profile.username')}</label>
                                <input
                                    type="text" id="username" name="username"
                                    className="form-control rounded-0" required minLength={3}
                                    value={user?.userName ?? ''} onChange={(e) => setUser(prevData => ({ ...prevData, userName: e.target.value }))} />
                            </div>
                            <div className="form-group required">
                                <label className="labels" htmlFor="email">{t('profile.email')}</label>
                                <input
                                    type="email" id="email" name="email"
                                    className="form-control rounded-0" required disabled={user && user.provider}
                                    value={user?.email ?? ''} onChange={(e) => setUser(prevData => ({ ...prevData, email: e.target.value }))} />
                            </div>
                            <div className="profile-form-actions">
                                <input type="submit" className="btn btn-outline-success rounded-0 btn-75px" disabled={saving} value={saving ? t('profile.saving') : t('profile.saveChanges')} />
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
    const memoOption = useMemo(renderOption, [user, optionType, option, saving]);

    if (loading) {
        return (
            <div id="profile" className="profile-card profile-card-loading">
                <LoadingCircle />
            </div>
        )
    }

    return (
        <div id="profile" className="profile-card">
            <div className="border-end profile-avatar-col">
                <div className="d-flex flex-column h-100 align-items-center justify-content-center">
                    {
                        user ?
                            <>
                                {
                                    user.imageURL && user.imageURL.length !== 0 ?
                                        <img className="profile-avatar" src={user.imageURL} alt="User" />
                                        :
                                        <div className="profile-avatar profile-avatar-placeholder">
                                            {(user.userName || '?').charAt(0).toUpperCase()}
                                        </div>
                                }
                                <span className="mt-2" style={{ color: 'var(--text-secondary)' }}>{user.userName}</span>
                            </>
                            : ''
                    }
                </div>
            </div>
            <div className="border-end profile-tabs-col">
                <div role="tablist" className="profile-tabs" onKeyDown={handleTabsKeyDown}>
                    <button
                        ref={settingsTabRef}
                        role="tab"
                        id={`tab-${option.settings}`}
                        aria-selected={optionType === option.settings}
                        aria-controls={`panel-${option.settings}`}
                        tabIndex={optionType === option.settings ? 0 : -1}
                        className={`btn rounded-0 nav-tab-item ${optionType === option.settings ? 'active' : ''}`}
                        onClick={() => handleTabClick(option.settings)}>{t('profile.tabs.settings')}</button>
                    <button
                        ref={ordersTabRef}
                        role="tab"
                        id={`tab-${option.orders}`}
                        aria-selected={optionType === option.orders}
                        aria-controls={`panel-${option.orders}`}
                        tabIndex={optionType === option.orders ? 0 : -1}
                        className={`btn rounded-0 nav-tab-item ${optionType === option.orders ? 'active' : ''}`}
                        onClick={() => handleTabClick(option.orders)}>{t('profile.tabs.orders')}</button>
                </div>
                <div role="tabpanel" id={`panel-${optionType}`} aria-labelledby={`tab-${optionType}`} className="profile-panel">
                    {memoOption}
                </div>
            </div>
        </div>
    )
}
export default Profile;
