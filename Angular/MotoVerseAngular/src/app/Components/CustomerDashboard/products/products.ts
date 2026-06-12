import { Component, signal, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

import { TitlePipe } from "../../../pipes/title-pipe";
import { StarPipe } from "../../../pipes/star-pipe";
import { EditProductDTO } from '../../../models/Product/EditProductDTO';
import { ProductDTO, ProductStatus } from '../../../models/Product/ProductDTO';
import { GetCategoryDTO } from '../../../models/Category/getCategory';
import { ProductService } from '../../../services/Products/ProductService';
import { CategoryService } from '../../../services/category-service';
import { IBasket, IBasketItem } from '../../../models/MotorcycleDTO/basket';
import { IBasketService } from '../../../services/MotorcycleServices/basket-service';
import { NoContent } from "../../Shared/no-content/no-content";
import { ProductSignalrService } from '../../../services/Products/product-signalr-service';
@Component({
  selector: 'app-products',
  imports: [TitlePipe, StarPipe, NoContent],
  templateUrl: './products.html',
  styleUrls: ['./products.css', '../../styleBoke.css'],
})
export class Products implements OnInit {

  // ----------------- injecting the category service
  _productSignlRService = inject(ProductSignalrService);
  productService = inject(ProductService);
  categoryService = inject(CategoryService);
  _basketService = inject(IBasketService)

  // ProductStatus
  ProductStatus = ProductStatus;

  // search
  search = '';

  basketId: string = ''

  // ----------------- Feilds
  categories = signal<GetCategoryDTO[]>([]);
  products = signal<ProductDTO[]>([]);

  // for pagination
  currentPage = signal(1);
  itemsPerPage = 10;
  totalItems = signal(0);
  orderBy = signal<number>(1)

  // for selected
  selectedCategoryId = '';
  selectedId = signal<string>('');
  selectedProduct = signal<EditProductDTO | null>(null);

  // toggle to show delet category Modal
  showDeleteModal = false;

  selectedCategory = signal('All');
  editMode = signal(false);


  // form for add or update category
  productForm!: FormGroup

  // ───────── IMAGE PREVIEW ─────────
  imagePreview = signal<string | ArrayBuffer | null>(null);

  // ----------------- Constructor
  constructor(
    private fb: FormBuilder,
    private toastr: ToastrService,
    private router: Router, private http: HttpClient
  ) { }


  ngOnInit(): void {
    // fetch all categories
    this.getAll();

    // get all categories for the dropdown
    this.getAllCategories();

    this.basketId = this._basketService.getBasketId();

    //start connection SignalR
    this.startConSignalR()

  }

  startConSignalR() {
     this._productSignlRService
    .startConnection()
    .then(() => {
      this._productSignlRService.onNewProduct(
        (product: ProductDTO) => {

          this.products.update(products => [
            product,
            ...products
          ]);

          this.totalItems.update(v => v + 1);

          this.toastr.info(
            `${product.name} added now`,
            'New Product'
          );
        }
      );
    });

  }

  // ---------------  Get All Categories ---------------
  filterBy() {
    this.currentPage.set(1);
    this.getAll();
  }
  resetFilter() {
    this.selectedCategory.set('All');
    this.selectedCategoryId = '';
    this.search = '';
    this.orderBy.set(1);
    this.currentPage.set(1);

    this.getAll();
  }
  getAll() {

    const getProductPaginatedListQueryDTO = {
      pageNumber: this.currentPage(),
      pageSize: this.itemsPerPage,
      orderBy: this.orderBy(),
      search: this.search,
      categoryId: this.selectedCategoryId
    };
    this.productService.getAll(getProductPaginatedListQueryDTO).subscribe({
      next: (result) => {
        if (!result.succeeded) {
          this.toastr.error(result.message, 'cannot get products');
          return;
        }
        this.products.set(result.data);

        this.totalItems.set(result.meta?.itemsCount ?? 0);

        this.currentPage.set(this.currentPage());

        this.toastr.success(result.message, 'Success');
      },

      error: (error) => {
        console.error(error);
        this.toastr.error(
          error?.error?.message || 'Something went wrong', 'Error');
      },
    });

  }

  // ----------------- Get All Categories
  getAllCategories() {
    this.categoryService.getAll().subscribe({
      next: (result) => {
        if (!result.succeeded) {
          this.toastr.error(result.message, 'cannot get categories');
          return;
        }
        this.categories.set(result.data);

        console.log("categories : ",result.data);

      },
      error: (error) => {
        console.error(error);
      }
    });
  }

  selectCategory(category: GetCategoryDTO) {
    this.selectedCategory.set(category.name);
    this.selectedCategoryId = category.id;
    this.currentPage.set(1);

    this.getAll();
  }


  loadingProductId = signal<string | null>(null);

  addToCart(product: ProductDTO) {

    if (this.loadingProductId() === product.id) return;

    this.loadingProductId.set(product.id);

    const basketDto: IBasket = {
      id: this.basketId,
      basketItems: [
        {
          id: product.id,
          name: product.name,
          imagePath: product.imagePath ?? '',
          quantity: 1,
          price: product.price,
          categoryId: product.categoryId
        }
      ]
    };

    this._basketService.edit(basketDto).subscribe({
      next: (res) => {

        if (!res.succeeded) {
          this.toastr.error(res.message);
          this.loadingProductId.set(null);
          return;
        }

        this.toastr.success('Added To Cart');
        this.loadingProductId.set(null);
      },

      error: (err) => {
        console.error(err);
        this.toastr.error('Something went wrong');
        this.loadingProductId.set(null);
      }
    });
  }

showDetails(id: string): void {
  this.router.navigate(['/home/product-details', id]);
}
}
