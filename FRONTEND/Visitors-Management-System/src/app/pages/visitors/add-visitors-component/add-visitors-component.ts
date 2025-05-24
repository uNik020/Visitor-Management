import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterModule } from '@angular/router';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-add-visitors-component',
  imports: [ReactiveFormsModule, RouterModule, CommonModule],
  templateUrl: './add-visitors-component.html',
  styleUrl: './add-visitors-component.css'
})
export class AddVisitorsComponent {
  visitorForm: FormGroup;

  constructor(private fb: FormBuilder) {
    this.visitorForm = this.fb.group({
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      contact: ['', [Validators.required, Validators.pattern('^[0-9]{10}$')]],
      address: [''],
      purpose: ['', Validators.required],
      hostName: ['', Validators.required],
      // checkInDate: ['', Validators.required],
      // checkInTime: ['', Validators.required]
    });
  }

  onSubmit(): void {
    if (this.visitorForm.valid) {
      console.log('Visitor Data:', this.visitorForm.value);
      // TODO: Send data to backend

      Swal.fire({
        toast: true,
        position: 'top-end',
        icon: 'success',
        title: 'Visitor added successfully!',
        showConfirmButton: false,
        timer: 2000
      });

      this.visitorForm.reset();
    } else {
      this.visitorForm.markAllAsTouched(); // Show validation errors immediately
      Swal.fire({
        icon: 'error',
        title: 'Form Incomplete',
        text: 'Please fill all required fields correctly.',
      });
    }
  }
}
