export interface Order {
  id: string;
  customer: string;
  date: string;
  totalPrice: number;
  paymentStatus: 'Paid' | 'Pending' | 'Refunded';
  deliveryStatus: 'Delivered' | 'Processing' | 'Shipped' | 'Canceled';
}
