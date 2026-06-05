import { Component ,Input,input, signal} from '@angular/core';
import { Booking } from '../../../../models/booking.model';
import { CommonModule, DatePipe } from '@angular/common';

@Component({
  selector: 'app-booking-details',
  imports: [CommonModule],
  templateUrl: './booking-details.html',
  styleUrl: './booking-details.css',
})
export class BookingDetails {
  @Input({required:true}) selected = signal<Booking | null>(null);
  @Input({required:true}) drawerOpen = signal(false);



  downloadReceipt(b: Booking) {
    // this.bookingService.downloadReceipt(b.id);
  }

  closeDetails() {
    this.drawerOpen.set(false);
    this.selected.set(null)
  }
}
