import { Component, EventEmitter, inject, Input, Output, signal, Signal } from '@angular/core';
import { UiButton } from "../../../Shared/ui-button/ui-button";
import { Booking } from '../../../../models/booking.model';
import { IBookingStatusService } from '../../../../services/MotorcycleServices/IBookingStatusService';
import { ToastrService } from 'ngx-toastr';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-booking-status',
  imports: [    CommonModule,
UiButton],
  templateUrl: './booking-status.html',
  styleUrl: './booking-status.css',
})
export class BookingStatus {

  private _bookingStatusService = inject(IBookingStatusService)
  private toastr = inject(ToastrService)

  @Input({ required: true }) booking!: Booking;
  @Input({ required: true }) openedMenuId!: string | null;   // openedMenuId
  @Output() toggleMenu = new EventEmitter<string>();

  toggleActions(id: string): void {
    this.toggleMenu.emit(id);
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


}
