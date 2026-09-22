import { Routes } from '@angular/router';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
  {
    path: 'dashboard',
    loadComponent: () =>
      import('./features/dashboard/dashboard.component')
        .then(m => m.DashboardComponent),
  },
  {
    path: 'stations',
    loadComponent: () =>
      import('./features/stations/stations.component')
        .then(m => m.StationsComponent),
  },
  {
    path: 'announce',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./features/announce/announce.component')
        .then(m => m.AnnounceComponent),
  },
  {
    path: 'login',
    loadComponent: () =>
      import('./features/login/login.component')
        .then(m => m.LoginComponent),
  },
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
];