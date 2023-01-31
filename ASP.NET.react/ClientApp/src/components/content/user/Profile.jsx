import React, { useEffect, useRef } from "react";
import './Profile.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/css/bootstrap.css';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPlus } from "@fortawesome/free-solid-svg-icons";
import { isEqual } from "lodash/isEqual"
import { Navigate } from "react-router-dom";
import { AppPaths } from "../../../utils/AppPaths";

const Profile = ({isAuthenticated}) => {
    var shouldLoad = true;

    const username = useRef(null);
    const name = useRef(null);
    const lastname = useRef(null);
    const phoneNumber = useRef(null);
    const email = useRef(null);
    const nameTop = useRef(null);
    const emailTop = useRef(null);


    const searchParams = {
        'ReturnUrl': AppPaths.profile
    }
    var savedUser;

    useEffect(() => {
        if (shouldLoad) {
            fetchData();
        }
    }, [savedUser]);

    async function fetchData() {
        const response = await fetch('account/data', {
            method: "GET",
            headers: {
                "Accept": "application/json",
            }
        })
        if (response.redirected) {
            let url = new URL(response.url)
            url.searchParams.set('ReturnUrl', window.location.pathname)
            window.location.href = url;
            return
        }
        if (!responseOk(response)) {
            return
        }
        const responseMessage = await response.json();
        savedUser = responseMessage;

        username.current.value = savedUser.userName;
        name.current.value = savedUser.name;
        lastname.current.value = savedUser.lastname;
        phoneNumber.current.value = savedUser.phoneNumber;
        email.current.value = savedUser.email;

        emailTop.current.innerText = savedUser.email;
        nameTop.current.innerText = savedUser.name;

        shouldLoad = false
    }

    const onSubmit = (e) => {
        e.preventDefault()
        let user = {
            userName: username.current.value,
            name: name.current.value,
            lastname: lastname.current.value,
            phoneNumber: phoneNumber.current.value,
            email: email.current.value
        }
        if (!isEqual(savedUser, user)) {
            send(user)
        }
    }
    async function send(user) {
        console.log(user);
        const response = await fetch(`account/update`, {
            method: 'POST',
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json",
            },
            body: JSON.stringify(user)
        })
        if (!responseOk(response)) {
            return
        }
        fetchData()
    }

    async function responseOk(response) {
        if (response)
            if (response.ok) {
                return true
            }
        const responseMessage = await response.json();
        const responseMessageElement = document.getElementById('responseMessage')
        responseMessageElement.style.color = 'red'
        responseMessageElement.innerText = responseMessage
        return false
    }
    return isAuthenticated ? (
        <main>
            <div className="container rounded bg-white mt-5 mb-5" >
                <form className="row profile-form" onSubmit={onSubmit}>
                    <div className="col-md-3 border-right">
                        <div className="d-flex flex-column align-items-center text-center p-3 py-5"><img className="rounded-circle mt-5" width="150px" src="https://st3.depositphotos.com/15648834/17930/v/600/depositphotos_179308454-stock-illustration-unknown-person-silhouette-glasses-profile.jpg" />
                            <span className="font-weight-bold" ref={nameTop}></span>
                            <span className="text-black-50" ref={emailTop}></span>
                            <span> </span>
                        </div>
                    </div>
                    <div className="col-md-5 border-right">
                        <div className="p-3 py-5">
                            <div className="d-flex justify-content-between align-items-center mb-3">
                                <h4 className="text-right">Profile Settings</h4>
                            </div>
                            <div id="responseMessage">

                            </div>
                            <div className="row mt-3">
                                <div className="col-md-12">
                                    <label className="labels" htmlFor="username">Username</label>
                                    <input type="text" id="username"
                                        name="username"
                                        className="form-control"
                                        placeholder="Username" required
                                        ref={username} />
                                </div>
                            </div>
                            <div className="row mt-2">
                                <div className="col-md-6">
                                    <label className="labels" htmlFor="name">Name</label>
                                    <input type="text" id="name"
                                        name="name"
                                        className="form-control"
                                        placeholder="First name" required
                                        ref={name} />
                                </div>
                                <div className="col-md-6">
                                    <label className="labels" htmlFor="lastname">Last name</label>
                                    <input type="text" id="lastname"
                                        name="lastname"
                                        className="form-control"
                                        placeholder="Last name"
                                        ref={lastname} />
                                </div>
                            </div>
                            <div className="row mt-3">
                                <div className="col-md-12">
                                    <label className="labels" htmlFor="phonenumber">Phone Number</label>
                                    <input type="tel" id="phonenumber"
                                        name="phonenumber"
                                        className="form-control"
                                        placeholder="+1 23 345 67 89" required
                                        ref={phoneNumber} />
                                </div>
                                <div className="col-md-12">
                                    <label className="labels" htmlFor="email">E-mail</label>
                                    <input type="email" id="email"
                                        name="email"
                                        className="form-control"
                                        placeholder="example@ex.com" required
                                        ref={email} />
                                </div>
                            </div>
                            <div className="mt-5 text-center">
                                <input type="submit" className="btn btn-primary profile-button" value='Save Profile' />
                            </div>
                        </div>
                    </div>
                    <div className="col-md-4">
                        <div className="p-3 py-5">
                            <div className="d-flex justify-content-between align-items-center experience">
                                <span>Edit Experience</span>
                                <span className="border p-2 add-experience d-flex">
                                    <FontAwesomeIcon icon={faPlus} style={{ margin: 'auto', minHeight: '20px', }} />
                                    <span className="d-none d-lg-block m-auto p-1">Experience</span>
                                </span>
                            </div>
                            <br />
                            <div className="col-md-12">
                                <label className="labels">Experience in Designing</label>
                                <input type="text" className="form-control" placeholder="experience" />
                            </div>
                            <div className="col-md-12">
                                <label className="labels">Additional Details</label>
                                <input type="text" className="form-control" placeholder="additional details" />
                            </div>
                        </div>
                    </div>
                </form>
            </div>
        </main>
    ) : (
        <>
            {
                shouldLoad = false
            }
            {
                console.log(searchParams)
            }
            <Navigate to={AppPaths.login} searchParams={searchParams.ReturnUrl}/>
        </>
    )
}
export default Profile;