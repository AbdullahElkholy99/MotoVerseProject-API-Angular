import { BookingStatus } from "../../Enums/booking-status.enum";
import { PaymentStatus } from "../../Enums/payment-status.enum";
import { Payment } from "./payment.interface";


export interface Booking {
  id: string;

  startDate: Date;

  endDate: Date;

  totalPrice: number;

  bookingStatus: BookingStatus;

  paymentStatus: PaymentStatus;

  createdAt: Date;

  payment?: Payment;

  customerId: string;


  motorcycleId: string;

}
