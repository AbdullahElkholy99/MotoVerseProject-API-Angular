import { PaymentMethod } from '../../Enums/payment-method.enum';
import { Booking } from './booking.interface';

export interface Payment {
  id: string;

  bookingId: string;

  booking?: Booking;

  amount: number;

  paymentMethod: PaymentMethod;

  transactionId?: string;

  paidAt: Date;
}
