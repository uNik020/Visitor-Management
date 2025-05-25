import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { baseurl } from '../../baseURL';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

    constructor(private httpClient : HttpClient) { }
 
    //register 
  public registerAdmin(admin : any ){
    return this.httpClient.post(`${baseurl}/Auth/`,admin);
 
  }
    //login 
  public loginAdmin(adminLogin : any ){
    return this.httpClient.post(`${baseurl}/Auth/login`,adminLogin);
  }
    //forget password
  public forgetPassword(forgotPass : any ){
    return this.httpClient.post(`${baseurl}/Auth/forgotpassword`,forgotPass);
  }

}
