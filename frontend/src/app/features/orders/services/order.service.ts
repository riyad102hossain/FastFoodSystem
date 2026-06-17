import { HttpClient } from '@angular/common/http';
import { Injectable, computed, signal } from '@angular/core';
import { catchError, finalize, tap } from 'rxjs/operators';
import { Observable, throwError } from 'rxjs';
import { API_BASE_URL } from '../../../core/services/api.config';
import { OrderItem } from '../../../core/models/order-item.model';

interface OrderPayload {
  items: Array<{ menuItemId: string; quantity: number }>;
}

export interface OrderResponse {
  orderId: string;
  status: string;
  message?: string;
}

@Injectable({
  providedIn: 'root',
})
export class OrderService {
  readonly cart = signal<OrderItem[]>([]);
  readonly loading = signal(false);
  readonly error = signal<string | null>(null);
  readonly orderResult = signal<OrderResponse | null>(null);

  readonly cartTotal = computed(() =>
    this.cart().reduce((total, item) => total + item.price * item.quantity, 0)
  );

  constructor(private http: HttpClient) {}

  addItem(item: OrderItem) {
    const current = this.cart();
    const index = current.findIndex(i => i.id === item.id);

    if (index >= 0) {
      const updated = [...current];
      updated[index] = {
        ...updated[index],
        quantity: updated[index].quantity + 1,
      };
      this.cart.set(updated);
    } else {
      this.cart.update(items => [...items, { ...item, quantity: 1 }]);
    }
  }

  updateQuantity(itemId: string, quantity: number) {
    const updated = this.cart()
      .map(item =>
        item.id === itemId ? { ...item, quantity: Math.max(1, quantity) } : item
      )
      .filter(item => item.quantity > 0);

    this.cart.set(updated);
  }

  removeItem(itemId: string) {
    this.cart.set(this.cart().filter(item => item.id !== itemId));
  }

  clearCart() {
    this.cart.set([]);
  }

  placeOrder(): Observable<OrderResponse> {
    const payload: OrderPayload = {
      items: this.cart().map(item => ({
        menuItemId: item.id,
        quantity: item.quantity,
      })),
    };

    this.loading.set(true);
    this.error.set(null);
    this.orderResult.set(null);

    return this.http.post<OrderResponse>(`${API_BASE_URL}/api/orders`, payload).pipe(
      tap(response => {
        this.orderResult.set(response);
        this.clearCart();
      }),
      catchError(error => {
        this.error.set(
          error?.message ?? 'Order submission failed. Please try again.'
        );
        return throwError(() => error);
      }),
      finalize(() => this.loading.set(false))
    );
  }
}
