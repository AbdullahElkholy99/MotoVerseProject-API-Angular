import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-modal-overlay',
  imports: [CommonModule],
  templateUrl: './modal-overlay.html',
  styleUrl: './modal-overlay.css',
})
export class ModalOverlay {

  @Input() isOpen: boolean = false;

  @Input() width: string = '500px';

  @Output() close = new EventEmitter<void>();


  closeModal() {
    this.close.emit();
  }

  stopPropagation(event: Event) {
    event.stopPropagation();
  }
  
}
