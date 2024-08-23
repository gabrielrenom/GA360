import { lazy, useEffect } from 'react';

// project import
import AuthLayout from 'layout/Auth';
import Loadable from 'components/Loadable';
import { Navigate } from 'react-router';

// render - login
const AuthLogin = Loadable(lazy(() => import('pages/auth/login')));
const AuthRegister = Loadable(lazy(() => import('pages/auth/register')));
const AuthForgotPassword = Loadable(lazy(() => import('pages/auth/forgot-password')));
const AuthCheckMail = Loadable(lazy(() => import('pages/auth/check-mail')));
const AuthResetPassword = Loadable(lazy(() => import('pages/auth/reset-password')));
const AuthCodeVerification = Loadable(lazy(() => import('pages/auth/code-verification')));

// ==============================|| AUTH ROUTING ||============================== //
const ExternalRedirect = () => {
  useEffect(() => {
    window.location.href = 'https://localhost:5173/bff/login';
  }, []);

  return null;
};

const LoginRoutes = {
  path: '/',
  children: [
    {
      path: '/',
      element: <AuthLayout />,
      children: [
        //  {
        //    path: 'login',
        //    element: <AuthLogin />
        //  },
        {
         path: 'login',
         element: <ExternalRedirect />
        },
        {
          path: 'register',
          element: <AuthRegister />
        },
        {
          path: 'forgot-password',
          element: <AuthForgotPassword />
        },
        {
          path: 'check-mail',
          element: <AuthCheckMail />
        },
        {
          path: 'reset-password',
          element: <AuthResetPassword />
        },
        {
          path: 'code-verification',
          element: <AuthCodeVerification />
        }
      ]
    }
  ]
};

export default LoginRoutes;
