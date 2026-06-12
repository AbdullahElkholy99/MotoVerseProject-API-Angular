import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { getUserRole } from '../Helpers/decode-jwt';

export const roleGuard: CanActivateFn = (route, state) => {


  const router = inject(Router);

  const userRoles = getUserRole();


  const allowedRoles = route.data?.['roles'] as string[];


  const hasAccess = userRoles.some(role =>
    allowedRoles.includes(role)
  );

  // if (!hasAccess) {
  //   return router.createUrlTree(['/login']);
  // }
  if (hasAccess) {
    return true;
  }

  // Redirect based on role
  if (userRoles.includes('Admin')) {
    return router.createUrlTree(['/admin']);
  }

  if (userRoles.includes('Customer')) {
    return router.createUrlTree(['/home']);
  }


  return router.createUrlTree(['/login']);

};
