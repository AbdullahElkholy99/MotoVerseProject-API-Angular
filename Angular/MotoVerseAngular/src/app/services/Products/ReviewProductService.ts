import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { ResponeResult } from '../../models/Auth/ResponeResult';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { GetProductPaginatedListQueryDTO, ProductDTO } from '../../models/Product/ProductDTO';

@Injectable({
  providedIn: 'root',
})
export class ReviewProductService {
  constructor(
    private router: Router,
    private _httpClient: HttpClient,
  ) {}

  baseUrl = `${environment.baseUrl}/ReviewProduct`
  // -------------- Create Product --------------
  create(payload: FormData) {
    return this._httpClient.post<ResponeResult<string>>(
      `${this.baseUrl}/Add`,
      payload,
    );
  }

  // -------------- Get Product Paginated--------------
  getAll(paginated: GetProductPaginatedListQueryDTO): Observable<ResponeResult<ProductDTO[]>> {
    let params = new HttpParams()
      .set('pageNumber', paginated.pageNumber)
      .set('pageSize', paginated.pageSize)
      .set('categoryId', paginated.categoryId)
      .set('orderBy', paginated.orderBy);

    if (paginated.search) {
      params = params.set('search', paginated.search);
    }

    return this._httpClient.get<ResponeResult<ProductDTO[]>>(
      `${this.baseUrl}/paginated`,
      { params },
    );
  }

  getById(id:string): Observable<ResponeResult<ProductDTO>> {
    return this._httpClient.get<ResponeResult<ProductDTO>>(`${this.baseUrl}/get-by-id/${id}`);
  }
    // -------------- Edit Product --------------
  edit(payload: FormData) {
    return this._httpClient.put<ResponeResult<string>>(`${this.baseUrl}/edit`,payload);
  }

  // -------------- Delete Product --------------
  delete(id: string): Observable<ResponeResult<string>> {
    return this._httpClient.delete<ResponeResult<string>>(
      `${this.baseUrl}/delete/${id}`
    );
  }


}
