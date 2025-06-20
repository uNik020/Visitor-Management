import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component } from '@angular/core';
import {
  FormArray,
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import Swal from 'sweetalert2';
import { HostService } from '../../../services/Host/host.service';
import { VisitorService } from '../../../services/Visitor/visitor.service';
import { QRCodeComponent } from 'angularx-qrcode';
import { ViewChild, ElementRef } from '@angular/core';
import { WebcamImage, WebcamInitError, WebcamModule, WebcamUtil } from 'ngx-webcam';
import { Subject, Observable } from 'rxjs';
import { SupabaseUpload } from '../../../services/supabase/supabase-upload';


@Component({
  selector: 'app-add-visitors-component',
  standalone: true,
  imports: [ReactiveFormsModule, RouterModule, CommonModule, QRCodeComponent, WebcamModule],
  templateUrl: './add-visitors-component.html',
  styleUrl: './add-visitors-component.css',
})
export class AddVisitorsComponent {
  visitorForm: FormGroup;
  hosts: any[] = [];
  photoError: string | null = null;
  passCode: string = '';
  qrCodeData: string = '';
  companionCount: number = 0;
  webcamImage: WebcamImage | null = null;
showWebcam = true;
multipleWebcamsAvailable = false;
deviceId: string | undefined;
errors: WebcamInitError[] = [];

private trigger: Subject<void> = new Subject<void>();



  // Getter methods for form controls
  get idProofType() {
  return this.visitorForm.get('idProofType');
}

// Getter for idProofNumber control
get idProofNumber() {
  return this.visitorForm.get('idProofNumber');
}

  constructor(
    private visitorService: VisitorService,
    private fb: FormBuilder,
    private hostService: HostService,
    private cd: ChangeDetectorRef,
    private router: Router,
    private uploadService: SupabaseUpload
  ) {
    this.visitorForm = this.fb.group({
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: [
        '',
        [Validators.required, Validators.pattern('^[0-9]{10}$')],
      ],
      address: [''],
      purpose: ['', Validators.required],
      hostId: ['', Validators.required],
      companyName: [''],
      comment: [''],
       idProofType: ['', Validators.required],
      idProofNumber: ['', Validators.required],
      licensePlateNumber: [''],
      isPreRegistered: [false],
      expectedVisitDateTime: [''],
      photoUrl: [null],
      passCode: [''],
      qrCodeData: [''],
      companions: this.fb.array([]),
    });
  }

  ngOnInit(): void {
    this.loadHosts();
    this.generatePassCode();

    WebcamUtil.getAvailableVideoInputs().then((mediaDevices: MediaDeviceInfo[]) => {
    this.multipleWebcamsAvailable = mediaDevices.length > 1;
    });

    this.visitorForm
      .get('isPreRegistered')
      ?.valueChanges.subscribe((isChecked) => {
        const expectedVisitControl = this.visitorForm.get(
          'expectedVisitDateTime'
        );
        if (isChecked) {
          expectedVisitControl?.setValidators([Validators.required]);
        } else {
          expectedVisitControl?.clearValidators();
        }
        expectedVisitControl?.updateValueAndValidity();
      });

      this.visitorForm.get('idProofType')?.valueChanges.subscribe((selectedType) => {
      this.setIdProofValidators(selectedType);
    });
  }

  get companionForms(): FormArray {
    return this.visitorForm.get('companions') as FormArray;
  }

  

  generatePassCode() {
    const chars = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789';
    this.passCode = Array.from({ length: 8 }, () =>
      chars.charAt(Math.floor(Math.random() * chars.length))
    ).join('');

    this.visitorForm.patchValue({ passCode: this.passCode });
  }

  onPhotoSelected(event: Event): void {
    const file = (event.target as HTMLInputElement)?.files?.[0];
    if (file) {
      if (!file.type.startsWith('image/')) {
        this.photoError = 'Only image files are allowed.';
        return;
      }

      const reader = new FileReader();
      reader.onload = () => {
        this.visitorForm.patchValue({ photoUrl: reader.result }); // Base64 string
        this.photoError = null;
      };
      reader.readAsDataURL(file);
    }
  }

  createCompanionForm(): FormGroup {
    return this.fb.group({
      fullName: ['', Validators.required],
      contactNumber: [''],
      email: [''],
    });
  }

  onCompanionCountChange(count: string): void {
    const parsedCount = parseInt(count, 10);
    this.companionCount = parsedCount;

    const formArray = this.companionForms;

    while (formArray.length < parsedCount) {
      formArray.push(this.createCompanionForm());
    }
    while (formArray.length > parsedCount) {
      formArray.removeAt(formArray.length - 1);
    }
  }

  loadHosts() {
    this.hostService.getHosts().subscribe(
      (data: any) => {
        setTimeout(() => {
          this.hosts = [...data];
          this.cd.detectChanges();
        }, 500);
      },
      (err) => Swal.fire('Error', err.message, 'error')
    );
  }

  // Observable to trigger image capture
get triggerObservable(): Observable<void> {
  return this.trigger.asObservable();
}

captureImage(): void {
  this.trigger.next();
}

handleImage(webcamImage: WebcamImage): void {
  this.webcamImage = webcamImage;
  this.visitorForm.patchValue({ photoUrl: webcamImage.imageAsDataUrl }); // Save Base64 image
}

handleInitError(error: WebcamInitError): void {
  this.errors.push(error);
  console.warn('Webcam error:', error);
}

clearImage(): void {
  this.webcamImage = null;
  this.visitorForm.patchValue({ photoUrl: null });
}


  onSubmit(): void {
  if (this.visitorForm.valid) {
    const formValue = this.visitorForm.value;

    // Handle optional expectedVisitDateTime
    if (!formValue.isPreRegistered) {
      formValue.expectedVisitDateTime = null;
    }

    // Ensure QR & PassCode are synced
    formValue.passCode = this.passCode;
    formValue.qrCodeData = JSON.stringify(formValue); // Store entire form data as JSON string

    const base64Image = formValue.photoUrl;

    // ✅ If image is captured, upload to Supabase first
    if (base64Image) {
      this.uploadService.uploadVisitorPhoto(base64Image).then((publicUrl) => {
        formValue.photoUrl = publicUrl; // Replace base64 with URL

        // Submit the full form to the backend
        this.visitorService.addVisitor(formValue).subscribe({
          next: () => {
            Swal.fire('Success', 'Visitor with visit added successfully', 'success');
            this.loadHosts();
            this.visitorForm.reset();
            this.router.navigate(['/admin/visitor-list'], {
              state: { qrData: formValue.qrCodeData }
            });
          },
          error: (err) => Swal.fire('Error', err.error, 'error'),
        });
      }).catch((err) => {
        Swal.fire('Upload Error', 'Failed to upload image: ' + err.message, 'error');
      });

    } else {
      // ❗ If no image was captured
      Swal.fire({
        icon: 'error',
        title: 'No Photo Captured',
        text: 'Please capture a visitor photo before submitting.',
      });
    }

  } else {
    this.visitorForm.markAllAsTouched();
    Swal.fire({
      icon: 'error',
      title: 'Form Incomplete',
      text: 'Please fill all required fields correctly.',
    });
  }
}


    setIdProofValidators(type: string): void {
    const idProofNumberControl = this.visitorForm.get('idProofNumber');

    if (!idProofNumberControl) return;

    switch (type) {
      case 'Aadhar Card':
        idProofNumberControl.setValidators([
          Validators.required,
          Validators.pattern(/^\d{12}$/)
        ]);
        break;
      case 'Pan Card':
        idProofNumberControl.setValidators([
          Validators.required,
          Validators.pattern(/^[A-Z]{5}[0-9]{4}[A-Z]{1}$/)
        ]);
        break;
      case 'Driving License':
        idProofNumberControl.setValidators([
          Validators.required,
          Validators.pattern(/^[A-Z]{2}\d{13}$/)
        ]);
        break;
      default:
        idProofNumberControl.clearValidators();
        break;
    }

    idProofNumberControl.updateValueAndValidity();
  }
}
