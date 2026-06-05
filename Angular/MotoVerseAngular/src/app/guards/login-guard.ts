import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { environment } from '../environments/environment';

export const loginGuard: CanActivateFn = (route, state) => {

  const router = inject(Router);

  const token = localStorage.getItem('token');

  console.log("token ---------- : ",token);

  if (token) {
    router.navigate(['/home']);
    return false;
  }

  return true;
};
