import { Directive, ElementRef } from '@angular/core';

@Directive({
  selector: '[appHiglightTitle]',
})
export class HiglightTitle {
     constructor(private el:ElementRef) {
    console.log(el);
    el.nativeElement.style.backgroundColor ='white'
    el.nativeElement.style.color ='black'
    el.nativeElement.style.fontSize ='22px'


  }
}
