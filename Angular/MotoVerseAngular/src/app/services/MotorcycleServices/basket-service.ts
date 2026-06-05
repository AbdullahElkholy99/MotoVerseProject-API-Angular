import { HttpClient, HttpParams } from '@angular/common/http';
import {v4 as uuidv4} from "uuid"
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, map, Observable, tap } from 'rxjs';

import { environment } from '../../environments/environment';
import { ResponeResult } from '../../models/Auth/ResponeResult';
import { MotorcycleDTO, MotorcyclePaginatedQueryDTO } from '../../models/MotorcycleDTO/motorcycle.interface';
import { IBasket } from '../../models/MotorcycleDTO/basket';

@Injectable({
  providedIn: 'root',
})
export class IBasketService {

  constructor(
    private router: Router,
    private _httpClient: HttpClient,
  ) { }

  baseUrl = environment.baseUrl + '/Basket'

private basketSource = new BehaviorSubject<IBasket>({} as IBasket);

  basket = this.basketSource.asObservable()

  getBasketId(): string {

    let basketId = localStorage.getItem('basket_id');

    if (!basketId) {
      basketId = uuidv4();
      localStorage.setItem('basket_id', basketId);
    }

    return basketId;
  }


  addItemToBasket(){

  }

  // -------------- Get Motorcycle PAginated--------------

  getBasket(id: string) {
    return this._httpClient
      .get<IBasket>(`${this.baseUrl}/get-by-id/${id}`)
      .pipe(
        tap((basket) => {
          this.basketSource.next(basket);
        })
      );
  }
  getById(id: string): Observable<ResponeResult<IBasket>> {
    return this._httpClient.get<ResponeResult<IBasket>>(`${this.baseUrl}/get-by-id/${id}`);
  }
  // -------------- Edit Motorcycle --------------
  edit(basketDto: IBasket) {
    return this._httpClient.put<ResponeResult<IBasket>>(`${this.baseUrl}/edit`, basketDto);
  }

  // -------------- Delete Motorcycle --------------
  delete(id: string): Observable<ResponeResult<boolean>> {
    return this._httpClient.delete<ResponeResult<boolean>>(`${this.baseUrl}/delete/${id}`);
  }
  deleteItem(basketId: string, productId: string) {
    return this._httpClient.delete<ResponeResult<IBasket>>(
      `${this.baseUrl}/deleteItem`,
      {
        params: {
          basketId,
          productId
        }
      }
    );
  }

  // ---------------------------------
  currentValue(){
    return this.basketSource.value;
  }

}
