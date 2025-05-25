import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-manage-departments',
  imports: [ReactiveFormsModule, CommonModule, RouterModule],
  templateUrl: './manage-departments.html',
  styleUrl: './manage-departments.css'
})
export class ManageDepartments implements OnInit {
  departmentForm: FormGroup;
  departments = [
    { id: 1, name: 'HR' },
    { id: 2, name: 'IT' }
  ]; // Dummy data – replace with API data
  isEditMode = false;
  editId: number | null = null;

  constructor(private fb: FormBuilder) {
    this.departmentForm = this.fb.group({
      name: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    // Fetch departments from API here
  }

  onSubmit(): void {
    if (this.departmentForm.invalid) return;

    const deptName = this.departmentForm.value.name.trim();

    if (this.isEditMode) {
      const dept = this.departments.find(d => d.id === this.editId);
      if (dept) dept.name = deptName;
      this.isEditMode = false;
      this.editId = null;
    } else {
      const newId = this.departments.length + 1;
      this.departments.push({ id: newId, name: deptName });
    }

    this.departmentForm.reset();
  }

  onEdit(dept: any): void {
    this.isEditMode = true;
    this.editId = dept.id;
    this.departmentForm.patchValue({ name: dept.name });
  }

  onDelete(id: number): void {
    this.departments = this.departments.filter(d => d.id !== id);
  }
}