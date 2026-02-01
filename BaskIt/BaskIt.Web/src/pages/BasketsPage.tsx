import {
  Box,
  Button,
  Container,
  Flex,
  Heading,
  IconButton,
  Input,
  Stack,
  Text,
  Spinner,
} from '@chakra-ui/react';
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useBaskets } from '../features/basket/hooks/useBaskets';
import { useCreateBasket } from '../features/basket/hooks/useCreateBasket';
import { useUpdateBasket } from '../features/basket/hooks/useUpdateBasket';
import { useDeleteBasket } from '../features/basket/hooks/useDeleteBasket';
import type { BasketListDto } from '../features/basket/types';

export default function BasketsPage() {
  const navigate = useNavigate();
  const { data: baskets, isLoading, error } = useBaskets();
  const createBasket = useCreateBasket();
  const updateBasket = useUpdateBasket();
  const deleteBasket = useDeleteBasket();

  const [editingId, setEditingId] = useState<string | null>(null);
  const [editName, setEditName] = useState('');
  const [editDescription, setEditDescription] = useState('');

  const [newBasketName, setNewBasketName] = useState('');
  const [newBasketDescription, setNewBasketDescription] = useState('');

  const handleCreate = async () => {
    if (!newBasketName.trim()) return;

    await createBasket.mutateAsync({
      name: newBasketName,
      description: newBasketDescription || undefined,
    });

    setNewBasketName('');
    setNewBasketDescription('');
  };

  const handleStartEdit = (basket: BasketListDto) => {
    setEditingId(basket.id);
    setEditName(basket.name);
    setEditDescription(basket.description || '');
  };

  const handleSaveEdit = async () => {
    if (!editingId || !editName.trim()) return;

    await updateBasket.mutateAsync({
      id: editingId,
      data: {
        name: editName,
        description: editDescription || undefined,
      },
    });

    setEditingId(null);
    setEditName('');
    setEditDescription('');
  };

  const handleCancelEdit = () => {
    setEditingId(null);
    setEditName('');
    setEditDescription('');
  };

  const handleDelete = async (id: string) => {
    if (confirm('Are you sure you want to delete this basket?')) {
      await deleteBasket.mutateAsync(id);
    }
  };

  const handleBasketClick = (id: string) => {
    if (!editingId) {
      navigate(`/baskets/${id}`);
    }
  };

  if (isLoading) {
    return (
      <Container maxW="container.lg" py={8}>
        <Flex justify="center" align="center" minH="400px">
          <Spinner size="xl" />
        </Flex>
      </Container>
    );
  }

  if (error) {
    return (
      <Container maxW="container.lg" py={8}>
        <Text color="red.500">Error loading baskets: {String(error)}</Text>
      </Container>
    );
  }

  return (
    <Container maxW="container.lg" py={8}>
      <Heading mb={6}>My Baskets</Heading>

      {/* Create New Basket */}
      <Box mb={8} p={4} borderWidth={1} borderRadius="md" bg="gray.50">
        <Heading size="sm" mb={3}>
          Create New Basket
        </Heading>
        <Stack gap={3}>
          <Input
            placeholder="Basket name"
            value={newBasketName}
            onChange={(e) => setNewBasketName(e.target.value)}
            bg="white"
          />
          <Input
            placeholder="Description (optional)"
            value={newBasketDescription}
            onChange={(e) => setNewBasketDescription(e.target.value)}
            bg="white"
          />
          <Button
            onClick={handleCreate}
            isLoading={createBasket.isPending}
            disabled={!newBasketName.trim()}
            colorScheme="blue"
            width="fit-content"
          >
            Create Basket
          </Button>
        </Stack>
      </Box>

      {/* Baskets List */}
      <Stack gap={4}>
        {baskets && baskets.length === 0 && (
          <Text color="gray.500">No baskets yet. Create one above!</Text>
        )}

        {baskets?.map((basket) => (
          <Box
            key={basket.id}
            p={4}
            borderWidth={1}
            borderRadius="md"
            _hover={editingId !== basket.id ? { bg: 'gray.50' } : undefined}
            cursor={editingId !== basket.id ? 'pointer' : 'default'}
            onClick={() => handleBasketClick(basket.id)}
          >
            {editingId === basket.id ? (
              <Stack gap={3}>
                <Input
                  placeholder="Basket name"
                  value={editName}
                  onChange={(e) => setEditName(e.target.value)}
                />
                <Input
                  placeholder="Description (optional)"
                  value={editDescription}
                  onChange={(e) => setEditDescription(e.target.value)}
                />
                <Flex gap={2}>
                  <Button
                    size="sm"
                    onClick={handleSaveEdit}
                    isLoading={updateBasket.isPending}
                    colorScheme="green"
                  >
                    Save
                  </Button>
                  <Button size="sm" onClick={handleCancelEdit} variant="outline">
                    Cancel
                  </Button>
                </Flex>
              </Stack>
            ) : (
              <Flex justify="space-between" align="start">
                <Box flex={1}>
                  <Heading size="md">{basket.name}</Heading>
                  {basket.description && (
                    <Text color="gray.600" mt={1}>
                      {basket.description}
                    </Text>
                  )}
                  <Flex gap={4} mt={2} fontSize="sm" color="gray.500">
                    <Text>
                      {basket.productCount} {basket.productCount === 1 ? 'item' : 'items'}
                    </Text>
                    <Text>Created: {new Date(basket.createdAt).toLocaleDateString()}</Text>
                  </Flex>
                </Box>
                <Flex gap={2} onClick={(e) => e.stopPropagation()}>
                  <Button size="sm" onClick={() => handleStartEdit(basket)} variant="outline">
                    Edit
                  </Button>
                  <Button
                    size="sm"
                    onClick={() => handleDelete(basket.id)}
                    isLoading={deleteBasket.isPending}
                    colorScheme="red"
                    variant="outline"
                  >
                    Delete
                  </Button>
                </Flex>
              </Flex>
            )}
          </Box>
        ))}
      </Stack>
    </Container>
  );
}
