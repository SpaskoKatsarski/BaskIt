import { apiClient } from '../../../shared/api/client';
import type { BasketListDto } from '../types';

export async function getBaskets(): Promise<BasketListDto[]> {
  const response = await apiClient.get<BasketListDto[]>('/Basket');
  return response.data;
}
