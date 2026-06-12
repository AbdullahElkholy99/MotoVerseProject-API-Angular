import { HttpClient } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { Router } from '@angular/router';
import { ResponeResult } from '../../models/Auth/ResponeResult';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { UserInfoDTO } from '../../models/Users/user-info';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  constructor(
    private router: Router,
    private _httpClient: HttpClient,
  ) { }

  baseUrl = environment.baseUrl + '/User'

  getInfo(): Observable<ResponeResult<UserInfoDTO>> {
    return this._httpClient.get<ResponeResult<UserInfoDTO>>(
      `${this.baseUrl}/get-info`,
      { withCredentials: true }
    );
  }

  changeInfo(name: string, email: string, phoneNumber: string): Observable<ResponeResult<string>> {
    const formData = new FormData();

    formData.append('name', name);
    formData.append('email', email);
    formData.append('phoneNumber', phoneNumber);

    return this._httpClient.put<ResponeResult<string>>(
      `${this.baseUrl}/update-info`, formData
    );
  }


}
