import { Outlet, Link } from 'react-router-dom';
import { Box, Container, Flex, Heading } from '@chakra-ui/react';

export default function MainLayout() {
  return (
    <Box minH="100vh">
      <Box as="header" bg="blue.500" color="white" py={4}>
        <Container maxW="container.xl">
          <Flex justify="space-between" align="center">
            <Heading size="lg">
              <Link to="/">BaskIt</Link>
            </Heading>
            <Flex gap={4}>
              <Link to="/">Home</Link>
              <Link to="/baskets">Baskets</Link>
            </Flex>
          </Flex>
        </Container>
      </Box>

      <Container maxW="container.xl" py={8}>
        <Outlet />
      </Container>
    </Box>
  );
}
