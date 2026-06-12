import { Component, EventEmitter, inject, Input, OnInit, Output, signal } from '@angular/core';
import { UiButton } from "../../../Shared/ui-button/ui-button";
import { MotorcycleDTO } from '../../../../models/MotorcycleDTO/motorcycle.interface';
import { IBasketService } from '../../../../services/MotorcycleServices/basket-service';
import { IBasket, IBasketItem } from '../../../../models/MotorcycleDTO/basket';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-motorcycle-card',
  imports: [UiButton],
  templateUrl: './motorcycle-card.html',
  styleUrl: './motorcycle-card.css',
})
export class MotorcycleCard implements OnInit {

  _basketService = inject(IBasketService)
  toastr = inject(ToastrService)

  @Input({ required: true }) motorcycle!: MotorcycleDTO;
  @Output() viewDetails = new EventEmitter<string>();
  @Output() bookNow = new EventEmitter<string>();
  @Output() toggleFavorite = new EventEmitter<string>();
  @Output() onAddToCart = new EventEmitter<string>();
  @Output() onDetails = new EventEmitter<string>();

  showAddModal = false;
  showDeleteModal = false;
  showUpdateModal = false;

  basketId: string = ''


  constructor(private router: Router) {

  }
  ngOnInit(): void {
    this.basketId = this._basketService.getBasketId();
  }

  emitAddToCart(id: string): void {
    this.onAddToCart.emit(id);
  }
  emitViewDetails(): void {
    this.viewDetails.emit(this.motorcycle.id);
  }

  emitBookNow(): void {
    this.bookNow.emit(this.motorcycle.id);
  }

  emitToggleFavorite(): void {
    this.toggleFavorite.emit(this.motorcycle.id);
  }


  statusMap: Record<number, string> = {
    1: 'Available',
    2: 'Rented',
    3: 'Maintenance',
  };


  emitDetails(id: string) {

    this.onDetails.emit(id);
  }


  // addToCart
  addToCart() {

    let basketItems: IBasketItem[] = []
    basketItems.push({
      id: this.motorcycle.id,
      name: this.motorcycle.nameEn,
      imagePath: this.motorcycle.imagePath ?? "",
      quantity: 1,
      price: this.motorcycle.pricePerDay,
      categoryId: this.motorcycle.brand,
    });
    const basketDto: IBasket = {
      id: this.basketId,
      basketItems: basketItems ?? []
    };

    this._basketService.edit(basketDto).subscribe({
      next: res => {
        if (!res.succeeded) {
          console.log(res.errors);
          this.toastr.error('Failed to add to cart');
          return;
        }
        console.log(res);
        this.toastr.success('Added to cart successfully');
      },
      error: err => {
        console.error(err);
        this.toastr.error('Failed to add to cart');
      }
    });

  }


  // rentalMotorcycle
  clicked = signal<boolean>(false)
  rentalMotorcycle() {
    this.clicked.set(true);

    this.router.navigate([
      '/home/motorcycle/rental',
      this.motorcycle.id
    ]).finally(() => {
      this.clicked.set(false);
    });
  }
}
