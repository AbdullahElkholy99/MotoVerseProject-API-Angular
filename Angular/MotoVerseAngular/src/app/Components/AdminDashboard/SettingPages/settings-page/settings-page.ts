import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-settings-page',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './settings-page.html',
  styleUrls: ['./settings-page.css'],
})
export class SettingsPage {
  profile = {
    name: 'E-Shop Admin',
    email: 'admin@eshop.com',
    phone: '+1 555-123-4567',
  };

  password = {
    current: '',
    newPassword: '',
    confirm: '',
  };

  notifications = {
    email: true,
    sms: false,
    updates: true,
  };

  theme = 'Dark';
}
