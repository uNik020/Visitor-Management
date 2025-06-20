import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { AppointmentService } from '../../../services/Appointment/appointment-service';
import { VisitorService } from '../../../services/Visitor/visitor.service';
import { HostService } from '../../../services/Host/host.service';
import Swal from 'sweetalert2';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-manage-appointments',
  imports: [FormsModule, ReactiveFormsModule, CommonModule],
  templateUrl: './manage-appointments.html',
  styleUrl: './manage-appointments.css'
})
export class ManageAppointments implements OnInit {
  appointmentForm: FormGroup;
  visitorForm: FormGroup;
  visitors: any[] = [];
  hosts: any[] = [];
  showAddVisitorForm = false;
  selectedVisitorId: number | null = null;

  constructor(
    private fb: FormBuilder,
    private appointmentService: AppointmentService,
    private visitorService: VisitorService,
    private hostService: HostService
  ) {
    this.appointmentForm = this.fb.group({
      visitorId: ['', Validators.required],
      hostId: ['', Validators.required],
      scheduledVisitDateTime: ['', Validators.required],
      visitPurpose: ['', Validators.required],
    });

    this.visitorForm = this.fb.group({
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', [Validators.required, Validators.pattern('^[0-9]{10}$')]],
      address: ['']
    });
  }

  ngOnInit(): void {
    this.loadVisitors();
    this.loadHosts();
  }

  loadVisitors() {
    this.visitorService.getVisitor().subscribe(
      (data) => this.visitors = data,
      (error) => console.error('Error loading visitors:', error)
    );
  }

  loadHosts() {
    this.hostService.getHosts().subscribe(
      (data) => this.hosts = data,
      (error) => console.error('Error loading hosts:', error)
    );
  }

  toggleAddVisitor() {
    this.showAddVisitorForm = !this.showAddVisitorForm;
  }

  submitVisitor() {
    if (this.visitorForm.valid) {
      this.visitorService.addVisitor(this.visitorForm.value).subscribe({
        next: (response) => {
          Swal.fire('Success', 'Visitor added successfully.', 'success');
          this.loadVisitors();
          this.showAddVisitorForm = false;
        },
        error: (err) => Swal.fire('Error', err.error, 'error')
      });
    } else {
      Swal.fire('Form Error', 'Please fill all required visitor details correctly.', 'warning');
    }
  }

  submitAppointment() {
    if (this.appointmentForm.valid) {
      this.appointmentService.createAppointment(this.appointmentForm.value).subscribe({
        next: () => {
          Swal.fire('Success', 'Appointment created successfully!', 'success');
          this.appointmentForm.reset();
        },
        error: (err) => Swal.fire('Error', err.error, 'error')
      });
    } else {
      Swal.fire('Error', 'Please complete all required appointment fields.', 'error');
    }
  }
}