import { Component, OnInit } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { StatCard } from '../shared/stat-card/stat-card';

@Component({
  selector: 'admin-overview',
  standalone: true,
  imports: [CommonModule, StatCard],
  templateUrl: './overview.html',
  styleUrls: ['./overview.css'],
})
export class AdminOverview implements OnInit {
  bookings = [] as any;
  totalRevenue = 0;

  constructor( ) {}

  ngOnInit(): void {
    // this.bookingService.getBookings().subscribe((bs) => {
    //   this.bookings = bs;
    //   this.totalRevenue = bs.reduce((s: number, b: any) => s + (b.totalPrice || 0), 0);
    // });
  }
}
