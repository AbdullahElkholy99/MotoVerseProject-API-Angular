import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { ResponeResult } from '../../models/Auth/ResponeResult';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { GetProductPaginatedListQueryDTO, ProductDTO, ProductRecommendationsDTO } from '../../models/Product/ProductDTO';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  constructor(
    private router: Router,
    private _httpClient: HttpClient,
  ) {}

  // -------------- Create Product --------------
  create(payload: FormData) {
    return this._httpClient.post<ResponeResult<string>>(
      `${environment.baseUrl}/Product/Add`,
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
      `${environment.baseUrl}/Product/paginated`,
      { params },
    );
  }

  getById(id:string): Observable<ResponeResult<ProductDTO>> {
    return this._httpClient.get<ResponeResult<ProductDTO>>(`${environment.baseUrl}/Product/get-by-id/${id}`);
  }
  getRecommendations(id:string): Observable<ResponeResult<ProductRecommendationsDTO[]>> {
    return this._httpClient.get<ResponeResult<ProductRecommendationsDTO[]>>(`${environment.baseUrl}/Product/get-recommendations/${id}`);
  }
    // -------------- Edit Product --------------
  edit(payload: FormData) {
    return this._httpClient.put<ResponeResult<string>>(`${environment.baseUrl}/Product/edit`,payload);
  }

  // -------------- Delete Product --------------
  delete(id: string): Observable<ResponeResult<string>> {
    return this._httpClient.delete<ResponeResult<string>>(
      `${environment.baseUrl}/Product/delete/${id}`
    );
  }


}
