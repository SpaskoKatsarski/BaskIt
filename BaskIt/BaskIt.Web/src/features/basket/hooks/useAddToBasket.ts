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
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['baskets'] });
    },
  });
}
