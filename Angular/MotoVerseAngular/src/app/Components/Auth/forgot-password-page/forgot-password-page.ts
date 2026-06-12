import { CommonModule } from '@angular/common';
import { Component, inject, OnDestroy, signal } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
  ValidationErrors,
  AbstractControl,
  ValidatorFn
} from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ManagePasswordService } from '../../../services/Auth/ManagePassword.service';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './forgot-password-page.html',
  styleUrls: ['./forgot-password-page.css'],
})
export class ForgotPasswordPage implements OnDestroy {

  private toastr = inject(ToastrService)
  private _forgotPasswordService = inject(ManagePasswordService)

  currentStep = signal(1);

  emailForm: FormGroup;
  codeForm: FormGroup;
  passwordForm: FormGroup;

secondsRemaining = signal(300);
  canResend = false;

  private timerId: any;

  constructor(private fb: FormBuilder, private router: Router) {
    this.emailForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
    });

    this.codeForm = this.fb.group({
      code: ['', [Validators.required, Validators.minLength(6)]],
    });

    this.passwordForm = this.fb.group(
      {
        password: [
          '',
          [
            Validators.required,
            Validators.minLength(8)
          ]
        ],
        confirmPassword: [
          '',
          Validators.required
        ]
      },
      {
        validators: passwordMatchValidator
      }
    );
  }

  get email(): string {
    return this.emailForm.get('email')?.value;
  }
  //  abdullah.ali.elkholy@gmail.com
  sendCode() {

    if (this.emailForm.invalid) {
      this.emailForm.markAllAsTouched();
      return;
    }
    console.log("this.emailForm.get('email')?.value : ", this.emailForm.get('email')?.value);


    // API Call

    this._forgotPasswordService.sendCode(this.emailForm.get('email')?.value).subscribe({
      next: (result) => {
        if (!result.succeeded) {
          this.toastr.error(result.message, 'Login Failed');
          return;
        }

        this.currentStep.set(2);
        this.startTimer();
        this.toastr.success(result.message, 'Success');
      },

      error: (error) => {

        this.toastr.error(
          error?.error?.message || 'Something went wrong',
          'Error'
        );
      },
    });

  }

  verifyCode() {
    if (this.codeForm.invalid) {
      this.codeForm.markAllAsTouched();
      return;
    }

    // Verify Code API
    const code = this.codeForm.get('code')?.value
    const email = this.emailForm.get('email')?.value

    this._forgotPasswordService.verifyCode(email, code).subscribe({
      next: (result) => {
        if (!result.succeeded) {
          this.toastr.error(result.message, 'Login Failed');
          return;
        }
        this.currentStep.set(3);
        this.toastr.success(result.message, 'Success');
      },

      error: (error) => {

        this.toastr.error(
          error?.error?.message || 'Something went wrong',
          'Error'
        );
      },
    });
  }

  resetPassword() {
    if (this.passwordForm.invalid) {
      this.passwordForm.markAllAsTouched();
      return;
    }

    // Reset Password API

    const email = this.emailForm.get('email')?.value
    const password = this.passwordForm.get('password')?.value
    const confirmPassword = this.passwordForm.get('confirmPassword')?.value

    this._forgotPasswordService.resetPassword(email, password, confirmPassword).subscribe({
      next: (result) => {
        if (!result.succeeded) {
          this.toastr.error(result.message, 'Login Failed');
          return;
        }
        this.router.navigate(['/login']); this.toastr.success(result.message, 'Success');
      },

      error: (error) => {

        this.toastr.error(
          error?.error?.message || 'Something went wrong',
          'Error'
        );
      },
    });
  }

  resendCode() {
    if (!this.canResend) return;

    // Resend API
    // Reset Password API

    const email = this.emailForm.get('email')?.value
    const password = this.passwordForm.get('password')?.value
    const confirmPassword = this.passwordForm.get('confirmPassword')?.value

    this._forgotPasswordService.resetPassword(email, password, confirmPassword).subscribe({
      next: (result) => {
        if (!result.succeeded) {
          this.toastr.error(result.message, 'Login Failed');
          return;
        }
        this.secondsRemaining.set(300);
        this.canResend = false;
      },

      error: (error) => {

        this.toastr.error(
          error?.error?.message || 'Something went wrong',
          'Error'
        );
      },
    });


    this.startTimer();
  }

 startTimer() {
  clearInterval(this.timerId);

  this.timerId = setInterval(() => {
    if (this.secondsRemaining() > 0) {
      this.secondsRemaining.update(v => v - 1);
    } else {
      this.canResend = true;
      clearInterval(this.timerId);
    }
  }, 1000);
}

  get formattedTime(): string {
    const minutes = Math.floor(this.secondsRemaining() / 60);
    const seconds = this.secondsRemaining() % 60;

    return `${minutes.toString().padStart(2, '0')}:${seconds
      .toString()
      .padStart(2, '0')}`;
  }

  ngOnDestroy(): void {
    clearInterval(this.timerId);
  }
}

const passwordMatchValidator: ValidatorFn = (
  control: AbstractControl
): ValidationErrors | null => {

  const password = control.get('password')?.value;
  const confirmPassword = control.get('confirmPassword')?.value;

  return password === confirmPassword
    ? null
    : { passwordsMismatch: true };
};
