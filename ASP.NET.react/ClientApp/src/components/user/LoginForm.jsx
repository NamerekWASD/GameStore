import React, { useRef, useState } from 'react'
import { useNavigate } from 'react-router-dom';
import './Account.css'

const LoginForm = (props) => {
    const navigate = useNavigate();
    var usernameOrEmail = useRef();
    var password = useRef();
    const [rememberMe, setRememberMe] = useState(false);

    const onSubmit = (e) => {
        e.preventDefault()
    
        send()
      }

    async function send() {
        const responseMessageElement = document.getElementById('responseMessage');
        const LoginModel = {
            UserNameOrEmail: usernameOrEmail.current.value,
            Password: password.current.value,
            RememberMe: rememberMe
        }
        console.log(LoginModel);
        await fetch(`account/login`, {
            method: 'POST',
            headers: { "Accept": "application/json", "Content-Type": "application/json" },
            body: JSON.stringify(LoginModel)
        })
        .then(response => {
            if(!response.ok){
                responseMessageElement.style.color = 'red';
            }else{
                responseMessageElement.style.color = 'black';
                props.CheckAuthenticated()
                navigate('/')
            }

            return response.json();
        })
        .then(response => {
            responseMessageElement.innerText = response;
        })
        .catch(err => {
        console.log(err);
        })

        await fetch('account')
        .then(response => {
            return response.json();
        })
        .then(response => {
            console.log(response)
        })
    };
    return (
        <main>
            <form onSubmit={ onSubmit }>
                <div className="container">
                    <div className="imgcontainer">
                        <img src='https://www.pngkey.com/png/full/73-730394_admin-approved-user-registration-user-registration-icon-png.png' alt=""/>
                    </div>
                    <label htmlFor="uname"><b>Username or E-mail</b></label>
                    <input type="text"
                        placeholder="Enter Username or E-mail"
                        className="uname"
                        required
                        ref={usernameOrEmail} />

                    <label htmlFor="psw"><b>Password</b></label>
                    <input type="password"
                        placeholder="Enter Password"
                        className="psw"
                        required
                        ref={password} />

                    <button type="submit">Login</button>
                    <label>
                        <input
                            type="checkbox"
                            className="remember"
                            checked={rememberMe}
                            onChange={() => setRememberMe(!rememberMe)}/> 
                            Remember me
                    </label>
                </div>
                <div id='responseMessage'></div>
                <div className="container">
                    <button type="button" className="cancelbtn">Cancel</button>
                    <span className="psw">Forgot <a href="#">password?</a></span>
                </div>
            </form>
        </main>
        )


}


export default LoginForm;