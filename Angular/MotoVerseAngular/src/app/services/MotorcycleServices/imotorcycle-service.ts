import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { ResponeResult } from '../../models/Auth/ResponeResult';
import {  MotorcycleDTO, MotorcyclePaginatedQueryDTO } from '../../models/MotorcycleDTO/motorcycle.interface';

@Injectable({
  providedIn: 'root',
})
export class IMotorcycleService {

   constructor(
    private router: Router,
    private _httpClient: HttpClient,
  ) {}

  baseUrl = environment.baseUrl +'/Motorcycle'
  // -------------- Create Motorcycle --------------
  create(payload: FormData) {
    return this._httpClient.post<ResponeResult<string>>(
      `${this.baseUrl}/Add`,
      payload,
    );
  }

  // -------------- Get Motorcycle PAginated--------------
getPaginated(
  paginated: MotorcyclePaginatedQueryDTO
): Observable<ResponeResult<MotorcycleDTO[]>> {

  let params = new HttpParams()
    .set('pageNumber', String(paginated.pageNumber))
    .set('pageSize', String(paginated.pageSize));

    if (paginated.status) params = params.set('search', paginated.status);
    if (paginated.search) params = params.set('search', paginated.search);
    if (paginated.brand) params = params.set('brand', paginated.brand);
    if (paginated.model) params = params.set('search', paginated.model);
    if (paginated.minPrice) params = params.set('minPrice', paginated.minPrice);
    if (paginated.maxPrice) params = params.set('maxPrice', paginated.maxPrice);


  return this._httpClient.get<ResponeResult<MotorcycleDTO[]>>(
    `${this.baseUrl}/paginated`,
    { params }
  );
}
 getAll(): Observable<ResponeResult<MotorcycleDTO[]>> {
    return this._httpClient.get<ResponeResult<MotorcycleDTO[]>>(`${this.baseUrl}/get-all`    );
  }
  getById(id:string): Observable<ResponeResult<MotorcycleDTO>> {
    return this._httpClient.get<ResponeResult<MotorcycleDTO>>(`${this.baseUrl}/get-by-id/${id}`);
  }
    // -------------- Edit Motorcycle --------------
  edit(payload: FormData) {
    return this._httpClient.put<ResponeResult<string>>(`${this.baseUrl}/edit`,payload);
  }

  // -------------- Delete Motorcycle --------------
  delete(id: string): Observable<ResponeResult<string>> {
    return this._httpClient.delete<ResponeResult<string>>(`${this.baseUrl}/delete/${id}`);
  }



}
