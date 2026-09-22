import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';
import { RailwayStore } from './store/railway.store';
import { LiveSyncService } from './services/live-sync.service';
import { AuthService } from './services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  template: `
    <nav class="navbar">
      <div class="nav-brand">🚊 RailwayTracker Ethiopia</div>
      <div class="nav-links">
        <a routerLink="/dashboard" routerLinkActive="active">Dashboard</a>
        <a routerLink="/stations" routerLinkActive="active">Stations</a>
        @if (auth.isLoggedIn()) {
          <a routerLink="/announce" routerLinkActive="active">Announce</a>
        }
      </div>
      <div class="nav-right">
        @if (auth.isLoggedIn()) {
          <button class="logout-btn" (click)="logout()">Logout</button>
        }
        <div class="nav-status" [class]="sync.connectionState()">
          ● {{ sync.connectionState() }}
        </div>
      </div>
    </nav>
    <router-outlet />
  `,
  styleUrl: './app.component.scss',
})
export class AppComponent implements OnInit {
  store = inject(RailwayStore);
  sync = inject(LiveSyncService);
  auth = inject(AuthService);
  private router = inject(Router);

  ngOnInit() {
    this.store.loadTrains();
    this.store.loadStations();
    this.store.listenForLivePositions();
  }

  logout() {
    this.auth.logout();
    this.router.navigate(['/dashboard']);
  }
}