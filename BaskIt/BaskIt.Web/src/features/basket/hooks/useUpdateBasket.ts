import { useMutation, useQueryClient } from '@tanstack/react-query';
import { updateBasket } from '../services/updateBasket';
import type { UpdateBasketRequest } from '../types';

export function useUpdateBasket() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, data }: { id: string; data: UpdateBasketRequest }) =>
      updateBasket(id, data),
    onSuccess: (_, variables) => {
      // Invalidate both the basket details and the list
      queryClient.invalidateQueries({ queryKey: ['basket', variables.id] });
      queryClient.invalidateQueries({ queryKey: ['baskets'] });
    },
  });
}
