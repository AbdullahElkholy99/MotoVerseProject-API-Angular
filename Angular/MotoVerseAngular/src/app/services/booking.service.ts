// import { Injectable } from '@angular/core';
// import { BehaviorSubject, Observable, of } from 'rxjs';
// import { Booking, BookingStatus } from '../models/booking.model';

// @Injectable({ providedIn: 'root' })
// export class BookingService {
//   private _bookings$ = new BehaviorSubject<Booking[]>(this._mockBookings());

//   getBookings(): Observable<Booking[]> {
//     return this._bookings$.asObservable();
//   }

//   getBookingById(id: string): Booking | undefined {
//     return this._bookings$.getValue().find(b => b.id === id);
//   }

//   cancelBooking(id: string) {
//     const items = this._bookings$.getValue().map(b => {
//       if (b.id === id && b.bookingStatus === 'Pending') {
//         return { ...b, bookingStatus: 'Cancelled', paymentStatus: b.paymentStatus === 'Paid' ? 'Refunded' : b.paymentStatus };
//       }
//       return b;
//     });
//     // this._bookings$.next(items);
//     return of(true);
//   }

//   downloadReceipt(id: string) {
//     const booking = this.getBookingById(id);
//     if (!booking) return;
//     const content = `Booking Receipt\n\nBooking ID: ${booking.id}\nMotorcycle: ${booking.motorcycle.brand} ${booking.motorcycle.name}\nStart: ${booking.startDate}\nEnd: ${booking.endDate}\nTotal: $${booking.totalPrice.toFixed(2)}\n\nThank you for renting with MotoVerse.`;
//     const blob = new Blob([content], { type: 'text/plain' });
//     const url = window.URL.createObjectURL(blob);
//     const a = document.createElement('a');
//     a.href = url;
//     a.download = `motoverse_receipt_${booking.id}.txt`;
//     a.click();
//     window.URL.revokeObjectURL(url);
//   }

//   // Mock dataset
//   private _mockBookings(): Booking[] {
//     const now = new Date();
//     const iso = (d: Date) => d.toISOString();
//     const addDays = (d: Date, days: number) => new Date(d.getTime() + days * 86400000);

//     const b1Start = addDays(now, 1);
//     const b1End = addDays(b1Start, 3);

//     const b2Start = addDays(now, -2);
//     const b2End = addDays(b2Start, 2);

//     const b3Start = addDays(now, 10);
//     const b3End = addDays(b3Start, 5);

//     return [
//       {
//         id: 'BK-1001',
//         startDate: iso(b1Start),
//         endDate: iso(b1End),
//         totalDays: 3,
//         totalPrice: 299.99,
//         bookingStatus: 'Approved',
//         paymentStatus: 'Paid',
//         createdAt: iso(now),
//         pickupLocation: 'Downtown MotoHub',
//         motorcycle: {
//           id: 'M-101',
//           name: 'Raptor 900',
//           brand: 'MotoPrime',
//           model: 'RP-900',
//           images: ['/public/assets/bikes/1.jpg'],
//           imagePath:"",
//           pricePerDay: 99.99
//         },
//         payment: {
//           amount: 299.99,
//           method: 'Card',
//           provider: 'Stripe',
//           transactionId: 'tx_9A1B2C3',
//           paidAt: iso(now)
//         },
//         timeline: [
//           { status: 'Pending', at: iso(now), note: 'Booking Submitted' },
//           { status: 'Approved', at: iso(now), note: 'Booking Approved' },
//         ]
//       },

//       {
//         id: 'BK-1002',
//         startDate: iso(b2Start),
//         endDate: iso(b2End),
//         totalDays: 4,
//         totalPrice: 399.5,
//         bookingStatus: 'Pending',
//         paymentStatus: 'Paid',
//         createdAt: iso(addDays(now, -5)),
//         pickupLocation: 'Airport Branch',
//         motorcycle: {
//           id: 'M-102',
//           name: 'TrailMaster 400',
//           brand: 'TrailCo',
//           model: 'TM-400',
//           images: ['/public/assets/bikes/2.jpg'],
//           imagePath:"",
//           pricePerDay: 99.99
//         },
//         payment: {
//           amount: 399.5,
//           method: 'PayPal',
//           provider: 'PayPal',
//           transactionId: 'pp_12345',
//           paidAt: iso(addDays(now, -4))
//         },
//         timeline: [
//           { status: 'Pending', at: iso(addDays(now, -6)), note: 'Booking Submitted' },
//           { status: 'Approved', at: iso(addDays(now, -5)), note: 'Booking Approved' },
//           { status: 'Pending', at: iso(addDays(now, -2)), note: 'Motorcycle Picked Up' },
//         ]
//       },

//       {
//         id: 'BK-1003',
//         startDate: iso(b3Start),
//         endDate: iso(b3End),
//         totalDays: 5,
//         totalPrice: 499.0,
//         bookingStatus: 'Pending',
//         paymentStatus: 'Pending',
//         createdAt: iso(addDays(now, -1)),
//         pickupLocation: 'Eastside Outlet',
//         motorcycle: {
//           id: 'M-103',
//           name: 'Cruiser LX',
//           brand: 'ClassicRide',
//           model: 'CR-LX',
//           images: ['/public/assets/bikes/3.jpg'],
//           imagePath:"",
//           pricePerDay: 99.99
//         },
//         timeline: [
//           { status: 'Pending', at: iso(addDays(now, -1)), note: 'Booking Submitted' },
//         ]
//       }
//     ];
//   }
// }
