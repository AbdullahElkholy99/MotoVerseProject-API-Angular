import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'star',
})
export class StarPipe implements PipeTransform {
  transform(value: number): string {
    if (value < 0 || value > 5) {
      return 'Invalid rating';
    }
    const fullStars = Math.floor(value);
    const halfStar = value % 1 >= 0.5 ? 1 : 0;
    const emptyStars = 5 - fullStars;

    // return '★'.repeat(fullStars) + '☆'.repeat(emptyStars);
    return '⭐'.repeat(fullStars) + '☆'.repeat(emptyStars);
  }
}
