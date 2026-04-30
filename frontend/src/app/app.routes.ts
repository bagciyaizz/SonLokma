import { Routes } from '@angular/router';
import { AdminDashboardComponent } from './features/admin/dashboard/admin-dashboard.component';
import { LoginComponent } from './features/auth/login/login.component';
import { RegisterComponent } from './features/auth/register/register.component';
import { BusinessDashboardComponent } from './features/business/dashboard/business-dashboard.component';
import { DiscoverComponent } from './features/customer/discover/discover.component';
import { ReservationsComponent } from './features/customer/reservations/reservations.component';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'customer/discover' },
  { path: 'auth/login', component: LoginComponent },
  { path: 'auth/register', component: RegisterComponent },
  { path: 'customer/discover', component: DiscoverComponent },
  { path: 'customer/reservations', component: ReservationsComponent },
  { path: 'business/dashboard', component: BusinessDashboardComponent },
  { path: 'admin/dashboard', component: AdminDashboardComponent },
  { path: '**', redirectTo: 'customer/discover' }
];
