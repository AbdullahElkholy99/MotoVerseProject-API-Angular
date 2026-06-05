import { Routes } from '@angular/router';
import { loginGuard } from './guards/login-guard';
import { roleGuard } from './guards/role-guard';
import { authGuard } from './guards/auth-guard';

export const routes: Routes = [
  // Default Route
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },

  // AUTH PAGES
  {
    path: 'login',
    loadComponent: () =>
      import('./Components/Auth/login-register/login-register')
        .then((m) => m.LoginRegister),
    canActivate: [loginGuard]
  },

  {
    path: 'register',
    loadComponent: () =>
      import('./Components/Auth/register-page/register-page')
        .then((m) => m.RegisterPage),
    // canActivate: [loginGuard]
  },
  {
    path: 'confirm-email',
    loadComponent: () =>
      import('./Components/Auth/confirm-email-page/confirm-email-page')
        .then(m => m.ConfirmEmailPage)
  },
  {
    path: 'forgot-password',
    loadComponent: () =>
      import('./Components/Auth/forgot-password-page/forgot-password-page')
        .then((m) => m.ForgotPasswordPage),
  },



  // ADMIN DASHBOARD
  {
    path: 'admin',
    loadComponent: () =>
      import('./Components/AdminDashboard/dashboard-layout/dashboard-layout')
        .then((m) => m.DashboardLayout),

    canActivate: [authGuard, roleGuard],
    data: { roles: ['Admin'] },


    children: [
      {
        path: 'dashboard',
        loadComponent: () =>
          import('./Components/AdminDashboard/dashboard-page/dashboard-page')
            .then((m) => m.DashboardPage)
      },
      {
        path: 'overview',
        loadComponent: () =>
          import('./Components/AdminDashboard/overview/overview')
            .then((m) => m.AdminOverview)
      },

      {
        path: 'products',
        loadComponent: () =>
          import('./Components/AdminDashboard/ProductPages/products-page')
            .then((m) => m.ProductsPage)
      },

      {
        path: 'categories',
        loadComponent: () =>
          import('./Components/AdminDashboard/CategoryPages/categories-page/categories-page')
            .then((m) => m.CategoriesPage)
      },

      {
        path: 'orders',
        loadComponent: () =>
          import('./Components/AdminDashboard/OrderPages/orders-page/orders-page')
            .then((m) => m.OrdersPage)
      },

      {
        path: 'bookings',
        loadComponent: () =>
          import('./Components/AdminDashboard/bookings/bookings')
            .then((m) => m.AdminBookings)
      },

      {
        path: 'payments',
        loadComponent: () =>
          import('./Components/AdminDashboard/payments/payments')
            .then((m) => m.AdminPayments)
      },

      {
        path: 'reports',
        loadComponent: () =>
          import('./Components/AdminDashboard/reports/reports')
            .then((m) => m.AdminReports)
      },

      {
        path: 'customers',
        loadComponent: () =>
          import('./Components/AdminDashboard/UsersPages/CustomerPages/customers-page/customers-page')
            .then((m) => m.CustomersPage)
      },

      {
        path: 'analytics',
        loadComponent: () =>
          import('./Components/AdminDashboard/analytics/analytics-page/analytics-page')
            .then((m) => m.AnalyticsPage)
      },

      {
        path: 'settings',
        loadComponent: () =>
          import('./Components/AdminDashboard/SettingPages/settings-page/settings-page')
            .then((m) => m.SettingsPage)
      },

      {
        path: 'motorcycles',
        loadComponent: () =>
          import('./Components/AdminDashboard/MotorcyclePages/motorcycle-page/motorcycle-page')
            .then((m) => m.MotorcyclePage)
      },

      {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full'
      }
    ]
  },

  // Customer Dashboard
  {
    path: 'home',
    loadComponent: () =>
      import('./Components/CustomerDashboard/landing-page/landing-page')
        .then((m) => m.LandingPage),

    canActivate: [authGuard, roleGuard],
    data: { roles: ['User'] },


    children: [
      // CUSTOMER BOOKING TRACKING
      {
        path: 'my-bookings',
        loadComponent: () =>
          import('./Components/CustomerDashboard/BookingTracking/booking-tracking/booking-tracking')
            .then((m) => m.BookingTracking)
      },
      {
        path: '',
        loadComponent: () =>
          import('./Components/CustomerDashboard/home-page/home-page')
            .then((m) => m.HomePage)
      },

      {
        path: 'aboutUs',
        loadComponent: () =>
          import('./Components/Bases/about-us/about-us')
            .then((m) => m.AboutUs)
      },

      {
        path: 'motorcycle',
        loadComponent: () =>
          import('./Components/CustomerDashboard/MotorcycleCompnents/motorcycle/motorcycle')
            .then((m) => m.Motorcycle)
      },

      {
        path: 'products',
        loadComponent: () =>
          import('./Components/CustomerDashboard/products/products')
            .then((m) => m.Products) ,
      },
  {
                path: 'product-details/:id',
                loadComponent: () =>
                  import('./Components/CustomerDashboard/products/product-details/product-details')
                    .then((m) => m.ProductDetails)
              },
      {
        path: 'motorcycle/rental/:id',
        loadComponent: () =>
          import('./Components/CustomerDashboard/MotorcycleCompnents/motorcycle-rental/motorcycle-rental')
            .then((m) => m.MotorcycleRental)
      },

      {
        path: 'my-bookings',
        loadComponent: () =>
          import('./Components/CustomerDashboard/BookingTracking/booking-tracking/booking-tracking')
            .then((m) => m.BookingTracking)
      }
    ]
  },

  // NOT FOUND
  // {
  //   path: '**',
  //   loadComponent: () =>
  //     import('./Components/CustomerDashboard/landing-page/landing-page')
  //       .then((m) => m.LandingPage),

  // }
];
