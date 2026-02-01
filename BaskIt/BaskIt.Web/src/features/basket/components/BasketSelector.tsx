import {
  Box,
  Button,
  DialogActionTrigger,
  DialogBody,
  DialogCloseTrigger,
  DialogContent,
  DialogFooter,
  DialogHeader,
  DialogRoot,
  DialogTitle,
  Heading,
  Stack,
  Text,
} from '@chakra-ui/react';
import { useBasketUIStore } from '../store/basketUIStore';
import { useBaskets } from '../hooks/useBaskets';
import { useAddToBasket } from '../hooks/useAddToBasket';

export function BasketSelector() {
  const { isSelectorOpen, productToAdd, closeSelector } = useBasketUIStore();
  const { data: baskets, isLoading } = useBaskets();
  const addToBasket = useAddToBasket();

  const handleSelectBasket = async (basketId: string) => {
    if (!productToAdd) return;

    try {
      await addToBasket.mutateAsync({
        basketId,
        product: productToAdd,
      });
      closeSelector();
    } catch (error) {
      console.error('Failed to add product to basket:', error);
      // Error will be handled by React Query
    }
  };

  return (
    <DialogRoot open={isSelectorOpen} onOpenChange={(e) => !e.open && closeSelector()}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Select a Basket</DialogTitle>
        </DialogHeader>
        <DialogCloseTrigger />

        <DialogBody>
          {isLoading ? (
            <Text>Loading baskets...</Text>
          ) : baskets && baskets.length > 0 ? (
            <Stack gap={2}>
              {baskets.map((basket) => (
                <Box
                  key={basket.id}
                  p={3}
                  borderWidth={1}
                  borderRadius="md"
                  cursor="pointer"
                  _hover={{ bg: 'blue.50', borderColor: 'blue.500' }}
                  onClick={() => handleSelectBasket(basket.id)}
                >
                  <Heading size="sm">{basket.name}</Heading>
                  {basket.description && (
                    <Text fontSize="sm" color="gray.600" mt={1}>
                      {basket.description}
                    </Text>
                  )}
                  <Text fontSize="xs" color="gray.500" mt={1}>
                    {basket.productCount} {basket.productCount === 1 ? 'item' : 'items'}
                  </Text>
                </Box>
              ))}
            </Stack>
          ) : (
            <Box textAlign="center" py={4}>
              <Text color="gray.500" mb={4}>
                You don't have any baskets yet.
              </Text>
              <Text fontSize="sm" color="gray.600">
                Go to the Baskets page to create one.
              </Text>
            </Box>
          )}
        </DialogBody>

        <DialogFooter>
          <DialogActionTrigger asChild>
            <Button variant="outline">Cancel</Button>
          </DialogActionTrigger>
        </DialogFooter>
      </DialogContent>
    </DialogRoot>
  );
}
