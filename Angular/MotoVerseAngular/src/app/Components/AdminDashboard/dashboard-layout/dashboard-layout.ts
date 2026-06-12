import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Tooltip } from '../../../directives/tooltip';
import { TopBar } from "./top-bar/top-bar";

@Component({
  selector: 'app-dashboard-layout',
  standalone: true,
  imports: [CommonModule, RouterModule, Tooltip, TopBar],
  templateUrl: './dashboard-layout.html',
  styleUrls: ['./dashboard-layout.css'],
})
export class DashboardLayout {
  sidebarOpen = signal(false);

  navItems = [
    { label: 'Dashboard', link: '/admin/dashboard', icon: 'ri-dashboard-line' },
    { label: 'Categories', link: '/admin/categories', icon: 'ri-apps-2-line' },
    { label: 'Products', link: '/admin/products', icon: 'ri-store-2-line' },
    // { label: 'Orders', link: '/admin/orders', icon: 'ri-shopping-bag-2-line' },
    { label: 'Customers', link: '/admin/customers', icon: 'ri-user-3-line' },
    { label: 'Motorcycle', link: '/admin/motorcycles', icon: 'ri-motorbike-fill' },
    { label: 'Bookings', link: '/admin/bookings', icon: 'ri-calendar-check-line' },
    // { label: 'Reviews', link: '/admin/reviews', icon: 'ri-chat-3-line' },
    { label: 'Settings', link: '/admin/settings', icon: 'ri-settings-4-line' },
  ];

  // Method to toggle the sidebar open/close state
  toggleSidebar() {
    this.sidebarOpen.update((value) => !value);
  }

}
