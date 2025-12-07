import { apiClient } from '../../../shared/api/client';
import type { RegisterRequest } from '../types';

export async function register(request: RegisterRequest): Promise<void> {
  await apiClient.post('/auth/register', request);
}
