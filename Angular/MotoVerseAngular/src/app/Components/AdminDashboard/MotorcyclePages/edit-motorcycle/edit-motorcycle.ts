import { Component, EventEmitter, inject, Input, OnInit, Output, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';

import { ToastrService } from 'ngx-toastr';

import { IMotorcycleService } from '../../../../services/MotorcycleServices/imotorcycle-service';
import { MotorcycleStatus } from '../../../../Enums/motorcycle-status.enum';
import { MotorcycleDTO } from '../../../../models/MotorcycleDTO/motorcycle.interface';
import { ImageUpload } from "../../../Shared/image-upload/image-upload";

@Component({
  selector: 'app-edit-motorcycle',
  imports: [CommonModule, ReactiveFormsModule, ImageUpload],
  templateUrl: './edit-motorcycle.html',
  styleUrl: './edit-motorcycle.css',
})
export class EditMotorcycle implements OnInit {


  @Input({ required: true }) motorCycleId!: string
  @Output() updatedSuccess = new EventEmitter<boolean>(false);

  private fb = inject(FormBuilder);

  private toastr = inject(ToastrService);

  private _motorcycleService = inject(IMotorcycleService);

  form!: FormGroup;

  // Main Image
  selectedImage: File | null = null;

  imagePreview: string | ArrayBuffer | null = null;

  // Multiple Images
  selectedImages: File[] = [];

  imagePreviews: string[] = [];

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

  isSubmitting = false;

  ngOnInit(): void {
    this.initializeForm();
    this.getById();
  }
  // ---------------------------- get by id
  getById() {
    this._motorcycleService.getById(this.motorCycleId).subscribe({
      next: (result) => {
        if (!result.succeeded) {
          this.toastr.error(result.message);
          return;
        }
        const motorCycle = result.data
        console.log("------------------- motorCycle get by id", motorCycle);

        this.formPatchValue(motorCycle)

      },
      error: (error) => {
        console.error(error);
        this.toastr.error(error?.error?.message || 'Something went wrong');
      }
    })
  }

  formPatchValue(motorCycle: MotorcycleDTO) {
    this.form.patchValue({
      nameAr: motorCycle.nameAr,
      nameEn: motorCycle.nameEn,
      brand: motorCycle.brand,
      model: motorCycle.model,
      year: motorCycle.year,
      color: motorCycle.color,
      plateNumber: motorCycle.plateNumber,
      engineCC: motorCycle.engineCC,
      pricePerDay: motorCycle.pricePerDay,
      status: motorCycle.status,
      description: motorCycle.description,
      ownerId: motorCycle.ownerId,
    });
    if (motorCycle.imagePath)
      this.imagePreview = motorCycle.imagePath;
    if (motorCycle.imagesPath)
      this.imagePreviews = motorCycle.imagesPath?.map((x: any) => x);
    console.log("imagePreviews : ", this.imagePreviews);

  }
  // ----------------------------- initializeForm
  initializeForm(): void {

    this.form = this.fb.group({

      nameAr: [
        '',
        [
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(100)
        ]
      ],

      nameEn: [
        '',
        [
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(100)
        ]
      ],

      brand: [
        '',
        [
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(100)
        ]
      ],

      model: [
        '',
        [
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(100)
        ]
      ],

      year: [
        '',
        [
          Validators.required,
          Validators.min(1800),
          Validators.max(2030)
        ]
      ],

      color: [
        '',
        [Validators.maxLength(50)]
      ],

      plateNumber: [
        '',
        [Validators.maxLength(50)]
      ],

      engineCC: [
        '',
        [
          Validators.required,
        ]
      ],

      pricePerDay: [
        '',
        [Validators.required]
      ],

      description: [''],

      status: [
        MotorcycleStatus.Available,
        [Validators.required]
      ],
    });
  }

  // ---------------- Main Image ----------------

  onImageSelected(event: Event): void {

    const input = event.target as HTMLInputElement;

    if (!input.files?.length) return;

    const file = input.files[0];

    this.selectedImage = file;

    const reader = new FileReader();

    reader.onload = () => {

      this.imagePreview = reader.result;
    };

    reader.readAsDataURL(file);
  }

  removeMainImage(): void {

    this.selectedImage = null;

    this.imagePreview = null;
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

          this.imagePreviews.push(
            reader.result as string
          );
        };

        reader.readAsDataURL(file);
      }
    );
  }

  removeImage(index: number): void {

    this.selectedImages.splice(index, 1);

    this.imagePreviews.splice(index, 1);
  }

  // ---------------- Submit ----------------
  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      this.testFormErrors()
      return;
    }

    this.isSubmitting = true;

    const formData = this.fillFormData()

    // API Call
    this._motorcycleService.edit(formData).subscribe({
      next: (result) => {
        if (!result.succeeded) {
          this.isSubmitting = false;
          this.toastr.error("result.message", 'Error');
          return;
        }

        this.toastr.success(result.message, 'Success');
        this.resetForm()
      },

      error: (error) => {

        this.isSubmitting = false;

        console.error(error);

        this.toastr.error(error?.error?.message || 'Something went wrong', 'Error');
      },
    });
  }

  fillFormData(): FormData {
    const formData = new FormData();

    // Basic Data
    formData.append('id', this.motorCycleId);
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

    return formData;
  }

  resetForm() {

    this.form.reset();

    this.selectedImage = null;

    this.imagePreview = null;

    this.selectedImages = [];

    this.imagePreviews = [];

    this.isSubmitting = false;

    this.updatedSuccess.emit(true)
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
