import { useMutation, useQueryClient } from '@tanstack/react-query';
import { createBasket } from '../services/createBasket';
import type { CreateBasketRequest } from '../types';

export function useCreateBasket() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (request: CreateBasketRequest) => createBasket(request),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['baskets'] });
    },
  });
}
