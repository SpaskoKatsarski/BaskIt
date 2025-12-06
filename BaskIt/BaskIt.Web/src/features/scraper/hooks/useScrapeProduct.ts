import { useQuery } from '@tanstack/react-query';
import { scrapeProduct } from '../services/scrapeProduct';
import { validateUrl } from '../../../shared/lib/utils';

export function useScrapeProduct(url: string) {
  return useQuery({
    queryKey: ['scrape-product', url],
    queryFn: () => scrapeProduct(url),
    enabled: validateUrl(url) && url.length > 0,
    retry: false,
    staleTime: Infinity,
  });
}
