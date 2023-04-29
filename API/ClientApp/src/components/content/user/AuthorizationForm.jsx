import React, { useEffect, useRef, useState } from 'react'
import { Navigate, useNavigate, useSearchParams } from 'react-router-dom';
import './Account.css';
import GoogleAuth from './Auth/GoogleAuth';
import { toast } from 'react-toastify';
import ModalEmailConfirmation from './Auth/ModalEmailConfirmation';
import { SendLoginData } from '../../../utils/ApiRequests';
import PreLoader from '../../../utils/PreLoader';

const AuthorizationForm = ({ refreshAuth, isAuthenticated }) => {
    const navigate = useNavigate();
    const [searchParams] = useSearchParams()
    const RefEmail = useRef(null);
    const RefModal = useRef(null);
    const [rememberMe, setRememberMe] = useState(false);
    const [showPreLoader, setShowPreLoader] = useState(false);

    useEffect(() => {
        refreshAuth();
    })

    const processResponse = async (response) => {
        if (!response.ok) {
            toast.error(await response.text());
            return false;
        }
    }

    const sendConfirmation = async (e) => {
        e.preventDefault();

        setShowPreLoader(true);

        const LoginModel = {
            email: RefEmail.current.value,
        };

        const response = await SendLoginData(LoginModel);

        if (processResponse(response)) {
            RefModal.current.style.display = "block";
        }
        setShowPreLoader(false);
    }

    async function sendAsExternalLogin(provider, user, userId) {
        const externalAuth = {
            userId: userId,
            loginProvider: provider,
            RememberMe: rememberMe,
            user: user
        }

        const requestInfo = `api/account/external-login-callback`
        const requestInit = {
            method: 'POST',
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json"
            },
            body: JSON.stringify(externalAuth)
        }

        const response = await fetch(requestInfo, requestInit)

        if (processResponse(response)) {
            refreshAuth();
            toast.success("Вітаємо")
        }
    }
    const after = () => {
        var ReturnUrl = searchParams.get('ReturnUrl')
        refreshAuth();
        navigate(ReturnUrl ? ReturnUrl : '/')
        return true;
    }

    return (
        <main>

        {showPreLoader && <PreLoader /> }
            {
                isAuthenticated === false ?
                    <>
                            <div id="sign-form" className='bg-white'>
                                <form onSubmit={sendConfirmation}>
                                    <div className="my-card-left">
                                        <h1>Авторизація</h1>
                                        <input
                                            className='text-input'
                                            type="text"
                                            name="email"
                                            placeholder="Електронна пошта"
                                            required
                                            ref={RefEmail}
                                            onChange={(e) => e.target.value} />
                                        <div className='checkbox-rect2'>
                                            <input
                                                id='checkbox-rect1'
                                                type="checkbox"
                                                className="checkbox-input"
                                                checked={rememberMe}
                                                onChange={() => setRememberMe(!rememberMe)} />
                                            <label className='remember' htmlFor='checkbox-rect1'>
                                                Запам'ятати мене
                                            </label>
                                        </div>
                                        <input type="submit"
                                            name="signin_submit"
                                            value="Увійти" />
                                    </div>
                                </form>
                                <div className='or'>Або</div>
                                <div className="my-card-right">
                                    <GoogleAuth sendToServer={sendAsExternalLogin} />
                                </div>
                            </div>
                        <ModalEmailConfirmation refModal={RefModal} rememberMe={rememberMe} refEmail={RefEmail} after={after} />
                    </>
                    :
                    <>
                        {
                            navigate('/')
                        }
                    </>
            }
        </main>
    )
}

export default AuthorizationForm;