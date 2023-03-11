import React, { useState, useEffect, useRef } from 'react'
import './Account.css'
import { useNavigate } from 'react-router-dom'
import GoogleAuth from './Auth/GoogleAuth';
import { AppPaths } from '../../../utils/AppPaths';

const RegistrationForm = ({ isRegister }) => {
    const navigate = useNavigate();
    const username = useRef(null);
    const email = useRef(null);
    const password = useRef(null);
    const passwordConfirmation = useRef(null);
    const [rememberMe, setRememberMe] = useState(false);
    const message = useRef(null)

    const onSubmit = (e) => {
        e.preventDefault()
        if (password.current.value != passwordConfirmation.current.value) {
            message.current.innerText = 'Passwords does not match!'
        }

        send()
        setToDefault()
    }
    async function send() {
        const registrationForm = {
            Username: username.current.value,
            Email: email.current.value,
            Password: password.current.value,
            RememberMe: rememberMe
        }
        await fetch(`api/account/register`, {
            method: 'PUT',
            headers: { "Accept": "application/json", "Content-Type": "application/json" },
            body: JSON.stringify(registrationForm)
        })
            .then(response => {
                if (!response.ok) {
                    message.current.style.color = 'red';
                } else {
                    navigate(AppPaths.login);
                    window.reload();
                }
                return response.json();
            })
            .then(response => {
                message.current.innerText = response;
            })
            .catch(err => {
                console.log(err);
            });
    }

    function setToDefault() {
    }

    return (
        <main>
            <form onSubmit={onSubmit}>
                <div className='card-conteiner'>
                    <div id="sign-form">
                        <div className="my-card-left">
                            <h1 style={{ width: '100%' }}>Sign up</h1>
                            <input
                                className='text-input'
                                type="text"
                                name="username"
                                placeholder="User name"
                                ref={username} />

                            <input
                                className='text-input'
                                type="email"
                                name="email"
                                placeholder="E-mail"
                                ref={email} />
                            <input
                                className='text-input'
                                type="password"
                                name="password"
                                placeholder="Password"
                                ref={password} />

                            <input
                                className='text-input'
                                type="password"
                                name="password2"
                                placeholder="Confirm password"
                                ref={passwordConfirmation} />

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

                            <div ref={message}>

                            </div>

                            <input type="submit"
                                name="signup_submit"
                                value="Sign me up" />

                        </div>
                        <div className='or'>OR</div>
                        <div className="my-card-right">
                            <button className="social-signin facebook">Log in with facebook</button>
                            <button className="social-signin twitter">Log in with Twitter</button>
                            <GoogleAuth sendToServer/>
                        </div>
                    </div>
                </div>
            </form>
        </main>
    )
}

export default RegistrationForm;