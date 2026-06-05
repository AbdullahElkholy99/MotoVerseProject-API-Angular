import { Component, EventEmitter, Input, Output } from '@angular/core';
import { PaginationModule, PageChangedEvent } from 'ngx-bootstrap/pagination';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-pagination',
  imports: [CommonModule, PaginationModule, FormsModule],
  templateUrl: './pagination.html',
  styleUrl: './pagination.css',
})
export class Pagination {

   // total items
  @Input() totalItems = 0;

  // items per page
  @Input() itemsPerPage = 10;

  // current page
  @Input() currentPage = 1;

  // emit page number
  @Output() pageChanged = new EventEmitter<number>();

  onPageChanged(event: PageChangedEvent): void {
    this.pageChanged.emit(event.page);
  }

}
