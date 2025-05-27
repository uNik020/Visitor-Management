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
   // Forgot password - Send OTP
  public sendResetCode(emailObj: { email: string }) {
    return this.httpClient.post(`${baseurl}/Auth/forgot-password`, emailObj,{responseType:'text'});
  }

  // Verify OTP
  public verifyResetCode(payload: { email: string; secretCode: string }) {
    return this.httpClient.post(`${baseurl}/Auth/verify-reset-code`, payload);
  }

  // Reset Password
  public resetPassword(payload: {
    email: string;
    secretCode: string;
    newPassword: string;
    confirmPassword: string;
  }) {
    return this.httpClient.post(`${baseurl}/Auth/reset-password`, payload);
  }
}
