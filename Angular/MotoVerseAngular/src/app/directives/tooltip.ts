import {
  Directive,
  ElementRef,
  HostListener,
  Input,
  Renderer2
} from '@angular/core';

@Directive({
  selector: '[appTooltip]',
  standalone: true
})
export class Tooltip {

  @Input() appTooltip = '';

  private tooltip!: HTMLElement;

  constructor(
    private el: ElementRef,
    private renderer: Renderer2
  ) {}

  @HostListener('mouseenter')
  onMouseEnter() {

    if (!this.appTooltip) return;

    this.tooltip = this.renderer.createElement('div');

    this.tooltip.innerText = this.appTooltip;

    this.renderer.appendChild(document.body, this.tooltip);

    this.renderer.setStyle(this.tooltip, 'position', 'fixed');
    this.renderer.setStyle(this.tooltip, 'background', '#111827');
    this.renderer.setStyle(this.tooltip, 'color', '#fff');
    this.renderer.setStyle(this.tooltip, 'padding', '8px 14px');
    this.renderer.setStyle(this.tooltip, 'borderRadius', '10px');
    this.renderer.setStyle(this.tooltip, 'fontSize', '14px');
    this.renderer.setStyle(this.tooltip, 'fontWeight', '500');
    this.renderer.setStyle(this.tooltip, 'zIndex', '999999');
    this.renderer.setStyle(this.tooltip, 'pointerEvents', 'none');
    this.renderer.setStyle(this.tooltip, 'whiteSpace', 'nowrap');
    this.renderer.setStyle(this.tooltip, 'boxShadow', '0 10px 25px rgba(0,0,0,.25)');
    this.renderer.setStyle(this.tooltip, 'opacity', '0');

    const rect = this.el.nativeElement.getBoundingClientRect();

    this.renderer.setStyle(this.tooltip, 'top', `${rect.top + rect.height / 2}px`);
    this.renderer.setStyle(this.tooltip, 'left', `${rect.right + 14}px`);
    this.renderer.setStyle(this.tooltip, 'transform', 'translateY(-50%)');

    requestAnimationFrame(() => {
      this.renderer.setStyle(this.tooltip, 'transition', '.2s ease');
      this.renderer.setStyle(this.tooltip, 'opacity', '1');
    });
  }

  @HostListener('mouseleave')
  onMouseLeave() {

    if (this.tooltip) {
      this.renderer.removeChild(document.body, this.tooltip);
    }
  }
}
