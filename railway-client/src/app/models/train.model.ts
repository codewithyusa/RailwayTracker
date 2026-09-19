export interface Train {
  id: number;
  code: string;
  name: string;
  latitude: number;
  longitude: number;
  isActive: boolean;
  status: 0 | 1 | 2 | 3; // OnTime, Delayed, Cancelled, Arrived
}

export interface Station {
  id: number;
  name: string;
  code: string;
  latitude: number;
  longitude: number;
}

export interface Arrival {
  id: number;
  trainId: number;
  stationId: number;
  platform: string;
  delayMinutes: number;
  scheduledTime: string;
}

export interface Announcement {
  id: number;
  title: string;
  body: string;
  stationId: number;
  createdAt: string;
}

export interface CreateAnnouncementDto {
  title: string;
  body: string;
  stationId: number;
}