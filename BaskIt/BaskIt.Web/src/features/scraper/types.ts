export interface Product {
  name: string | null;
  price: number | null;
  websiteUrl: string | null;
  size: string | null;
  color: string | null;
  description: string | null;
  imageUrl: string | null;
}

export interface ScrapeProductRequest {
  pageUrl: string;
}
