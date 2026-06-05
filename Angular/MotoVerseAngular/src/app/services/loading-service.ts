import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root',
})
export class LoadingService {

  RequestCount = 0;

  constructor(private spinner: NgxSpinnerService) {}

  showLoading() {

    this.RequestCount++;

    this.spinner.show(undefined, {
      bdColor: 'rgba(0,0,0,0.5)',
      size: 'large',
      color: '#fff',
      type: 'square-jelly-box',
      fullScreen: true
    });
  }

  hideLoader() {
    this.RequestCount--;
    if (this.RequestCount <= 0) {
      this.RequestCount = 0;
      this.spinner.hide();
    }
  }
}
