import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { getUserRole } from '../Helpers/decode-jwt';

export const redirectGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const roles = getUserRole();

  if (roles.includes('Admin')) {
    return router.createUrlTree(['/admin']);
  }

  if (roles.includes('User')) {
    return router.createUrlTree(['/home']);
  }

  return router.createUrlTree(['/login']);
};
