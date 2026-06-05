import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'mv-stat-card',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="stat-card-admin">
      <div class="top">
        <div class="title">{{ title }}</div>
        <div class="delta" *ngIf="delta">{{ delta }}</div>
      </div>
      <div class="value">{{ value }}</div>
      <div class="subtitle" *ngIf="subtitle">{{ subtitle }}</div>
    </div>
  `,
  styles: [
    `:host{display:block}
    .stat-card-admin{background:linear-gradient(180deg, rgba(255,255,255,0.02), rgba(0,0,0,0.02));padding:14px;border-radius:10px;color:#fff}
    .stat-card-admin .title{font-size:13px;color:var(--text-light)}
    .stat-card-admin .value{font-size:22px;font-weight:700;margin-top:6px}
    .stat-card-admin .delta{font-size:12px;color:var(--primary-color)}
    .stat-card-admin .subtitle{font-size:12px;color:var(--text-light);margin-top:6px}
    `,
  ],
})
export class StatCard {
  @Input() title = '';
  @Input() value: string | number = '';
  @Input() subtitle?: string;
  @Input() delta?: string;
}
