import React, { useState, useEffect } from 'react'
const LoginForm = () => {
    const [userName, setUserName] = useState([''])
    const [password, setPassword] = useState([''])
    const [rememberMe, setRememberMe] = useState([false])

    const closeModal = () => {
        console.log("Hello")
        document.getElementById('id01').style.display = 'none'
    }
    const onSubmit = () => {
        console.log('it was submittet')
    }

    return (

        <main>
            <form onSubmit={ onSubmit }>
                <div className="container">
                    <div className="imgcontainer">
                        <img src="https://png.pngtree.com/png-vector/20191110/ourmid/pngtree-avatar-icon-profile-icon-member-login-vector-isolated-png-image_1978396.jpg" />
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

                    <button type="submit">Login</button>
                    <label>
                        <input
                            type="checkbox"
                            className="remember"
                            checked={ rememberMe }
                            value={ rememberMe }
                            onChange={(e) => setRememberMe(e.currentTarget.checked)} /> Remember me
                    </label>
                </div>

                <div className="container">
                    <button type="button" className="cancelbtn">Cancel</button>
                    <span className="psw">Forgot <a href="#">password?</a></span>
                </div>
            </form>
        </main>
        )

}


export default LoginForm;