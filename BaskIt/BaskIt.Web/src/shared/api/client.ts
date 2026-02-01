import axios from 'axios';

const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5033/api';

export const apiClient = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
  timeout: 30000,
});

// Request interceptor to add auth token to every request
apiClient.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('auth_token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Response interceptor for error handling
apiClient.interceptors.response.use(
  (response) => response,
  (error) => {
    // Handle 401 Unauthorized - token expired or invalid
    if (error.response?.status === 401) {
      const token = localStorage.getItem('auth_token');
      const currentPath = window.location.pathname;

      // Only redirect if we had a token (expired) and we're not already on login/register
      if (token && !currentPath.startsWith('/login') && !currentPath.startsWith('/register')) {
        localStorage.removeItem('auth_token');
        window.location.href = '/login';
      } else if (token) {
        // Just clear the token if we're already on login/register
        localStorage.removeItem('auth_token');
      }
    }

    if (!error.response) {
      return Promise.reject({
        message: error.message,
        statusCode: 0,
      });
    }

    return Promise.reject({
      message: error.response.data?.message || error.message || 'An error occurred',
      statusCode: error.response.status,
      errors: error.response.data?.errors,
    });
  }
);
