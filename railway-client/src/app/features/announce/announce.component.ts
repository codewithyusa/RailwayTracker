import { Component, inject, signal } from '@angular/core';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { Subject, exhaustMap, takeUntilDestroyed } from 'rxjs';
import { TrainService } from '../../services/train.service';
import { RailwayStore } from '../../store/railway.store';
import { CreateAnnouncementDto } from '../../models/train.model';

@Component({
  selector: 'app-announce',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule, MatInputModule,
    MatButtonModule, MatProgressSpinnerModule, MatSelectModule
  ],
  templateUrl: './announce.component.html',
  styleUrl: './announce.component.scss',
})
export class AnnounceComponent {
  private fb = inject(FormBuilder);
  private api = inject(TrainService);
  store = inject(RailwayStore);

  isSubmitting = signal(false);
  status = signal('');

  form = this.fb.nonNullable.group({
    title: ['', [Validators.required, Validators.minLength(1)]],
    body: ['', [Validators.required, Validators.minLength(1)]],
    stationId: [0, [Validators.required, Validators.min(1)]],
  });

  private submit$ = new Subject<CreateAnnouncementDto>();

  constructor() {
    this.submit$.pipe(
      exhaustMap(dto => {
        this.isSubmitting.set(true);
        this.status.set('Sending announcement...');
        return this.api.createAnnouncement(dto);
      }),
      takeUntilDestroyed()
    ).subscribe({
      next: id => {
        this.isSubmitting.set(false);
        this.status.set(`✅ Announcement posted (ID: ${id})`);
        this.form.reset({ title: '', body: '', stationId: 0 });
      },
      error: err => {
        this.isSubmitting.set(false);
        this.status.set(`❌ Failed: ${err.message}`);
      }
    });
  }

  onSubmit() {
    if (this.form.valid) {
      this.submit$.next(this.form.getRawValue() as CreateAnnouncementDto);
    } else {
      this.form.markAllAsTouched();
    }
  }
}