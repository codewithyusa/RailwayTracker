import { Routes } from '@angular/router';

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
    loadComponent: () =>
      import('./features/announce/announce.component')
        .then(m => m.AnnounceComponent),
  },
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
];