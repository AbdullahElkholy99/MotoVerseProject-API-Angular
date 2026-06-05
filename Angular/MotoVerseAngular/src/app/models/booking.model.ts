export type BookingStatus = 'Pending' | 'Active' | 'Approved' | 'Completed' | 'Cancelled' | 'Rejected';
export type PaymentStatus = 'Pending' | 'Paid' | 'Failed' | 'Refunded' | 'Partial';

export interface Payment {
  amount: number;
  method: string;
  provider?: string;
  transactionId?: string;
  paidAt?: string; // ISO
}

export interface Booking {
  id: string;
  startDate: string; // ISO
  endDate: string; // ISO
  totalPrice: number;
  totalDays: number;
  bookingStatus: BookingStatus;
  paymentStatus: PaymentStatus;
  createdAt: string; // ISO
  pickupLocation: string;
  customerName:string;
  motorcycle: {
    id: string;
    name: string;
    brand: string;
    model: string;
    imagePath: string;
    images: string[];
    pricePerDay?: number;
    ownerName :string
  };
  payment?: Payment;
  timeline?: { status: BookingStatus | 'Submitted' | 'PaymentConfirmed' | 'PickedUp' | 'Other'; at: string; note?: string }[];
}
