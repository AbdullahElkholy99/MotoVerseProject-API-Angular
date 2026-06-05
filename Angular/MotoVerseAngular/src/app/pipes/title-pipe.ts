import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'title',
})
export class TitlePipe implements PipeTransform {
  transform(value: string, length: number = 12): string {
    return value.length > length ? value.substring(0, length) + '...' : value;
  }
}
