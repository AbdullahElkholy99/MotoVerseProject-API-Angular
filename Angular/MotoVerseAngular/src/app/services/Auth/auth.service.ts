import { Injectable, signal } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

import { AuthResponse, ResponeResult } from '../../models/Auth/ResponeResult';
import { environment } from '../../environments/environment';
import { AddUserDTO } from '../../models/Auth/addUserDTO';
import { signinDTO } from '../../models/Auth/SigninDTO';
import { Observable } from 'rxjs';
import { getUserRole } from '../../Helpers/decode-jwt';

@Injectable({ providedIn: 'root' })

export class AuthService {
  private readonly storageKey = 'motoverse-auth-user';
  private readonly userSignal = signal<AddUserDTO | null>(this.getStoredUser());
  constructor(
    private router: Router,
    private _httpClient: HttpClient,
  ) { }

  get user() {
    return this.userSignal();
  }

  // --------------- Register
  register(payload: AddUserDTO) {
    const user: AddUserDTO = {
      displayName: payload.displayName,
      email: payload.email,
      address: payload.address,
      phoneNumber: payload.phoneNumber,
      password: payload.password,
      confirmPassword: payload.password,
    };

    this.userSignal.set(user);

    return this._httpClient.post<ResponeResult<string>>(`${environment.baseUrl}/User/Create`, user);
  }

  // ---------------- Login
  login(payload: signinDTO) {
    const formData = new FormData();

    formData.append('email', payload.email);
    formData.append('password', payload.password);
    return this._httpClient.post<ResponeResult<AuthResponse>>(
      `${environment.baseUrl}/Authentication/SignIn`,
      formData,
    );
  }
  // ---------------- logout

  logout():Observable<ResponeResult<string>>{
    return this._httpClient.post<ResponeResult<string>>(
      `${environment.baseUrl}/Authentication/Logout`,
      {},
      { withCredentials: true }
    );
  }
  // ---------------- confirm Email

  confirmEmail():Observable<ResponeResult<string>>{
    return this._httpClient.post<ResponeResult<string>>(
      `${environment.baseUrl}/Authentication/Logout`,
      {},
      { withCredentials: true }
    );
  }




  private getStoredUser(): AddUserDTO | null {
    const raw = localStorage.getItem(this.storageKey);
    return raw ? (JSON.parse(raw) as AddUserDTO) : null;
  }


    redirectAfterLogin() {

    const roles = getUserRole();

    if (roles.includes('Admin')) {
      this.router.navigate(['/admin']);
      return;
    }
    if (roles.includes('Customer')) {
      this.router.navigate(['/home']);
      return;
    }
  }

}
