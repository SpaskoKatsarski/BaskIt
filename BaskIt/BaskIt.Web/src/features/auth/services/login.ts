import { apiClient } from '../../../shared/api/client';
import type { LoginRequest } from '../types';

export async function login(request: LoginRequest): Promise<string> {
  const response = await apiClient.post<string>('/auth/login', request);
  return response.data;
}
