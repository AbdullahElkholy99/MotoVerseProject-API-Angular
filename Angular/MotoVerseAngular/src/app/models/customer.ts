export interface Customer {
  id: string;
  avatar: string;
  name: string;
  email: string;
  phone: string;
  orders: number;
  status: 'Active' | 'Paused' | 'New';
}
