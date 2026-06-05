import {
  Component,
  EventEmitter,
  Output,
  computed,
  effect,
  inject,
  input,
  signal,
} from '@angular/core';
import { ToastrService } from 'ngx-toastr';

import { MotorcycleDTO } from '../../../../models/MotorcycleDTO/motorcycle.interface';
import { ModalOverlay } from '../../../Shared/modal-overlay/modal-overlay';
import { NoContent } from '../../../Shared/no-content/no-content';
import { IMotorcycleService } from '../../../../services/MotorcycleServices/imotorcycle-service';

@Component({
  selector: 'app-details-motorcycle',
  standalone: true,
  imports: [ModalOverlay, NoContent],
  templateUrl: './details-motorcycle.html',
  styleUrl: './details-motorcycle.css',
})
export class DetailsMotorcycle {

  // SIGNAL INPUT
  selectedMotorcycleId = input.required<string>();

  @Output() close = new EventEmitter<void>();

  // SIGNALS
  selectedMotorcycle =signal<MotorcycleDTO | null>(null);

  currentImageIndex =signal(0);

  private toastr =inject(ToastrService);

  private _motorcycleService =inject(IMotorcycleService);

  constructor() {
    effect(() => {
      const id = this.selectedMotorcycleId();
      if (id) {this.getById(id);}
    });
  }

  // GET DATA

  getById(id: string) {

    this._motorcycleService.getById(id).subscribe({
        next: (result) => {
          if (!result.succeeded) {
            this.toastr.error(result.message);
            return;
          }
          this.selectedMotorcycle.set(result.data);
          const mainImage = result.data.imagePath
          if(mainImage) this.selectedMotorcycle()?.imagesPath?.push(mainImage)
          this.currentImageIndex.set(0);
        },
        error: (error) => {
          console.error(error);
          this.toastr.error(error?.error?.message ||'Something went wrong');
        },
      });
  }

  // CLOSE

  closeDetailsModal() {
    this.close.emit();
  }

  // NEXT

  nextImage() {

    const motorcycle =
      this.selectedMotorcycle();

    const images =
      motorcycle?.imagesPath;

    if (!images?.length) return;

    this.currentImageIndex.update(
      (value) =>
        (value + 1) % images.length
    );
  }

  // PREV

  prevImage() {

    const motorcycle =
      this.selectedMotorcycle();

    const images =
      motorcycle?.imagesPath;

    if (!images?.length) return;

    this.currentImageIndex.update(
      (value) =>
        (
          value -
          1 +
          images.length
        ) %
        images.length
    );
  }

  // CURRENT IMAGE

  currentImage = computed(() => {

    const motorcycle =
      this.selectedMotorcycle();

    const index =
      this.currentImageIndex();

    const images =
      motorcycle?.imagesPath;

    if (!images?.length) {

      return null;
    }

    return (
      'https://localhost:7081/images/motorcycle/' +
      images[index]
    );
  });
}
