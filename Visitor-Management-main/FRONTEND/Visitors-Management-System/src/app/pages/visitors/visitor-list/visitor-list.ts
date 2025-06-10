import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component } from '@angular/core';
import { FormBuilder, FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { VisitorService } from '../../../services/Visitor/visitor.service';
import Swal from 'sweetalert2';
import { HostService } from '../../../services/Host/host.service';
import { AgGridModule } from 'ag-grid-angular';
import { ModuleRegistry } from 'ag-grid-community';
import { AllCommunityModule } from 'ag-grid-community';

ModuleRegistry.registerModules([AllCommunityModule]);

@Component({
  selector: 'app-visitor-list',
  imports: [RouterModule,FormsModule, CommonModule, AgGridModule],
  templateUrl: './visitor-list.html',
  styleUrl: './visitor-list.css'
})

export class VisitorList {
  visitors: any[] = [];
  hosts: any[] = [];
  
  searchQuery = '';
  editingVisitor: any = null;

  sortAscending = true;
  showFilterModal = false;
filterStatus = '';
filterHostId = '';

columnDefs = [
  { headerName: 'Name', field: 'fullName' },
  { headerName: 'Email', field: 'email' },
  { headerName: 'Purpose', field: 'purpose' },
  {
    headerName: 'Host',
    valueGetter: (params:any) => params.data.visits?.[0]?.host?.fullName || 'N/A'
  },
  {
    headerName: 'Status',
    valueGetter: (params:any) => params.data.visits?.[0]?.visitStatus || 'N/A'
  },
  {
    headerName: 'Checkin At',
    valueGetter: (params:any) =>
      params.data.visits?.[0]?.checkInTime
        ? new Date(params.data.visits[0].checkInTime).toLocaleString()
        : 'N/A'
  },
  {
    headerName: 'Checkout At',
    valueGetter: (params:any) =>
      params.data.visits?.[0]?.checkOutTime
        ? new Date(params.data.visits[0].checkOutTime).toLocaleString()
        : 'Yet to checkout'
  },
  {
  headerName: 'Actions',
  cellRenderer: (params: any) => {
    return `
      <button class="btn-edit" data-action="edit" style="margin-right: 5px; padding: 4px 8px; background-color: #0d6efd; color: white; border: none; border-radius: 4px; cursor: pointer;">
        Edit
      </button>
      <button class="btn-delete" data-action="delete" style="padding: 4px 8px; background-color: #dc3545; color: white; border: none; border-radius: 4px; cursor: pointer;">
        Delete
      </button>
    `;
  }
}
];


defaultColDef = {
  sortable: true,
  filter: true,
  resizable: true
};


rowData: any[] = [];



  constructor(
    private cd: ChangeDetectorRef,
    private visitorService: VisitorService,
    private hostService: HostService
  ) {}


  ngOnInit(): void {
      this.loadVisitors();
      this.loadHosts();
    }

    onActionClicked(params: any) {
  const action = params.event?.target?.getAttribute('data-action');
  if (action === 'edit') {
    this.onEdit(params.data);
  } else if (action === 'delete') {
    this.onDelete(params.data.visitorId);
  }
}

onGridCellClicked(event: any): void {
  if (event.colDef.headerName === 'Actions') {
    const clickedButton = event.event.target as HTMLElement;
    if (clickedButton.classList.contains('btn-edit')) {
      this.onEdit(event.data);
    } else if (clickedButton.classList.contains('btn-delete')) {
      this.onDelete(event.data.visitorId);
    }
  }
}

  
 loadVisitors() {
  this.visitorService.getVisitor().subscribe(
    (data: any) => {
      setTimeout(() => {
        this.rowData = [...data]; // ✅ changed
        this.cd.detectChanges();
      }, 500);
    },
    (err) => Swal.fire('Error', err.message, 'error')
  );
}

  get filteredVisitors() {
  return this.rowData.filter((visitor) => {
    const nameMatch = visitor.fullName
      .toLowerCase()
      .includes(this.searchQuery.toLowerCase());

    const statusMatch = this.filterStatus
      ? visitor.visits[0]?.visitStatus === this.filterStatus
      : true;

    const hostMatch = this.filterHostId
      ? visitor.visits[0]?.hostId == this.filterHostId
      : true;

    return nameMatch && statusMatch && hostMatch;
  });
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

    toggleSort(){
      this.sortAscending = !this.sortAscending;
      this.visitors.sort((a,b)=>{
        const nameA=a.fullName.toLowerCase();
        const nameB=b.fullName.toLowerCase();

        if(this.sortAscending){
          return nameA.localeCompare(nameB);
        }
        else{
          return nameB.localeCompare(nameA)
        }
      });
    }

    toggleFilterModal() {
  this.showFilterModal = !this.showFilterModal;
}

    clearFilters() {
  this.filterStatus = '';
  this.filterHostId = '';
  this.showFilterModal = false;
}

}
