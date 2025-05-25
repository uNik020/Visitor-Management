import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-navbar',
  imports: [RouterModule, CommonModule],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css'
})
export class Navbar {

  constructor(private router: Router){}
   isMobileMenuOpen = false;
   isAdminLoggedIn: boolean = false;

   ngOnInit(): void {
  this.checkAdminLoginStatus();
}

checkAdminLoginStatus(): void {
  this.isAdminLoggedIn = sessionStorage.getItem('adminEmail') !== null;
}


  toggleMobileMenu(): void {
    this.isMobileMenuOpen = !this.isMobileMenuOpen;
  }

  logoutAdmin(){
    sessionStorage.clear();
    this.isAdminLoggedIn = false;
    this.router.navigate(['']);

     Swal.fire({
            icon: 'success',
            title: 'Log out',
            text: 'Logged Out',
            confirmButtonText: 'Ok'
          });
  }


}
