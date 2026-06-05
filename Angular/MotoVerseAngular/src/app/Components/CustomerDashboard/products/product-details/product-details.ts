import { Component, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';

import { ProductService } from '../../../../services/Products/ProductService';
import { IBasketService } from '../../../../services/MotorcycleServices/basket-service';

import {
  ProductDTO,
  ProductStatus
} from '../../../../models/Product/ProductDTO';

import { ProductReview } from '../product-review/product-review';
import { ProductsRecommendation } from '../products-recommendation/products-recommendation';

@Component({
  selector: 'app-product-details',
  imports: [
    RouterLink,
    ProductReview,
    ProductsRecommendation
  ],
  templateUrl: './product-details.html',
  styleUrls: ['./product-details.css', '../../../styleBoke.css'],
})
export class ProductDetails implements OnInit {

  private route = inject(ActivatedRoute);
  private productService = inject(ProductService);
  private basketService = inject(IBasketService);

  product = signal<ProductDTO | null>(null);
  isLoading = signal(false);

  ProductStatus = ProductStatus;
  currentId!:string
  ngOnInit(): void {

    this.route.paramMap.subscribe(params => {

       this.currentId = params.get('id') ?? '';

      if (!this.currentId) return;

      this.loadProduct(this.currentId);

    });

  }

 loadProduct(id: string): void {

    this.isLoading.set(true);

    this.productService.getById(id).subscribe({
      next: (res) => {

        if (!res.succeeded) {
          this.isLoading.set(false);
          return;
        }

        this.product.set(res.data);

        this.isLoading.set(false);
      },

      error: (err) => {
        console.error(err);
        this.isLoading.set(false);
      }
    });

  }

}
