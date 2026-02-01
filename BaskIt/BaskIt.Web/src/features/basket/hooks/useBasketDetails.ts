import { useQuery } from '@tanstack/react-query';
import { getBasketById } from '../services/getBasketById';

export function useBasketDetails(id: string) {
  return useQuery({
    queryKey: ['basket', id],
    queryFn: () => getBasketById(id),
    enabled: !!id,
  });
}
