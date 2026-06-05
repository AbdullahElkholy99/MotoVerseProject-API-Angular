import { HttpInterceptorFn } from '@angular/common/http';
import { environment } from '../environments/environment';
export const authInterceptor: HttpInterceptorFn = (req, next) => {

  // get token from environment
  const token = environment.token || localStorage.getItem('token');

  // if token exists, clone the request and add the Authorization header
  if (token) {
    const authReq = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`,
      },
    });
    return next(authReq);
  }

  // if no token, pass the request
  return next(req);
};
