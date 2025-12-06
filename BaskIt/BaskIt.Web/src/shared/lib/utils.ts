export function formatPrice(price: number | null | undefined): string {
  if (price === null || price === undefined) {
    return 'Price not available';
  }

  return price.toFixed(2);
}

export function validateUrl(url: string): boolean {
  try {
    new URL(url);
    return true;
  } catch {
    return false;
  }
}
