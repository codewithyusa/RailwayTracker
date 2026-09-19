import { computed, inject } from '@angular/core';
import {
  signalStore, withComputed, withMethods,
  patchState, withState,
} from '@ngrx/signals';
import {
  withEntities, setAllEntities, updateEntity,
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
  withComputed((store) => ({
    activeTrains: computed(() =>
      store.entities().filter((t) => t.isActive)
    ),
    delayedTrains: computed(() =>
      store.entities().filter((t) => t.status === 1)
    ),
    selectedStation: computed(() =>
      store.stations().find((s) => s.id === store.selectedStationId()) ?? null
    ),
  })),
  withMethods((store, api = inject(TrainService), sync = inject(LiveSyncService)) => ({

    loadTrains: rxMethod<void>(pipe(
      tap(() => patchState(store, { isLoading: true })),
      concatMap(() => api.getTrains().pipe(
        tap((trains) =>
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
        tap((stations) => patchState(store, { stations })),
        catchError(() => EMPTY)
      ))
    )),

    selectStation: rxMethod<number>(pipe(
      tap((id) =>
        patchState(store, { selectedStationId: id, arrivals: [], announcements: [] })
      ),
      concatMap((id) => api.getArrivals(id).pipe(
        tap((arrivals) => patchState(store, { arrivals })),
        catchError(() => EMPTY)
      )),
      concatMap(() => {
        const id = store.selectedStationId();
        if (!id) return EMPTY;
        return api.getAnnouncements(id).pipe(
          tap((announcements) => patchState(store, { announcements })),
          catchError(() => EMPTY)
        );
      })
    )),

    listenForLivePositions: rxMethod<void>(pipe(
      tap(() => sync.connect()),
      switchMap(() => sync.positions$),
      tap((event) => {
        patchState(store, updateEntity({
          id: event.trainId,
          changes: {
            latitude: event.latitude,
            longitude: event.longitude,
          } as Partial<Train>,
        }));
      })
    )),
  }))
);