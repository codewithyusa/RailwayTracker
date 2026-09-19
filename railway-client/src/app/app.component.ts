import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';
import { TitleCasePipe } from '@angular/common';
import { RailwayStore } from './store/railway.store';
import { LiveSyncService } from './services/live-sync.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive, TitleCasePipe],
  templateUrl: './app.html',      // ← match whatever your file is named
  styleUrl: './app.scss',         // ← match whatever your file is named
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