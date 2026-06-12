import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule, } from '@angular/common';
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
import { ChangePassword } from "../change-password/change-password";
import { AdminProfile } from "../admin-profile/admin-profile";
import { ImageUpload } from "../../../Shared/image-upload/image-upload";
@Component({
  selector: 'app-settings-page',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, ChangePassword, AdminProfile, ImageUpload],
  templateUrl: './settings-page.html',
  styleUrls: ['./settings-page.css'],
})
export class SettingsPage implements OnInit {

  _toaster =inject(ToastrService)

  image = ''
  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
        const userInfo = JSON.parse(localStorage.getItem("userInfo") || '')
    this.image = userInfo[0].imagePath
    this.imagePreview.set(this.image)
   }

  imagePreview = signal<string | ArrayBuffer | null>(null);


  handleFile(file: File) {
    const reader = new FileReader();
    reader.onload = () => {
      this.imagePreview.set(reader.result);
    };

    reader.readAsDataURL(file);
  }



}
