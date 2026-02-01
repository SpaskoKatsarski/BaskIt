import { apiClient } from '../../../shared/api/client';
import type { Product } from '../../scraper/types';

export async function addProductToBasket(
  basketId: string,
  product: Product
): Promise<void> {
  await apiClient.post(`/Basket/${basketId}/products`, product);
}
