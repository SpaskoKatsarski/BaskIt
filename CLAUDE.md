# BaskIt Project Documentation

## Project Overview

BaskIt is a hybrid basket application that allows users to scrape product information from any web page by pasting a URL. Users can preview scraped products and organize them into multiple baskets.

**Tech Stack:**
- **Backend:** .NET Core Web API with Playwright for web scraping
- **Frontend:** React + TypeScript + Vite + Chakra UI + TanStack Query + Zustand

## Architecture

### Backend (.NET Core Web API)
- **Purpose:** Receive URLs and extract product information using web scraping
- **Scraping:** Implemented with Playwright using multiple scraping strategies with priority/fallback mechanism

### Frontend (BaskIt.Web)
- **Framework:** React 18+ with TypeScript
- **Build Tool:** Vite
- **UI Library:** Chakra UI
- **State Management:**
  - TanStack Query (React Query) for server state
  - Zustand for UI state
- **Routing:** React Router

## User Flow

1. User pastes a product URL into an input field
2. **Without clicking a button**, the URL is debounced and sent to the API
3. API scrapes the product information using multiple strategies
4. Product preview is displayed to the user
5. User can add the product to one of their baskets (they may have multiple baskets)

## Frontend Project Structure

```
BaskIt.Web/
  ├── public/                              # Static assets
  │   ├── favicon.ico
  │   └── logo.png
  │
  ├── src/
  │   ├── app/                             # Application initialization & setup
  │   │   ├── App.tsx                      # Root component, renders router
  │   │   ├── router.tsx                   # Route definitions (/, /baskets, /baskets/:id)
  │   │   └── providers.tsx                # Wraps app with ChakraProvider, QueryClientProvider
  │   │
  │   ├── pages/                           # Page components (one per route)
  │   │   ├── HomePage.tsx                 # Main page: URL input + basket list preview
  │   │   ├── BasketsPage.tsx              # All baskets view with grid/list
  │   │   └── BasketDetailsPage.tsx        # Single basket with all products inside
  │   │
  │   ├── features/                        # Feature modules (self-contained business logic)
  │   │   │
  │   │   ├── scraper/                     # Product scraping feature
  │   │   │   ├── services/
  │   │   │   │   └── scrapeProduct.ts     # POST /api/scraper - sends URL, returns Product
  │   │   │   ├── components/
  │   │   │   │   ├── UrlInput.tsx         # Input field with debounce, triggers scraping
  │   │   │   │   ├── ProductPreview.tsx   # Shows scraped product with "Add to Basket" button
  │   │   │   │   ├── ScrapingStatus.tsx   # Loading spinner + "Analyzing URL..." message
  │   │   │   │   └── ErrorMessage.tsx     # Shows scraping errors (invalid URL, failed scrape)
  │   │   │   ├── hooks/
  │   │   │   │   └── useScrapeProduct.ts  # React Query hook: scrapes product when URL valid
  │   │   │   └── types.ts                 # Product interface (name, price, image, url, etc.)
  │   │   │
  │   │   ├── baskets/                     # Basket management feature
  │   │   │   ├── services/
  │   │   │   │   ├── getBaskets.ts        # GET /api/baskets - fetches all user baskets
  │   │   │   │   ├── createBasket.ts      # POST /api/baskets - creates new basket
  │   │   │   │   ├── deleteBasket.ts      # DELETE /api/baskets/:id
  │   │   │   │   └── addProductToBasket.ts # POST /api/baskets/:id/products - adds product to basket
  │   │   │   ├── components/
  │   │   │   │   ├── BasketList.tsx       # Grid/list of all baskets
  │   │   │   │   ├── BasketCard.tsx       # Single basket preview (name, product count, thumbnail)
  │   │   │   │   ├── BasketSelector.tsx   # Modal: shows baskets, user picks one to add product
  │   │   │   │   ├── CreateBasketDialog.tsx # Modal: form to create new basket (name, description)
  │   │   │   │   └── CreateBasketButton.tsx # Button that opens CreateBasketDialog
  │   │   │   ├── hooks/
  │   │   │   │   ├── useBaskets.ts        # React Query: fetches all baskets (GET)
  │   │   │   │   ├── useCreateBasket.ts   # React Query: creates basket (POST mutation)
  │   │   │   │   ├── useDeleteBasket.ts   # React Query: deletes basket (DELETE mutation)
  │   │   │   │   └── useAddToBasket.ts    # React Query: adds product to basket (POST mutation)
  │   │   │   ├── store/
  │   │   │   │   └── basketUIStore.ts     # Zustand: UI state (modal open/closed, productToAdd)
  │   │   │   └── types.ts                 # Basket interface (id, name, products[], createdAt)
  │   │   │
  │   │   └── products/                    # Product display components (reusable)
  │   │       ├── components/
  │   │       │   ├── ProductCard.tsx      # Card showing product in basket (image, name, price)
  │   │       │   ├── ProductImage.tsx     # Optimized image component with fallback
  │   │       │   └── ProductList.tsx      # List of products in a basket
  │   │       └── types.ts                 # Shared Product type (if different from scraper)
  │   │
  │   ├── shared/                          # Shared utilities & components
  │   │   ├── api/
  │   │   │   ├── client.ts                # Base fetch wrapper (handles base URL, headers, errors)
  │   │   │   └── queryClient.ts           # React Query client configuration (staleTime, retry, etc.)
  │   │   ├── components/
  │   │   │   └── ui/                      # Reusable UI primitives (if not using Chakra directly)
  │   │   │       ├── LoadingSpinner.tsx   # Centered spinner for loading states
  │   │   │       └── EmptyState.tsx       # Empty state illustration (no baskets, no products)
  │   │   ├── hooks/
  │   │   │   ├── useDebounce.ts           # Debounces value (for URL input delay)
  │   │   │   └── useToast.ts              # Wrapper around Chakra's useToast (success/error helpers)
  │   │   ├── lib/
  │   │   │   └── utils.ts                 # Utility functions (formatPrice, validateUrl, etc.)
  │   │   └── types/
  │   │       └── api.ts                   # Shared API types (ApiError, PaginatedResponse, etc.)
  │   │
  │   ├── layouts/                         # Layout components
  │   │   └── MainLayout.tsx               # Header/navbar + content container + footer
  │   │
  │   ├── main.tsx                         # Entry point: renders App wrapped in Providers
  │   └── vite-env.d.ts                    # Vite TypeScript definitions
  │
  ├── .env                                 # Environment variables (VITE_API_URL=http://localhost:5000)
  ├── .env.development                     # Dev environment overrides
  ├── .gitignore
  ├── index.html                           # HTML entry point (Vite uses this)
  ├── package.json                         # Dependencies (react, chakra-ui, tanstack/query, zustand)
  ├── tsconfig.json                        # TypeScript configuration
  ├── tsconfig.node.json                   # TypeScript config for Vite config files
  └── vite.config.ts                       # Vite configuration (port, proxy to API, etc.)
```

## Development Guidelines

### Frontend Best Practices (React)

1. **Component Organization**
   - Use functional components with hooks
   - Keep components small and focused on single responsibility
   - Co-locate related files (components, hooks, types) within feature folders

2. **State Management**
   - Use TanStack Query for ALL server state (fetching, caching, mutations)
   - Use Zustand ONLY for UI state (modals, temporary form state, etc.)
   - Never duplicate server state in Zustand

   **Query Keys Management (CRITICAL):**
   - **Centralize query keys** - Define query keys in a constants file or at the top of each feature module
   - **Reuse query keys** across different hooks for the same operation
     - Example: If `useScrapeProduct` uses `['scrape-product', url]`, any other hook fetching the same data must use the exact same key
     - This ensures TanStack Query can properly deduplicate requests and share cache
   - **Invalidate query keys** when mutations change related data:
     - After creating a basket → invalidate `['baskets']` to refetch the list
     - After adding product to basket → invalidate `['baskets']` and `['basket', basketId]`
     - After deleting a basket → invalidate `['baskets']`
     - Use `queryClient.invalidateQueries()` in mutation's `onSuccess` callback
   - **Query key structure:**
     - Collections: `['baskets']`, `['products']`
     - Single items: `['basket', id]`, `['product', id]`
     - Filtered/parameterized: `['baskets', { userId }]`, `['scrape-product', url]`

3. **TypeScript**
   - Define interfaces for all data structures
   - Use strict TypeScript configuration
   - Avoid `any` type - use `unknown` if type is truly unknown

4. **API Integration**
   - All API calls go through the base client in `shared/api/client.ts`
   - Use React Query hooks for data fetching
   - Handle loading, error, and success states consistently

5. **Styling**
   - Use Chakra UI components as primary UI library
   - Follow Chakra's design system for consistency
   - Custom UI components go in `shared/components/ui/`

6. **Performance**
   - Debounce user inputs (like URL input) to avoid excessive API calls
   - Use React Query's caching to minimize network requests
   - Lazy load routes if needed

### Backend Best Practices (.NET)

1. **Architecture**
   - Follow clean architecture principles
   - Separate concerns (API, Business Logic, Data Access)
   - Use dependency injection

2. **API Design**
   - RESTful endpoints
   - Consistent response formats
   - Proper HTTP status codes
   - Include error details in responses

3. **Error Handling**
   - Global exception handling middleware
   - Return meaningful error messages to frontend
   - Log errors for debugging

4. **Scraping**
   - Multiple scraping strategies with priority/fallback
   - Handle failed scrapes gracefully
   - Consider rate limiting and caching

## Questions to Ask Before Implementation

When working on this project, if you encounter any of the following, **ASK THE USER** instead of assuming:

1. **API Endpoints:** What is the exact endpoint URL and response format?
2. **Data Models:** What fields does the Product/Basket contain?
3. **Authentication:** Is there user authentication? How is it handled?
4. **Persistence:** Where is basket data stored? (Database, localStorage, API?)
5. **API Base URL:** What is the backend API URL? (development vs production)
6. **Business Logic:** How should edge cases be handled? (duplicate products, invalid URLs, etc.)
7. **UI/UX Decisions:** Specific design preferences, modal behavior, confirmation dialogs, etc.
