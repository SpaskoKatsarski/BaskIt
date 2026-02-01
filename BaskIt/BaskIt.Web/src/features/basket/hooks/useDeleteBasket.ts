import { useMutation, useQueryClient } from '@tanstack/react-query';
import { deleteBasket } from '../services/deleteBasket';

export function useDeleteBasket() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: string) => deleteBasket(id),
    onSuccess: () => {
      // Invalidate the baskets list after deletion
      queryClient.invalidateQueries({ queryKey: ['baskets'] });
    },
  });
}
