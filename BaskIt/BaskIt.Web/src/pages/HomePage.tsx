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

  const handleAddToBasket = () => {
    // TODO: Implement basket selection and add product
    // For now, just log to console
    console.log('Add to basket clicked:', product);
    alert('Basket functionality coming soon!');
  };

  return (
    <Box maxW="800px" mx="auto" w="100%" pt={12}>
      <Stack gap={8}>
        {/* Heading - Centered */}
        <Heading size="2xl" textAlign="center" fontWeight="bold" color="gray.800">
          Add Product to Basket
        </Heading>

        {/* Input - Centered and Constrained */}
        <Box maxW="600px" mx="auto" w="100%">
          <UrlInput value={url} onChange={setUrl} />
        </Box>

        {/* Status and Results - Centered */}
        {isLoading && <ScrapingStatus />}

        {error && <ErrorMessage message={(error as any).message || 'Failed to scrape product'} />}

        {product && <ProductPreview product={product} onAddToBasket={handleAddToBasket} />}
      </Stack>
    </Box>
  );
}
