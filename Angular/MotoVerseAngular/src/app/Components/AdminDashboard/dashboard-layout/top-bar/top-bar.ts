import { NgIf } from '@angular/common';
import { Component, EventEmitter, inject, OnInit, Output, signal } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../../../services/Auth/auth.service';

@Component({
  selector: 'app-top-bar',
  imports: [NgIf],
  templateUrl: './top-bar.html',
  styleUrl: './top-bar.css',
})
export class TopBar implements OnInit {

private _authService = inject(AuthService)
private toastr = inject(ToastrService)

  @Output() sidebarOpen = new EventEmitter<boolean>(false);
  profileSettingToggle = signal<boolean>(false)

  isLoggin=false
  image = ''
  name = ''
  constructor(private router:Router){
    if(localStorage.getItem('token')) this.isLoggin = true
  }
  ngOnInit(): void {
    const userInfo = JSON.parse(localStorage.getItem("userInfo") || '')
    this.image = userInfo[0].imagePath
    this.name = userInfo[0].displayName
  }


  toggleSidebar() {
    this.sidebarOpen.emit(true);
  }

  //---------------------- toggle menu top bar profile
  toggleProfile(){
    this.profileSettingToggle.update((value)=>!value);
  }



  login() {
    this.router.navigate(['/login']);
  }

  register() {
    this.router.navigate(['/register']);
  }

  logout() {
    // logout logic
    this._authService.logout().subscribe({
      next:(res)=>{
        if(!res.succeeded)
        {
          this.toastr.error(res.errors,"error")
          return;
        }
        this.toastr.success(res.message,"success")
        this.router.navigate(['/login']);
        localStorage.removeItem('token');
        localStorage.removeItem('userInfo');
      },
      error:(err)=>{
          this.toastr.error(err.errors,"error")
      }
    })

  }

}
