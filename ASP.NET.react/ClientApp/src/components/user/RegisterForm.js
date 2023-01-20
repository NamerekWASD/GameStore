import React, { useState, useEffect, Component } from 'react'
import './Form.css';

const RegisterForm = () => {

    const [userName, setUserName] = useState([''])
    const [password, setPassword] = useState([''])
    const [rememberMe, setRememberMe] = useState([false])

    const closeModal = () => {
        console.log('Register form was closed')
    }

    return (
        <main>
            <form className="modal-content animate">

                <div className="imgcontainer">
                    <img src="https://www.freeiconspng.com/thumbs/login-icon/register-secure-security-user-login-icon--7.png"
                        alt="Avatar" className="avatar" />
                </div>

                <div className="container">
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

                    <button type="submit">Register</button>
                    <label>
                        <input
                            type="checkbox"
                            className="remember"
                            checked={rememberMe}
                            value={rememberMe}
                            onChange={(e) => setRememberMe(e.currentTarget.checked)} /> Remember me
                    </label>
                </div>

                <div className="container">
                    <button type="button" onClick={() => closeModal()} className="cancelbtn">Cancel</button>
                    <span className="psw">Forgot <a href="#">password?</a></span>
                </div>
            </form>
        </main>
    )
}

export default RegisterForm;