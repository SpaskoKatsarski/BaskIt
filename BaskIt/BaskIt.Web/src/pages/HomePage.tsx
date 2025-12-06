import { useState } from 'react';
import { Box, Heading, Stack } from '@chakra-ui/react';
import { UrlInput } from '../features/scraper/components/UrlInput';
import { ScrapingStatus } from '../features/scraper/components/ScrapingStatus';
import { ErrorMessage } from '../features/scraper/components/ErrorMessage';
import { ProductPreview } from '../features/scraper/components/ProductPreview';
import { useScrapeProduct } from '../features/scraper/hooks/useScrapeProduct';
import { useDebounce } from '../shared/hooks/useDebounce';

export default function HomePage() {
  const [url, setUrl] = useState('');
  const debouncedUrl = useDebounce(url, 800);

  const { data: product, isLoading, error } = useScrapeProduct(debouncedUrl);

  return (
    <Stack gap={6}>
      <Heading>Add Product to Basket</Heading>

      <UrlInput value={url} onChange={setUrl} />

      {isLoading && <ScrapingStatus />}

      {error && <ErrorMessage message={(error as any).message || 'Failed to scrape product'} />}

      {product && <ProductPreview product={product} />}
    </Stack>
  );
}
