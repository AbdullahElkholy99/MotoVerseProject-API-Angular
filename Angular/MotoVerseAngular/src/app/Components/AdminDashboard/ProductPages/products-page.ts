import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { ProductService } from '../../../services/Products/ProductService';
import { ProductDTO, ProductStatus } from '../../../models/Product/ProductDTO';
import { ToastrService } from 'ngx-toastr';
import { HttpClient } from '@angular/common/http';
import { ModalOverlay } from "../../Shared/modal-overlay/modal-overlay";
import { ModalDelete } from "../../Shared/modal-delete/modal-delete";
import { EditProductDTO } from '../../../models/Product/EditProductDTO';
import {
  ReactiveFormsModule,
  FormGroup,
  FormBuilder,
  Validators,

} from '@angular/forms';
import { NgIf, NgFor } from '@angular/common';
import { GetCategoryDTO } from '../../../models/Category/getCategory';
import { CategoryService } from '../../../services/category-service';
import { ImageUpload } from "../../Shared/image-upload/image-upload";
import { NoContent } from "../../Shared/no-content/no-content";
import { UiButton } from "../../Shared/ui-button/ui-button";

@Component({
  selector: 'app-products-page',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule,
    ModalOverlay, ModalDelete, ReactiveFormsModule, NgIf, NgFor,
    ImageUpload, NoContent, UiButton],
  templateUrl: './products-page.html',
  styleUrls: ['./products-page.css'],
})
export class ProductsPage implements OnInit {

  // ----------------- injecting the category service
  productService = inject(ProductService);
  categoryService = inject(CategoryService);

  // ProductStatus
  ProductStatus = ProductStatus;

  // search
  search = '';

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

    // build form
    this.buildForm();
  }

  // --------------- Build the form productForm
  buildForm(): void {
    this.productForm = this.fb.group({
      id: [''],
      nameAr: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
      nameEn: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
      quantity: [0, [Validators.required, Validators.min(0)]],
      price: [0, [Validators.required, Validators.min(0)]],
      imageFile: [null,],
      categoryId: ['', [Validators.required]],
      adminId: [''],
      description: ['', [Validators.required, Validators.minLength(10), Validators.maxLength(1000)]],
      status: [ProductStatus.NoStatus, [Validators.required]],
    })
  }
  // ---------------  Create ---------------

  onSubmit() {
    if (this.productForm.invalid) {
      this.productForm.markAllAsTouched();
      return;
    }
    const formData = this.fillForm();

    if (!this.editMode()) this.addProductRequest(formData);
    else this.updateProductRequest(formData);


  }
  fillForm(): FormData {
    const formData = new FormData();

    formData.append('nameAr', this.productForm.get('nameAr')?.value);
    formData.append('nameEn', this.productForm.get('nameEn')?.value);
    formData.append('description', this.productForm.get('description')?.value);
    formData.append('quantity', String(this.productForm.get('quantity')?.value));
    formData.append('price', String(this.productForm.get('price')?.value));
    formData.append('categoryId', this.productForm.get('categoryId')?.value);
    formData.append('status', String(this.productForm.get('status')?.value));
    formData.append('adminId', 'c47b1f93-0e04-43a5-b8ef-d0e00205bd44');

    // image
    const file = this.productForm.get('imageFile')?.value;

    if (file) {
      formData.append('imageFile', file);
    }
    return formData;
  }
  // ---------------  Add
  addProductRequest(formData: FormData) {
    this.productService.create(formData).subscribe({
      next: (result) => {
        if (!result.succeeded) {
          this.toastr.error(result.message);
          return;
        }
        this.toastr.success(result.message);
        this.productForm.reset();
        this.imagePreview.set(null);

        this.getAll();
      },

      error: (error) => {
        console.error(error);
        this.toastr.error(
          error?.error?.message || 'Something went wrong'
        );
      },
    });
  }
  // ---------------  Update
  updateProductRequest(formData: FormData) {
    formData.append('id', this.selectedId());

    this.productService.edit(formData).subscribe({

      next: (result) => {
        if (!result.succeeded) {
          this.toastr.error(result.message);
          return;
        }
        this.getAll();
        this.toastr.success(result.message);
        this.productForm.reset();
        this.imagePreview.set(null);
        this.editMode.set(false)
      },

      error: (error) => {
        console.error(error);
        this.toastr.error(error?.error?.message || 'Something went wrong');
      },
    });
  }

  // selected Product for update
  SelectedProduct(product: ProductDTO) {

    this.selectedId.set(product.id);

    this.productForm.patchValue({
      id: product.id,
      categoryId: product.categoryId,
      nameAr: product.name,
      nameEn: product.name,
      quantity: product.quantity,
      price: product.price,
      description: product.description,
      status: product.status,
    });

    this.imagePreview.set(this.imageURL + product.imagePath);
    this.editMode.set(true);
    this.showForm.update(value => false);

  }

  //------------------------------------------ FILE SELECT
  handleFile(file: File) {

    this.productForm.patchValue({
      imageFile: file
    });
    this.productForm.get('imageFile')?.updateValueAndValidity();

    const reader = new FileReader();
    reader.onload = () => {
      this.imagePreview.set(reader.result);
    };

    reader.readAsDataURL(file);
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
    this.productForm.patchValue({
      imageFile: file
    });

    this.productForm.get('imageFile')?.updateValueAndValidity();

    // image preview
    const reader = new FileReader();

    reader.onload = () => {

      this.imagePreview.set(reader.result);
    };

    reader.readAsDataURL(file);
  }

  // ---------------  Get All Categories ---------------
  filterBy() {
    this.currentPage.set(1);
    this.getAll();
  }
  resetFilter() {
    this.selectedCategoryId = '';
    this.orderBy.set(1)
    this.search = ''
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

  // ---------------  delete a Product ---------------
  openDeleteModal(id: string) {
    this.selectedId.set(id);
    this.showDeleteModal = true;
  }

  closeDeleteModal() {
    this.showDeleteModal = false;
  }

  confirmDelete() {

    this.productService.delete(this.selectedId()).subscribe({
      next: (result) => {
        console.log(result);

        if (!result.succeeded) {
          this.toastr.error(result.message, 'cannot get product');
          return;
        }
        this.toastr.success(result.message, 'Success');

        const totalPages = Math.ceil(
          this.totalItems() / this.itemsPerPage
        );
        if (this.currentPage() > totalPages) {
          this.currentPage.set(totalPages || 1);
        }

        this.currentPage.set(this.currentPage());
        this.getAll();
        this.closeDeleteModal();
      },

      error: (error) => {
        console.error(error);

        this.toastr.error(error?.error?.message || 'Something went wrong', 'Error');
      },
    });
  }



  // ----------------- pagination
  get PageCount() {
    return Math.ceil(this.totalItems() / this.itemsPerPage);
  }

  setPage(page: number) {
    this.currentPage.set(page);
    this.getAll();
  }


  get imageURL() {
    return 'https://localhost:7081/images/product/'
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

        console.log(result.data);

      },
      error: (error) => {
        console.error(error);
      }
    });
  }




  showForm = signal(true);

  toggleForm() {
    this.showForm.update(value => !value);
  }


  //custome button
  get editOraddType(): 'edit' | 'create' {
    return this.editMode() ? 'edit' : 'create';
  }

  get editOraddIcon(): string {
    return this.editMode()
      ? 'ri-pencil-line'
      : 'ri-add-line';
  }

  get editOraddLabel(): string {
    return this.editMode()
      ? 'Update Product'
      : 'Create Product';
  }



  switchMode() {
    console.log("switchModeswitchMode");

    this.editMode.set(false)
    this.productForm.reset()
    this.imagePreview.set(null)
  }








}
