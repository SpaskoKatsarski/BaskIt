import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig({
  plugins: [
    react({
      babel: {
        plugins: [['babel-plugin-react-compiler']],
      },
    }),
  ],
  server: {
    port: parseInt(process.env.PORT || '5173'),
    host: true, // Listen on all network interfaces
    strictPort: false, // Allow fallback to another port if specified port is in use
  },
})
