import { Component, inject, AfterViewInit, signal } from '@angular/core';
import { DecimalPipe } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { RailwayStore } from '../../store/railway.store';

declare const L: any;

const STATUS_LABELS = ['On Time', 'Delayed', 'Cancelled', 'Arrived'];
const STATUS_COLORS = ['#4ade80', '#fb923c', '#f87171', '#60a5fa'];

// Green Line: Torhailoch (west) → Ayat (east)
const GREEN_LINE: [number, number][] = [
  [9.0140, 38.7140], // Torhailoch
  [9.0148, 38.7242], // CocaCola
  [9.0155, 38.7358], // St. Lideta
  [9.0068, 38.7435], // Tegbared
  [9.0098, 38.7505], // Mexico
  [9.0122, 38.7570], // Leghar
  [9.0145, 38.7636], // Stadium
  [9.0155, 38.7690], // St. Estraos
  [9.0163, 38.7745], // Bambis
  [9.0171, 38.7800], // St. Urael
  [9.0178, 38.7855], // Hayahulet 2
  [9.0185, 38.7910], // Hayahulet 1
  [9.0192, 38.7965], // Lem Hotel
  [9.0200, 38.8010], // Megenagna
  [9.0200, 38.8055], // Gurd Shola 2
  [9.0200, 38.8100], // Gurd Shola 1
  [9.0200, 38.8145], // Management Institute
  [9.0200, 38.8190], // Civil Service College
  [9.0200, 38.8235], // St. Michael
  [9.0200, 38.8332], // CMC
  [9.0208, 38.8435], // Meri
  [9.0229, 38.8773], // Ayat
];

// Blue Line: Menelik II Square (north) → Kality (south)
const BLUE_LINE: [number, number][] = [
  [9.0483, 38.7468], // Menelik II Square
  [9.0421, 38.7471], // Atikilt Tera
  [9.0360, 38.7474], // Gojam Berenda
  [9.0295, 38.7478], // Autobus Tera
  [9.0230, 38.7478], // Sebategna
  [9.0165, 38.7468], // Abnet
  [9.0100, 38.7455], // Darmar
  [9.0068, 38.7435], // Tegbared (interchange)
  [9.0098, 38.7505], // Mexico
  [9.0122, 38.7570], // Leghar
  [9.0145, 38.7636], // Stadium
  [9.0080, 38.7636], // Meshwlekya
  [9.0010, 38.7636], // Riche
  [8.9940, 38.7636], // Temenja Yazh
  [8.9870, 38.7636], // Lancha
  [8.9800, 38.7636], // Nefas Silk 2
  [8.9730, 38.7636], // Nefas Silk 1
  [8.9660, 38.7636], // Adey Ababa
  [8.9590, 38.7636], // Saris
  [8.9510, 38.7636], // Abo Junction
  [8.9430, 38.7636], // Kality
];

// Common Section: Tegbared → Stadium
const COMMON: [number, number][] = [
  [9.0068, 38.7435], // Tegbared
  [9.0098, 38.7505], // Mexico
  [9.0122, 38.7570], // Leghar
  [9.0145, 38.7636], // Stadium
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
    L.polyline(GREEN_LINE, {
      color: '#4ade80', weight: 5, opacity: 0.9,
    }).addTo(this.map).bindTooltip('Green Line — Line 1', { sticky: true });

    L.polyline(BLUE_LINE, {
      color: '#60a5fa', weight: 5, opacity: 0.9,
    }).addTo(this.map).bindTooltip('Blue Line — Line 2', { sticky: true });

    L.polyline(COMMON, {
      color: '#f87171', weight: 6, opacity: 1, dashArray: '10 5',
    }).addTo(this.map).bindTooltip('Common Section', { sticky: true });

    // Interchange marker at Tegbared
    L.circleMarker([9.0068, 38.7435], {
      radius: 9, fillColor: '#fff',
      color: '#f87171', weight: 3, fillOpacity: 1,
    }).bindTooltip('Tegbared — Interchange').addTo(this.map);

    // Legend
    const legend = (L.control as any)({ position: 'bottomleft' });
    legend.onAdd = () => {
      const div = L.DomUtil.create('div');
      div.innerHTML = `
        <div style="background:#111827;padding:10px 14px;border-radius:8px;
          border:1px solid #1e293b;font-size:12px;color:#e2e8f0;line-height:2">
          <div><span style="color:#4ade80;font-weight:700">●</span> Line 1 — Green</div>
          <div><span style="color:#60a5fa;font-weight:700">●</span> Line 2 — Blue</div>
          <div><span style="color:#f87171;font-weight:700">╌</span> Common Section</div>
          <div><span style="color:#fff;font-weight:700">◎</span> Interchange</div>
        </div>`;
      return div;
    };
    legend.addTo(this.map);
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
        const isCommon = s.line?.name?.includes('Common');
        const dotColor = isCommon ? '#f87171' : isGreen ? '#4ade80' : '#60a5fa';
        L.circleMarker([s.latitude, s.longitude], {
          radius: 5, fillColor: dotColor,
          color: '#fff', weight: 2, fillOpacity: 1,
        }).bindTooltip(`${s.name} (${s.code})`, { direction: 'top' })
          .addTo(this.map);
      });
    }
  }

  statusLabel(status: number) { return STATUS_LABELS[status] ?? 'Unknown'; }
  statusColor(status: number) { return STATUS_COLORS[status] ?? '#fff'; }
}