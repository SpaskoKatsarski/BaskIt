import { QueryClientProvider } from '@tanstack/react-query';
import { ChakraProvider, defaultSystem } from '@chakra-ui/react';
import { queryClient } from '../shared/api/queryClient';

interface ProvidersProps {
  children: React.ReactNode;
}

/**
 * Application-wide providers wrapper
 *
 * Order matters:
 * 1. ChakraProvider - Provides UI theming and components
 * 2. QueryClientProvider - Provides React Query for server state management
 *
 * This component wraps the entire app to make these contexts available everywhere
 */
export function Providers({ children }: ProvidersProps) {
  return (
    <ChakraProvider value={defaultSystem}>
      <QueryClientProvider client={queryClient}>
        {children}
      </QueryClientProvider>
    </ChakraProvider>
  );
}
