import { HttpInterceptorFn } from '@angular/common/http';
import { LoadingService } from '../services/loading-service';
import { inject } from '@angular/core';
import { delay, finalize } from 'rxjs';

export const loaderInterceptor: HttpInterceptorFn = (req, next) => {

  const _loaderService = inject(LoadingService)

  _loaderService.showLoading();
  return next(req).pipe(
    finalize(() =>
      _loaderService.hideLoader()
    )
  );


};
