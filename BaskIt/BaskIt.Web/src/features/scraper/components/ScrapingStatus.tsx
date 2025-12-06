import { Flex, Spinner, Text } from '@chakra-ui/react';

export function ScrapingStatus() {
  return (
    <Flex align="center" gap={3} py={8}>
      <Spinner size="lg" color="blue.500" />
      <Text fontSize="lg">Analyzing URL...</Text>
    </Flex>
  );
}
