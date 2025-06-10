import { Component } from '@angular/core';
import { AuthService } from '../../../services/auth/auth.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, FormsModule],
  templateUrl: './forgot-password.html',
  styleUrl: './forgot-password.css'
})
export class ForgotPassword {
  email: string = '';
  otp: string = '';
  password: string = '';
  confirmPassword: string = '';

  showPassword: boolean = false;
  showConfirmPassword: boolean = false;

  // error messages
  emailError: string = '';
  otpError: string = '';
  passwordError: string = '';
  confirmPasswordError: string = '';

  constructor(private authService: AuthService) {}

 showStep(step: string) {
  const steps = ['emailStep', 'otpStep', 'resetStep', 'doneStep'];
  steps.forEach(id => {
    const el = document.getElementById(id);
    if (el) {
      el.classList.toggle('hidden', id !== step);
    }
  });
}


  toggleVisibility(id: string) {
    const input = document.getElementById(id) as HTMLInputElement;
    input.type = input.type === 'password' ? 'text' : 'password';
  }

  restartFlow() {
    this.emailError = '';
    this.otpError = '';
    this.passwordError = '';
    this.confirmPasswordError = '';
    this.showStep('emailStep');
  }

  onSendOtp(event: Event) {
  event.preventDefault();
  this.emailError = '';

  if (!this.email) {
    this.emailError = 'Email is required.';
    return;
  }

  this.authService.sendResetCode({ email: this.email }).subscribe({
    next: (response: string) => {
      console.log('Response:', response); // Optional: For debugging
      Swal.fire('Success', response, 'success');
      this.showStep('otpStep');
    },
    error: (err) => {
      console.error('Send OTP error:', err);
      this.emailError = 'Something went wrong while sending OTP.';
      Swal.fire('Error', this.emailError, 'error');
    }
  });
}


  onVerifyOtp(event: Event) {
    event.preventDefault();
    this.otpError = '';

    if (!this.otp) {
      this.otpError = 'OTP is required.';
      return;
    }

    this.authService.verifyResetCode({ email: this.email, secretCode: this.otp }).subscribe({
      next: () => {
        Swal.fire('Verified', 'OTP verified successfully!', 'success');
        this.showStep('resetStep');
      },
      error: () => {
        this.otpError = 'Invalid or expired OTP.';
        Swal.fire('Error', this.otpError, 'error');
      }
    });
  }

  onResetPassword(event: Event) {
    event.preventDefault();
    this.passwordError = '';
    this.confirmPasswordError = '';

    if (!this.password || !this.confirmPassword) {
      this.passwordError = 'All fields are required.';
      return;
    }

    if (this.password !== this.confirmPassword) {
      this.confirmPasswordError = 'Passwords do not match.';
      return;
    }

    this.authService.resetPassword({
      email: this.email,
      secretCode: this.otp,
      newPassword: this.password,
      confirmPassword: this.confirmPassword
    }).subscribe({
      next: () => {
        Swal.fire('Done!', 'Password reset successfully!', 'success');
        this.showStep('doneStep');
      },
      error: () => {
        this.passwordError = 'Failed to reset password.';
        Swal.fire('Error', this.passwordError, 'error');
      }
    });
  }
}
