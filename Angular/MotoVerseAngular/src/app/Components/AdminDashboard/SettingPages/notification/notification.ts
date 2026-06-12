import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

@Component({
  selector: 'app-notification',
  imports: [CommonModule ],
  templateUrl: './notification.html',
  styleUrls: ['./notification.css', '../settings-page/settings-page.css'],

})
export class Notification {
   notifications = {
    email: true,
    sms: false,
    updates: true,
  };


  theme = 'Dark';
}
