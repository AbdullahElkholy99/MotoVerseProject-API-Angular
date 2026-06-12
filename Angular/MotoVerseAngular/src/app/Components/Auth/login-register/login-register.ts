import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { environment } from '../../../environments/environment';
import { ToastrService } from 'ngx-toastr';
import { Router, RouterLink } from '@angular/router';
import { RegisterPage } from "../register-page/register-page";
import { AuthService } from '../../../services/Auth/auth.service';
import { getUserInfo, getUserRole } from '../../../Helpers/decode-jwt';

@Component({
  selector: 'app-login-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RegisterPage, RouterLink],
  templateUrl: './login-register.html',
  styleUrl: './login-register.css',
})
export class LoginRegister {

  private toastr = inject(ToastrService)
  private authService = inject(AuthService)
  isRegisterMode = signal(false);


  form: FormGroup;
  error = '';
  message = '';
  isSubmitting = signal<boolean>(false);
  showPassword = false;


  constructor(private fb: FormBuilder, private router: Router) {
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8)]],
    });
  }

  showRegister() {
    this.isRegisterMode.set(true);
  }

  showLogin() {
    this.isRegisterMode.set(false);
  }

  // login
  togglePassword() {
    this.showPassword = !this.showPassword;
  }
  submit(): void {
    this.isSubmitting.set(true);
    this.error = '';
    this.message = '';

    if (this.form.invalid) {
      this.isSubmitting.set(false);
      this.form.markAllAsTouched();
      return;
    }

    const { email, password } = this.form.value;

    this.authService.login({ email, password }).subscribe({
      next: (result) => {
        if (!result.succeeded) {
          this.toastr.error(result.message, 'Login Failed');
          this.isSubmitting.set(false);
          return;
        }
        this.toastr.success(result.message, 'Success');

        const token = result.data.accessToken;

        environment.token = token;
        localStorage.setItem('token', token);

        this.isSubmitting.set(false);
        this.authService.redirectAfterLogin();
      },

      error: (error) => {
        this.isSubmitting.set(false);

        this.toastr.error(
          error?.error?.message ||
          'Something went wrong',
          'Error'
        );
      },
    });
  }


  //register
}
