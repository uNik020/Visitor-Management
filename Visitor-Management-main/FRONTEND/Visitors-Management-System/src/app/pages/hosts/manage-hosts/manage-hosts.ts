import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ChangeDetectorRef } from '@angular/core';
import Swal from 'sweetalert2';
import { HostService } from '../../../services/Host/host.service';
import { DepartmentService } from '../../../services/Department/department.service';
import { DesignationService } from '../../../services/Designation/designation.service'; // You must create this service if not already created
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { WebcamImage, WebcamInitError, WebcamModule, WebcamUtil } from 'ngx-webcam';
import { Observable, Subject } from 'rxjs';
import { SupabaseUpload } from '../../../services/supabase/supabase-upload';

@Component({
  selector: 'app-manage-hosts',
  templateUrl: './manage-hosts.html',
  imports: [RouterModule, FormsModule, CommonModule, ReactiveFormsModule, WebcamModule],
  styleUrls: ['./manage-hosts.css'],
})
export class ManageHosts implements OnInit {
  hostForm: FormGroup;
  hosts: any[] = [];
  departments: any[] = [];
  designations: any[] = [];
  editingId: number | null = null;
  viewingHost: any = null;

  //webcam properties
  webcamImage: WebcamImage | null = null;
  showWebcam = true;
  multipleWebcamsAvailable = false;
  deviceId: string | undefined;
  errors: WebcamInitError[] = [];
    
  private trigger: Subject<void> = new Subject<void>();
  
  constructor(
    private fb: FormBuilder,
    private cd: ChangeDetectorRef,
    private hostService: HostService,
    private departmentService: DepartmentService,
    private uploadService: SupabaseUpload,
    private designationService: DesignationService
  ) {
    this.hostForm = this.fb.group({
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', [Validators.required, Validators.pattern('^[0-9]{10}$')]],
      departmentId: ['', Validators.required],
      designationId: ['', Validators.required],
      profilePictureUrl: [''],
      about: [''],
    });
  }

  ngOnInit(): void {
    this.loadHosts();
    this.loadDepartments();
    this.loadDesignations();

    WebcamUtil.getAvailableVideoInputs().then((mediaDevices: MediaDeviceInfo[]) => {
    this.multipleWebcamsAvailable = mediaDevices.length > 1;
    });
  }

  //webcam methods
   // Observable to trigger image capture
  get triggerObservable(): Observable<void> {
    return this.trigger.asObservable();
  }
  
  captureImage(): void {
    this.trigger.next();
  }
  
  handleImage(webcamImage: WebcamImage): void {
    this.webcamImage = webcamImage;
    this.hostForm.patchValue({ profilePictureUrl: webcamImage.imageAsDataUrl }); // Save Base64 image
  }
  
  handleInitError(error: WebcamInitError): void {
    this.errors.push(error);
    console.warn('Webcam error:', error);
  }
  
  clearImage(): void {
    this.webcamImage = null;
    this.hostForm.patchValue({ profilePictureUrl: null });
  }

  onViewHost(host: any): void {
  this.viewingHost = { ...host };

  console.log('Viewing Host:', this.viewingHost);
}

closeViewModal(): void {
  this.viewingHost = null;
}

  loadHosts() {
    this.hostService.getHosts().subscribe({
      next: (data) => {
        this.hosts = [...data];
        this.cd.detectChanges();
      },
      error: (err) => Swal.fire('Error', err.message, 'error'),
    });
  }

  loadDepartments() {
    this.departmentService.getDepartments().subscribe({
      next: (data:any) => {
        this.departments = [...data];
        this.cd.detectChanges();
      },
      error: (err) => Swal.fire('Error', err.message, 'error'),
    });
  }

  loadDesignations() {
    this.designationService.getDesignations().subscribe({
      next: (data:any) => {
        this.designations = [...data];
        this.cd.detectChanges();
      },
      error: (err) => Swal.fire('Error', err.message, 'error'),
    });
  }

onSubmit(): void {
  if (this.hostForm.invalid) {
    Swal.fire('Validation Failed', 'Please fill all required fields.', 'warning');
    return;
  }

  const hostData = this.hostForm.value;
  const base64Image = hostData.profilePictureUrl; // ✅ Correct form control name

  if (base64Image) {
    this.uploadService.uploadHostPhoto(base64Image).then((publicUrl) => {
      hostData.profilePictureUrl = publicUrl; // ✅ Upload to Supabase

      if (this.editingId !== null) {
        this.hostService.updateHost(hostData, this.editingId).subscribe({
          next: () => {
            Swal.fire('Success', 'Host updated successfully', 'success');
            this.loadHosts();
            this.cancelEdit();
          },
          error: (err) => Swal.fire('Error', err.message, 'error'),
        });
      } else {
        this.hostService.addHost(hostData).subscribe({
          next: () => {
            Swal.fire('Success', 'Host added successfully', 'success');
            this.loadHosts();
            this.hostForm.reset();
            this.webcamImage = null;
          },
          error: (err) => Swal.fire('Error', err.message, 'error'),
        });
      }
    }).catch((err) => {
      Swal.fire('Upload Error', 'Failed to upload host photo: ' + err.message, 'error');
    });
  } else {
    Swal.fire({
      icon: 'error',
      title: 'No Photo Captured',
      text: 'Please capture a host photo before submitting.',
    });
  }
}


  editHost(host: any): void {
    this.editingId = host.hostId;
    this.hostForm.setValue({
      fullName: host.fullName,
      email: host.email,
      phoneNumber: host.phoneNumber,
      departmentId: host.departmentId,
      designationId: host.designationId,
      profilePictureUrl: host.profilePictureUrl || '',
      about: host.about || '',
    });
  }

  deleteHost(id: number): void {
    Swal.fire({
      title: 'Are you sure?',
      text: 'This will delete the host!',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Yes, delete it!',
    }).then((result) => {
      if (result.isConfirmed) {
        this.hostService.deleteHost(id).subscribe({
          next: () => {
            Swal.fire('Deleted!', 'Host has been deleted.', 'success');
            this.loadHosts();
          },
          error: (err) => Swal.fire('Error', err.message, 'error'),
        });
      }
    });
  }

  cancelEdit(): void {
    this.editingId = null;
    this.hostForm.reset();
  }
}
