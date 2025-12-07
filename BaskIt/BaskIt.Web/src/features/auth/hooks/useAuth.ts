import { useEffect } from 'react';
import { useAuthStore } from '../store/authStore';

export function useAuth() {
  const { isAuthenticated, setAuth, clearAuth, checkAuth } = useAuthStore();

  // Check auth status on mount
  useEffect(() => {
    checkAuth();
  }, [checkAuth]);

  return {
    isAuthenticated,
    setAuth,
    clearAuth,
    checkAuth,
  };
}
