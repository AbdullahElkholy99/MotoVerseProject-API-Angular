import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';

import { environment } from '../../environments/environment';
import { ResponeResult } from '../../models/Auth/ResponeResult';
import { Booking } from '../../models/booking.model';

@Injectable({
  providedIn: 'root',
})
export class IBookingStatusService {

  constructor(
    private router: Router,
    private _httpClient: HttpClient,
  ) { }

  baseUrl = environment.baseUrl + '/BookingStatus'
  // --------------
  approveBooking(id: string) : Observable<ResponeResult<string>> {
    return this._httpClient.patch<ResponeResult<string>>(
      `${this.baseUrl}/approve/${id}`,
      {}
    );
  }

  activateBooking(id: string): Observable<ResponeResult<string>> {
    return this._httpClient.patch<ResponeResult<string>>(
      `${this.baseUrl}/activate/${id}`,
      {}
    );
  }

  completeBooking(id: string): Observable<ResponeResult<string>> {
    return this._httpClient.patch<ResponeResult<string>>(
      `${this.baseUrl}/complete/${id}`,
      {}
    );
  }

  cancelBookingByCustomer(id: string) {
    return this._httpClient.patch<ResponeResult<string>>(
      `${this.baseUrl}/cancel-by-customer/${id}`,
      {}
    );
  }
  cancelBooking(id: string) {
    return this._httpClient.patch<ResponeResult<string>>(
      `${this.baseUrl}/cancel/${id}`,
      {}
    );
  }
  reCancelBooking(id: string) {
    return this._httpClient.patch<ResponeResult<string>>(
      `${this.baseUrl}/re-cancel/${id}`,
      {}
    );
  }

  rejectBooking(id: string) {
    return this._httpClient.patch<ResponeResult<string>>(
      `${this.baseUrl}/reject/${id}`,
      {}
    );
  }

  payBooking(id: string) {
    return this._httpClient.patch<ResponeResult<string>>(
      `${this.baseUrl}/pay/${id}`,
      {}
    );
  }

}
