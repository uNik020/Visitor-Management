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
import { RouteGuardService } from './services/RouteGuard/route-guard-service';

export const routes: Routes = [
  { path: '', component: Login },
  { path: 'login', component: Login },
  { path: 'register', component: Register },
  
  {
    path: 'admin',
    component: AdminDashboard,
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' }, // 👈 Default child route
      { path: 'dashboard', component: Dashboard ,canActivate:[RouteGuardService]},
      { path: 'add-visitor', component: AddVisitorsComponent ,canActivate:[RouteGuardService]},
      { path: 'visitor-list', component: VisitorList ,canActivate:[RouteGuardService] },
      { path: 'visitor-details', component: VisitorDetails ,canActivate:[RouteGuardService]},
      { path: 'manage-hosts', component: ManageHosts ,canActivate:[RouteGuardService]},
      { path: 'manage-departments', component: ManageDepartments ,canActivate:[RouteGuardService]}
    ]
  },
  { path: '**', redirectTo: 'login' }
];

