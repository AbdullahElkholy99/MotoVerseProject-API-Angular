import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators,
  AbstractControl,
  ValidationErrors,
  ValidatorFn,
  FormGroup,
} from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AddUserDTO } from '../../../models/Auth/addUserDTO';
import { AuthService } from '../../../services/Auth/auth.service';
const passwordMatchValidator: ValidatorFn = (control: AbstractControl): ValidationErrors | null => {
  const password = control.get('password')?.value;
  const confirmPassword = control.get('confirmPassword')?.value;
  return password !== confirmPassword ? { passwordsMismatch: true } : null;
};

@Component({
  selector: 'app-register-page',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './register-page.html',
  styleUrls: ['./register-page.css', '../../../../../public/assets/css/button.css'],
})
export class RegisterPage {
  // ---------- Fields
  currentStep = 0;
  totalSteps = 3;

  error = '';
  message = '';

  step1!: FormGroup;
  step2!: FormGroup;
  step3!: FormGroup;

  isSubmitting = false;
  // ---------- constructor
  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private toastr: ToastrService,
  ) {
    this.step1 = this.fb.group({
      displayName: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
    });

    this.step2 = this.fb.group({
      address: ['', Validators.required],
      phoneNumber: ['', [Validators.required,
        Validators.maxLength(11),
         Validators.pattern(/^\+?\d{8,12}$/)]],
    });

    this.step3 = this.fb.group(
      {
        password: [
          '',
          [
            Validators.required,
            Validators.minLength(8),
            Validators.pattern(
              /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z\d]).{8,}$/
            ),
          ],
        ],
        confirmPassword: ['', Validators.required],
      },
      { validators: passwordMatchValidator },
    );
  }

  // ---------- currentGroup
  get currentGroup(): FormGroup {
    return [this.step1, this.step2, this.step3][this.currentStep];
  }

  // ---------- getters for validation states for password mismatch and last step
  get passwordsMismatch(): boolean {
    return !!(
      this.step3.hasError('passwordsMismatch') && this.step3.get('confirmPassword')?.touched
    );
  }

  // ---------- isLastStep
  get isLastStep(): boolean {
    return this.currentStep === this.totalSteps - 1;
  }

  // --------- navigation methods
  next(): void {
    if (this.currentGroup.invalid) {
      this.currentGroup.markAllAsTouched();
      return;
    }
    this.currentStep++;
  }

  // --------- prev method
  prev(): void {
    if (this.currentStep > 0) this.currentStep--;
  }

  // --------- submit method
  submit(): void {
    this.isSubmitting = true;
    this.error = '';
    this.message = '';

    if (this.step3.invalid) {
      this.step3.markAllAsTouched();

      this.showStep3Errors();
      this.isSubmitting = false;

      return;
    }


    const payload: AddUserDTO = {
      ...this.step1.value,
      ...this.step2.value,
      ...this.step3.value,
    };


    this.authService.register(payload).subscribe({
      next: (result) => {
        console.log(result);

        if (!result.succeeded) {
          this.toastr.error(result.message, 'Registration Failed');
          this.isSubmitting = false;
          return;
        }
        this.isSubmitting = false;

        this.toastr.success(result.message, 'Success');
        // this.router.navigate(['/login']);

        this.router.navigate(
          ['/confirm-email'],
          {
            queryParams: {
              email: payload.email
            }
          }
        );

      },

      error: (error) => {
        console.error(error);
        this.isSubmitting = false;
        this.toastr.error(error?.error?.message || 'Something went wrong', 'Error');
      },
    });
  }

  private showStep3Errors(): void {
    const errors: string[] = [];

    const password = this.step3.get('password');
    const confirmPassword = this.step3.get('confirmPassword');

    if (password?.hasError('required')) {
      errors.push('Password is required');
    }

    if (password?.hasError('minlength')) {
      errors.push('Password must be at least 8 characters');
    }

    if (password?.hasError('pattern')) {
      errors.push('Password must contain:');
      errors.push('• One uppercase letter');
      errors.push('• One lowercase letter');
      errors.push('• One number');
      errors.push('• One special character');
    }

    if (confirmPassword?.hasError('required')) {
      errors.push('Confirm password is required');
    }

    if (this.step3.hasError('passwordsMismatch')) {
      errors.push('Passwords do not match');
    }

    this.toastr.error(
      errors.join('<br>'),
      'Validation Error',
      {
        enableHtml: true,
        timeOut: 5000,
        closeButton: true
      }
    );
  }
}
