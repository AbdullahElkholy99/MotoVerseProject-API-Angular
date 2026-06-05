import { Component, inject, OnInit, Output, EventEmitter, signal } from '@angular/core';

import {
  ReactiveFormsModule,
  FormBuilder,
  FormGroup,
  Validators
} from '@angular/forms';

import { NgIf } from '@angular/common';
import { CategoryService } from '../../../../services/category-service';
import { ToastrService } from 'ngx-toastr';
import { ImageUpload } from '../../../Shared/image-upload/image-upload';
import { UiButton } from "../../../Shared/ui-button/ui-button";

@Component({
  selector: 'app-add-category',
  standalone: true,
  imports: [ReactiveFormsModule, NgIf, ImageUpload, UiButton],
  templateUrl: './add-category.html',
  styleUrl: './add-category.css',
})
export class AddCategory implements OnInit {

  // tell parent component that category is added to refresh the list
  @Output() categoryAdded = new EventEmitter<void>();


  // inject category service
  _categoryService = inject(CategoryService)

  // ───────── FORM ─────────
  categoryForm: FormGroup;

  // ───────── IMAGE PREVIEW ─────────
  imagePreview = signal<string | ArrayBuffer | null>(null);

  constructor(private fb: FormBuilder, private toastr: ToastrService) {
    this.categoryForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      description: ['', Validators.required],
      imageFile: [null]
    });
  }

  ngOnInit(): void { }

  // FILE SELECT
  handleFile(file: File) {

    this.categoryForm.patchValue({
      imageFile: file
    });

    const reader = new FileReader();

    reader.onload = () => {

      this.imagePreview.set(reader.result);
    };

    reader.readAsDataURL(file);
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;

    if (!input.files || input.files.length === 0) {
      return;
    }

    const file = input.files[0];

    // check image type
    if (!file.type.startsWith('image/')) {

      alert('Please select a valid image.');

      return;
    }

    // set file to form
    this.categoryForm.patchValue({
      imageFile: file
    });

    this.categoryForm.get('imageFile')?.updateValueAndValidity();

    // image preview
    const reader = new FileReader();

    reader.onload = () => {

      this.imagePreview.set(reader.result);
    };

    reader.readAsDataURL(file);
  }

  // SUBMIT
  onSubmit(): void {

    if (this.categoryForm.invalid) {

      this.categoryForm.markAllAsTouched();

      return;
    }
    // prepare form data for API
    const formData = new FormData();

    formData.append('nameAr', this.categoryForm.get('name')?.value);
    formData.append('nameEn', this.categoryForm.get('name')?.value);
    formData.append('description', this.categoryForm.get('description')?.value);
    formData.append('imageFile', this.categoryForm.get('imageFile')?.value);
    formData.append('adminId', 'c47b1f93-0e04-43a5-b8ef-d0e00205bd44');
    console.log([...formData.entries()]);

    // call API here
    this._categoryService.create(formData).subscribe({
      next: (result) => {
        if (!result.succeeded) {
          this.toastr.error(result.message, 'cannot get categories');
          return;
        }
        this.toastr.success(result.message, 'Success');

        // emit event to parent component
        this.categoryAdded.emit();
        // reset form
        this.categoryForm.reset();

        this.imagePreview.set(null);
      },

      error: (error) => {
        console.error(error);
        this.toastr.error(error?.error?.message || 'Something went wrong', 'Error');
      },
    });


  }
}
