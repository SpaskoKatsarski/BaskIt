import { apiClient } from '../../../shared/api/client';
import type { CreateBasketRequest } from '../types';

export async function createBasket(request: CreateBasketRequest): Promise<string> {
  const response = await apiClient.post<string>('/Basket', request);
  return response.data;
}
