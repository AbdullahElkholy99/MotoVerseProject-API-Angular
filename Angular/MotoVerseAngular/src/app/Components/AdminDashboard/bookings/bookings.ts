import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { StatusBadge } from '../shared/status-badge/status-badge';
import { ToastrService } from 'ngx-toastr';
import { IBookingService } from '../../../services/MotorcycleServices/IBookingService';
import { Booking } from '../../../models/booking.model';
import { UiButton } from "../../Shared/ui-button/ui-button";
import { IBookingStatusService } from '../../../services/MotorcycleServices/IBookingStatusService';
import { BookingStatus } from "./booking-status/booking-status";
@Component({
  selector: 'admin-bookings',
  standalone: true,
  imports: [CommonModule, FormsModule, StatusBadge, UiButton, BookingStatus],
  templateUrl: './bookings.html',
  styleUrls: ['./bookings.css'],
})
export class AdminBookings implements OnInit {

  //inject booking service
  private _bookingService = inject(IBookingService)
  private _bookingStatusService = inject(IBookingStatusService)
  private toastr = inject(ToastrService)

  //
  bookings = signal<Booking[] | null>(null);
  filtered = signal<Booking[] | null>(null);
  loading = signal(true);
  totalItems = signal(0);


  search = '';
  statusFilter = 'All';
  paymentFilter = 'All';

  page = 1;
  pageSize = 10;

  constructor() { }

  ngOnInit(): void {
    this.getAllBookings()
  }


  getAllBookings() {
    this.loading.set(true);

    this._bookingService.getBookings().subscribe({
      next: (result) => {
        if (!result.succeeded) {
          this.toastr.error(result.message);
          this.loading.set(false);
          return;
        }
        this.bookings.set(result.data);
        console.log("000000000000 : result.data  ", result.data);

        this.applyFilters();
        this.totalItems.set(result.meta.count)
        this.loading.set(false);
      },

      error: (error) => {
        console.error(error);
        this.toastr.error(error?.error?.message || 'Something went wrong');
        this.loading.set(false);
      }
    });
  }

  //--------------------------------- Filters
  applyFilters() {

    const bookings = this.bookings();
    if (!bookings) {
      this.filtered.set([]);
      return;
    }
    const searchText = this.search.trim().toLowerCase();
    const filtered = bookings.filter((booking) => {
      // Status
      if (
        this.statusFilter !== 'All' &&
        booking.bookingStatus !== this.statusFilter
      ) {
        return false;
      }
      if (
        this.paymentFilter !== 'All' &&
        booking.paymentStatus !== this.paymentFilter
      ) {
        return false;
      }
      // Search
      if (searchText) {

        const searchableText = `
        ${booking.id}
        ${booking.motorcycle?.brand ?? ''}
        ${booking.motorcycle?.model ?? ''}
        ${booking.pickupLocation ?? ''}
      `.toLowerCase();

        if (!searchableText.includes(searchText)) {
          return false;
        }
      }

      return true;
    });

    this.filtered.set(filtered);
  }

  resetFilters() {
    this.search = '';
    this.statusFilter = 'All';
    this.paymentFilter = 'All';
    this.applyFilters();
  }


  // change booking status
  approve(booking: Booking) {
    this._bookingStatusService.approveBooking(booking.id).subscribe({
      next: (res) => {
        if (!res.succeeded) {
          this.toastr.error(res.errors, 'error')
          return;
        }
        booking.bookingStatus = 'Approved';
        this.toastr.success(res.message, 'success')
      },
      error: (err) => {
        this.toastr.error(err.error?.message || err.error || 'Something went wrong', 'Error');
      }
    });
  }
  activate(booking: Booking) {
    this._bookingStatusService.activateBooking(booking.id).subscribe({
      next: (res) => {
        if (!res.succeeded) {
          this.toastr.error(res.errors, 'error')
          return;
        }
        booking.bookingStatus = 'Active';
                this.toastr.success(res.message, 'success')
      },
      error: (err) => {
        this.toastr.error(err.error?.message || err.error || 'Something went wrong', 'Error');
      }
    });
  }
  complete(booking: Booking) {
    this._bookingStatusService.completeBooking(booking.id).subscribe({
      next: (res) => {
        if (!res.succeeded) {
          this.toastr.error(res.errors, 'error')
          return;
        }
                this.toastr.success(res.message, 'success')
        booking.bookingStatus = 'Completed';
      },
      error: (err) => {
        this.toastr.error(err.error?.message || err.error || 'Something went wrong', 'Error');
      }
    });
  }
  cancel(booking: Booking) {
    this._bookingStatusService.cancelBooking(booking.id).subscribe({
      next: (res) => {
        if (!res.succeeded) {
          this.toastr.error(res.errors, 'error')
          return;
        }
                this.toastr.success(res.message, 'success')
        booking.bookingStatus = 'Cancelled';
      },
      error: (err) => {
        this.toastr.error(err.error?.message || err.error || 'Something went wrong', 'Error');
      }
    });
  }
  reCancel(booking: Booking) {
    this._bookingStatusService.reCancelBooking(booking.id).subscribe({
      next: (res) => {
        if (!res.succeeded) {
          this.toastr.error(res.errors, 'error')
          return;
        }
                this.toastr.success(res.message, 'success')
        booking.bookingStatus = 'Pending';
      },
      error: (err) => {
        this.toastr.error(err.error?.message || err.error || 'Something went wrong', 'Error');
      }
    });
  }
  pay(booking: Booking) {
    this._bookingStatusService.payBooking(booking.id).subscribe({
      next: (res) => {
        if (!res.succeeded) {
          this.toastr.error(res.errors, 'error')
          return;
        }
                this.toastr.success(res.message, 'success')
        booking.paymentStatus = 'Paid';
      },
      error: (err) => {
        this.toastr.error(err.error?.message || err.error || 'Something went wrong', 'Error');

      }
    });
  }

  reject(booking: Booking) {
    this._bookingStatusService.rejectBooking(booking.id).subscribe({
      next: (res) => {
        if (!res.succeeded) {
          this.toastr.error(res.errors, 'error')
          return;
        }
                this.toastr.success(res.message, 'success')
        booking.bookingStatus = 'Rejected';
      },
      error: (err) => {
        this.toastr.error(err.error?.message || err.error || 'Something went wrong', 'Error');

      }
    });
  }


  //endItem
  get endItem(): number {
    return Math.min(this.page * this.pageSize, this.totalItems());
  }

  // openedMenuId
openedMenuId = signal<string | null>(null);

toggleActions(id: string) {
  this.openedMenuId.set(
    this.openedMenuId() === id ? null : id
  );
}
}
