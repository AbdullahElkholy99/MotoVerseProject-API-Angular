import { Injectable, signal } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { ResponeResult } from '../../models/Auth/ResponeResult';


@Injectable({ providedIn: 'root' })

export class ManagePasswordService {


  apiUrl = `${environment.baseUrl}/ManagePassword`;

  constructor(
    private router: Router,
    private _httpClient: HttpClient,
  ) { }

  /**
     * Step 1
     * Send verification code to email
  */
  sendCode(email: string): Observable<any> {

    return this._httpClient.post(
      `${this.apiUrl}/SendResetPasswordCode?email=${email}`,{}
    );
  }

  /**
   * Step 2
   * Verify code
   */
  verifyCode(email: string, code: string): Observable<any> {
    return this._httpClient.get(
      `${this.apiUrl}/ConfirmResetPasswordCode?email=${email}&code=${code}`
    );
  }

  /**
   * Step 3
   * Reset password
   */
  resetPassword(
    email: string,
    password: string,
    confirmPassword: string
  ): Observable<any> {
     const formData = new FormData();

  formData.append('email', email);
  formData.append('password', password);
  formData.append('confirmPassword', confirmPassword);

    return this._httpClient.post(
      `${this.apiUrl}/ResetPassword`,formData
    );
  }

  changePassword(
    current: string,
    password: string,
    confirmPassword: string
  ): Observable<ResponeResult<string>> {
     const formData = new FormData();

  formData.append('CurrentPassword', current);
  formData.append('NewPassword', password);
  formData.append('ConfirmPassword', confirmPassword);

    return this._httpClient.put<ResponeResult<string>>(
      `${this.apiUrl}/change-password`,formData
    );
  }

}
