import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterModule } from '@angular/router';
import Swal from 'sweetalert2';
import { HostService } from '../../../services/Host/host.service';

@Component({
  selector: 'app-manage-hosts',
  standalone: true,
  imports: [RouterModule, ReactiveFormsModule, CommonModule],
  templateUrl: './manage-hosts.html',
  styleUrl: './manage-hosts.css'
})
export class ManageHosts implements OnInit {
  hostForm: FormGroup;
  hosts: any[] = [];
  editingId: number | null = null;

  constructor(private fb: FormBuilder,private cd: ChangeDetectorRef, private hostService: HostService) {
    this.hostForm = this.fb.group({
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', [Validators.required, Validators.pattern('^[0-9]{10}$')]],
      departmentId: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.loadHosts();
  }

  loadHosts(): void {
    this.cd.detectChanges();
    this.hostService.getHosts().subscribe({
      next: (data) => (this.hosts = data),
      error: (err) => Swal.fire('Error', err.message, 'error')
    });
  }

  onSubmit(): void {
    if (this.hostForm.invalid) {
      Swal.fire('Validation Failed', 'Please correct the form.', 'warning');
      return;
    }

    const hostData = this.hostForm.value;

    if (this.editingId !== null) {
      this.hostService.updateHost(this.editingId, hostData).subscribe({
        next: () => {
          Swal.fire('Success', 'Host updated successfully', 'success');
          this.loadHosts();
          this.cancelEdit();
        },
        error: (err) => Swal.fire('Error', err.message, 'error')
      });
    } else {
      this.hostService.addHost(hostData).subscribe({
        next: () => {
          Swal.fire('Success', 'Host added successfully', 'success');
          this.loadHosts();
          this.hostForm.reset();
        },
        error: (err) => Swal.fire('Error', err.message, 'error')
      });
    }
  }

  editHost(host: any): void {
    this.editingId = host.hostId;
    this.hostForm.setValue({
      fullName: host.fullName,
      email: host.email,
      phoneNumber: host.phoneNumber,
      departmentId: host.departmentId
    });
  }

  deleteHost(id: number): void {
    Swal.fire({
      title: 'Are you sure?',
      text: 'This will delete the host!',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
      if (result.isConfirmed) {
        this.hostService.deleteHost(id).subscribe({
          next: () => {
            Swal.fire('Deleted!', 'Host has been deleted.', 'success');
            this.loadHosts();
          },
          error: (err) => Swal.fire('Error', err.message, 'error')
        });
      }
    });
  }

  cancelEdit(): void {
    this.editingId = null;
    this.hostForm.reset();
  }
}
