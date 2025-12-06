import { apiClient } from '../../../shared/api/client';
import type { Product, ScrapeProductRequest } from '../types';

export async function scrapeProduct(pageUrl: string): Promise<Product> {
  const response = await apiClient.post<Product>('/Product/preview', {
    pageUrl,
  } satisfies ScrapeProductRequest);

  return response.data;
}
