import {
  Component,
  effect,
  inject,
  input,
  signal
} from '@angular/core';

import { Router } from '@angular/router';
import { NgIf } from '@angular/common';

import { ProductService } from '../../../../services/Products/ProductService';
import { ProductRecommendationsDTO } from '../../../../models/Product/ProductDTO';

@Component({
  selector: 'app-products-recommendation',
  imports: [NgIf],
  templateUrl: './products-recommendation.html',
  styleUrl: './products-recommendation.css',
})
export class ProductsRecommendation {

  private productService = inject(ProductService);
  private router = inject(Router);

  productId = input.required<string>();

  recommendations = signal<ProductRecommendationsDTO[]>([]);

  constructor() {

    effect(() => {

      console.log("productId : ", this.productId());
      const id = this.productId();

      if (!id) return;

      this.productService
        .getRecommendations(id)
        .subscribe({
          next: (res) => {

            if (!res.succeeded) return;

            console.log('Recommendations:', res.data);

            this.recommendations.set(res.data);
          },
          error: (err) => {
            console.error(err);
          }
        });

    });

  }

  showDetails(id: string): void {

    this.router.navigate([
      '/home/product-details',
      id
    ]);

  }

}
