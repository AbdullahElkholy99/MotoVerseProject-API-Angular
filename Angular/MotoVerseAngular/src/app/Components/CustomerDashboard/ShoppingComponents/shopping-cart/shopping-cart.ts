import {
  Component,
  computed,
  effect,
  inject,
  input,
  output,
  signal
} from '@angular/core';

import { IBasketService } from '../../../../services/MotorcycleServices/basket-service';
import { IBasket } from '../../../../models/MotorcycleDTO/basket';

@Component({
  selector: 'app-shopping-cart',
  imports: [],
  templateUrl: './shopping-cart.html',
  styleUrl: './shopping-cart.css',
})
export class ShoppingCart {

  private _basketService = inject(IBasketService);

  basketId = this._basketService.getBasketId();

  showCart = input.required<boolean>();

  closeCart = output<void>();

  cart = signal<IBasket | null>(null);

  cartItems = computed(() =>
    this.cart()?.basketItems ?? []
  );

  totalPrice = computed(() =>
    this.cartItems().reduce(
      (total, item) => total + item.price * item.quantity,
      0
    )
  );

  constructor() {
    // First load
    this.fetchAllBasketItems();

    // Reload every time cart opens
    effect(() => {
      const isOpen = this.showCart();
      if (isOpen) {
        this.fetchAllBasketItems();
      }

    });
  }

  //toggleCart
  toggleCart() {
    this.closeCart.emit();
  }

  //fetchAllBasketItems
  fetchAllBasketItems() {

    this._basketService.getById(this.basketId).subscribe({
      next: res => {

        if (!res.succeeded) {
          console.error(res.errors);
          return;
        }

        this.cart.set(res.data);
      },
      error: err => console.error(err)
    });
  }
  //remove item from cart
  removeFromCart(productId: string) {

    this._basketService
      .deleteItem(this.basketId, productId)
      .subscribe({
        next: res => {
          if (!res.succeeded) {
            console.error(res.errors);
            return;
          }
          this.cart.set(res.data);
        },
        error: err => console.error(err)
      });

  }

  increaseQty(productId: string) {
    const basket = this.cart();
    if (!basket) return;
    this.cart.set({
      ...basket,
      basketItems: basket.basketItems.map(item =>
        item.id === productId
          ? { ...item, quantity: item.quantity + 1 }
          : item
      )
    });
  }

  decreaseQty(productId: string) {
    const basket = this.cart();

    if (!basket) return;

    this.cart.set({
      ...basket,
      basketItems: basket.basketItems
        .map(item =>
          item.id === productId
            ? { ...item, quantity: item.quantity - 1 }
            : item
        )
        .filter(item => item.quantity > 0)
    });
  }
  
}
