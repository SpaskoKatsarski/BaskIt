import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Box, Button, Input, Stack, Text, Heading, Link } from '@chakra-ui/react';
import { Link as RouterLink } from 'react-router-dom';
import { useRegister } from '../hooks/useRegister';

export function RegisterForm() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const navigate = useNavigate();
  const { mutate: registerMutation, isPending, error } = useRegister();

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();

    if (password !== confirmPassword) {
      alert('Passwords do not match');
      return;
    }

    registerMutation(
      { email, password, confirmPassword },
      {
        onSuccess: () => {
          navigate('/login');
        },
      }
    );
  };

  return (
    <Box maxW="400px" mx="auto" w="100%">
      <Stack gap={6}>
        <Heading size="xl" textAlign="center">
          Register
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

            <Box>
              <Text mb={2} fontWeight="medium">
                Confirm Password
              </Text>
              <Input
                type="password"
                value={confirmPassword}
                onChange={(e) => setConfirmPassword(e.target.value)}
                placeholder="Confirm your password"
                required
                bg="white"
                borderColor="gray.300"
                h={12}
              />
            </Box>

            {error && (
              <Text color="red.500" fontSize="sm">
                {(error as any).message || 'Registration failed'}
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
              Register
            </Button>

            <Text textAlign="center" fontSize="sm">
              Already have an account?{' '}
              <Link asChild color="blue.600" fontWeight="medium">
                <RouterLink to="/login">Login</RouterLink>
              </Link>
            </Text>
          </Stack>
        </form>
      </Stack>
    </Box>
  );
}
