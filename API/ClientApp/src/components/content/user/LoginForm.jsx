import React, { useEffect, useRef, useState } from 'react'
import { useNavigate, useSearchParams } from 'react-router-dom';
import './Account.css';
import GoogleAuth from './Auth/GoogleAuth';

const LoginForm = ({ refreshAuth }) => {
    const navigate = useNavigate();
    const [searchParams] = useSearchParams()
    var usernameOrEmail = useRef();
    const [rememberMe, setRememberMe] = useState(false);

    useEffect(() => {
        refreshAuth();
    })

    const onSubmit = (e) => {
        e.preventDefault()
        sendPrimalDataToServer()
    }

    async function sendPrimalDataToServer() {
        const LoginModel = {
            UserNameOrEmail: usernameOrEmail.current.value,
            RememberMe: rememberMe
        };

        const requestInfo = `api/account/login`;
        const requestInit = {
            method: 'POST',
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json"
            },
            body: JSON.stringify(LoginModel)
        };

        const response = await fetch(requestInfo, requestInit);

        processResponse(response)
    };
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

        processResponse(response)
    }

    async function processResponse(response) {
        if (response.ok) {
            var ReturnUrl = searchParams.get('ReturnUrl')
            localStorage.isAuthenticated = true;
            refreshAuth();
            navigate(ReturnUrl ? ReturnUrl : '/')
            return
        }
        const responseMessageElement = document.getElementById('responseMessage');
        const responseMessage = await response.json()
        responseMessageElement.style.color = 'red';
        responseMessageElement.innerText = responseMessage;
    }

    return (
        <main>
            <div className='card-conteiner'>
                <div id="sign-form">
                    <form onSubmit={onSubmit}>
                        <div className="my-card-left">
                            <h1>Sign in</h1>
                            <input
                                className='text-input'
                                type="text"
                                name="email"
                                placeholder="Username or E-mail"
                                required
                                ref={usernameOrEmail} />
                            <input
                                className='text-input'
                                type="password"
                                name="password"
                                placeholder="Password"
                                required
                                ref={password} />
                            <div className='checkbox-rect2'>
                                <input
                                    id='checkbox-rect1'
                                    type="checkbox"
                                    className="checkbox-input"
                                    checked={rememberMe}
                                    onChange={() => setRememberMe(!rememberMe)} />
                                <label className='remember' htmlFor='checkbox-rect1'>
                                    Remember me
                                </label>
                            </div>
                            <div id='responseMessage'>

                            </div>
                            <input type="submit"
                                name="signin_submit"
                                value="Sign me in" />
                        </div>
                    </form>
                    <div className='or'>Or</div>
                    <div className="my-card-right">
                        <GoogleAuth sendToServer={sendAsExternalLogin} />
                    </div>
                </div>
            </div>
        </main >
    )
}

export default LoginForm;