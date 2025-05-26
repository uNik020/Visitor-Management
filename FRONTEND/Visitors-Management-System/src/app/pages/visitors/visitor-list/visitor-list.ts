import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component } from '@angular/core';
import { FormBuilder, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { VisitorService } from '../../../services/Visitor/visitor.service';
import Swal from 'sweetalert2';
import { HostService } from '../../../services/Host/host.service';

@Component({
  selector: 'app-visitor-list',
  imports: [RouterModule, CommonModule, FormsModule],
  templateUrl: './visitor-list.html',
  styleUrl: './visitor-list.css'
})

export class VisitorList {
  visitors: any[] = [];
  hosts: any[] = [];

  constructor(
    private fb: FormBuilder,
    private cd: ChangeDetectorRef,
    private visitorService: VisitorService,
    private hostService: HostService
  ) {}

visitors1 = [
    { name: 'John Doe', email: 'john@example.com', host: 'Alice', status: 'Inside' },
    { name: 'Jane Smith', email: 'jane@example.com', host: 'Bob', status: 'Checked Out' }
  ];

  ngOnInit(): void {
      this.loadVisitors();
      this.loadHosts();
    }
  
    loadVisitors() {
      this.visitorService.getVisitor().subscribe(
        (data: any) => {
          setTimeout(() => {
            this.visitors = [...data];
            this.cd.detectChanges();
          },500);
        },
        (err) => Swal.fire('Error', err.message, 'error')
      );
    }

  searchQuery = '';
  editingVisitor: any = null;

  get filteredVisitors() {
    return this.visitors.filter(visitor =>
      visitor.name.toLowerCase().includes(this.searchQuery.toLowerCase())
    );
  }

  onEdit(visitor: any) {
    this.editingVisitor = { ...visitor }; // clone to avoid live editing
    console.log(this.editingVisitor)
  }

  cancelEdit() {
    this.editingVisitor = null;
  }

  saveVisitor() {
  const visit:any = {
    hostId: this.editingVisitor.visits[0].hostId ,
    visitId: this.editingVisitor.visits[0].visitId ,
    visitStatus: this.editingVisitor.visits[0].visitStatus ,
  };

  this.visitorService.updateVisitor(visit, visit.visitId).subscribe({
          next: () => {
            Swal.fire('Success', 'Visit updated successfully', 'success');
            this.loadVisitors();
            this.loadHosts();
            this.editingVisitor = null;
          },
          error: (err) => Swal.fire('Error', err.message, 'error'),
        });
  }

  onDelete(visitorId: any) {
    //console.log(visitorId)
    Swal.fire({
          title: 'Are you sure?',
          text: 'This will delete the Visitor!',
          icon: 'warning',
          showCancelButton: true,
          confirmButtonText: 'Yes, delete it!',
        }).then((result) => {
          if (result.isConfirmed) {
            this.visitorService.deleteVisitor(visitorId).subscribe({
              next: () => {
                Swal.fire('Deleted!', 'Visitor has been deleted.', 'success');
                this.loadHosts();
              },
              error: (err) => Swal.fire('Warning', err.error, 'info'),
            });
          }
        });
  }

  // onView(visitor: any) {
  //   console.log('View visitor:', visitor);
  // }

  //hosts
  loadHosts() {
      this.hostService.getHosts().subscribe(
        (data: any) => {
          setTimeout(() => {
            this.hosts = [...data];
            this.cd.detectChanges();
          });
        },
        (err) => Swal.fire('Error', err.message, 'error')
      );
    }


}
