import { useQuery } from '@tanstack/react-query';
import { getBaskets } from '../services/getBaskets';

export function useBaskets() {
  return useQuery({
    queryKey: ['baskets'],
    queryFn: getBaskets,
  });
}
