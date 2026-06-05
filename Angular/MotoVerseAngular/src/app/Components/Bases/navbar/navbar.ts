import { Component, inject, signal } from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { ShoppingCart } from "../../CustomerDashboard/ShoppingComponents/shopping-cart/shopping-cart";
 import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../../services/Auth/auth.service';

@Component({
  selector: 'app-navbar',
  imports: [RouterLink, RouterLinkActive, ShoppingCart],
  templateUrl: './navbar.html',
  styleUrls: ['./navbar.css', '../../styleBoke.css']
})
export class Navbar {


private _authService = inject(AuthService)
private toastr = inject(ToastrService)


  showCart = signal(false);
  showProfileMenu = signal(false);
  isLoggedIn = signal(false);
  isMenuOpen = signal(false);


  constructor(private router:Router){
    if(localStorage.getItem('token')) this.isLoggedIn.set( true)
  }


  toggleCart() {
    this.showCart.update(v => !v);
  }

  closeCart() {
    this.showCart.set(false);
  }


  toggleMenu() {
    this.isMenuOpen.update(v => !v);
  }




  toggleProfileMenu() {
    this.showProfileMenu.update(v => !v);
  }

  closeProfileMenu() {
    this.showProfileMenu.set(false);
  }



  login() {
    this.router.navigate(['/login']);
  }

  register() {
    this.router.navigate(['/register']);
  }

  logout() {
        this.isLoggedIn.set(false);
    this.closeProfileMenu();
    // logout logic
    this._authService.logout().subscribe({
      next:(res)=>{
        if(!res.succeeded)
        {
          this.toastr.error(res.errors,"error")
          return;
        }
        console.log(res);

        this.toastr.success(res.data,"success")
        localStorage.removeItem('token');
        localStorage.removeItem('userInfo');
        this.router.navigate(['/login']);
      },
      error:(err)=>{
          this.toastr.error(err.errors,"error")
      }
    })

  }
}
