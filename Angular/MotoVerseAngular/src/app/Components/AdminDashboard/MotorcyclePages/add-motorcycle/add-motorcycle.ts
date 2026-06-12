import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, OnInit, Output, signal } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';

import { ToastrService } from 'ngx-toastr';

import { IMotorcycleService } from '../../../../services/MotorcycleServices/imotorcycle-service';

import { MotorcycleStatus } from '../../../../Enums/motorcycle-status.enum';
import { ImageUpload } from "../../../Shared/image-upload/image-upload";


@Component({
  selector: 'app-add-motorcycle',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, ImageUpload],
  styleUrls: ['./add-motorcycle.css'],
  templateUrl: './add-motorcycle.html',
})
export class AddMotorcycle implements OnInit {

  @Output() addedSuccess = new EventEmitter<boolean>(false);


  private fb = inject(FormBuilder);

  private toastr = inject(ToastrService);

  private motorcycleService =
    inject(IMotorcycleService);

  form!: FormGroup;

  // Main Image
  selectedImage: File | null = null;

  imagePreview = signal<string | ArrayBuffer | null>(null);
  imagePreviews = signal<string[]>([]);
mainImagePreview = signal<string>('assets/images/defualt.jpg');
  // Multiple Images
  selectedImages: File[] = [];


  motorcycleStatus = MotorcycleStatus;

  statusOptions = [
    {
      value: MotorcycleStatus.Available,
      label: 'Available',
    },
    {
      value: MotorcycleStatus.Rented,
      label: 'Rented',
    },
    {
      value: MotorcycleStatus.Maintenance,
      label: 'Maintenance',
    },
  ];

  isSubmitting = signal<boolean>(false);

  ngOnInit(): void {

    this.initializeForm();
  }

  // ----------------------------- initializeForm
  initializeForm(): void {

    this.form = this.fb.group({

      nameAr: ['', Validators.required, Validators.minLength(3), Validators.maxLength(100)],
      nameEn: ['', Validators.required, Validators.minLength(3), Validators.maxLength(100)],
      brand: ['', Validators.required, Validators.minLength(3), Validators.maxLength(100)],

      model: ['', Validators.required, Validators.minLength(3), Validators.maxLength(100)],

      year: ['', [Validators.required, Validators.min(1800), Validators.max(2030),]],

      color: ['', Validators.maxLength(50)],

      plateNumber: ['', Validators.maxLength(50)],

      engineCC: ['', Validators.required, Validators.minLength(3), Validators.maxLength(100)],

      pricePerDay: ['', Validators.required,],

      description: [''],

      status: [MotorcycleStatus.Available, Validators.required],
    });
  }

  // ---------------- Main Image ----------------
  handleFile(file: File) {

    console.log("handleFile : ",file)
    this.selectedImage = file;

    const reader = new FileReader();

    reader.onload = () => {
      this.mainImagePreview.set(reader.result as string);
    };

    reader.readAsDataURL(file);
  }
  onImageSelected(event: Event): void {

    const input = event.target as HTMLInputElement;

    if (!input.files?.length) return;

    const file = input.files[0];

    this.selectedImage = file;
    console.log("this.selectedImage: ", this.selectedImage);

    const reader = new FileReader();

    reader.onload = () => {

      this.imagePreview.set(reader.result);
    };

    reader.readAsDataURL(file);
  }

  removeMainImage(): void {

    this.selectedImage = null;

    this.imagePreview.set(null);
  }
  // ---------------- Multiple Images ----------------
  onMultipleImagesSelected(event: Event): void {

    const input =
      event.target as HTMLInputElement;

    if (!input.files?.length) return;

    Array.from(input.files).forEach(
      (file) => {

        this.selectedImages.push(file);

        const reader = new FileReader();

        reader.onload = () => {

          this.imagePreviews.set([...this.imagePreviews(), reader.result as string]);
        };

        reader.readAsDataURL(file);
      }
    );
  }
  removeImage(index: number): void {

    this.selectedImages.splice(index, 1);

    this.imagePreviews.set(this.imagePreviews().filter((_, i) => i !== index));
  }

  // ---------------- Submit ----------------
  submit(): void {

    if (this.form.invalid) {

      this.form.markAllAsTouched();
      this.testFormErrors()
      return;
    }

    this.isSubmitting.set(true);

    const formData = this.fillFormData()

    // API Call
    this.motorcycleService.create(formData).subscribe({
      next: (result) => {

        if (!result.succeeded) {
          this.isSubmitting.set(false);
          this.toastr.error(result.message, 'Error');
          return;
        }

        this.toastr.success(result.message, 'Success');
        this.imagePreview.set(null)
        this.resetForm();
      },

      error: (error) => {

        this.isSubmitting.set(false);

        console.error(error);
        this.toastr.error(error?.error?.message || 'Something went wrong', 'Error');
      },
    });
  }

  fillFormData(): FormData {
    const formData = new FormData();

    // Basic Data
    formData.append('nameAr', this.form.value.nameAr);
    formData.append('nameEn', this.form.value.nameEn);
    formData.append('Brand', this.form.value.brand);
    formData.append('Model', this.form.value.model);
    formData.append('Year', this.form.value.year);
    formData.append('EngineCC', this.form.value.engineCC);
    formData.append('PricePerDay', this.form.value.pricePerDay);
    formData.append('Status', this.form.value.status);
    formData.append('OwnerId', 'c47b1f93-0e04-43a5-b8ef-d0e00205bd44');

    // Optional Fields
    if (this.form.value.color) formData.append('Color', this.form.value.color);
    if (this.form.value.plateNumber) formData.append('PlateNumber', this.form.value.plateNumber);
    if (this.form.value.description) formData.append('Description', this.form.value.description);

    // Main Image
    if (this.selectedImage) formData.append('ImageFile', this.selectedImage, this.selectedImage.name);

    // Multiple Images
    if (this.selectedImages.length > 0) this.selectedImages.forEach((image) => { formData.append('Images', image); });
    console.log("------ formData", [...formData]);

    return formData;
  }

  resetForm() {

    this.form.reset();

    this.selectedImage = null;

    this.imagePreview.set(null);

    this.selectedImages = [];
    this.selectedImage = null;

    this.imagePreviews.set([]);

    this.isSubmitting.set(false);

    this.addedSuccess.emit(true)
  }
  testFormErrors(): void {

    Object.keys(this.form.controls).forEach((key) => {

      const control = this.form.get(key);

      if (control?.invalid) {

        console.log(
          'Field:',
          key
        );

        console.log(
          'Errors:',
          control.errors
        );
      }
    });
  }


}
