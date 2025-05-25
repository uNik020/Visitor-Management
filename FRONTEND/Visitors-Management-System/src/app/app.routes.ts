import { Routes } from '@angular/router';
import { Login } from './pages/login/login';
import { Register } from './pages/register/register';
import { AdminDashboard } from './pages/admin-dashboard/admin-dashboard';
import { AddVisitorsComponent } from './pages/visitors/add-visitors-component/add-visitors-component';
import { Dashboard } from './components/dashboard/dashboard';
import { VisitorList } from './pages/visitors/visitor-list/visitor-list';
import { VisitorDetails } from './pages/visitors/visitor-details/visitor-details';
import { ManageHosts } from './pages/hosts/manage-hosts/manage-hosts';
import { ManageDepartments } from './pages/manage-departments/manage-departments';

export const routes: Routes = [
  { path: '', component: Login },
  { path: 'register', component: Register },
  
  {
    path: 'admin',
    component: AdminDashboard,
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' }, // 👈 Default child route
      { path: 'dashboard', component: Dashboard },
      { path: 'add-visitor', component: AddVisitorsComponent },
      { path: 'visitor-list', component: VisitorList },
      { path: 'visitor-details', component: VisitorDetails },
      { path: 'manage-hosts', component: ManageHosts },
      { path: 'manage-departments', component: ManageDepartments }
    ]
  },
  { path: '**', redirectTo: 'login' }
];

