import React, { useRef, useState } from 'react'
import { useNavigate, useSearchParams } from 'react-router-dom';
import { GoogleOAuthProvider } from '@react-oauth/google';
import { GoogleLogin } from '@react-oauth/google';
import './Account.css'

const LoginForm = ({ setNeedRefresh }) => {
    const navigate = useNavigate();
    const [searchParams] = useSearchParams()

    var usernameOrEmail = useRef();
    var password = useRef();
    const [rememberMe, setRememberMe] = useState(false);

    const onSubmit = (e) => {
        e.preventDefault()
        send()
    }

    async function send() {
        const LoginModel = {
            UserNameOrEmail: usernameOrEmail.current.value,
            Password: password.current.value,
            RememberMe: rememberMe
        }

        const response = await fetch(`account/external-auth`, {
            method: 'POST',
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json"
            },
            body: JSON.stringify(LoginModel)
        })

        ProcessResponse(response)
    };
    async function send(provider, credentialResponse){
        const externalAuth = {
            loginProvider: provider,
            providerKey: credentialResponse.credential
        }
        const response = await fetch(`account/external-auth`, {
            method: 'POST',
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json",
            },
            body: JSON.stringify(externalAuth),
            mode: 'no-cors'
        })
        ProcessResponse(response)
    }

    async function ProcessResponse(response){
        if (!response.ok) {
            const responseMessageElement = document.getElementById('responseMessage');
            const responseMessage = await response.json()
            responseMessageElement.style.color = 'red';
            responseMessageElement.innerText = responseMessage;
            console.log(responseMessage);
            return
        }
        setNeedRefresh(true)
        var ReturnUrl = searchParams.get('ReturnUrl')
        navigate(ReturnUrl ? ReturnUrl : '/')

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
                        <button className="social-signin facebook">Log in with facebook</button>
                        <button className="social-signin twitter">Log in with Twitter</button>
                        <GoogleOAuthProvider clientId="389273208401-48vuqa2aj1avoo777vkkpb84u81c4co1.apps.googleusercontent.com" >
                            <GoogleLogin
                                onSuccess={credentialResponse => {
                                    console.log(credentialResponse);
                                    send('google', credentialResponse);
                                }}
                                onError={() => {
                                    console.log('Login Failed');
                                }}
                                type="icon"
                            />
                        </GoogleOAuthProvider>
                    </div>
                </div>
            </div>
        </main >
    )
}


export default LoginForm;