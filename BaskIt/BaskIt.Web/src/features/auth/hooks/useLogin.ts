import { useMutation } from '@tanstack/react-query';
import { login } from '../services/login';
import type { LoginRequest } from '../types';
import { useAuthStore } from '../store/authStore';

export function useLogin() {
  const setAuth = useAuthStore((state) => state.setAuth);

  return useMutation({
    mutationFn: (request: LoginRequest) => login(request),
    onSuccess: (token) => {
      // Store token and update auth state
      setAuth(token);
    },
  });
}
