import { inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Train, Station, Arrival, Announcement, CreateAnnouncementDto } from '../models/train.model';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class TrainService {
  private http = inject(HttpClient);
  private base = 'http://localhost:5285/api';

  getTrains(): Observable<Train[]> {
    return this.http.get<Train[]>(`${this.base}/trains`);
  }

  getStations(): Observable<Station[]> {
    return this.http.get<Station[]>(`${this.base}/stations`);
  }

  getArrivals(stationId: number): Observable<Arrival[]> {
    return this.http.get<Arrival[]>(`${this.base}/stations/${stationId}/arrivals`);
  }

  getAnnouncements(stationId: number): Observable<Announcement[]> {
    return this.http.get<Announcement[]>(`${this.base}/announcements/station/${stationId}`);
  }

  getAllAnnouncements(): Observable<Announcement[]> {
    return this.http.get<Announcement[]>(`${this.base}/announcements`);
  }

  createAnnouncement(dto: CreateAnnouncementDto): Observable<number> {
    return this.http.post<number>(`${this.base}/announcements`, dto);
  }

  updatePosition(trainId: number, lat: number, lng: number, token: string): Observable<void> {
    return this.http.put<void>(
      `${this.base}/trains/${trainId}/position`,
      { latitude: lat, longitude: lng },
      { headers: { Authorization: `Bearer ${token}` } }
    );
  }
}