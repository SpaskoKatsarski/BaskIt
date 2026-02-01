import { apiClient } from '../../../shared/api/client';
import type { UpdateBasketRequest } from '../types';

export async function updateBasket(
  id: string,
  data: UpdateBasketRequest
): Promise<void> {
  await apiClient.put(`/Basket/${id}`, data);
}
