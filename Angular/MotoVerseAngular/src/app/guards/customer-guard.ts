import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { getUserRole } from '../Helpers/decode-jwt';

export const customerGuard: CanActivateFn = (route, state) => {

  const router = inject(Router);

  const token = localStorage.getItem('token');
  const role = localStorage.getItem('role') || getUserRole();
  console.log("=====customerGuard===== token : ",token);
  console.log("=====customerGuard===== role : ",role);

  if (!token) {
    router.navigate(['/login']);
    return false;
  }

  if (role !== 'Customer') {
    router.navigate(['/']);
    return false;
  }

  return true;
};
