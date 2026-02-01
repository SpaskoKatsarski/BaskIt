export interface BasketListDto {
  id: string;
  name: string;
  description?: string;
  productCount: number;
  thumbnailUrl?: string;
  createdAt: string;
}

export interface BasketDetailsDto {
  id: string;
  name: string;
  description?: string;
  products: ProductDto[];
  createdAt: string;
}

export interface ProductDto {
  id: string;
  name: string;
  price?: number;
  websiteUrl: string;
  size?: string;
  color?: string;
  description?: string;
  imageUrl?: string;
}

export interface CreateBasketRequest {
  name: string;
  description?: string;
}
