import { Component, inject, viewChild, effect } from '@angular/core';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatPaginatorModule, MatPaginator } from '@angular/material/paginator';
import { MatSortModule, MatSort } from '@angular/material/sort';
import { RailwayStore } from '../../store/railway.store';
import { Station, Arrival, Announcement } from '../../models/train.model';

@Component({
  selector: 'app-stations',
  standalone: true,
  imports: [MatTableModule, MatPaginatorModule, MatSortModule],
  templateUrl: './stations.component.html',
  styleUrl: './stations.component.scss',
})
export class StationsComponent {
  store = inject(RailwayStore);
  columns = ['name', 'code', 'latitude', 'longitude', 'actions'];
  dataSource = new MatTableDataSource<Station>();

  readonly paginator = viewChild.required(MatPaginator);
  readonly sort = viewChild.required(MatSort);

  constructor() {
    effect(() => { this.dataSource.data = this.store.stations(); });
    effect(() => {
      this.dataSource.paginator = this.paginator();
      this.dataSource.sort = this.sort();
    });
  }

  selectStation(id: number) {
    this.store.selectStation(id);
  }

  delayLabel(mins: number) {
    return mins > 0 ? `+${mins} min` : 'On Time';
  }
}