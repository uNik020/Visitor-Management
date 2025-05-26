import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { VisitorService } from '../../../services/Visitor/visitor.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-visitor-list',
  imports: [RouterModule, CommonModule, FormsModule],
  templateUrl: './visitor-list.html',
  styleUrl: './visitor-list.css'
})

export class VisitorList implements OnInit {
  visitors: any[] = [];
  searchQuery = '';
  editingVisitor: any = null;

  constructor(private visitorService: VisitorService) {}

  ngOnInit() {
    this.loadVisitors();
  }

  get filteredVisitors() {
    return this.visitors.filter(visitor =>
      visitor.fullName.toLowerCase().includes(this.searchQuery.toLowerCase())
    );
  }

  loadVisitors() {
    this.visitorService.getVisitor().subscribe(
      (data: any) => {
        this.visitors = data;
      },
      (error) => {
        Swal.fire('Error', 'Failed to load visitors.', 'error');
      }
    );
  }

  onEdit(visitor: any) {
    this.editingVisitor = { ...visitor }; // Shallow copy
  }

  cancelEdit() {
    this.editingVisitor = null;
  }

  saveVisitor() {
    const id = this.editingVisitor.visitorId;
    this.visitorService.updateVisitor(this.editingVisitor, id).subscribe(
      (res) => {
        Swal.fire('Updated', 'Visitor updated successfully!', 'success');
        this.loadVisitors(); // Refresh list
        this.editingVisitor = null;
      },
      (error) => {
        Swal.fire('Error', 'Failed to update visitor.', 'error');
      }
    );
  }

  onDelete(visitor: any) {
    const id = visitor.visitorId;
    Swal.fire({
      title: 'Are you sure?',
      text: 'You will not be able to recover this visitor!',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
      if (result.isConfirmed) {
        this.visitorService.deleteVisitor(id).subscribe(
          (res) => {
            Swal.fire('Deleted!', 'Visitor has been deleted.', 'success');
            this.loadVisitors();
          },
          (error) => {
            Swal.fire('Error', 'Failed to delete visitor.', 'error');
          }
        );
      }
    });
  }

  onView(visitor: any) {
    Swal.fire({
      title: visitor.fullName,
      html: `
        <p><strong>Email:</strong> ${visitor.email}</p>
        <p><strong>Phone:</strong> ${visitor.phoneNumber}</p>
        <p><strong>Host ID:</strong> ${visitor.hostId}</p>
        <p><strong>Purpose:</strong> ${visitor.purpose}</p>
        <p><strong>Status:</strong> TBD (if stored in Visit table)</p>
      `
    });
  }
}
