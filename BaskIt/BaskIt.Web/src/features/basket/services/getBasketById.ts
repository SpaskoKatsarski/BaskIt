import { apiClient } from '../../../shared/api/client';
import type { BasketDetailsDto } from '../types';

export async function getBasketById(id: string): Promise<BasketDetailsDto> {
  const response = await apiClient.get<BasketDetailsDto>(`/Basket/${id}`);
  return response.data;
}
