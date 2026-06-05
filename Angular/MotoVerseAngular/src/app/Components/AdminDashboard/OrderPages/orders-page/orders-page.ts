import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MockDataService } from '../mock-data.service';
import { MetricCard } from '../../dashboard-page/metric-card/metric-card';

@Component({
  selector: 'app-orders-page',
  standalone: true,
  imports: [CommonModule, RouterModule, MetricCard],
  templateUrl: './orders-page.html',
  styleUrls: ['./orders-page.css'],
})
export class OrdersPage {
  constructor(public readonly data: MockDataService) {}
}
