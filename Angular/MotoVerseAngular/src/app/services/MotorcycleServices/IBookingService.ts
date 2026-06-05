import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import {  Observable } from 'rxjs';

import { environment } from '../../environments/environment';
import { ResponeResult } from '../../models/Auth/ResponeResult';
import { Booking } from '../../models/booking.model';

@Injectable({
  providedIn: 'root',
})
export class IBookingService {

  constructor(
    private router: Router,
    private _httpClient: HttpClient,
  ) { }

  baseUrl = environment.baseUrl + '/Booking'
  // -------------- add --------------

  add(request: any): Observable<ResponeResult<string>> {
    return this._httpClient.post<ResponeResult<string>>(
      `${this.baseUrl}/add`,
      request
    );
  }
  // -------------- Get --------------

  getBookingsForCustomer(): Observable<ResponeResult<Booking[]>> {
    return this._httpClient.get<ResponeResult<Booking[]>>(`${this.baseUrl}/get-all-for-customer`);
  }
  getBookings(): Observable<ResponeResult<Booking[]>> {
    return this._httpClient.get<ResponeResult<Booking[]>>(`${this.baseUrl}/get-all`);
  }

  // getBookingById(id: string): Booking | undefined {
  //   return this._bookings$.getValue().find(b => b.id === id);
  // }

  // cancelBooking(id: string) {
  //   const items = this._bookings$.getValue().map(b => {
  //     if (b.id === id && b.bookingStatus === 'Pending') {
  //       return { ...b, bookingStatus: 'Cancelled', paymentStatus: b.paymentStatus === 'Paid' ? 'Refunded' : b.paymentStatus };
  //     }
  //     return b;
  //   });
  //   // this._bookings$.next(items);
  //   return of(true);
  // }
  // -------------- Edit  --------------


  // -------------- Delete  --------------
  // ---------------------------------


}
