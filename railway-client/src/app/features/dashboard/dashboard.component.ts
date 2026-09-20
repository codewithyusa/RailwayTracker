import { Component, inject, AfterViewInit, signal } from '@angular/core';
import { DecimalPipe } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { RailwayStore } from '../../store/railway.store';

declare const L: any;

const STATUS_LABELS = ['On Time', 'Delayed', 'Cancelled', 'Arrived'];
const STATUS_COLORS = ['#4ade80', '#fb923c', '#f87171', '#60a5fa'];

// Real GPS coords from your DataSeeder — Green Line west→east
const GREEN_LINE: [number, number][] = [
  [9.0175, 38.7358], // St. Lideta
  [9.0148, 38.7435], // Tegbared
  [9.0120, 38.7505], // Mexico
  [9.0090, 38.7570], // Leghar
  [9.0055, 38.7636], // Stadium
  [8.9908, 38.7598], // Meshwalekya
  [8.9988, 38.7710], // Bambis
  [9.0030, 38.7768], // St. Urael
  [9.0068, 38.7820], // Hayahulet 2
  [9.0100, 38.7868], // Hayahulet 1
  [9.0155, 38.7938], // Lem Hotel
  [9.0207, 38.8010], // Megenagna
  [9.0207, 38.8027], // Gurd Sholla 2
  [9.0207, 38.8045], // Gurd Sholla 1
  [9.0207, 38.8080], // Management Institute
  [9.0207, 38.8140], // Civil Service College
  [9.0207, 38.8230], // St. Michael Church
  [9.0207, 38.8332], // C.M.C.
  [9.0208, 38.8435], // Meri
  [9.0215, 38.8538], // Yard
  [9.0222, 38.8648], // Lege Tapo
  [9.0229, 38.8773], // Ayat
];

// Real GPS coords — Blue Line north→south
const BLUE_LINE: [number, number][] = [
  [9.0483, 38.7628], // Menilik Square
  [9.0420, 38.7600], // Shiro Meda
  [9.0360, 38.7620], // Sidist Kilo
  [9.0297, 38.7600], // Atikilt Tera
  [9.0235, 38.7568], // Gojam Berenda
  [9.0188, 38.7535], // Autobus Tera
  [9.0148, 38.7510], // Sebategna
  [9.0110, 38.7488], // Abnet
  [9.0075, 38.7468], // Darmar
  [9.0175, 38.7358], // St. Lideta (interchange)
  [9.0148, 38.7435], // Tegbared
  [9.0120, 38.7505], // Mexico
  [9.0090, 38.7570], // Leghar
  [9.0055, 38.7636], // Stadium
  [8.9908, 38.7598], // Meshwalekya
  [8.9838, 38.7528], // Riche
  [8.9768, 38.7458], // Temenja Yazh
  [8.9698, 38.7388], // Lancha
  [8.9628, 38.7318], // Nifas Silk 1
  [8.9558, 38.7248], // Nifas Silk 2
  [8.9488, 38.7178], // Adey Abebe
  [8.9418, 38.7108], // Saris
  [8.9348, 38.7038], // Abo Junction
  [8.9278, 38.6968], // Kaliti
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
      this.map = L.map('railway-map').setView([9.02, 38.76], 12);

      L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '© OpenStreetMap contributors',
        maxZoom: 19,
      }).addTo(this.map);

      this.drawLines();
      this.leafletReady = true;
      this.renderMarkers();
      setInterval(() => this.renderMarkers(), 5000);
    };
    document.head.appendChild(script);
  }

  private drawLines() {
    // Green Line
    L.polyline(GREEN_LINE, {
      color: '#4ade80',
      weight: 4,
      opacity: 0.85,
    }).addTo(this.map).bindTooltip('Green Line', { sticky: true });

    // Blue Line
    L.polyline(BLUE_LINE, {
      color: '#60a5fa',
      weight: 4,
      opacity: 0.85,
    }).addTo(this.map).bindTooltip('Blue Line', { sticky: true });

    // Common section highlight (St. Lideta → Stadium)
    const COMMON: [number, number][] = [
      [9.0175, 38.7358],
      [9.0148, 38.7435],
      [9.0120, 38.7505],
      [9.0090, 38.7570],
      [9.0055, 38.7636],
    ];
    L.polyline(COMMON, {
      color: '#f87171',
      weight: 4,
      opacity: 0.85,
      dashArray: '8 4',
    }).addTo(this.map).bindTooltip('Common Section', { sticky: true });
  }

  private renderMarkers() {
    if (!this.leafletReady || !this.map) return;

    // Train markers
    (this.store.entities() as any[]).forEach((train: any) => {
      const color = STATUS_COLORS[train.status] ?? '#fff';
      const html = `
        <div style="background:${color};width:16px;height:16px;
          border-radius:50%;border:2px solid #fff;
          box-shadow:0 0 10px ${color};"></div>`;
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

    // Station markers — added once
    if (!this.stationMarkersAdded && this.store.stations().length > 0) {
      this.stationMarkersAdded = true;
      (this.store.stations() as any[]).forEach((s: any) => {
        const isGreen = s.lineId === 1 || s.line?.name?.includes('Green');
        const dotColor = isGreen ? '#4ade80' : '#60a5fa';
        L.circleMarker([s.latitude, s.longitude], {
          radius: 5,
          fillColor: dotColor,
          color: '#fff',
          weight: 2,
          fillOpacity: 1,
        }).bindTooltip(`${s.name} (${s.code})`, { direction: 'top' })
          .addTo(this.map);
      });
    }
  }

  statusLabel(status: number) { return STATUS_LABELS[status] ?? 'Unknown'; }
  statusColor(status: number) { return STATUS_COLORS[status] ?? '#fff'; }
}