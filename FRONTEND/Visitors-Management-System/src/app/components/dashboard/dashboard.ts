import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  imports: [RouterModule, CommonModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class Dashboard {
  summary = [
    { title: 'Total Visitors', count: 120, icon: '👥' },
    { title: 'Today\'s Visitors', count: 15, icon: '📅' },
    { title: 'Currently Inside', count: 4, icon: '✅' }
  ];

  recentVisitors = [
    { name: 'John Doe', time: '10:30 AM', host: 'Alice', status: 'Inside' },
    { name: 'Jane Smith', time: '9:45 AM', host: 'Bob', status: 'Checked Out' },
    { name: 'Mike Brown', time: '8:15 AM', host: 'Charlie', status: 'Inside' }
  ];
}
