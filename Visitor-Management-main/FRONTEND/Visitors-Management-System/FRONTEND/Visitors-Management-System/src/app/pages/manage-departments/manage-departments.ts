import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { RouterModule } from '@angular/router';
import { DepartmentService } from '../../services/Department/department.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-manage-departments',
  imports: [ReactiveFormsModule, CommonModule, RouterModule],
  templateUrl: './manage-departments.html',
  styleUrl: './manage-departments.css',
})
export class ManageDepartments implements OnInit {
  departmentForm: FormGroup;
  departments: any[] = []; // Dummy data – replace with API data
  isEditMode = false;
  editId: number | null = null;

  constructor(
    private cd: ChangeDetectorRef,
    private fb: FormBuilder,
    private departmentService: DepartmentService
  ) {
    this.departmentForm = this.fb.group({
      departmentName: ['', Validators.required],
    });
  }

  ngOnInit(): void {
    // Fetch departments from API here
    this.load();
  }

  load() {
    this.departmentService.getDepartments().subscribe(
      (data: any) => {
        this.departments = [...data];
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
    if (this.departmentForm.invalid) return;

    const deptName = this.departmentForm.value.departmentName.trim();

    if (this.isEditMode) {
      this.departmentService
        .updateDepartments(this.departmentForm.value,this.editId)
        .subscribe(
          (data: any) => {
            Swal.fire({
              title: 'Success!',
              text: 'Department Updated successful.',
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
      this.departmentService
        .addDepartments(this.departmentForm.value)
        .subscribe(
          (data: any) => {
            Swal.fire({
              title: 'Success!',
              text: 'Department added successful.',
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

    this.departmentForm.reset();
  }

  onEdit(dept: any): void {
    this.isEditMode = true;
    this.editId = dept.departmentId;
    this.departmentForm.patchValue({
      departmentName: dept.departmentName,
      departmentId: dept.departmentId,
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
        this.departmentService.deleteDepartments(id).subscribe(
          (data: any) => {
            Swal.fire({
              title: 'Success!',
              text: 'Department deleted successfully.',
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
