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

@Component({
  selector: 'app-add-visitors-component',
  standalone: true,
  imports: [ReactiveFormsModule, RouterModule, CommonModule, QRCodeComponent],
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
    private router: Router
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

  onSubmit(): void {
    if (this.visitorForm.valid) {
      const formValue = this.visitorForm.value;

      // Handle optional expectedVisitDateTime
      if (!formValue.isPreRegistered) {
        formValue.expectedVisitDateTime = null;
      }

      // Ensure QR & PassCode are synced
      formValue.passCode = this.passCode;
      formValue.qrCodeData = this.qrCodeData;

      this.visitorService.addVisitor(formValue).subscribe({
        next: () => {
          Swal.fire(
            'Success',
            'Visitor with visit added successfully',
            'success'
          );
          this.loadHosts();
          this.visitorForm.reset();
          this.router.navigate(['/admin/visitor-list']);
        },
        error: (err) => Swal.fire('Error', err.error, 'error'),
      });
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
