import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MotorcycleStatus } from '../../../../Enums/motorcycle-status.enum';
import { UiButton } from "../../../Shared/ui-button/ui-button";

@Component({
  selector: 'app-motorcycle-filter',
  imports: [CommonModule, FormsModule, UiButton],
  templateUrl: './motorcycle-filter.html',
  styleUrl: './motorcycle-filter.css',
})
export class MotorcycleFilter {
  //Filter
    searchTerm = '';
    selectedBrand = signal('');
    selectedModel = signal('');
    selectedStatus = signal<MotorcycleStatus | ''>('');
    minPrice = signal(0);
    maxPrice = signal(0);

  statusOptions = Object.values(MotorcycleStatus);
  // --------------------------------- statusMap Status
  statusMap: Record<number, string> = {
    1: 'Available',
    2: 'Rented',
    3: 'Maintenance',
  };

      // ----------------------- Filters
  onSearch(): void {
    // this.currentPage = 1;
    // this.loadMotorcycles();
  }


  clearFilters(): void {
    this.searchTerm = '';
    this.selectedBrand.set('');
    this.selectedModel.set('');
    this.selectedStatus.set('');
    this.minPrice.set(0);
    this.maxPrice.set(0);
  }


}
