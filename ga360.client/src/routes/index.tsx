import { lazy } from 'react';
import { Navigate, createBrowserRouter } from 'react-router-dom';

// project import
import MainRoutes from './MainRoutes';
import LoginRoutes from './LoginRoutes';
import ComponentsRoutes from './ComponentsRoutes';

import { SimpleLayoutType } from 'config';
import SimpleLayout from 'layout/Simple';
import Loadable from 'components/Loadable';
import DashboardDefault from 'pages/dashboard/default';
import DashboardAnalytics from 'pages/dashboard/analytics';
import DashboardLayout from 'layout/Dashboard';

// render - landing page
const PagesLanding = Loadable(lazy(() => import('pages/landing')));

// ==============================|| ROUTING RENDER ||============================== //

const router = createBrowserRouter(
  [
    {
      path: '/',
      element: <DashboardLayout />,
      children: [
        {
          path: '/',
          element: <Navigate to="dashboard/default" replace />
        },
        {
          path: 'dashboard',
          children: [
            {
              path: 'default',
              element: <DashboardDefault />
            },
            {
              path: 'analytics',
              element: <DashboardAnalytics />
            }
          ]
        }
      ]
    },
    // {
    //   path: '/',
    //   element: <SimpleLayout layout={SimpleLayoutType.LANDING} />,
    //   children: [
    //     {
    //       index: true,
    //       element: <PagesLanding />
    //     }
    //   ]
    // },
    LoginRoutes,
    ComponentsRoutes,
    MainRoutes
  ],
  { basename: import.meta.env.VITE_APP_BASE_NAME }
);

export default router;
