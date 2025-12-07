import { Box } from '@chakra-ui/react';
import { RegisterForm } from '../features/auth/components/RegisterForm';

export default function RegisterPage() {
  return (
    <Box pt={12}>
      <RegisterForm />
    </Box>
  );
}
