import { inject, Injectable, signal, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Subject } from 'rxjs';

export interface TrainPositionEvent {
  trainId: number;
  latitude: number;
  longitude: number;
}

@Injectable({ providedIn: 'root' })
export class LiveSyncService {
  private platformId = inject(PLATFORM_ID);
  private connection: HubConnection | null = null;
  private positionSubject = new Subject<TrainPositionEvent>();

  positions$ = this.positionSubject.asObservable();
  connectionState = signal<'connected' | 'reconnecting' | 'disconnected'>('disconnected');

  connect() {
    if (this.connection) return;
    if (!isPlatformBrowser(this.platformId)) return;

    this.connection = new HubConnectionBuilder()
      .withUrl('http://localhost:5285/hubs/trains')
      .withAutomaticReconnect([0, 2000, 10000, 30000])
      .build();

    this.connection.on('ReceiveTrainPosition',
      (trainId: number, lat: number, lng: number) => {
        this.positionSubject.next({ trainId, latitude: lat, longitude: lng });
      }
    );

    this.connection.onreconnecting(() => this.connectionState.set('reconnecting'));
    this.connection.onreconnected(() => this.connectionState.set('connected'));
    this.connection.onclose(() => this.connectionState.set('disconnected'));

    this.connection
      .start()
      .then(() => this.connectionState.set('connected'))
      .catch((err: unknown) => console.error('SignalR error:', err));
  }
}