import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { DesignationService } from '../../services/Designation/designation.service';
import Swal from 'sweetalert2';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-manage-designations',
  imports: [ReactiveFormsModule, CommonModule, RouterModule],
  templateUrl: './manage-designations.html',
  styleUrl: './manage-designations.css'
})
export class ManageDesignations implements OnInit {
  designationForm: FormGroup;
  designations: any[] = []; // Dummy data – replace with API data
  isEditMode = false;
  editId: number | null = null;

  constructor(
    private cd: ChangeDetectorRef,
    private fb: FormBuilder,
    private designationService: DesignationService
  ) {
    this.designationForm = this.fb.group({
      designationName: ['', Validators.required],
    });
  }

  ngOnInit(): void {
    // Fetch designations from API here
    this.load();
  }

  load() {
    this.designationService.getDesignations().subscribe(
      (data: any) => {
        this.designations = [...data];
        this.cd.detectChanges();
      },
      (error) => {
        if (error.status === 500) {
          Swal.fire({
            title: 'Error!',
            text: 'Email-Id already registered.',
            icon: 'error',
            confirmButtonText: 'Ok',
          });
        } else {
          Swal.fire({
            title: 'Error!',
            text: 'An unexpected error occurred.',
            icon: 'error',
            confirmButtonText: 'Ok',
          });
        }
      }
    );
  }

  onSubmit(): void {
    if (this.designationForm.invalid) return;

    const deptName = this.designationForm.value.designationName.trim();

    if (this.isEditMode) {
      this.designationService
        .updateDesignations(this.designationForm.value,this.editId)
        .subscribe(
          (data: any) => {
            Swal.fire({
              title: 'Success!',
              text: 'designation Updated successful.',
              icon: 'success',
              confirmButtonText: 'Cool',
            });
            this.isEditMode = false;
            this.editId = null;
            this.load();
          },
          (error) => {
            console.log(error);
            Swal.fire({
              title: 'Error!',
              text: `An unexpected error occurred.{${error.message}}`,
              icon: 'error',
              confirmButtonText: 'Ok',
            });
          }
        );
    } else {
      this.designationService
        .addDesignations(this.designationForm.value)
        .subscribe(
          (data: any) => {
            Swal.fire({
              title: 'Success!',
              text: 'designation added successful.',
              icon: 'success',
              confirmButtonText: 'Cool',
            });
            this.load();
          },
          (error) => {
            console.log(error);
            Swal.fire({
              title: 'Error!',
              text: `An unexpected error occurred.{${error.error}}`,
              icon: 'error',
              confirmButtonText: 'Ok',
            });
          }
        );
    }

    this.designationForm.reset();
  }

  onEdit(dept: any): void {
    this.isEditMode = true;
    this.editId = dept.designationId;
    this.designationForm.patchValue({
      designationName: dept.designationName,
      designationId: dept.designationId,
    });
  }

  onDelete(id: number): void {
    Swal.fire({
      title: 'Are you sure?',
      text: "You won't be able to revert this!",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Yes, delete it!',
    }).then((result) => {
      if (result.isConfirmed) {
        // User confirmed, proceed with delete
        this.designationService.deleteDesignations(id).subscribe(
          (data: any) => {
            Swal.fire({
              title: 'Success!',
              text: 'designation deleted successfully.',
              icon: 'success',
              confirmButtonText: 'Cool',
            });
            this.load();
          },
          (error) => {
            console.log(error);
            Swal.fire({
              title: 'Info!',
              text: `An unexpected error occurred: ${error.error}`,
              icon: 'info',
              confirmButtonText: 'Ok',
            });
          }
        );
      }
    });
  }
}
