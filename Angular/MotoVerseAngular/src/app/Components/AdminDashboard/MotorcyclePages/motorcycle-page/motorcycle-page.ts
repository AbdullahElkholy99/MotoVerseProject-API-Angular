import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UiButton } from "../../../Shared/ui-button/ui-button";
import { MotorcyclePaginatedQueryDTO, type MotorcycleDTO } from '../../../../models/MotorcycleDTO/motorcycle.interface';
import { Pagination } from "../../../Shared/pagination/pagination";
import { ToastrService } from 'ngx-toastr';
import { IMotorcycleService } from '../../../../services/MotorcycleServices/imotorcycle-service';
import { NoContent } from "../../../Shared/no-content/no-content";
import { MotorcycleCard } from "../motorcycle-card/motorcycle-card";
import { AddMotorcycle } from "../add-motorcycle/add-motorcycle";
import { MotorcycleStatus } from '../../../../Enums/motorcycle-status.enum';
import { EditMotorcycle } from "../edit-motorcycle/edit-motorcycle";
import { ModalOverlay } from "../../../Shared/modal-overlay/modal-overlay";
import { ModalDelete } from "../../../Shared/modal-delete/modal-delete";
import { DetailsMotorcycle } from "../details-motorcycle/details-motorcycle";

@Component({
  selector: 'app-motorcycle-page',
  standalone: true,
  imports: [CommonModule, FormsModule, UiButton, Pagination, NoContent, MotorcycleCard, AddMotorcycle, EditMotorcycle, ModalOverlay, ModalDelete,  DetailsMotorcycle],
  templateUrl: './motorcycle-page.html',
  styleUrls: ['./motorcycle-page.css'],
})
export class MotorcyclePage implements OnInit {

  constructor(private toastr: ToastrService) {
  }

  ngOnInit(): void {
    this.loadMotorcycles();
  }
  _motorcycleService = inject(IMotorcycleService)

  motorcyleStatus = MotorcycleStatus

  // models toogle
  showAddModal = false;
  showDeleteModal = false;
  showUpdateModal = false;

  selectedMotorcycleId = signal<string>('');

  // pagination
  currentPage = 1;
  itemsPerPage = 10;
  totalItems = signal(0);


  motorcycles = signal<MotorcycleDTO[]>([]);

  //Filter
  searchTerm = '';
  selectedBrand = signal('');
  selectedModel = signal('');
  selectedStatus = signal<MotorcycleStatus | ''>('');
  minPrice = signal(0);
  maxPrice = signal(0);

  statusOptions = Object.values(MotorcycleStatus);

  filteredCount = computed(() => this.motorcycles().length);

  brandOptions = computed(() => {
    const brands = new Set(this.motorcycles().map((motorcycle) => motorcycle.brand));
    return Array.from(brands).sort();
  });

  modelOptions = computed(() => {
    const models = new Set(this.motorcycles().map((motorcycle) => motorcycle.model));
    return Array.from(models).sort();
  });



  // --------------------------------- loadMotorcycles
  private buildPaginatedQuery(): MotorcyclePaginatedQueryDTO {
    return {
      pageNumber: this.currentPage,
      pageSize: this.itemsPerPage,
      search: this.searchTerm || undefined,
      brand: this.selectedBrand() || undefined,
      model: this.selectedModel() || undefined,
      // status: this.selectedStatus(),
      minPrice: this.minPrice() > 0 ? this.minPrice() : undefined,
      maxPrice: this.maxPrice() > 0 ? this.maxPrice() : undefined,
    };
  }

  loadMotorcycles(page: number = 1): void {


    this._motorcycleService.getPaginated(this.buildPaginatedQuery()).subscribe({
      next: (result) => {
        if (!result.succeeded) {
          this.toastr.error(result.message);
          return;
        }
        this.motorcycles.set(result.data);
        this.totalItems.set(result.meta?.totalCount ?? 0);
        this.currentPage = page;
        this.toastr.success(result.message, "success");
      },

      error: (error) => {
        console.error(error);
        this.toastr.error(error?.error?.message || 'Something went wrong');
      }
    });

  }

  // --------------------------------- statusMap Status
  statusMap: Record<number, string> = {
    1: 'Available',
    2: 'Rented',
    3: 'Maintenance',
  };

  // ----------------------- Filters
  onSearch(): void {
    this.currentPage = 1;
    this.loadMotorcycles();
  }


  clearFilters(): void {
    this.searchTerm = '';
    this.selectedBrand.set('');
    this.selectedModel.set('');
    this.selectedStatus.set('');
    this.minPrice.set(0);
    this.maxPrice.set(0);
    this.currentPage = 1;
    this.loadMotorcycles();
  }

  // toggleFavorite(id: string): void {
  //   // this.motorcycles.update((list) =>
  //   //   list.map((motorcycle) =>
  //   //     motorcycle.id === id ? { ...motorcycle, isFavorite: !motorcycle.isFavorite } : motorcycle,
  //   //   ),
  //   // );
  // }

  // viewDetails(id: string): void {
  //   console.log('View details for', id);
  // }

  // bookNow(id: string): void {
  //   console.log('Book now for', id);
  // }


  // ---------------------------------------- Delete
  openDeleteModal(id: string) {
    this.selectedMotorcycleId.set(id);
    this.showDeleteModal = true;
  }
  closeDeleteModal() {
    this.showDeleteModal = false;
  }
  confirmDelete() {
    this._motorcycleService.delete(this.selectedMotorcycleId()).subscribe({
      next: (result) => {
        if (!result.succeeded) {
          this.toastr.error(result.message);
          return;
        }
        this.toastr.success(result.message, "success");
        this.loadMotorcycles(this.currentPage)
        this.showDeleteModal = false;
      },
      error: (error) => {
        console.error(error);
        this.toastr.error(error?.error?.message || 'Something went wrong');
      }
    })
  }

  // ---------------------------------------- Update
  openUpdateModal(id: string) {
    this.selectedMotorcycleId.set(id);
    this.showUpdateModal = true;
  }
  refreshData(success: boolean) {
    if (success) this.loadMotorcycles()
  }

  // ---------------  Pagination ---------------
  pageChanged(pageNumber: number) {
    this.currentPage = pageNumber;
    this.loadMotorcycles(this.currentPage)
  }









showDetailsModal = signal(false);

openDetailsModal(id: string) {

  this.selectedMotorcycleId.set(id);

  this.showDetailsModal.set(true);
}
closeDetailsModal() {

  this.showDetailsModal.set(false);

  this.selectedMotorcycleId.set('');
}

}
