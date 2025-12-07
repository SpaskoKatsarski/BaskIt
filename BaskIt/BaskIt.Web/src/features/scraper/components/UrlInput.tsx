import { Input } from '@chakra-ui/react';

interface UrlInputProps {
  value: string;
  onChange: (value: string) => void;
  placeholder?: string;
}

export function UrlInput({ value, onChange, placeholder = 'Paste product URL here...' }: UrlInputProps) {
  return (
    <Input
      size="lg"
      h={14}
      value={value}
      onChange={(e) => onChange(e.target.value)}
      placeholder={placeholder}
      type="url"
      fontSize="lg"
      bg="white"
      borderWidth="2px"
      borderColor="gray.200"
      _hover={{
        borderColor: 'gray.300'
      }}
      _focus={{
        borderColor: 'blue.500',
        boxShadow: '0 0 0 3px rgba(66, 153, 225, 0.1)',
        outline: 'none'
      }}
      _placeholder={{
        color: 'gray.400'
      }}
      transition="all 0.2s"
    />
  );
}
