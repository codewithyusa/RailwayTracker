import { computed, inject } from '@angular/core';
import {
  signalStore,
  withComputed,
  withMethods,
  patchState,
  withState,
} from '@ngrx/signals';
import {
  withEntities,
  setAllEntities,
  updateEntity,
} from '@ngrx/signals/entities';
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { pipe, concatMap, tap, catchError, EMPTY, switchMap } from 'rxjs';
import { TrainService } from '../services/train.service';
import { LiveSyncService } from '../services/live-sync.service';
import { Train, Station, Arrival, Announcement } from '../models/train.model';

type RailwayState = {
  stations: Station[];
  arrivals: Arrival[];
  announcements: Announcement[];
  selectedStationId: number | null;
  isLoading: boolean;
  error: string | null;
};

export const RailwayStore = signalStore(
  { providedIn: 'root' },
  withState<RailwayState>({
    stations: [],
    arrivals: [],
    announcements: [],
    selectedStationId: null,
    isLoading: false,
    error: null,
  }),
  withEntities<Train>(),
  withComputed((store: any) => ({
    activeTrains: computed(() =>
      (store.entities() as Train[]).filter((t: Train) => t.isActive)
    ),
    delayedTrains: computed(() =>
      (store.entities() as Train[]).filter((t: Train) => t.status === 1)
    ),
    selectedStation: computed(() =>
      (store.stations() as Station[]).find(
        (s: Station) => s.id === store.selectedStationId()
      ) ?? null
    ),
  })),
  withMethods((store: any, api = inject(TrainService), sync = inject(LiveSyncService)) => ({

    loadTrains: rxMethod<void>(pipe(
      tap(() => patchState(store, { isLoading: true })),
      concatMap(() => api.getTrains().pipe(
        tap((trains: Train[]) =>
          patchState(store, setAllEntities(trains), { isLoading: false })
        ),
        catchError((err: Error) => {
          patchState(store, { isLoading: false, error: err.message });
          return EMPTY;
        })
      ))
    )),

    loadStations: rxMethod<void>(pipe(
      concatMap(() => api.getStations().pipe(
        tap((stations: Station[]) => patchState(store, { stations })),
        catchError(() => EMPTY)
      ))
    )),

    selectStation: rxMethod<number>(pipe(
      tap((id: number) =>
        patchState(store, { selectedStationId: id, arrivals: [], announcements: [] })
      ),
      concatMap((id: number) => api.getArrivals(id).pipe(
        tap((arrivals: Arrival[]) => patchState(store, { arrivals })),
        catchError(() => EMPTY)
      )),
      concatMap(() => {
        const id = store.selectedStationId() as number | null;
        if (!id) return EMPTY;
        return api.getAnnouncements(id).pipe(
          tap((announcements: Announcement[]) => patchState(store, { announcements })),
          catchError(() => EMPTY)
        );
      })
    )),

    listenForLivePositions: rxMethod<void>(pipe(
      tap(() => sync.connect()),
      switchMap(() => sync.positions$),
      tap((event: any) => {
        patchState(store, updateEntity({
          id: event.trainId,
          changes: { latitude: event.latitude, longitude: event.longitude }
        }));
      })
    )),
  }))
);