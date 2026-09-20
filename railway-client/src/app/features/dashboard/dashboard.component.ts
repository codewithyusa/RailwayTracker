import { Component, inject, AfterViewInit, signal } from '@angular/core';
import { DecimalPipe } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { RailwayStore } from '../../store/railway.store';

declare const L: any;

const STATUS_LABELS = ['On Time', 'Delayed', 'Cancelled', 'Arrived'];
const STATUS_COLORS = ['#4ade80', '#fb923c', '#f87171', '#60a5fa'];

const GREEN_LINE: [number, number][] = [
  [9.0175, 38.7358], [9.0148, 38.7435], [9.0120, 38.7505],
  [9.0090, 38.7570], [9.0055, 38.7636], [8.9908, 38.7598],
  [8.9948, 38.7655], [8.9988, 38.7710], [9.0030, 38.7768],
  [9.0068, 38.7820], [9.0100, 38.7868], [9.0155, 38.7938],
  [9.0207, 38.8010], [9.0207, 38.8027], [9.0207, 38.8045],
  [9.0207, 38.8080], [9.0207, 38.8140], [9.0207, 38.8230],
  [9.0207, 38.8332], [9.0208, 38.8435], [9.0215, 38.8538],
  [9.0222, 38.8648], [9.0229, 38.8773],
];

const BLUE_LINE: [number, number][] = [
  [9.0483, 38.7628], [9.0420, 38.7600], [9.0360, 38.7620],
  [9.0297, 38.7600], [9.0235, 38.7568], [9.0188, 38.7535],
  [9.0148, 38.7510], [9.0110, 38.7488], [9.0075, 38.7468],
  [9.0175, 38.7358], [9.0148, 38.7435], [9.0120, 38.7505],
  [9.0090, 38.7570], [9.0055, 38.7636],
  // South — straight south
  [8.9990, 38.7620], [8.9920, 38.7608], [8.9845, 38.7595],
  [8.9768, 38.7580], [8.9693, 38.7565], [8.9618, 38.7552],
  [8.9543, 38.7538], [8.9468, 38.7525], [8.9393, 38.7512],
  [8.9318, 38.7498],
];

const COMMON: [number, number][] = [
  [9.0175, 38.7358], [9.0148, 38.7435],
  [9.0120, 38.7505], [9.0090, 38.7570], [9.0055, 38.7636],
];

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [MatTableModule, DecimalPipe],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
})
export class DashboardComponent implements AfterViewInit {
  store = inject(RailwayStore) as any;
  selectedTrain = signal<any>(null);

  private map: any;
  private trainMarkers = new Map<number, any>();
  private stationMarkersAdded = false;
  private leafletReady = false;

  ngAfterViewInit() { this.initMap(); }

  private initMap() {
    if (typeof window === 'undefined') return;

    const link = document.createElement('link');
    link.rel = 'stylesheet';
    link.href = 'https://unpkg.com/leaflet@1.9.4/dist/leaflet.css';
    document.head.appendChild(link);

    const script = document.createElement('script');
    script.src = 'https://unpkg.com/leaflet@1.9.4/dist/leaflet.js';
    script.onload = () => {
      this.map = L.map('railway-map').setView([9.02, 38.76], 12);
      L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '© OpenStreetMap contributors', maxZoom: 19,
      }).addTo(this.map);
      this.drawLines();
      this.leafletReady = true;
      this.renderMarkers();
      setInterval(() => this.renderMarkers(), 5000);
    };
    document.head.appendChild(script);
  }

  private drawLines() {
    L.polyline(GREEN_LINE, { color: '#4ade80', weight: 5, opacity: 0.9 })
      .addTo(this.map).bindTooltip('Green Line', { sticky: true });

    L.polyline(BLUE_LINE, { color: '#60a5fa', weight: 5, opacity: 0.9 })
      .addTo(this.map).bindTooltip('Blue Line', { sticky: true });

    L.polyline(COMMON, { color: '#f87171', weight: 5, opacity: 0.9, dashArray: '8 4' })
      .addTo(this.map).bindTooltip('Common Section', { sticky: true });
  }

  private renderMarkers() {
    if (!this.leafletReady || !this.map) return;

    (this.store.entities() as any[]).forEach((train: any) => {
      const color = STATUS_COLORS[train.status] ?? '#fff';
      const html = `<div style="background:${color};width:16px;height:16px;
        border-radius:50%;border:2px solid #fff;box-shadow:0 0 10px ${color}"></div>`;
      const icon = L.divIcon({ className: '', html, iconSize: [16, 16], iconAnchor: [8, 8] });

      if (this.trainMarkers.has(train.id)) {
        this.trainMarkers.get(train.id).setLatLng([train.latitude, train.longitude]);
      } else {
        const m = L.marker([train.latitude, train.longitude], { icon })
          .bindPopup(`<b>${train.code}</b> — ${train.name}<br>Status: ${STATUS_LABELS[train.status]}`)
          .on('click', () => this.selectedTrain.set(train));
        m.addTo(this.map);
        this.trainMarkers.set(train.id, m);
      }
    });

    if (!this.stationMarkersAdded && this.store.stations().length > 0) {
      this.stationMarkersAdded = true;
      (this.store.stations() as any[]).forEach((s: any) => {
        const isGreen = s.lineId === 1 || s.line?.name?.includes('Green');
        L.circleMarker([s.latitude, s.longitude], {
          radius: 5,
          fillColor: isGreen ? '#4ade80' : '#60a5fa',
          color: '#fff', weight: 2, fillOpacity: 1,
        }).bindTooltip(`${s.name} (${s.code})`, { direction: 'top' })
          .addTo(this.map);
      });
    }
  }

  statusLabel(status: number) { return STATUS_LABELS[status] ?? 'Unknown'; }
  statusColor(status: number) { return STATUS_COLORS[status] ?? '#fff'; }
}