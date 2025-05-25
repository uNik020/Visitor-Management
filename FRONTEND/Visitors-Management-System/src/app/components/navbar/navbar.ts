import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import Swal from 'sweetalert2';
import { SessionStorageService } from '../../services/sessionStorage/session-storage-service';

@Component({
  selector: 'app-navbar',
  imports: [RouterModule, CommonModule],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css'
})
export class Navbar {

  constructor(private router: Router, private sessionStorage:SessionStorageService){}
   isMobileMenuOpen = false;
   isAdminLoggedIn: boolean = false;

   ngOnInit(): void {
  this.checkAdminLoginStatus();
}

checkAdminLoginStatus(): void {
  this.isAdminLoggedIn = this.sessionStorage.getItem('adminEmail') !== null;
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
