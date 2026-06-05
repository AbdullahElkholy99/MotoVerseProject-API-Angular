import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MockDataService } from '../../../OrderPages/mock-data.service';
import { CustomerService } from '../../../../../services/Users/CustomerService';
import { CustomerDTO, GetCustomerPaginatedListQueryDTO } from '../../../../../models/Users/customer.model';
import { ToastrService } from 'ngx-toastr';
import { NoContent } from "../../../../Shared/no-content/no-content";
import { Pagination } from "../../../../Shared/pagination/pagination";
import { ModalOverlay } from "../../../../Shared/modal-overlay/modal-overlay";
import { CustomerDetails } from "../customer-details/customer-details";
import { ModalDelete } from "../../../../Shared/modal-delete/modal-delete";

@Component({
  selector: 'app-customers-page',
  standalone: true,
  imports: [CommonModule, NoContent, Pagination, ModalOverlay, CustomerDetails, ModalDelete],
  templateUrl: './customers-page.html',
  styleUrls: ['./customers-page.css'],
})
export class CustomersPage implements OnInit {

  // inject services
  private _customerService = inject(CustomerService)
  private _toastr = inject(ToastrService)

  //fields
  customers = signal<CustomerDTO[]>([]);

  totalItems = signal(0)
  currentPage = signal(1)
  pageSize = signal(10)

  selectedCustomerId = signal<string | null>(null);
  selectedDeletedCustomer = signal<CustomerDTO | null>(null);
  showDetailsModal = computed(() => !!this.selectedCustomerId());

  toggleDeleteModal = signal<boolean>(false)

  searchTerm = signal('');
  //CTOR
  constructor(public readonly data: MockDataService) { }

  ngOnInit(): void {
    this.getAll()
  }

  getAll(page: number = this.currentPage()) {

    const pagedDto: GetCustomerPaginatedListQueryDTO = {
      pageNumber: page,
      pageSize: this.pageSize(),
      orderBy: 0,
      search: this.searchTerm()
    }

    this._customerService.getAllPaginated(pagedDto).subscribe({
      next: (res) => {
        if (!res.succeeded) {
          this._toastr.error(res.messages, "error")
          return;
        }

        this._toastr.success(res.messages, "success")
        this.customers.set(res.data || [])
        this.totalItems.set(res.totalCount || 0)
        this.currentPage.set(res.currentPage || 0)
        this.pageSize.set(res.pageSize || 10)
      },
      error: (err) => {
        this._toastr.error(err.messages, "error")
      }


    }) //end of call api

  }

  // -------------------- View Details
  viewCustomer(id: string) {
    this.selectedCustomerId.set(id)
  }
  closeModal() {
    this.selectedCustomerId.set(null);
  }
  // -------------------- Delete\
  showDeleteModal(customer: CustomerDTO) {
    this.toggleDeleteModal.set(true);
    this.selectedDeletedCustomer.set(customer)
    console.log('Delete Customer:', customer.id);
  }
  confirmDelete() {
    this._customerService.delete(this.selectedDeletedCustomer()?.id!).subscribe({
      next: (res) => {
        if (!res.succeeded) {
          this._toastr.error(res.errors, "error")
          return;
        }

        this._toastr.success(res.message, "success")
        this.customers.update(customers =>
          customers.map(customer =>
            customer.id === this.selectedDeletedCustomer()?.id
              ? { ...customer, isActive: !customer.isActive }
              : customer
          )
        );
        this.closeDeleteModal()
      },
      error: (err) => {
        this._toastr.error(err.messages, "error")
      }
    }) //end of call api
  }
  closeDeleteModal() {
    this.toggleDeleteModal.set(false);
  }



  editCustomer(id: string) {
    console.log('Edit Customer:', id);
  }

  //  -------------------- pagination
  onSearch() {
    this.currentPage.set(1);
    this.getAll();
  }
  onPageChange(page: any) {
    console.log("------------- page ", page)
    this.currentPage.set(page);
    this.getAll(this.currentPage());
  }

  get isActive(){
    return this.selectedDeletedCustomer()?.isActive;
  }
}
