import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, RouterModule],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class Register {
  showPassword = false;
showConfirmPassword = false;

togglePasswordVisibility() {
  this.showPassword = !this.showPassword;
}

toggleConfirmPasswordVisibility() {
  this.showConfirmPassword = !this.showConfirmPassword;
}

 registerForm: FormGroup;

  constructor(private fb: FormBuilder,private router: Router,private authService : AuthService) {

    this.registerForm = this.fb.group({
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      contact: ['', [Validators.required, Validators.pattern('^[0-9]{10}$')]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required]
    }, { validators: this.passwordMatchValidator });
  }

  passwordMatchValidator(form: FormGroup) {
    const pass = form.get('password')?.value;
    const confirm = form.get('confirmPassword')?.value;
    return pass === confirm ? null : { mismatch: true };
  }

  onSubmit(): void {
    if (this.registerForm.valid) {
      console.log('Registration Data', this.registerForm.value);
      const admin = {
        fullName : this.registerForm.value.fullName,
        email: this.registerForm.value.email,
        phoneNumber: this.registerForm.value.contact,
        passwordHash: this.registerForm.value.password
      };
      console.log(admin);
      // Call AuthService.register() here

      this.authService.registerAdmin(admin).subscribe(
      (data:any)=>{
        Swal.fire({
        title: 'Success!',
        text: 'Your registration was successful.',
        icon: 'success',
        confirmButtonText: 'Cool'
      });
        // console.log(data);
        sessionStorage.setItem('adminPass', data.passwordHash);
        sessionStorage.setItem('adminEmail', data.email);
        this.router.navigate(['/']);
      },
      (error) => {
        if (error.status === 500) {
          Swal.fire({
            title: 'Error!',
            text: 'Email-Id already registered.',
            icon: 'error',
            confirmButtonText: 'Ok'
          });
        } else {
          Swal.fire({
            title: 'Error!',
            text: 'An unexpected error occurred.',
            icon: 'error',
            confirmButtonText: 'Ok'
          });
        }
      }
    );
    }
  }
}