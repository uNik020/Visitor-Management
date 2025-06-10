import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import Swal from 'sweetalert2';
import { SessionStorageService } from '../sessionStorage/session-storage-service';

@Injectable({
  providedIn: 'root'
})
export class RouteGuardService {

  constructor(private route:Router,private sessionStorage :SessionStorageService) { }

   public isAdminLoggedIn(){
    let admin=this.sessionStorage.getItem('adminEmail')
    return !(admin==null)
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
  if(this.isAdminLoggedIn()){
    return true;
  }else{
    Swal.fire({
      icon: 'error',
      title: 'Oops...',
      text: 'Admin not Logged-In!',
    });
    this.route.navigate(['']);
    return false;
  }
  
  
  }
}
