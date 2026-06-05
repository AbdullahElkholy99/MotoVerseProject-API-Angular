import { Component, signal, computed, OnInit, inject } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

import { Pagination } from "../../../Shared/pagination/pagination";
import { NoContent } from "../../../Shared/no-content/no-content";
import { MotorcycleCard } from "../motorcycle-card/motorcycle-card";
import { MotorcycleFilter } from "../motorcycle-filter/motorcycle-filter";
import { IMotorcycleService } from '../../../../services/MotorcycleServices/imotorcycle-service';
import { MotorcycleStatus } from '../../../../Enums/motorcycle-status.enum';
import { MotorcycleDTO, MotorcyclePaginatedQueryDTO } from '../../../../models/MotorcycleDTO/motorcycle.interface';
import { DetailsMotorcycle } from '../../../AdminDashboard/MotorcyclePages/details-motorcycle/details-motorcycle';

@Component({
  selector: 'app-motorcycle',
  imports: [Pagination, NoContent, MotorcycleCard, MotorcycleFilter, DetailsMotorcycle],
  templateUrl: './motorcycle.html',
  styleUrls: ['./motorcycle.css', '../../../styleBoke.css'],

})
export class Motorcycle implements OnInit {

  constructor(private toastr: ToastrService) {
  }

  ngOnInit(): void {
    this.loadMotorcycles();
  }
  // inject service
  _motorcycleService = inject(IMotorcycleService)

  //MotorcycleStatus
  motorcyleStatus = MotorcycleStatus
  statusOptions = Object.values(MotorcycleStatus);


  selectedMotorcycleId = signal<string>('');

  // pagination
  currentPage = 1;
  itemsPerPage = 6;
  totalItems = signal(0);

  //MotorcycleDTO
  motorcycles = signal<MotorcycleDTO[]>([]);

  //Filter
  searchTerm = '';
  selectedBrand = signal('');
  selectedModel = signal('');
  selectedStatus = signal<MotorcycleStatus | ''>('');
  minPrice = signal(0);
  maxPrice = signal(0);


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

  // ---------------  Pagination ---------------
  pageChanged(pageNumber: number) {
    this.currentPage = pageNumber;
    this.loadMotorcycles(this.currentPage)
  }






  // ---------------  openDetailsModal ---------------
  showDetailsModal = signal(false);
  openDetailsModal(id: string) {
    this.selectedMotorcycleId.set(id);
    this.showDetailsModal.set(true);
  }
  closeDetailsModal() {
    this.showDetailsModal.set(false);
    this.selectedMotorcycleId.set('');
  }



  openAddToCartModal(id: string) {

  }
}
