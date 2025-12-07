import { createBrowserRouter } from 'react-router-dom';
import HomePage from '../pages/HomePage';
import BasketsPage from '../pages/BasketsPage';
import BasketDetailsPage from '../pages/BasketDetailsPage';
import LoginPage from '../pages/LoginPage';
import RegisterPage from '../pages/RegisterPage';
import MainLayout from '../layouts/MainLayout';

export const router = createBrowserRouter([
  {
    path: '/',
    element: <MainLayout />,
    children: [
      {
        index: true,
        element: <HomePage />,
      },
      {
        path: 'baskets',
        element: <BasketsPage />,
      },
      {
        path: 'baskets/:id',
        element: <BasketDetailsPage />,
      },
      {
        path: 'login',
        element: <LoginPage />,
      },
      {
        path: 'register',
        element: <RegisterPage />,
      },
    ],
  },
]);
