import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Tooltip } from "../../../directives/tooltip";

@Component({
  selector: 'app-ui-button',
  imports: [CommonModule, Tooltip],
  templateUrl: './ui-button.html',
  styleUrl: './ui-button.css',
})
export class UiButton {

  @Input() label = 'Button';
  @Input() tooltip = '';
  @Input() loading = false;

  @Input() icon = '';
  @Input() iconOnly = false;
  @Input() type:
    | 'create'
    | 'submit'
    | 'edit'
    | 'delete'
    | 'details'
    | 'previous'
    | 'next'
    | 'reset'
    | 'primary'
    | 'secondary'
    | 'addToCart'
    | 'approve'
    | 'mark-active'
    | 'complete'
    | 'cancel'
    | 're-cancel'
    | 'pay'
    | 'reject'
    = 'primary';

  @Input() fullWidth = false;

  @Input() disabled = false;

  @Output() clicked = new EventEmitter<void>();

  onClick() {
    if (!this.disabled) {
      this.clicked.emit();
    }
  }
}


// <!-- create -->
// <app-ui-button
//   label="Add Product"
//   icon="ri-add-line"
//   type="create"
// ></app-ui-button>

// <!-- edit -->
// <app-ui-button
//   label="Update"
//   icon="ri-pencil-line"
//   type="edit"
// ></app-ui-button>

// <!-- delete -->
// <app-ui-button
//   label="Delete"
//   icon="ri-delete-bin-line"
//   type="delete"
// ></app-ui-button>

// <!-- reset -->
// <app-ui-button
//   label="Reset Filters"
//   icon="ri-refresh-line"
//   type="reset"
// ></app-ui-button>
// <div class="table-actions">

//   <app-ui-button
//     icon="ri-pencil-line"
//     type="edit"
//   ></app-ui-button>

//   <app-ui-button
//     icon="ri-delete-bin-line"
//     type="delete"
//   ></app-ui-button>

// </div>
