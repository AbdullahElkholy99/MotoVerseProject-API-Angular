import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-booking-stats',
  imports: [],
  templateUrl: './booking-stats.html',
  styleUrl: './booking-stats.css',
})
export class BookingStats {

  @Input({required:true}) stats! : {bookingCompleted : number , bookingPending :number,bookingActive :number,totalCount:number}
}
