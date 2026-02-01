import { Box, Card, Heading, Text, Image, Stack } from '@chakra-ui/react';
import type { BasketListDto } from '../types';

interface BasketCardProps {
  basket: BasketListDto;
  onClick: () => void;
}

export function BasketCard({ basket, onClick }: BasketCardProps) {
  return (
    <Card.Root
      onClick={onClick}
      cursor="pointer"
      _hover={{ borderColor: 'blue.500', transform: 'translateY(-2px)' }}
      transition="all 0.2s"
    >
      <Card.Body>
        <Stack gap="3">
          {basket.thumbnailUrl && (
            <Image
              src={basket.thumbnailUrl}
              alt={basket.name}
              borderRadius="md"
              objectFit="cover"
              height="120px"
              width="100%"
            />
          )}
          {!basket.thumbnailUrl && (
            <Box
              height="120px"
              bg="gray.100"
              borderRadius="md"
              display="flex"
              alignItems="center"
              justifyContent="center"
            >
              <Text color="gray.400">No products</Text>
            </Box>
          )}
          <Box>
            <Heading size="sm">{basket.name}</Heading>
            {basket.description && (
              <Text color="gray.600" fontSize="sm" mt="1">
                {basket.description}
              </Text>
            )}
            <Text color="gray.500" fontSize="xs" mt="2">
              {basket.productCount} {basket.productCount === 1 ? 'item' : 'items'}
            </Text>
          </Box>
        </Stack>
      </Card.Body>
    </Card.Root>
  );
}
