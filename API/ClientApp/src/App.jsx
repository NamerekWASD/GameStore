import React, { useEffect, useState } from 'react';
import { Route, Routes } from 'react-router-dom';

import './custom.css';

import Layout from './components/Layout';
import Home from "./components/content/Home";
import RegistrationForm from "./components/content/user/RegistrationForm";
import LoginForm from "./components/content/user/LoginForm"
import Profile from "./components/content/user/Profile"
import { AppPaths } from './utils/AppPaths';
import { CheckAuthenticated } from './utils/ApiRequests';
import GameDetails from './components/content/game/GameDetails';
import ShoppingCart from './components/content/game/ShoppingCart';
import GameSearch from './components/content/game/GameSearch';
import Payment from './components/content/payment/Payment';
import PaymentSuccess from './components/content/payment/PaymentSuccess';
import PaymentError from './components/content/payment/PaymentError';
import Orders from './components/content/order/Orders';
import OrderDetails from './components/content/order/OrderDetails';

const App = () => {
    const [isAuthenticated, setAuthenticated] = useState();
    useEffect(() => {
        refreshAuth();
    });

    const refreshAuth = () => {
        CheckAuthenticated().then(result => {
            setAuthenticated(result);
        })
    }
    const AppRoutes = [
        {
            index: true,
            element: <Home />
        },
        {
            path: AppPaths.login,
            element: <LoginForm refreshAuth={refreshAuth} />
        },
        {
            path: AppPaths.register,
            element: <RegistrationForm />
        },
        {
            path: AppPaths.profile,
            element: <Profile isAuthenticated={isAuthenticated}/>
        },
        {
            path: AppPaths.gameDetails,
            element: <GameDetails/>
        },
        {
            path: AppPaths.shoppingCart,
            element: <ShoppingCart isAuthenticated={isAuthenticated}/>
        },
        {
            path: AppPaths.gameSearch,
            element: <GameSearch/>
        },
        {
            path: AppPaths.payment,
            element: <Payment refreshAuth={refreshAuth}/>
        },
        {
            path: AppPaths.paymentSuccess,
            element: <PaymentSuccess/>
        },
        {
            path: AppPaths.paymentError,
            element: <PaymentError/>
        },
        {
            path: AppPaths.order,
            element: <OrderDetails/>
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