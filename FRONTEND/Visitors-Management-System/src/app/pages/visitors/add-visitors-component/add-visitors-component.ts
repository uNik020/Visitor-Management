import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterModule } from '@angular/router';
import Swal from 'sweetalert2';
import { HostService } from '../../../services/Host/host.service';

@Component({
  selector: 'app-add-visitors-component',
  imports: [ReactiveFormsModule, RouterModule, CommonModule],
  templateUrl: './add-visitors-component.html',
  styleUrl: './add-visitors-component.css'
})
export class AddVisitorsComponent {
  visitorForm: FormGroup;
  hosts:any[] = [];

  constructor(private fb: FormBuilder,private hostService: HostService,private cd: ChangeDetectorRef) {
    this.visitorForm = this.fb.group({
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', [Validators.required, Validators.pattern('^[0-9]{10}$')]],
      address: [''],
      purpose: ['', Validators.required],
      hostId: ['', Validators.required],
      // checkInDate: ['', Validators.required],
      // checkInTime: ['', Validators.required]
    });
  }

  ngOnInit(): void {
      this.loadHosts();
    }
  
    loadHosts() {
      this.hostService.getHosts().subscribe(
        (data: any) => {
          setTimeout(() => {
            this.hosts = [...data];
            console.log(this.hosts);
            this.cd.detectChanges();
          },500);
        },
        (err) => Swal.fire('Error', err.message, 'error')
      );
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
