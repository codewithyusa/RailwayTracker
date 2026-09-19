import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';
import { RailwayStore } from './store/railway.store';
import { LiveSyncService } from './services/live-sync.service';

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
        <a routerLink="/announce" routerLinkActive="active">Announce</a>
      </div>
      <div class="nav-status" [class]="sync.connectionState()">
        ● {{ sync.connectionState() | titlecase }}
      </div>
    </nav>
    <router-outlet />
  `,
  styleUrl: './app.component.scss',
})
export class AppComponent implements OnInit {
  store = inject(RailwayStore);
  sync = inject(LiveSyncService);

  ngOnInit() {
    this.store.loadTrains();
    this.store.loadStations();
    this.store.listenForLivePositions();
  }
}