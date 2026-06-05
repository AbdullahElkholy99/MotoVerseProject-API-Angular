import { Component, inject,Input, OnChanges, Output, EventEmitter, signal } from '@angular/core';

import {
  ReactiveFormsModule,
  FormBuilder,
  FormGroup,
  Validators
} from '@angular/forms';

import { NgIf } from '@angular/common';
import { CategoryService } from '../../../../services/category-service';
import { ToastrService } from 'ngx-toastr';
import { GetCategoryDTO } from '../../../../models/Category/getCategory';
import { UiButton } from "../../../Shared/ui-button/ui-button";

@Component({
  selector: 'app-update-category',
  standalone: true,
  imports: [ReactiveFormsModule, NgIf, UiButton],
  templateUrl: './update-category.html',
  styleUrl: './update-category.css',
})
export class UpdateCategory implements OnChanges  {

  @Output() categoryUpdated = new EventEmitter<void>();

  // inject category service
  _categoryService = inject(CategoryService)

  @Input({ required: true }) categoryId!: string;
  // ───────── FORM
  categoryForm: FormGroup;

// currentCategory
  currentCategory : GetCategoryDTO | null = null;

  // ───────── IMAGE PREVIEW ─────────
  imagePreview = signal< string | ArrayBuffer | null>(null);

  constructor(private fb: FormBuilder, private toastr :ToastrService) {
    this.categoryForm = this.fb.group({
      name: [this.currentCategory?.name || '', [Validators.required, Validators.minLength(3)]],
      description: [this.currentCategory?.description || '', Validators.required],
      imageFile: [null]
    });
  }

  ngOnChanges(): void {
      if ( this.categoryId) {
      console.log('CATEGORY ID:', this.categoryId);
      this.getById();
    }
  }


  getById(){
    console.log("------------ category id in update component");
    this._categoryService.getById(this.categoryId).subscribe({
      next: (result) => {
        console.log(result);
        if (!result.succeeded) {
          this.toastr.error(result.message, 'cannot get category');
          return;
        }
        this.toastr.success(result.message, 'Success');
        this.currentCategory = result.data;
        this.categoryForm.patchValue({
          name: this.currentCategory.name,
          description: this.currentCategory.description
        });
      },

      error: (error) => {
        console.error(error);

        this.toastr.error(error?.error?.message || 'Something went wrong update', 'Error');
      },
    });
  }

  // FILE SELECT
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

    formData.append('id',this.categoryId);
    formData.append('nameAr',this.categoryForm.get('name')?.value);
    formData.append('nameEn',this.categoryForm.get('name')?.value);
    formData.append('description',this.categoryForm.get('description')?.value  );
    formData.append('imageFile',this.categoryForm.get('imageFile')?.value );
    formData.append('adminId','c47b1f93-0e04-43a5-b8ef-d0e00205bd44' );
    console.log([...formData.entries()]);

    // call API here
    this._categoryService.edit(formData).subscribe({
      next: (result) => {
        if (!result.succeeded) {
          this.toastr.error(result.message, 'cannot get categories');
          return;
        }
        this.toastr.success(result.message, 'Success');

        // emit event to parent component to refresh the list of categories
        this.categoryUpdated.emit();
      },

      error: (error) => {
        console.error(error);
        this.toastr.error(error?.error?.message || 'Something went wrong', 'Error');
      },
    });
  }
}
