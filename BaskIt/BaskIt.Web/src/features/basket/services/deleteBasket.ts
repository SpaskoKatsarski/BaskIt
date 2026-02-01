import { apiClient } from '../../../shared/api/client';

export async function deleteBasket(id: string): Promise<void> {
  await apiClient.delete(`/Basket/${id}`);
}
