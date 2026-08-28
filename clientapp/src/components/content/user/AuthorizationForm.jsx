import React, { useEffect, useRef, useState } from 'react'
import { useNavigate, useSearchParams } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import './Account.css';
import GoogleAuth from './Auth/GoogleAuth';
import { toast } from 'react-toastify';
import ModalEmailConfirmation from './Auth/ModalEmailConfirmation';
import { SendLoginData } from '../../../utils/ApiRequests';
import PreLoader from '../../../utils/PreLoader';

const AuthorizationForm = ({ refreshAuth, isAuthenticated }) => {
    const { t } = useTranslation();
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
            toast.success(t('auth.welcome'))
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
                                        <h1>{t('auth.title')}</h1>
                                        <input
                                            className='text-input'
                                            type="text"
                                            name="email"
                                            placeholder={t('payment.email')}
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
                                                {t('auth.rememberMe')}
                                            </label>
                                        </div>
                                        <input type="submit"
                                            name="signin_submit"
                                            value={t('auth.signIn')} />
                                    </div>
                                </form>
                                <div className='or'>{t('auth.or')}</div>
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