import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  Output,
  signal,
  SimpleChanges
} from '@angular/core';

import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-image-upload',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './image-upload.html',
  styleUrls: ['./image-upload.css']
})

export class ImageUpload implements OnChanges {

  @Input() label = 'Upload Image';

  @Input() preview: string | ArrayBuffer | null = null;

  @Input() errorMessage = '';

  @Output() fileSelected = new EventEmitter<File>();

  imagePreview = signal<string | ArrayBuffer | null>(null);

  ngOnChanges(changes: SimpleChanges) {
    if (changes['preview']) {
      this.imagePreview.set(
        changes['preview'].currentValue
      );
    }

  }

  onFileSelected(event: Event): void {

    const input =
      event.target as HTMLInputElement;

    if (!input.files?.length) return;

    const file = input.files[0];

    if (!file.type.startsWith('image/')) {
      return;
    }

    this.fileSelected.emit(file);

    const reader = new FileReader();

    reader.onload = () => {
      this.imagePreview.set(reader.result);
    };

    reader.readAsDataURL(file);
  }
}
