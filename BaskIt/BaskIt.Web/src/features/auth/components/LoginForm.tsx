import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Box, Button, Input, Stack, Text, Heading, Link } from '@chakra-ui/react';
import { Link as RouterLink } from 'react-router-dom';
import { useLogin } from '../hooks/useLogin';

export function LoginForm() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const navigate = useNavigate();
  const { mutate: loginMutation, isPending, error } = useLogin();

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    loginMutation(
      { email, password },
      {
        onSuccess: () => {
          navigate('/');
        },
      }
    );
  };

  return (
    <Box maxW="400px" mx="auto" w="100%">
      <Stack gap={6}>
        <Heading size="xl" textAlign="center">
          Login
        </Heading>

        <form onSubmit={handleSubmit}>
          <Stack gap={4}>
            <Box>
              <Text mb={2} fontWeight="medium">
                Email
              </Text>
              <Input
                type="email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                placeholder="Enter your email"
                required
                bg="white"
                borderColor="gray.300"
                h={12}
              />
            </Box>

            <Box>
              <Text mb={2} fontWeight="medium">
                Password
              </Text>
              <Input
                type="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                placeholder="Enter your password"
                required
                bg="white"
                borderColor="gray.300"
                h={12}
              />
            </Box>

            {error && (
              <Text color="red.500" fontSize="sm">
                {(error as any).message || 'Login failed'}
              </Text>
            )}

            <Button
              type="submit"
              bg="blue.600"
              color="white"
              _hover={{ bg: 'blue.700' }}
              _active={{ bg: 'blue.800' }}
              h={12}
              isLoading={isPending}
            >
              Login
            </Button>

            <Text textAlign="center" fontSize="sm">
              Don't have an account?{' '}
              <Link asChild color="blue.600" fontWeight="medium">
                <RouterLink to="/register">Register</RouterLink>
              </Link>
            </Text>
          </Stack>
        </form>
      </Stack>
    </Box>
  );
}
