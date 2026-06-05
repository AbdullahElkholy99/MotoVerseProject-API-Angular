export interface Product {
  id: string;
  image: string;
  name: string;
  price: number;
  category: string;
  stock: number;
  rating: number;
  status: 'Active' | 'Out of Stock' | 'Draft';
}
