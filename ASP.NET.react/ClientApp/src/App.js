import React, { useState } from 'react';
import { Route, Routes } from 'react-router-dom';
import Layout from './components/Layout';
import './custom.css';

import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import RegistrationForm from "./components/user/RegistrationForm";
import LoginForm from "./components/user/LoginForm"
import Profile from "./components/user/Profile"


const App = () => {
  const [isAuthenticated, setAuthenticated] = useState();

  const CheckAuthenticated = () => {
    fetch('account')
      .then(response => response.json())
      .then(response => {
        setAuthenticated(response)
        console.log(`user auth status: ${isAuthenticated}`);
      })
      .catch(err => console.log(err));
  };
  const logout = () => {
    fetch('account/logout')
      .finally(
        CheckAuthenticated()
      )
  }

  const AppRoutes = [
    {
        index: true,
        element: <Home />
    },
    {
        path: '/counter',
        element: <Counter />
    },
    {
        path: '/fetch-data',
        element: <FetchData />
    },
    {
        path: '/account/login',
        element: <LoginForm CheckAuthenticated={CheckAuthenticated}/>
    },
    {
        path: '/account/register',
        element: <RegistrationForm />
    },
    {
        path: '/account/profile',
        element: <Profile />
    }

];

  return (
    <Layout isAuthenticated={isAuthenticated} logout={logout}>
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