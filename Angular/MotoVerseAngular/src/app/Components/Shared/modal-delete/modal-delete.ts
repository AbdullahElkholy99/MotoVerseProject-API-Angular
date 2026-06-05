import { CommonModule, NgIf } from '@angular/common';
import { Component, EventEmitter, inject, Input, Output } from '@angular/core';

@Component({
  selector: 'app-modal-delete',
  imports: [CommonModule,NgIf],
  templateUrl: './modal-delete.html',
  styleUrl: './modal-delete.css',
})
export class ModalDelete {
  @Input() visible: boolean = false;

  @Input() title: string = 'Delete Item';
  @Input() icon: string = '🗑';
  @Input() labelBtn: string = 'Delete';
  @Input() labelColor: string = 'confirm-btn';
  @Input() iconColor: string = 'icon';

  @Input() message: string = 'Are you sure you want to delete this item?';

  @Output() onConfirm = new EventEmitter<void>();

  @Output() onClose = new EventEmitter<void>();


  confirm() {
    this.onConfirm.emit();
  }

  close() {
    this.onClose.emit();
  }
}
