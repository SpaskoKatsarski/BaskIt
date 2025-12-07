import { Outlet, Link as RouterLink, useLocation } from 'react-router-dom';
import {
  Box,
  Container,
  Flex,
  Heading,
  Link,
  HStack,
  Button
} from '@chakra-ui/react';
import { useAuth } from '../features/auth/hooks/useAuth';
import { useLogout } from '../features/auth/hooks/useLogout';

export default function MainLayout() {
  const location = useLocation();
  const { isAuthenticated } = useAuth();
  const { logout } = useLogout();

  const isActive = (path: string) => location.pathname === path;

  return (
    <Box minH="100vh" bg="gray.50" display="flex" flexDirection="column">
      <Box
        as="header"
        bg="blue.600"
        color="white"
        py={4}
        boxShadow="sm"
        position="sticky"
        top={0}
        zIndex={1000}
        width="100%"
      >
        <Container maxW="7xl" px={8}>
          <Flex justify="space-between" align="center">
            <Heading
              size="lg"
              fontWeight="bold"
              color="white"
              _hover={{ opacity: 0.9, transition: 'opacity 0.2s' }}
            >
              <Link
                asChild
                _hover={{ textDecoration: 'none' }}
              >
                <RouterLink to="/">BaskIt</RouterLink>
              </Link>
            </Heading>

            <HStack gap={1}>
              <Link
                asChild
                px={4}
                py={2}
                rounded="md"
                fontWeight="medium"
                color="white"
                bg={isActive('/') ? 'whiteAlpha.200' : 'transparent'}
                _hover={{
                  textDecoration: 'none',
                  bg: 'whiteAlpha.200',
                  transition: 'all 0.2s'
                }}
                transition="all 0.2s"
              >
                <RouterLink to="/">Home</RouterLink>
              </Link>
              <Link
                asChild
                px={4}
                py={2}
                rounded="md"
                fontWeight="medium"
                color="white"
                bg={isActive('/baskets') ? 'whiteAlpha.200' : 'transparent'}
                _hover={{
                  textDecoration: 'none',
                  bg: 'whiteAlpha.200',
                  transition: 'all 0.2s'
                }}
                transition="all 0.2s"
              >
                <RouterLink to="/baskets">Baskets</RouterLink>
              </Link>

              {/* Conditional Auth Navigation */}
              {isAuthenticated ? (
                <Button
                  size="sm"
                  px={4}
                  py={2}
                  rounded="md"
                  fontWeight="medium"
                  color="white"
                  bg="red.500"
                  _hover={{
                    bg: 'red.600',
                    transition: 'all 0.2s'
                  }}
                  onClick={logout}
                >
                  Logout
                </Button>
              ) : (
                <>
                  <Link
                    asChild
                    px={4}
                    py={2}
                    rounded="md"
                    fontWeight="medium"
                    color="white"
                    bg={isActive('/login') ? 'whiteAlpha.200' : 'transparent'}
                    _hover={{
                      textDecoration: 'none',
                      bg: 'whiteAlpha.200',
                      transition: 'all 0.2s'
                    }}
                    transition="all 0.2s"
                  >
                    <RouterLink to="/login">Login</RouterLink>
                  </Link>
                  <Link
                    asChild
                    px={4}
                    py={2}
                    rounded="md"
                    fontWeight="medium"
                    color="white"
                    bg={isActive('/register') ? 'whiteAlpha.200' : 'transparent'}
                    _hover={{
                      textDecoration: 'none',
                      bg: 'whiteAlpha.200',
                      transition: 'all 0.2s'
                    }}
                    transition="all 0.2s"
                  >
                    <RouterLink to="/register">Register</RouterLink>
                  </Link>
                </>
              )}
            </HStack>
          </Flex>
        </Container>
      </Box>

      <Box flex="1" width="100%">
        <Container maxW="7xl" py={8}>
          <Outlet />
        </Container>
      </Box>
    </Box>
  );
}
