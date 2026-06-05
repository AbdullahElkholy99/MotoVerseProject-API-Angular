import { Component, effect, inject, input, OnInit, signal } from '@angular/core';
import { CustomerService } from '../../../../../services/Users/CustomerService';
import { ToastrService } from 'ngx-toastr';
import { GetCustomerByIdResponse } from '../../../../../models/Users/customer.model';

@Component({
  selector: 'app-customer-details',
  imports: [],
  templateUrl: './customer-details.html',
  styleUrl: './customer-details.css',
})
export class CustomerDetails   {
  // inject services
  private _customerService = inject(CustomerService)
  private _toastr = inject(ToastrService)



  selectedUser = signal<GetCustomerByIdResponse | null>(null);
  showDetailsModal = signal(false);

customerId = input.required<string>();

constructor() {
  effect(() => {
    const id = this.customerId();

    if (id) {
      this.viewCustomer(id);
    }
  });
}
  viewCustomer(id: string) {
    if (id === '') return;
    console.log(" ------------------- view : ",id)
    this._customerService.getById(id).subscribe({
      next: (res) => {
        if (!res.succeeded) {
          this._toastr.error(res.errorsBag, 'Error');
          return;
        }

        this.selectedUser.set(res.data);
        this.showDetailsModal.set(true);
      },
      error: () => {
        this._toastr.error('Failed to load customer details');
      }
    });
  }

}
