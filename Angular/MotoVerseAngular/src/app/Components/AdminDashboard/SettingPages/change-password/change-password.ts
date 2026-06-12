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
  ValidationErrors , FormsModule
} from '@angular/forms';
import { ManagePasswordService } from '../../../../services/Auth/ManagePassword.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-change-password',
  imports: [CommonModule, FormsModule, ReactiveFormsModule, NgIf],
  templateUrl: './change-password.html',
   styleUrls: ['./change-password.css','../settings-page/settings-page.css'],

})
export class ChangePassword implements OnInit {

  _ManagePasswordService =inject(ManagePasswordService)
  _toaster =inject(ToastrService)

  changePasswordForm!: FormGroup;


  buttonChangePasswordLabel =signal('Update password')

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    this.buildChangePasswordForm();
  }

  buildChangePasswordForm(): void {
    this.changePasswordForm = this.fb.group({
        current: ['',
          Validators.required,
          Validators.minLength(8),
        ],
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
      });


   }

  profile = {
    name: 'E-Shop Admin',
    email: 'admin@eshop.com',
    phone: '+1 555-123-4567',
  };


  notifications = {
    email: true,
    sms: false,
    updates: true,
  };

  theme = 'Dark';


  onSubmitChangePassword(){
    this.buttonChangePasswordLabel.set('Updating ...')
    const current = this.changePasswordForm.get('current')?.value
    const password = this.changePasswordForm.get('password')?.value
    const confirmPassword = this.changePasswordForm.get('confirmPassword')?.value

   console.log(current);
   console.log(password);
   console.log(confirmPassword);



    this._ManagePasswordService.changePassword(current,password,confirmPassword).subscribe({
      next:(res)=>{
        if(!res.succeeded){
          this._toaster.error(res.errors,'error')
          return ;
        }
        this._toaster.success(res.data,'success' )
        this.changePasswordForm.reset()
      },
      error:(err)=>{
          this._toaster.error(err.errors,'error')
      },
      complete:()=>{
          this.buttonChangePasswordLabel.set('Update password')
      }
    })
  }


}
