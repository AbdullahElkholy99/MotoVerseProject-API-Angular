import { Injectable, signal } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';


@Injectable({ providedIn: 'root' })

export class ForgotPasswordService {


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


}
