import {
  Component,
  EventEmitter,
  inject,
  Input,
  Output
} from '@angular/core';
import { CommonModule, NgIf } from '@angular/common';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

import { ReviewDTO } from '../../../../models/Product/reviews/reviewDto';
import { ReviewProductService } from '../../../../services/Products/ReviewProductService';

@Component({
  selector: 'app-product-review',
  imports: [CommonModule, ReactiveFormsModule,NgIf],
  templateUrl: './product-review.html',
  styleUrl: './product-review.css',
})
export class ProductReview {

  private reviewService = inject(ReviewProductService);
  private toastr = inject(ToastrService);
  private fb = inject(FormBuilder);

  @Input({ required: true })
  reviews: ReviewDTO[] = [];

  @Input({ required: true })
  productId!: string;

  @Output()
  reviewAdded = new EventEmitter<void>();

  reviewForm = this.fb.group({
    rating: [5, [Validators.required]],
    comment: ['', [
      Validators.required,
      Validators.minLength(5),
      Validators.maxLength(500)
    ]]
  });

  submitReview() {

    if (this.reviewForm.invalid) {
      this.reviewForm.markAllAsTouched();

      this.toastr.warning(
        'Please fill all required fields correctly.',
        'Validation'
      );

      return;
    }

    const formData = new FormData();
    const { rating, comment } = this.reviewForm.value;

    formData.append('rating', String(rating));
    formData.append('comment', comment ?? '');
    formData.append('productId', this.productId);

console.log("--------- this.productId ",this.productId);

    this.reviewService.create(formData).subscribe({
      next: () => {

        this.toastr.success(
          'Review added successfully.',
          'Success'
        );

        this.reviewForm.reset({
          rating: 5,
          comment: ''
        });

        this.reviewAdded.emit();
      },

      error: (err) => {

        this.toastr.error(
          err?.error?.message || 'Failed to add review.',
          'Error'
        );
      }
    });
  }

  get commentControl() {
    return this.reviewForm.get('comment');
  }
}
