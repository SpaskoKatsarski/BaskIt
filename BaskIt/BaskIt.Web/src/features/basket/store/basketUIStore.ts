import { create } from 'zustand';
import type { Product } from '../../scraper/types';

interface BasketUIState {
  isSelectorOpen: boolean;
  isCreateDialogOpen: boolean;
  productToAdd: Product | null;
  openSelector: (product: Product) => void;
  closeSelector: () => void;
  openCreateDialog: () => void;
  closeCreateDialog: () => void;
}

export const useBasketUIStore = create<BasketUIState>((set) => ({
  isSelectorOpen: false,
  isCreateDialogOpen: false,
  productToAdd: null,

  openSelector: (product: Product) => {
    set({ isSelectorOpen: true, productToAdd: product });
  },

  closeSelector: () => {
    set({ isSelectorOpen: false, productToAdd: null });
  },

  openCreateDialog: () => {
    set({ isCreateDialogOpen: true });
  },

  closeCreateDialog: () => {
    set({ isCreateDialogOpen: false });
  },
}));
