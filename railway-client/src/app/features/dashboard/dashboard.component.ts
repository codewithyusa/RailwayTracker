import { Component, inject, OnInit, AfterViewInit, signal } from '@angular/core';
import { DecimalPipe } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { RailwayStore } from '../../store/railway.store';

declare const L: any;

const STATUS_LABELS = ['On Time', 'Delayed', 'Cancelled', 'Arrived'];
const STATUS_COLORS = ['#4ade80', '#fb923c', '#f87171', '#60a5fa'];

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [MatTableModule, DecimalPipe],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
})
export class DashboardComponent implements OnInit, AfterViewInit {
  store = inject(RailwayStore) as any;
  selectedTrain = signal<any>(null);

  private map: any;
  private markers = new Map<number, any>();
  private leafletReady = false;

  ngOnInit() {}

  ngAfterViewInit() {
    this.initMap();
  }

  private initMap() {
    if (typeof window === 'undefined') return;

    const link = document.createElement('link');
    link.rel = 'stylesheet';
    link.href = 'https://unpkg.com/leaflet@1.9.4/dist/leaflet.css';
    document.head.appendChild(link);

    const script = document.createElement('script');
    script.src = 'https://unpkg.com/leaflet@1.9.4/dist/leaflet.js';
    script.onload = () => {
      this.map = L.map('railway-map').setView([9.02, 38.74], 9);
      L.tileLayer('https://{s}.basemaps.cartocdn.com/dark_all/{z}/{x}/{y}{r}.png', {
        attribution: '© OpenStreetMap © CARTO'
      }).addTo(this.map);
      this.leafletReady = true;
      this.renderMarkers();
      setInterval(() => this.renderMarkers(), 5000);
    };
    document.head.appendChild(script);
  }

  private renderMarkers() {
    if (!this.leafletReady || !this.map) return;

    (this.store.entities() as any[]).forEach((train: any) => {
      const color = STATUS_COLORS[train.status] ?? '#fff';
      const html = `<div style="background:${color};width:14px;height:14px;
        border-radius:50%;border:2px solid #fff;box-shadow:0 0 8px ${color}"></div>`;
      const icon = L.divIcon({
        className: '', html, iconSize: [14, 14], iconAnchor: [7, 7]
      });

      if (this.markers.has(train.id)) {
        this.markers.get(train.id).setLatLng([train.latitude, train.longitude]);
      } else {
        const m = L.marker([train.latitude, train.longitude], { icon })
          .bindPopup(`<b>${train.code}</b> — ${train.name}`)
          .on('click', () => this.selectedTrain.set(train));
        m.addTo(this.map);
        this.markers.set(train.id, m);
      }
    });

    (this.store.stations() as any[]).forEach((s: any) => {
      L.circleMarker([s.latitude, s.longitude], {
        radius: 7, fillColor: '#60a5fa',
        color: '#fff', weight: 2, fillOpacity: 1
      }).bindTooltip(s.name).addTo(this.map);
    });
  }

  statusLabel(status: number) { return STATUS_LABELS[status] ?? 'Unknown'; }
  statusColor(status: number) { return STATUS_COLORS[status] ?? '#fff'; }
}