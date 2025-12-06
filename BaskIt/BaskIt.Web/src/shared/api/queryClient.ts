import { QueryClient } from '@tanstack/react-query';

/**
 * Global React Query client configuration
 *
 * Benefits:
 * - Centralized cache management for all API calls
 * - Automatic background refetching
 * - Request deduplication (multiple components using same data = 1 API call)
 *
 * Configuration:
 * - staleTime: How long data is considered fresh (won't auto-refetch)
 * - gcTime: How long unused data stays in cache before cleanup
 * - retry: Number of retry attempts for failed requests
 */
export const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      staleTime: 5 * 60 * 1000,
      gcTime: 10 * 60 * 1000,
      retry: 1,
    },
    mutations: {
      // Retry failed mutations once
      retry: 1,
    },
  },
});
