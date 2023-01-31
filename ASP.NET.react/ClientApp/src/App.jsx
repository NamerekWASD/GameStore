import React, { useEffect, useState } from 'react';
import { Route, Routes } from 'react-router-dom';

import './custom.css';

import Layout from './components/Layout';
import AboutCompany from "./components/content/AboutCompany";
import Home from "./components/content/Home";
import RegistrationForm from "./components/content/user/RegistrationForm";
import LoginForm from "./components/content/user/LoginForm"
import Profile from "./components/content/user/Profile"
import { AppPaths } from './utils/AppPaths';
import { CheckAuthenticated } from './utils/ApiRequests';


const App = () => {

    const [needRefresh, setNeedRefresh] = useState(true)
    const [isAuthenticated, setAuthenticated] = useState(false)
    const refreshAuthenticated = () => {
        CheckAuthenticated().then(result => {
            console.log(result);
            setAuthenticated(result)
            setNeedRefresh(false)
        })
    }
    useEffect(() => {
        if (needRefresh) {
            refreshAuthenticated()
        }
    }, [needRefresh])

    function setNeedRefreshFunction(flag) {
        setNeedRefresh(flag)
    }
    const AppRoutes = [
        {
            index: true,
            element: <Home />
        },
        {
            path: AppPaths.aboutCompany,
            element: <AboutCompany />
        },
        {
            path: AppPaths.login,
            element: <LoginForm setNeedRefresh={setNeedRefreshFunction} />
        },
        {
            path: AppPaths.register,
            element: <RegistrationForm />
        },
        {
            path: AppPaths.profile,
            element: <Profile isAuthenticated={isAuthenticated} />
        }

    ];

    return (
        <Layout isAuthenticated={isAuthenticated}>
            <Routes>
                {AppRoutes.map((route, index) => {
                    const { element, ...rest } = route;
                    return <Route key={index} {...rest} element={element} />;
                })}
            </Routes>
        </Layout>
    );
}
export default App;