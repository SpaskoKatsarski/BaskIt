import { useParams, useNavigate } from 'react-router-dom';
import {
  Box,
  Button,
  Container,
  Flex,
  Heading,
  Spinner,
  Stack,
  Text,
} from '@chakra-ui/react';
import { useBasketDetails } from '../features/basket/hooks/useBasketDetails';
import { ProductCard } from '../features/product/components/ProductCard';

export default function BasketDetailsPage() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { data: basket, isLoading, error } = useBasketDetails(id || '');

  const handleProductClick = (websiteUrl: string) => {
    if (websiteUrl) {
      window.open(websiteUrl, '_blank', 'noopener,noreferrer');
    }
  };

  if (isLoading) {
    return (
      <Container maxW="container.lg" py={8}>
        <Flex justify="center" align="center" minH="400px">
          <Spinner size="xl" />
        </Flex>
      </Container>
    );
  }

  if (error) {
    return (
      <Container maxW="container.lg" py={8}>
        <Text color="red.500">Error loading basket: {String(error)}</Text>
        <Button mt={4} onClick={() => navigate('/baskets')}>
          Back to Baskets
        </Button>
      </Container>
    );
  }

  if (!basket) {
    return (
      <Container maxW="container.lg" py={8}>
        <Text>Basket not found</Text>
        <Button mt={4} onClick={() => navigate('/baskets')}>
          Back to Baskets
        </Button>
      </Container>
    );
  }

  return (
    <Container maxW="container.lg" py={8}>
      <Flex justify="space-between" align="center" mb={6}>
        <Box>
          <Heading>{basket.name}</Heading>
          {basket.description && (
            <Text color="gray.600" mt={2}>
              {basket.description}
            </Text>
          )}
          <Text color="gray.500" fontSize="sm" mt={2}>
            Created: {new Date(basket.createdAt).toLocaleDateString()}
          </Text>
        </Box>
        <Button onClick={() => navigate('/baskets')} variant="outline">
          Back to Baskets
        </Button>
      </Flex>

      <Heading size="md" mb={4}>
        Products ({basket.products.length})
      </Heading>

      {basket.products.length === 0 ? (
        <Box p={8} textAlign="center" borderWidth={1} borderRadius="md" bg="gray.50">
          <Text color="gray.500">No products in this basket yet.</Text>
        </Box>
      ) : (
        <Stack gap={4}>
          {basket.products.map((product) => (
            <ProductCard
              key={product.id}
              product={product}
              onClick={() => handleProductClick(product.websiteUrl)}
            />
          ))}
        </Stack>
      )}
    </Container>
  );
}
