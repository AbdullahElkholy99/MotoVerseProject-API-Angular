import { Component, OnInit, computed, effect, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { startWith } from 'rxjs';
//stepper
import { MatStepperModule } from '@angular/material/stepper';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
//date picker
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';


import { IMotorcycleService } from '../../../../services/MotorcycleServices/imotorcycle-service';
import { MotorcycleDTO } from '../../../../models/MotorcycleDTO/motorcycle.interface';
import { IBookingService } from '../../../../services/MotorcycleServices/IBookingService';

@Component({
  selector: 'app-motorcycle-rental',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,

    //stepper
    MatStepperModule,
    MatButtonModule,
    MatSelectModule,
    //date picker
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule
  ],
  templateUrl: './motorcycle-rental.html',
  styleUrl: './motorcycle-rental.css',
})
export class MotorcycleRental implements OnInit {

  private _motorcycleService = inject(IMotorcycleService);
  private _bookingService = inject(IBookingService);

  motorcycleId!: string;
  today = new Date();
  motorcycle = signal<MotorcycleDTO>({} as MotorcycleDTO);
  selectedImage = signal('');

  startDate!: Date
  endDate!: Date
  totalDays = 0;
  totalPrice = 0;
  rentalForm!: FormGroup;
  paymentForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router
  ) {
  }

  ngOnInit(): void {

    this.motorcycleId = this.route.snapshot.paramMap.get('id') ?? '';

    this.getById(this.motorcycleId);

    // intialForm
    this.intialForm()

    this.rentalForm.valueChanges.subscribe(() => { this.changeDays(); });

  }

  intialForm() {

    // Step 2 Form
    this.rentalForm = this.fb.group(
      {
        startDate: ['', Validators.required],
        endDate: ['', Validators.required],
        pickupLocation: ['', Validators.required]
      },
      {
        validators: dateRangeValidator
      }
    );

    // Step 3 form
    this.paymentForm = this.fb.group({
      paymentMethod: ['', Validators.required],

      // Card
      cardProvider: [''],
      cardNumber: [''],
      expiryDate: [''],
      cvv: [''],

      // Wallet
      walletName: [''],
      walletPhone: ['']
    });

    this.paymentForm.get('paymentMethod')?.valueChanges.subscribe(method => {

      const cardProvider = this.paymentForm.get('cardProvider');
      const cardNumber = this.paymentForm.get('cardNumber');
      const expiryDate = this.paymentForm.get('expiryDate');
      const cvv = this.paymentForm.get('cvv');

      const walletName = this.paymentForm.get('walletName');
      const walletPhone = this.paymentForm.get('walletPhone');

      // Clear all validators
      cardProvider?.clearValidators();
      cardNumber?.clearValidators();
      expiryDate?.clearValidators();
      cvv?.clearValidators();

      walletName?.clearValidators();
      walletPhone?.clearValidators();

      // Card validation
      if (method === 'card') {
        cardProvider?.setValidators([Validators.required]);

        cardNumber?.setValidators([Validators.required,Validators.minLength(16)]);

        expiryDate?.setValidators([Validators.required]);

        cvv?.setValidators([
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(4)
        ]);
      }

      // Wallet validation
      if (method === 'wallet') {
        walletName?.setValidators([
          Validators.required
        ]);

        walletPhone?.setValidators([
          Validators.required,
          Validators.pattern(/^[0-9]{11}$/)
        ]);
      }

      // Refresh validation state
      cardProvider?.updateValueAndValidity();
      cardNumber?.updateValueAndValidity();
      expiryDate?.updateValueAndValidity();
      cvv?.updateValueAndValidity();

      walletName?.updateValueAndValidity();
      walletPhone?.updateValueAndValidity();
    });
  }

  //changeDays
  changeDays(): void {
    const start = this.rentalForm.get('startDate')?.value;
    const end = this.rentalForm.get('endDate')?.value;

    if (!start || !end) {
      this.totalDays = 0;
      this.totalPrice = 0;
      return;
    }

    const diffTime = new Date(end).getTime() - new Date(start).getTime();

    const days = Math.ceil(diffTime / (1000 * 60 * 60 * 24));

    this.totalDays = days > 0 ? days : 0;

    this.totalPrice = this.totalDays * (this.motorcycle()?.pricePerDay ?? 0);
  }

  //getById
  getById(id: string) {

    this._motorcycleService.getById(id).subscribe({

      next: (result) => {

        if (!result.succeeded) {
          return;
        }

        this.motorcycle.set(result.data);
        this.motorcycle().imagesPath?.push(this.motorcycle().imagePath ?? "")
        if (this.motorcycle()?.imagesPath?.length) {
          this.selectedImage.set(
            'https://localhost:7081/images/motorcycle/' + this.motorcycle().imagesPath![0]
          );
        }

      },

      error: (error) => {
        console.error(error);
      }

    });

  }
  confirmRental(): void {

    if (this.rentalForm.invalid) {
      this.rentalForm.markAllAsTouched();
      return;
    }

    if (this.paymentForm.invalid) {
      this.paymentForm.markAllAsTouched();
      return;
    }

    const paymentMethodMap: Record<string, number> = {
      cash: 1,
      card: 2,
      wallet: 3
    };

    const providerMap: Record<string, number> = {
      visa: 1,
      stripe: 2,
      paypal: 3,
      googlepay: 4
    };

    const request = {
      motorcycleId: this.motorcycleId,

      startDate: this.rentalForm.value.startDate,
      endDate: this.rentalForm.value.endDate,

      pickupLocation: this.rentalForm.value.pickupLocation,

      paymentMethod:
        paymentMethodMap[
        this.paymentForm.value.paymentMethod
        ] ?? 0,

      provider:
        providerMap[
        this.paymentForm.value.cardProvider
        ] ?? 0,

      paypalEmail:
        this.paymentForm.value.paypalEmail,

      walletName:
        this.paymentForm.value.walletName,

      walletPhone:
        this.paymentForm.value.walletPhone
    };

    console.log(request);

    this._bookingService.add(request).subscribe({
      next: (res) => {
        if (!res.succeeded) {
          console.log(res);
          return;
        }
        //  Return to Motorcycle Page
        this.router.navigate(['/home/motorcycle']);
      },
      error: (err) => {
        console.error(err);
      }
    });
  }
  //imageUrl
  get imageUrl(): string {
    return this.motorcycle().imagePath
      ? `https://localhost:7081/images/motorcycle/${this.motorcycle().imagePath}`
      : 'assets/images/default.jpg';
  }
  changeImage(image: string) {
    this.selectedImage.set(
      'https://localhost:7081/images/motorcycle/' + image
    );
  }



}

export function dateRangeValidator(
  control: AbstractControl
): ValidationErrors | null {

  const start = control.get('startDate')?.value;
  const end = control.get('endDate')?.value;

  if (!start || !end) {
    return null;
  }

  return new Date(end) > new Date(start)
    ? null
    : { invalidDateRange: true };
}
