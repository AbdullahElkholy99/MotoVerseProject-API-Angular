import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'admin-payments',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './payments.html',
  styleUrls: ['./payments.css']
})
export class AdminPayments implements OnInit {
  payments: any[] = [];

  constructor() {}

  ngOnInit(): void {
    // this.bookingService.getBookings().subscribe((bs) => {
    //   this.payments = bs.filter((b: any) => b.payment).map((b: any) => ({
    //     id: b.payment.transactionId || ('pay-' + b.id),
    //     bookingId: b.id,
    //     customer: 'Customer ' + b.id.slice(-3),
    //     amount: b.payment.amount,
    //     method: b.payment.method,
    //     provider: b.payment.provider,
    //     paidAt: b.payment.date,
    //     status: b.payment ? 'Paid' : 'Pending'
    //   }));
    // });
  }

  refund(p: any) { alert('Refund requested for ' + p.id); }
  verify(p: any) { alert('Verified ' + p.id); }
}
