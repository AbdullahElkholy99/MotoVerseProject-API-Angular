import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule, NgIf } from '@angular/common';
import {
  ReactiveFormsModule,   // ← Required for reactive forms
  FormControl,
  FormGroup,
  FormArray,
  FormBuilder,
  Validators,
  AbstractControl,
  ValidationErrors, FormsModule
} from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { UserService } from '../../../../services/applicationUser/user-service';
@Component({
  selector: 'app-admin-profile',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, NgIf],
  templateUrl: './admin-profile.html',

  styleUrls: ['./admin-profile.css', '../settings-page/settings-page.css'],
})

export class AdminProfile implements OnInit {

  _userService = inject(UserService)
  _toaster = inject(ToastrService)

  adminProfileForm!: FormGroup;


  buttonLabel = signal('Update Profile')

  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    this.buildadminProfileForm();

    this.getInfo()
  }

  buildadminProfileForm(): void {
    this.adminProfileForm = this.fb.group({
      name: ['',
        Validators.required,
        Validators.minLength(2),
      ],
      email: [
        '',
        [
          Validators.required,
          Validators.email
        ],
      ],
      phoneNumber: ['', [Validators.required, Validators.pattern(/^\+?\d{8,12}$/)]],
    });
  }

  getInfo() {
    this._userService.getInfo().subscribe({
      next: (res) => {
        if (!res.succeeded) {
          this._toaster.error(res.errors, 'error')
          return;
        }

        this._toaster.success(res.message, 'success')
       this.adminProfileForm.get('email')?.setValue(res.data.email);
       this.adminProfileForm.get('name')?.setValue(res.data.name);
       this.adminProfileForm.get('phoneNumber')?.setValue(res.data.phoneNumber);
      },
      error: (err) => {
        this._toaster.error(err.errors, 'error')
      },
      complete: () => {
        this.buttonLabel.set('Update Profile')
      }
    })
  }

  onSubmit() {
    this.buttonLabel.set('Updating ...')
    const name = this.adminProfileForm.get('name')?.value
    const email = this.adminProfileForm.get('email')?.value
    const phoneNumber = this.adminProfileForm.get('phoneNumber')?.value

    this._userService.changeInfo(name, email, phoneNumber).subscribe({
      next: (res) => {
        if (!res.succeeded) {
          this._toaster.error(res.errors, 'error')
          return;
        }
        this._toaster.success(res.data, 'success')
      },
      error: (err) => {
        this._toaster.error(err.errors, 'error')
      },
      complete: () => {
        this.buttonLabel.set('Update Profile')
      }
    })
  }
}
