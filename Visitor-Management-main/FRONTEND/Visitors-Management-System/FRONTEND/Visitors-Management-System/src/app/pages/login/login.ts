import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';
import Swal from 'sweetalert2';
import { SessionStorageService } from '../../services/sessionStorage/session-storage-service';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, CommonModule ,RouterModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {
  loginForm: FormGroup;

  showPassword = false;

  togglePasswordVisibility() {
  this.showPassword = !this.showPassword;
  }

  constructor(private fb: FormBuilder,private sessionStorage:SessionStorageService, private router: Router,private authService : AuthService) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

  onSubmit(): void {
    if (this.loginForm.valid) {
      console.log('Form Submitted', this.loginForm.value);
      // Call AuthService here

      this.authService.loginAdmin(this.loginForm.value).subscribe(
        (response: any) => {
          Swal.fire({
            icon: 'success',
            title: 'Login Successful',
            text: 'Welcome back!',
            confirmButtonText: 'Continue'
          });

          // Store session values or tokens as needed
          this.sessionStorage.setItem('adminEmail', response.email);
          this.sessionStorage.setItem('adminPass', response.passwordHash); // if your API returns it

          this.router.navigate(['/admin/dashboard']); // navigate to your desired route
        },
        (error) => {
          console.error('Login error:', error);
          let message = 'An unexpected error occurred. Please try again.';

          if (error.status === 401) {
            message = 'Invalid email or password.';
          }

          Swal.fire({
            icon: 'error',
            title: 'Login Failed',
            text: message,
            confirmButtonText: 'Retry'
          });
        }
      );
      
    }
  }
}