import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import RegistrationForm from "./components/user/RegistrationForm";
import LoginForm from "./components/user/LoginForm"
import Profile from "./components/user/Profile"

const AppRoutes = [
    {
        id: 'Home',
        index: true,
        element: <Home />
    },
    {
        id: 'Counter',
        path: '/counter',
        element: <Counter />
    },
    {
        id: 'FetchData',
        path: '/fetch-data',
        element: <FetchData />
    },
    {
        id: 'LoginForm',
        path: '/account/login',
        element: <LoginForm />
    },
    {
        id: 'RegistrationForm',
        path: '/account/register',
        element: <RegistrationForm />
    },
    {
        id: 'Profile',
        path: '/account/profile',
        element: <Profile />
    }

];


export default AppRoutes;
