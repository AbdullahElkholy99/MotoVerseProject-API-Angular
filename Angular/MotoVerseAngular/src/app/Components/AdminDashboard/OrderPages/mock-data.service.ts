import { Injectable, computed, signal } from '@angular/core';
import { Product } from '../../../models/product';
import { Order } from '../../../models/order';
import { Customer } from '../../../models/customer';
import { RevenuePoint, TrafficSource } from '../../../models/analytics';

@Injectable({ providedIn: 'root' })
export class MockDataService {
  readonly products = signal<Product[]>([
    {
      id: 'p1',
      image: 'https://images.unsplash.com/photo-1512436991641-6745cdb1723f?auto=format&fit=crop&w=400&q=80',
      name: 'Urban Trail Sneakers',
      price: 129.99,
      category: 'Footwear',
      stock: 42,
      rating: 4.8,
      status: 'Active',
    },
    {
      id: 'p2',
      image: 'https://images.unsplash.com/photo-1523275335684-37898b6baf30?auto=format&fit=crop&w=400&q=80',
      name: 'Wireless Headphones',
      price: 199.0,
      category: 'Electronics',
      stock: 18,
      rating: 4.6,
      status: 'Active',
    },
    {
      id: 'p3',
      image: 'https://images.unsplash.com/photo-1483985988355-763728e1935b?auto=format&fit=crop&w=400&q=80',
      name: 'Leather Desk Organizer',
      price: 59.5,
      category: 'Accessories',
      stock: 76,
      rating: 4.2,
      status: 'Active',
    },
    {
      id: 'p4',
      image: 'https://images.unsplash.com/photo-1526170375885-4d8ecf77b99f?auto=format&fit=crop&w=400&q=80',
      name: 'Smart Watch Pro',
      price: 249.0,
      category: 'Wearables',
      stock: 12,
      rating: 4.7,
      status: 'Out of Stock',
    },
    {
      id: 'p5',
      image: 'https://images.unsplash.com/photo-1494526585095-c41746248156?auto=format&fit=crop&w=400&q=80',
      name: 'Minimal Office Lamp',
      price: 89.99,
      category: 'Home',
      stock: 33,
      rating: 4.4,
      status: 'Active',
    },
  ]);

  readonly orders = signal<Order[]>([
    {
      id: 'ORD-1029',
      customer: 'Emma Roberts',
      date: '2026-05-09',
      totalPrice: 410.5,
      paymentStatus: 'Paid',
      deliveryStatus: 'Shipped',
    },
    {
      id: 'ORD-1030',
      customer: 'Noah Carter',
      date: '2026-05-10',
      totalPrice: 129.99,
      paymentStatus: 'Pending',
      deliveryStatus: 'Processing',
    },
    {
      id: 'ORD-1031',
      customer: 'Olivia Smith',
      date: '2026-05-11',
      totalPrice: 299.0,
      paymentStatus: 'Paid',
      deliveryStatus: 'Delivered',
    },
    {
      id: 'ORD-1032',
      customer: 'Ava Martinez',
      date: '2026-05-11',
      totalPrice: 79.99,
      paymentStatus: 'Refunded',
      deliveryStatus: 'Canceled',
    },
    {
      id: 'ORD-1033',
      customer: 'Liam Johnson',
      date: '2026-05-12',
      totalPrice: 220.0,
      paymentStatus: 'Paid',
      deliveryStatus: 'Delivered',
    },
  ]);

  readonly customers = signal<Customer[]>([
    {
      id: 'c1',
      avatar: 'https://images.unsplash.com/photo-1544005313-94ddf0286df2?auto=format&fit=crop&w=200&q=80',
      name: 'Sophia Brown',
      email: 'sophia@example.com',
      phone: '+1 555-916-4835',
      orders: 8,
      status: 'Active',
    },
    {
      id: 'c2',
      avatar: 'https://images.unsplash.com/photo-1506794778202-cad84cf45f1d?auto=format&fit=crop&w=200&q=80',
      name: 'Mason Lee',
      email: 'mason@example.com',
      phone: '+1 555-782-6437',
      orders: 5,
      status: 'Paused',
    },
    {
      id: 'c3',
      avatar: 'https://images.unsplash.com/photo-1544723795-3fb6469f5b39?auto=format&fit=crop&w=200&q=80',
      name: 'Isabella Walker',
      email: 'isabella@example.com',
      phone: '+1 555-628-1910',
      orders: 12,
      status: 'Active',
    },
    {
      id: 'c4',
      avatar: 'https://images.unsplash.com/photo-1500648767791-00dcc994a43e?auto=format&fit=crop&w=200&q=80',
      name: 'Ethan Davis',
      email: 'ethan@example.com',
      phone: '+1 555-494-2214',
      orders: 3,
      status: 'New',
    },
  ]);

  readonly revenueSeries = signal<RevenuePoint[]>([
    { month: 'Jan', value: 21000 },
    { month: 'Feb', value: 24000 },
    { month: 'Mar', value: 28000 },
    { month: 'Apr', value: 32000 },
    { month: 'May', value: 36000 },
    { month: 'Jun', value: 41000 },
  ]);

  readonly trafficSources = signal<TrafficSource[]>([
    { source: 'Organic Search', percent: 38, color: '#4F46E5' },
    { source: 'Social Ads', percent: 27, color: '#0EA5E9' },
    { source: 'Email', percent: 18, color: '#16A34A' },
    { source: 'Referral', percent: 17, color: '#F97316' },
  ]);

  readonly categories = signal<string[]>(['All', 'Footwear', 'Electronics', 'Accessories', 'Wearables', 'Home']);

  readonly totalRevenue = computed(() => this.orders().reduce((sum, order) => sum + order.totalPrice, 0));
  readonly totalOrders = computed(() => this.orders().length);
  readonly totalCustomers = computed(() => this.customers().length);
  readonly totalProducts = computed(() => this.products().length);

  readonly topSellingProducts = computed(() => this.products().slice(0, 4));
  readonly recentOrders = computed(() => this.orders().slice(0, 5));
  readonly recentCustomers = computed(() => this.customers().slice(0, 4));
  readonly orderStatuses = computed(() => [
    { label: 'Delivered', value: this.orders().filter((item) => item.deliveryStatus === 'Delivered').length, accent: 'success' },
    { label: 'Processing', value: this.orders().filter((item) => item.deliveryStatus === 'Processing').length, accent: 'warning' },
    { label: 'Shipped', value: this.orders().filter((item) => item.deliveryStatus === 'Shipped').length, accent: 'info' },
    { label: 'Canceled', value: this.orders().filter((item) => item.deliveryStatus === 'Canceled').length, accent: 'danger' },
  ]);
}
