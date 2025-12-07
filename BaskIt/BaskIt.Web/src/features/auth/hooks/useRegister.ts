import { useMutation } from '@tanstack/react-query';
import { register } from '../services/register';
import type { RegisterRequest } from '../types';

export function useRegister() {
  return useMutation({
    mutationFn: (request: RegisterRequest) => register(request),
  });
}
