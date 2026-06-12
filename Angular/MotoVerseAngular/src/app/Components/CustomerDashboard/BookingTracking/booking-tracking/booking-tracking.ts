import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Booking } from '../../../../models/booking.model';
import { IBookingService } from '../../../../services/MotorcycleServices/IBookingService';
import { ToastrService } from 'ngx-toastr';
import { Meta } from '@angular/platform-browser';
import { BookingDetails } from "../booking-details/booking-details";
import { BookingStats } from "../booking-stats/booking-stats";
import { IBookingStatusService } from '../../../../services/MotorcycleServices/IBookingStatusService';
import { ModalDelete } from "../../../Shared/modal-delete/modal-delete";
import { ModalOverlay } from "../../../Shared/modal-overlay/modal-overlay";
import { NoContent } from "../../../Shared/no-content/no-content";

@Component({
  selector: 'app-booking-tracking',
  standalone: true,
  imports: [CommonModule, FormsModule, BookingDetails, BookingStats, ModalDelete, ModalOverlay, NoContent],
  templateUrl: './booking-tracking.html',
  styleUrls: ['./booking-tracking.css', `../../../styleBoke.css`],
})
export class BookingTracking implements OnInit {

  alert(arg0: string) {
  }


  //inject booking service
  private _bookingStatusService = inject(IBookingStatusService)
  private _bookingService = inject(IBookingService)
  private toastr = inject(ToastrService)

  //
  bookings = signal<Booking[] | null>(null);
  filtered = signal<Booking[] | null>(null);
  loading = signal(true);

  stats!: { bookingCompleted: number, bookingPending: number, bookingActive: number, totalCount: number }
  totalCount = signal(0)
  bookingCompleted = signal(0)
  bookingPending = signal(0)
  bookingActive = signal(0)

  // UI state
  search = '';
  statusFilter: string = 'All';
  fromDate?: string;
  toDate?: string;


  drawerOpen = signal(false);
  selected = signal<Booking | null>(null);

  constructor() { }

  ngOnInit(): void {

    this.getAllBookings();
  }


  getAllBookings() {
    this.loading.set(true);

    this._bookingService.getBookingsForCustomer().subscribe({
      next: (result) => {
        if (!result.succeeded) {
          this.toastr.error(result.message);
          this.loading.set(false);
          return;
        }
        this.bookings.set(result.data);
        this.applyFilters();

        this.bookingActive.set(result.meta.bookingActive ?? 0)
        this.totalCount.set(result.meta.count ?? 0)
        this.bookingCompleted.set(result.meta.bookingCompleted ?? 0)
        this.bookingPending.set(result.meta.bookingPending ?? 0)
        this.stats = {
          bookingActive: result.meta.bookingActive ?? 0,
          bookingPending: result.meta.bookingPending ?? 0,
          totalCount: result.meta.count ?? 0,
          bookingCompleted: result.meta.bookingCompleted ?? 0
        }
        this.loading.set(false);
      },

      error: (error) => {
        console.error(error);
        this.toastr.error(error?.error?.message || 'Something went wrong');
        this.loading.set(false);
      }
    });
  }


  applyFilters(): void {

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

      // Start Date
      if (this.fromDate) {
        const fromDate = new Date(this.fromDate);

        if (new Date(booking.startDate) < fromDate) {
          return false;
        }
      }

      // End Date
      if (this.toDate) {
        const toDate = new Date(this.toDate);

        if (new Date(booking.endDate) > toDate) {
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
    this.fromDate = undefined;
    this.toDate = undefined;
    this.applyFilters();
  }

  openDetails(b: Booking) {
    this.selected.set(b);
    this.drawerOpen.set(true);
  }

  closeDetails() {
    this.drawerOpen.set(false);
  }

  openCancelModal = signal(false)
  selectedId = signal<string>('')
  toogleCancelModel(id: string) {
    this.openCancelModal.update((v) => !v)
    this.selectedId.set(id)
  }
  confirmCancelationBooking() {
    if (!this.selectedId()) return;
    this._bookingStatusService.cancelBookingByCustomer(this.selectedId()).subscribe({
      next: (res) => {
        if (!res.succeeded) {
          this.toastr.error(res.errors, 'error')
          return;
        }
        this.toastr.success(res.message, 'success')
        const booking = this.filtered()?.find(
          (i) => i.id === this.selectedId()
        );

        if (booking) {
          booking.bookingStatus = 'Cancelled';
        }
    this.openCancelModal.set(false)

      },
      error: (err) => {
        this.toastr.error(err.error?.message || err.error || 'Something went wrong', 'Error');
      }

    })
  }

  downloadReceipt(b: Booking) {
    // this.bookingService.downloadReceipt(b.id);
  }

  statusBadgeClass(status: string) {
    switch (status) {
      case 'Pending':
        return 'badge pending';
      case 'Approved':
        return 'badge approved';
      case 'Active':
        return 'badge active';
      case 'Completed':
        return 'badge completed';
      case 'Cancelled':
      case 'Rejected':
        return 'badge cancelled';
      default:
        return 'badge';
    }
  }

  progressPercent(status: string) {
    switch (status) {
      case 'Pending':
        return 10;
      case 'Approved':
        return 35;
      case 'Active':
        return 65;
      case 'Completed':
        return 100;
      case 'Cancelled':
      case 'Rejected':
        return 0;
      default:
        return 0;
    }
  }

  countByStatus(status: string) {
    // if (status === 'Total') return this.bookings.length;
    // return this.bookings().filter((b) => {
    //   if (status === 'Active') return b.bookingStatus === 'Active';
    //   if (status === 'Upcoming') return b.bookingStatus === 'Approved' && new Date(b.startDate) > new Date();
    //   if (status === 'Completed') return b.bookingStatus === 'Completed';
    //   return false;
    // }).length;
  }

  trackById(_: number, item: Booking) {
    return item.id;
  }
}
