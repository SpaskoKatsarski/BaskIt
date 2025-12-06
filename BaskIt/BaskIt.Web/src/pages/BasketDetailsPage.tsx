import { useParams } from 'react-router-dom';
import { Heading } from '@chakra-ui/react';

export default function BasketDetailsPage() {
  const { id } = useParams();

  return (
    <div>
      <Heading>Basket Details: {id}</Heading>
    </div>
  );
}
