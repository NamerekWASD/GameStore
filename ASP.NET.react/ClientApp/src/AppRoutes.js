import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import LoginForm from "./components/user/LoginForm";
import RegisterForm from "./components/user/RegisterForm";
import Users from "./components/Users";

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
        path: '/users',
        element: <Users />
    },
    {
        path: '/login',
        element: <LoginForm />
    }
    ,
    {
        path: '/register',
        element: <RegisterForm />
    }
];

export default AppRoutes;
