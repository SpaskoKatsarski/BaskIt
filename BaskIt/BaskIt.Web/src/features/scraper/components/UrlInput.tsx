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
      value={value}
      onChange={(e) => onChange(e.target.value)}
      placeholder={placeholder}
      type="url"
    />
  );
}
