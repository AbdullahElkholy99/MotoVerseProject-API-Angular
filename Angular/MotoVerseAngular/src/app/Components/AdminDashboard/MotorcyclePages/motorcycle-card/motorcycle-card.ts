import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output, signal } from '@angular/core';
import { MotorcycleDTO } from '../../../../models/MotorcycleDTO/motorcycle.interface';
import { UiButton } from "../../../Shared/ui-button/ui-button";

@Component({
  selector: 'app-motorcycle-card',
  standalone: true,
  imports: [CommonModule, UiButton],
  templateUrl: './motorcycle-card.html',
  styleUrls: ['./motorcycle-card.css'],
})
export class MotorcycleCard {
  @Input({required:true}) motorcycle!: MotorcycleDTO;
  @Output() viewDetails = new EventEmitter<string>();
  @Output() bookNow = new EventEmitter<string>();
  @Output() toggleFavorite = new EventEmitter<string>();
  @Output() onDelete = new EventEmitter<string>();
  @Output() onUpdate = new EventEmitter<string>();
@Output() onDetails = new EventEmitter<string>();

  showAddModal = false;
  showDeleteModal = false;
  showUpdateModal = false;

  emitDelete(id:string): void {
    this.onDelete.emit(id);
  }
  emitUpdate(id:string): void {
    this.onUpdate.emit(id);
  }
  emitViewDetails(): void {
    this.viewDetails.emit(this.motorcycle.id);
  }

  emitBookNow(): void {
    this.bookNow.emit(this.motorcycle.id);
  }

  emitToggleFavorite(): void {
    this.toggleFavorite.emit(this.motorcycle.id);
  }


   statusMap: Record<number, string> = {
    1: 'Available',
    2: 'Rented',
    3: 'Maintenance',
  };




emitDetails(id:string) {

  this.onDetails.emit(id);
}




}
