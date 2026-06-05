import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'mv-status-badge',
  standalone: true,
  imports: [CommonModule],
  template: `
    <span class="badge" [ngClass]="variant">{{ label }}</span>
  `,
  styles: [
    `:host{display:inline-block}
    .badge{padding:6px 8px;border-radius:8px;font-size:12px}
    .pending{background:rgba(240,161,58,0.08);color:var(--primary-color);border:1px solid rgba(240,161,58,0.12)}
    .approved{background:rgba(10,132,255,0.08);color:#0a84ff}
    .active{background:rgba(52,199,89,0.08);color:#34c759}
    .completed{background:rgba(64,255,128,0.06);color:#00c853}
    .cancelled{background:rgba(255,59,48,0.06);color:#ff3b30}
    .failed{background:rgba(255,59,48,0.06);color:#ff3b30}
    .refunded{background:rgba(153,102,255,0.06);color:#b388ff}
    `,
  ],
})
export class StatusBadge {
  @Input() label = '';
  @Input() variant = '';
}
