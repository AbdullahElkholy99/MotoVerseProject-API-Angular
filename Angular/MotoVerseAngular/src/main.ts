import { bootstrapApplication } from '@angular/platform-browser';

import { App } from './app/app';
import { provideRouter } from '@angular/router';
import { routes } from './app/app.routes';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideToastr } from 'ngx-toastr';
import { loaderInterceptor } from './app/Interceptor/loader-interceptor';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { authInterceptor } from './app/Interceptor/auth-interceptor';

bootstrapApplication(App, {
  providers: [
    provideRouter(routes),

    // configure toastr
    provideAnimations(),
    provideToastr({
      timeOut: 3000,
      positionClass: 'toast-top-right',
      preventDuplicates: true,
    }),

   provideHttpClient(
  withInterceptors([loaderInterceptor,authInterceptor])
),
  ]
})
.catch((err) => console.error(err));
