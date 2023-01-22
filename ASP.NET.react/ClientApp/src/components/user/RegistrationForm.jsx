import React, { useState, useEffect } from 'react'
import './Account.css'
import  { Navigate  } from 'react-router-dom'

const RegistrationForm = ({isRegister}) => {
    const [userName, setUserName] = useState([''])
    const [password, setPassword] = useState([''])
    const [rememberMe, setRememberMe] = useState([false])

    const onSubmit = (e) => {
        e.preventDefault()
    
        send()
    
        setUserName('')
        setPassword('')
        setRememberMe(false)
      }
      async function send() {
        const responseMessageElement = document.getElementById('responseMessage');
        const LoginForm = {
            FirstData: userName,
            Password: password
        }
        await fetch(`account/register`, {
            method: 'PUT',
            headers: { "Accept": "application/json", "Content-Type": "application/json" },
            body: JSON.stringify(LoginForm)
        })
        .then(response => {
            if(!response.ok){
                responseMessageElement.style.color = 'red';
            }else{
                Navigate('')
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
    return (
        <main>
            <form onSubmit={ onSubmit }>
                <div className="container">
                    <div className="imgcontainer">
                        <img src='https://cdn-icons-png.flaticon.com/512/5087/5087579.png' alt=""/>
                    </div>
                    <label htmlFor="uname"><b>Username</b></label>
                    <input type="text"
                        placeholder="Enter Username"
                        className="uname"
                        required
                        value={userName}
                        onChange={(e) => setUserName(e.target.value)} />

                    <label htmlFor="psw"><b>Password</b></label>
                    <input type="password"
                        placeholder="Enter Password"
                        className="psw"
                        required
                        value={password}
                        onChange={(e) => setPassword(e.target.value)} />

                    <button type="submit">{isRegister ? 'Register' : 'Login'}</button>
                    <label>
                        <input
                            type="checkbox"
                            className="remember"
                            checked={ rememberMe }
                            value={ rememberMe }
                            onChange={(e) => setRememberMe(e.currentTarget.checked)} /> Remember me
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


export default RegistrationForm;