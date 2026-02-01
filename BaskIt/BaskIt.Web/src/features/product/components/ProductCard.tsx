import { Box, Card, Flex, Heading, Image, Text } from '@chakra-ui/react';
import type { ProductDto } from '../../basket/types';
import { formatPrice } from '../../../shared/lib/utils';

interface ProductCardProps {
  product: ProductDto;
  onClick: () => void;
}

export function ProductCard({ product, onClick }: ProductCardProps) {
  return (
    <Card.Root
      onClick={onClick}
      cursor="pointer"
      _hover={{ borderColor: 'blue.500', transform: 'translateY(-2px)' }}
      transition="all 0.2s"
    >
      <Card.Body>
        <Flex gap={4}>
          {product.imageUrl && (
            <Box flexShrink={0}>
              <Image
                src={product.imageUrl}
                alt={product.name || 'Product'}
                boxSize="120px"
                objectFit="cover"
                borderRadius="md"
              />
            </Box>
          )}

          <Flex direction="column" gap={2} flex={1}>
            <Heading size="md">{product.name || 'Unknown Product'}</Heading>

            {product.price !== null && product.price !== undefined && (
              <Text fontSize="lg" fontWeight="bold" color="blue.600">
                ${formatPrice(product.price)}
              </Text>
            )}

            {product.description && (
              <Text color="gray.600" fontSize="sm" lineClamp={2}>
                {product.description}
              </Text>
            )}

            <Flex gap={3} fontSize="sm">
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
          </Flex>
        </Flex>
      </Card.Body>
    </Card.Root>
  );
}
