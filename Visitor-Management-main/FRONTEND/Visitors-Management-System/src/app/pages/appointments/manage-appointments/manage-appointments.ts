// manage-appointments.ts (Updated)
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { VisitorService } from '../../../services/Visitor/visitor.service';
import { HostService } from '../../../services/Host/host.service';
import Swal from 'sweetalert2';
import { CommonModule } from '@angular/common';
import { WebcamImage, WebcamInitError, WebcamModule, WebcamUtil } from 'ngx-webcam';
import { Observable, Subject } from 'rxjs';
import { SupabaseUpload } from '../../../services/supabase/supabase-upload';

@Component({
  selector: 'app-manage-appointments',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule, CommonModule, WebcamModule],
  templateUrl: './manage-appointments.html',
  styleUrl: './manage-appointments.css'
})
export class ManageAppointments implements OnInit {
  visitorForm: FormGroup;
  hosts: any[] = [];
  webcamImage: WebcamImage | null = null;
  showWebcam = true;
  trigger: Subject<void> = new Subject<void>();

  constructor(
    private fb: FormBuilder,
    private visitorService: VisitorService,
    private hostService: HostService,
    private uploadService: SupabaseUpload
  ) {
    this.visitorForm = this.fb.group({
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', [Validators.required, Validators.pattern('^[0-9]{10}$')]],
      address: [''],
      companyName: ['', Validators.required],
      purpose: ['', Validators.required],
      hostId: ['', Validators.required],
      expectedVisitDateTime: ['', Validators.required],
      idProofType: ['', Validators.required],
      idProofNumber: ['', Validators.required],
      comment: ['', Validators.required],
      licensePlateNumber: ['', Validators.required],
      photoUrl: [null, Validators.required],
      isPreRegistered: [true] // Pre-registered by default for appointments
    });
  }

  ngOnInit(): void {
    this.loadHosts();
    WebcamUtil.getAvailableVideoInputs().then(devices => {
      this.showWebcam = devices.length > 0;
    });
  }

  get triggerObservable(): Observable<void> {
    return this.trigger.asObservable();
  }

  captureImage(): void {
    this.trigger.next();
  }

  handleImage(webcamImage: WebcamImage): void {
    this.webcamImage = webcamImage;
    this.visitorForm.patchValue({ photoUrl: webcamImage.imageAsDataUrl });
  }

  clearImage(): void {
    this.webcamImage = null;
    this.visitorForm.patchValue({ photoUrl: null });
  }

  handleInitError(error: WebcamInitError): void {
    console.error('Webcam init error:', error);
  }

  loadHosts(): void {
    this.hostService.getHosts().subscribe(
      data => this.hosts = data,
      error => console.error('Error loading hosts:', error)
    );
  }

  setIdProofValidators(type: string): void {
    const control = this.visitorForm.get('idProofNumber');
    if (!control) return;
    switch (type) {
      case 'AadharCard':
        control.setValidators([Validators.required, Validators.pattern(/^\d{12}$/)]);
        break;
      case 'PanCard':
        control.setValidators([Validators.required, Validators.pattern(/^[A-Z]{5}[0-9]{4}[A-Z]{1}$/)]);
        break;
      case 'DrivingLicense':
        control.setValidators([Validators.required, Validators.pattern(/^[A-Z]{2}\d{13}$/)]);
        break;
      default:
        control.clearValidators();
        break;
    }
    control.updateValueAndValidity();
  }

  submitAppointment(): void {
    if (this.visitorForm.invalid) {
      this.visitorForm.markAllAsTouched();
      Swal.fire('Form Error', 'Please complete all required fields.', 'warning');
      return;
    }

    const formData = this.visitorForm.value;
    const base64Image = formData.photoUrl;

    if (!base64Image) {
      Swal.fire('Photo Missing', 'Please capture visitor photo before submitting.', 'error');
      return;
    }

    this.uploadService.uploadVisitorPhoto(base64Image).then(publicUrl => {
      formData.photoUrl = publicUrl;
      this.visitorService.addVisitor(formData).subscribe({
        next: () => {
          Swal.fire('Success', 'Appointment submitted successfully.', 'success');
          this.visitorForm.reset({ isPreRegistered: true });
          this.webcamImage = null;
        },
        error: err => Swal.fire('Error', err.error || 'Something went wrong.', 'error')
      });
    }).catch(err => {
      Swal.fire('Upload Error', 'Image upload failed: ' + err.message, 'error');
    });
  }

  get idProofType() {
    return this.visitorForm.get('idProofType');
  }

  get idProofNumber() {
    return this.visitorForm.get('idProofNumber');
  }
}
