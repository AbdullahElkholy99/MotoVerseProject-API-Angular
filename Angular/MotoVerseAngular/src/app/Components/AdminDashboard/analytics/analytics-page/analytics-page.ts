import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MockDataService } from '../../OrderPages/mock-data.service';

@Component({
  selector: 'app-analytics-page',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './analytics-page.html',
  styleUrls: ['./analytics-page.css'],
})
export class AnalyticsPage {
  constructor(public readonly data: MockDataService) {}
}
