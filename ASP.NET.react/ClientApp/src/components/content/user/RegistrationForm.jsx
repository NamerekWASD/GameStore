import React, { useState, useEffect, useRef } from 'react'
import './Account.css'
import { useNavigate } from 'react-router-dom'

const RegistrationForm = ({ isRegister }) => {
    const navigate = useNavigate();
    const username = useRef(null);
    const name = useRef(null);
    const lastname = useRef(null);
    const email = useRef(null);
    const phoneNumber = useRef(null)
    const password = useRef(null);
    const passwordConfirmation = useRef(null);
    const [rememberMe, setRememberMe] = useState(false);

    const onSubmit = (e) => {
        e.preventDefault()
        const responseMessageElement = document.getElementById('responseMessageElement')
        if (password.current.value != passwordConfirmation.current.value) {
            responseMessageElement.current.innerText = 'Passwords does not match!'
        }

        send(responseMessageElement)

        setToDefault()
    }
    async function send(responseMessageElement) {
        const registrationForm = {
            Username: username.current.value,
            Name: name.current.value,
            Lastname: lastname.current.value,
            Email: email.current.value,
            PhoneNumber: phoneNumber.current.value,
            Password: password.current.value,
            RememberMe: rememberMe
        }
        await fetch(`account/register`, {
            method: 'PUT',
            headers: { "Accept": "application/json", "Content-Type": "application/json" },
            body: JSON.stringify(registrationForm)
        })
            .then(response => {
                if (!response.ok) {
                    responseMessageElement.style.color = 'red';
                } else {
                    navigate('account/login')
                    window.reload();
                }
                return response.json();
            })
            .then(response => {
                responseMessageElement.innerText = response;
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
                                type="text"
                                name="name"
                                placeholder="Name"
                                ref={name} />

                            <input
                                className='text-input'
                                type="text"
                                name="username"
                                placeholder="Last name"
                                ref={lastname} />

                            <input
                                className='text-input'
                                type="email"
                                name="email"
                                placeholder="E-mail"
                                ref={email} />

                            <input
                                className='text-input'
                                type="tel"
                                name="email"
                                placeholder="Phone number"
                                ref={phoneNumber} />

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

                            <div id='responseMessageElement'>

                            </div>

                            <input type="submit"
                                name="signup_submit"
                                value="Sign me up" />

                        </div>
                        <div className='or'>OR</div>
                        <div className="my-card-right">
                            <button className="social-signin facebook">Log in with facebook</button>
                            <button className="social-signin twitter">Log in with Twitter</button>
                            <button className="social-signin google">Log in with Google+</button>
                        </div>
                    </div>
                </div>
            </form>
        </main>
    )


}


export default RegistrationForm;