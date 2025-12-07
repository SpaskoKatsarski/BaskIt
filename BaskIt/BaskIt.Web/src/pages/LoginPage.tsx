import { Box } from '@chakra-ui/react';
import { LoginForm } from '../features/auth/components/LoginForm';

export default function LoginPage() {
  return (
    <Box pt={12}>
      <LoginForm />
    </Box>
  );
}
