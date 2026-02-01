import { useMutation, useQueryClient } from '@tanstack/react-query';
import { addProductToBasket } from '../services/addProductToBasket';
import type { Product } from '../../scraper/types';

interface AddToBasketParams {
  basketId: string;
  product: Product;
}

export function useAddToBasket() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ basketId, product }: AddToBasketParams) =>
      addProductToBasket(basketId, product),
    onSuccess: (_, variables) => {
      // Invalidate both the baskets list and the specific basket details
      queryClient.invalidateQueries({ queryKey: ['baskets'] });
      queryClient.invalidateQueries({ queryKey: ['basket', variables.basketId] });
    },
  });
}
