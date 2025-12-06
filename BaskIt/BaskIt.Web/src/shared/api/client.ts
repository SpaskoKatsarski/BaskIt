import axios from 'axios';

const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5033/api';

export const apiClient = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
  timeout: 30000,
});

apiClient.interceptors.response.use(
  (response) => response,
  (error) => {
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
