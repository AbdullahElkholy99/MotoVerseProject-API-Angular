import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { ResponeResult } from '../models/Auth/ResponeResult';
import { environment } from '../environments/environment';
import { GetCategoryDTO, GetCategoryPaginatedListQueryDTO } from '../models/Category/getCategory';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CategoryService {
  constructor(
    private router: Router,
    private _httpClient: HttpClient,
  ) {}

  // -------------- Create Category --------------
  create(payload: FormData) {
    return this._httpClient.post<ResponeResult<string>>(
      `${environment.baseUrl}/Category/Add`,
      payload,
    );
  }

  // -------------- Get Category PAginated--------------
  getAllPaginated(paginated: GetCategoryPaginatedListQueryDTO): Observable<ResponeResult<GetCategoryDTO[]>> {
    let params = new HttpParams()
      .set('pageNumber', paginated.pageNumber)
      .set('pageSize', paginated.pageSize)
      .set('orderBy', paginated.orderBy);

    if (paginated.search) {
      params = params.set('search', paginated.search);
    }

    return this._httpClient.get<ResponeResult<GetCategoryDTO[]>>(
      `${environment.baseUrl}/Category/paginated`,
      { params },
    );
  }
 getAll(): Observable<ResponeResult<GetCategoryDTO[]>> {
    return this._httpClient.get<ResponeResult<GetCategoryDTO[]>>(`${environment.baseUrl}/Category/get-all`    );
  }
  getById(id:string): Observable<ResponeResult<GetCategoryDTO>> {
    return this._httpClient.get<ResponeResult<GetCategoryDTO>>(`${environment.baseUrl}/Category/get-by-id/${id}`);
  }
    // -------------- Edit Category --------------
  edit(payload: FormData) {
    return this._httpClient.put<ResponeResult<string>>(`${environment.baseUrl}/Category/edit`,payload);
  }

  // -------------- Delete Category --------------
  delete(id: string): Observable<ResponeResult<string>> {
    return this._httpClient.delete<ResponeResult<string>>(
      `${environment.baseUrl}/Category/delete/${id}`
    );
  }


}
