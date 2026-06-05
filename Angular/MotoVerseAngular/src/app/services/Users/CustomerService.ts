import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { CustomerDTO, GetCustomerByIdResponse, GetCustomerPaginatedListQueryDTO } from '../../models/Users/customer.model';
import { ResponePagedResult, ResponeResult } from '../../models/Auth/ResponeResult';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class CustomerService {
  constructor(
    private router: Router,
    private _httpClient: HttpClient,
  ) { }

  baseUrl = environment.baseUrl + '/User';

  // -------------- Get Pginated--------------
  getAllPaginated(paginated: GetCustomerPaginatedListQueryDTO): Observable<ResponePagedResult<CustomerDTO[]>> {
    let params = new HttpParams()
      .set('pageNumber', paginated.pageNumber)
      .set('pageSize', paginated.pageSize)
      .set('orderBy', paginated.orderBy);

    if (paginated.search) {
      params = params.set('search', paginated.search);
    }

    return this._httpClient.get<ResponePagedResult<CustomerDTO[]>>(
      `${this.baseUrl}/paginated`,
      { params },
    );
  }

  // -------------- Edit  --------------
  getById(id: string): Observable<ResponeResult<GetCustomerByIdResponse>> {
    return this._httpClient.get<ResponeResult<GetCustomerByIdResponse>>(
      `${this.baseUrl}/get-by-id/${id}`
    );
  }
  // -------------- Delete  --------------
  delete(id: string): Observable<ResponeResult<string>> {
    return this._httpClient.delete<ResponeResult<string>>(
      `${this.baseUrl}/delete/${id}`
    );
  }


}
