import { Box, Card, Flex, Heading, Image, Text, Button } from '@chakra-ui/react';
import type { Product } from '../types';
import { formatPrice } from '../../../shared/lib/utils';

interface ProductPreviewProps {
  product: Product;
  onAddToBasket?: () => void;
}

export function ProductPreview({ product, onAddToBasket }: ProductPreviewProps) {
  return (
    <Card.Root>
      <Card.Body>
        <Flex gap={6}>
          {product.imageUrl && (
            <Box flexShrink={0}>
              <Image
                src={product.imageUrl}
                alt={product.name || 'Product'}
                boxSize="200px"
                objectFit="cover"
                borderRadius="md"
              />
            </Box>
          )}

          <Flex direction="column" gap={3} flex={1}>
            <Heading size="lg">{product.name || 'Unknown Product'}</Heading>

            {product.price !== null && (
              <Text fontSize="2xl" fontWeight="bold" color="blue.600">
                {formatPrice(product.price)}
              </Text>
            )}

            {product.description && (
              <Text color="gray.600" lineClamp={3}>
                {product.description}
              </Text>
            )}

            <Flex gap={4}>
              {product.size && (
                <Text>
                  <strong>Size:</strong> {product.size}
                </Text>
              )}
              {product.color && (
                <Text>
                  <strong>Color:</strong> {product.color}
                </Text>
              )}
            </Flex>

            {onAddToBasket && (
              <Button colorScheme="blue" size="lg" onClick={onAddToBasket} mt={4}>
                Add to Basket
              </Button>
            )}
          </Flex>
        </Flex>
      </Card.Body>
    </Card.Root>
  );
}
