import { Component, ElementRef, inject, OnInit, signal, ViewChild, ViewRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MockDataService } from '../../OrderPages/mock-data.service';
import { CategoryService } from '../../../../services/category-service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { GetCategoryDTO } from '../../../../models/Category/getCategory';
import { PaginationModule, PageChangedEvent } from 'ngx-bootstrap/pagination';
import { FormsModule } from '@angular/forms';
import { Pagination } from "../../../Shared/pagination/pagination";
import { AddCategory } from '../add-category/add-category';
import { UpdateCategory } from '../update-category/update-category';
import { ModalOverlay } from '../../../Shared/modal-overlay/modal-overlay';
import { HttpClient } from '@angular/common/http';
import { ModalDelete } from '../../../Shared/modal-delete/modal-delete';
import { NoContent } from "../../../Shared/no-content/no-content";
import { UiButton } from "../../../Shared/ui-button/ui-button";

@Component({
  selector: 'app-categories-page',
  standalone: true,
  imports: [ModalOverlay, ModalDelete, CommonModule, PaginationModule, FormsModule, Pagination, AddCategory, UpdateCategory, NoContent, UiButton],
  templateUrl: './categories-page.html',
  styleUrls: ['./categories-page.css'],
})
export class CategoriesPage implements OnInit {

  // search
  search = '';

  // ----------------- injecting the category service
  categoryService = inject(CategoryService);

  // ----------------- Feilds
  categories = signal<GetCategoryDTO[]>([]);
  currentPage = 1;
  itemsPerPage = 10;
  totalItems = 0;

  selectedCategoryId = signal<string>('');

  // toggle to show add / update category Modal
  showAddCategoryModal = false
  showUpdateCategoryModal = false
  showDeleteModal = false;

  // ----------------- Constructor
  constructor(
    public readonly data: MockDataService,
    private toastr: ToastrService,
    private router: Router, private http: HttpClient
  ) { }


  // ----------------- ngOnInit
  ngOnInit(): void {
    this.getAll();
  }

  // ---------------  Get All Categories ---------------
onSearch() {
  this.currentPage = 1;
  this.getAll();
}
  getAll(page: number = 1) {

    const getCategoryPaginatedListQueryDTO = {
      pageNumber: page,
      pageSize: this.itemsPerPage,
      orderBy: 0,
      search: this.search
    };

    this.categoryService.getAllPaginated(getCategoryPaginatedListQueryDTO).subscribe({
      next: (result) => {
        if (!result.succeeded) {
          this.toastr.error(result.message, 'cannot get categories');
          return;
        }
        this.categories.set(result.data);

        this.totalItems = result.meta?.totalCount ?? 0;

        this.currentPage = page;

        this.toastr.success(result.message, 'Success');
      },

      error: (error) => {

        console.error(error);

        this.toastr.error(
          error?.error?.message || 'Something went wrong', 'Error');
      },
    });

  }
  // ---------------  delete a Category ---------------
  openDeleteModal(id: string) {
    this.selectedCategoryId.set(id);
    this.showDeleteModal = true;
  }

  closeDeleteModal() {
    this.showDeleteModal = false;
  }

  confirmDelete() {

    this.categoryService.delete(this.selectedCategoryId()).subscribe({
      next: (result) => {
        console.log(result);

        if (!result.succeeded) {
          this.toastr.error(result.message, 'cannot get categories');
          return;
        }
        this.toastr.success(result.message, 'Success');

        const totalPages = Math.ceil(
          this.totalItems / this.itemsPerPage
        );
        if (this.currentPage > totalPages) {
          this.currentPage = totalPages || 1;
        }

        this.getAll(this.currentPage);

        this.closeDeleteModal();
      },

      error: (error) => {
        console.error(error);

        this.toastr.error(error?.error?.message || 'Something went wrong', 'Error');
      },
    });
  }

  // ---------------  update a Category ---------------
  openUpdateModal(id: string) {
    this.selectedCategoryId.set(id);
    this.showUpdateCategoryModal = true;
    console.log(this.selectedCategoryId());
  }
  // refresh the list of categories after updating a category
  onCategoryUpdated() {
    // refresh the list of categories after adding a new one
    this.getAll(this.currentPage);
    this.showUpdateCategoryModal = false;

  }

  // ---------------  Create ---------------
  //  refresh the list of categories after adding a new one
  onCategoryAdded() {
    this.getAll(this.currentPage);
  }


  // ---------------  Pagination ---------------
  pageChanged(pageNumber: number) {
    this.currentPage = pageNumber;
    this.getAll(this.currentPage);
  }

}
