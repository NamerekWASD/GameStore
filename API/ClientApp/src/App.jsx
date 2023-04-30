import React, { useEffect, useState } from 'react';
import { Route, Routes } from 'react-router-dom';

import './custom.css';

import Layout, { scrollToTop } from './components/Layout';
import Home from "./components/content/Home";
import Profile from "./components/content/user/Profile"
import { AppPaths } from './utils/AppPaths';
import { CheckAuthenticated } from './utils/ApiRequests';
import GameDetails from './components/content/game/GameDetails';
import ShoppingCart from './components/content/game/ShoppingCart';
import GameSearch from './components/content/game/GameSearch';
import Payment from './components/content/payment/Payment';
import OrderDetails from './components/content/order/OrderDetails';
import AuthorizationForm from './components/content/user/AuthorizationForm';
import GameCatalog from './components/content/game/GameCatalog';
import Manager from './components/content/manager/Manager';
import EditGame from './components/content/manager/EditGame';
import CreateGame from './components/content/manager/CreateGame';
import { useLocation } from "react-router-dom";

const App = () => {
    const [isAuthenticated, setAuthenticated] = useState();
    const location = useLocation();
    useEffect(() => {
        refreshAuth();
    }, []);

    const refreshAuth = () => {
        CheckAuthenticated().then(result => {
            setAuthenticated(result);
        })
    }

    useEffect(() => {
        scrollToTop();
    }, [location]);
    const AppRoutes = [
        {
            index: true,
            path: AppPaths.home,
            element: <Home />
        },
        {
            path: AppPaths.authorization,
            element: <AuthorizationForm isAuthenticated={isAuthenticated} refreshAuth={refreshAuth} />
        },
        {
            path: AppPaths.profile,
            element: <Profile isAuthenticated={isAuthenticated} refreshAuth={refreshAuth}/>
        },
        {
            path: AppPaths.gameDetails,
            element: <GameDetails isAuthenticated ={isAuthenticated} refreshAuth={refreshAuth}/>
        },
        {
            path: AppPaths.shoppingCart,
            element: <ShoppingCart isAuthenticated={isAuthenticated} refreshAuth={refreshAuth}/>
        },
        {
            path: AppPaths.gameSearch,
            element: <GameSearch/>
        },
        {
            path: AppPaths.gameCatalog,
            element: <GameCatalog/>
        },
        {
            path: AppPaths.payment,
            element: <Payment refreshAuth={refreshAuth}/>
        },
        {
            path: AppPaths.orderDetails,
            element: <OrderDetails/>
        },
        {
            path: AppPaths.manager,
            element: <Manager isAuthenticated={isAuthenticated} refreshAuth={refreshAuth}/>
        },
        {
            path: AppPaths.editGame,
            element: <EditGame/>
        },
        {
            path: AppPaths.createGame,
            element: <CreateGame/>
        }

    ];

    return (
        <Layout isAuthenticated={isAuthenticated} refreshAuth={refreshAuth}>
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