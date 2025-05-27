import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { VisitorService } from '../../services/Visitor/visitor.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-dashboard',
  imports: [RouterModule, CommonModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class Dashboard implements OnInit {
  visitors: any[] = [];

  summary:any = [
    { title: 'Total Visitors', count: 0, icon: '👥' },
    { title: "Today's Visitors", count: 0, icon: '📅' },
    { title: 'Currently Inside', count: 0, icon: '✅' }
  ];

  recentVisitors: any[] = [];

  constructor(private visitorService: VisitorService, private cd: ChangeDetectorRef ) {}

  ngOnInit(): void {
    this.loadVisitors();
  }

  loadVisitors() {
    this.visitorService.getVisitor().subscribe(
      (data: any[]) => {
        setTimeout(()=>{
          this.visitors = [...data];
          this.updateSummary();
          this.updateRecentVisitors();
          this.cd.detectChanges();
        },500);
      },
      (err) => {
        console.error('Failed to load visitors', err);
        Swal.fire('Error', err.message, 'error');
      }
    );
   
  }

  updateSummary() {
    const todayStr = new Date().toDateString();

    const total = this.visitors.length;

    const todaysVisitors = this.visitors.filter(v =>
      new Date(v.visits[0]?.checkInTime).toDateString() === todayStr
    ).length;

    const currentlyInside = this.visitors.filter(v =>
      v.visits[0]?.visitStatus === 'Inside'
    ).length;

    this.summary = [
      { title: 'Total Visitors', count: total, icon: '👥' },
      { title: "Today's Visitors", count: todaysVisitors, icon: '📅' },
      { title: 'Currently Inside', count: currentlyInside, icon: '✅' }
    ];
  }

  updateRecentVisitors() {
    this.recentVisitors = this.visitors
      .sort((a, b) => new Date(b.visits[0]?.checkInTime).getTime() - new Date(a.visits[0]?.checkInTime).getTime())
      .slice(0, 5)
      .map(v => ({
        name: v.fullName,
        time: new Date(v.visits[0]?.checkInTime).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }),
        host: v.visits[0]?.host?.fullName,
        status: v.visits[0]?.visitStatus
      }));
  }
}