import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-metric-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './metric-card.html',
  styleUrls: ['./metric-card.css'],
})
export class MetricCard {
  @Input() icon = 'ri-bar-chart-2-line';
  @Input() label = '';
  @Input() value: string | number = '';
  @Input() change = '';
  @Input() accent = 'primary';
}
