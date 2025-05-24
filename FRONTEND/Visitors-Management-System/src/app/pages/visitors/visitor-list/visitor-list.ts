import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-visitor-list',
  imports: [RouterModule, CommonModule, FormsModule],
  templateUrl: './visitor-list.html',
  styleUrl: './visitor-list.css'
})

export class VisitorList {
visitors = [
    { name: 'John Doe', email: 'john@example.com', host: 'Alice', status: 'Inside' },
    { name: 'Jane Smith', email: 'jane@example.com', host: 'Bob', status: 'Checked Out' }
  ];

  searchQuery = '';
  editingVisitor: any = null;

  get filteredVisitors() {
    return this.visitors.filter(visitor =>
      visitor.name.toLowerCase().includes(this.searchQuery.toLowerCase())
    );
  }

  onEdit(visitor: any) {
    this.editingVisitor = { ...visitor }; // clone to avoid live editing
  }

  cancelEdit() {
    this.editingVisitor = null;
  }

  saveVisitor() {
    const index = this.visitors.findIndex(v => v.email === this.editingVisitor.email);
    if (index !== -1) {
      this.visitors[index] = { ...this.editingVisitor };
    }
    this.editingVisitor = null;
  }

  onDelete(visitor: any) {
    console.log('Delete visitor:', visitor);
  }

  onView(visitor: any) {
    console.log('View visitor:', visitor);
  }
}
