import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { NgxSpinnerModule } from "ngx-spinner";
import { getUserInfo } from './Helpers/decode-jwt';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet
    , PaginationModule,
    NgxSpinnerModule


  ],
  templateUrl: './app.html',
  styleUrls: ['./app.css'],
})
export class App implements OnInit {

  image = ''
  ngOnInit(): void {
    const userInfo = getUserInfo()
    if (userInfo.length > 0) {
      this.image = `https://localhost:7081/${userInfo[0].imagePath}`
      localStorage.setItem("userInfo", JSON.stringify(userInfo))
    }
  }
}
