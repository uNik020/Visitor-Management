import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-manage-hosts',
  imports: [RouterModule,ReactiveFormsModule, CommonModule],
  templateUrl: './manage-hosts.html',
  styleUrl: './manage-hosts.css'
})
export class ManageHosts {
  hostForm: FormGroup;
  editingIndex: number | null = null;

  hosts = [
    { fullName: 'John Doe', email: 'john@example.com', contact: '1234567890' },
    { fullName: 'Jane Smith', email: 'jane@example.com', contact: '9876543210' }
  ];

  constructor(private fb: FormBuilder) {
    this.hostForm = this.fb.group({
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      contact: ['', [Validators.required, Validators.pattern('^[0-9]{10}$')]],
    });
  }

  onSubmit(): void {
    if (this.hostForm.invalid) return;

    if (this.editingIndex !== null) {
      this.hosts[this.editingIndex] = this.hostForm.value;
      this.editingIndex = null;
    } else {
      this.hosts.push(this.hostForm.value);
    }

    this.hostForm.reset();
  }

  editHost(index: number) {
    this.editingIndex = index;
    this.hostForm.setValue(this.hosts[index]);
  }

  deleteHost(index: number) {
    if (confirm('Are you sure you want to delete this host?')) {
      this.hosts.splice(index, 1);
    }
  }

  cancelEdit() {
    this.editingIndex = null;
    this.hostForm.reset();
  }
}