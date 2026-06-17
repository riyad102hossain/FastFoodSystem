import { HttpClient } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { catchError, finalize, tap } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { API_BASE_URL } from '../../../core/services/api.config';
import { MenuItem } from '../../../core/models/menu-item.model';

@Injectable({
  providedIn: 'root',
})
export class CatalogService {
  readonly menuItems = signal<MenuItem[]>([]);
  readonly loading = signal(false);
  readonly error = signal<string | null>(null);

  constructor(private http: HttpClient) {}

  loadMenuItems() {
    this.loading.set(true);
    this.error.set(null);

    return this.http.get<MenuItem[]>(`${API_BASE_URL}/api/menu-items/available`).pipe(
      tap(items => this.menuItems.set(items)),
      catchError(error => {
        this.error.set(
          error?.message ?? 'Unable to load menu items. Please try again.'
        );
        return throwError(() => error);
      }),
      finalize(() => this.loading.set(false))
    );
  }
}
