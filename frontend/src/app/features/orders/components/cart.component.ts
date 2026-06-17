import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { OrderService } from '../services/order.service';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule],
  template: `
    <aside class="cart-panel">
      <div class="cart-header">
        <h2>Your Cart <span class="cart-count">{{ order.cart().length }}</span></h2>
        <p class="cart-status" *ngIf="order.loading()">Submitting order...</p>
      </div>

      <div *ngIf="order.error()" class="alert alert-warning">
        {{ order.error() }}
      </div>

      <div *ngIf="order.orderResult()" class="alert alert-success">
        Order {{ order.orderResult()?.orderId }} placed successfully!
      </div>

      <div *ngIf="order.cart().length === 0" class="empty-cart">
        Your cart is empty. Add menu items to begin.
      </div>

      <ul class="cart-list" *ngIf="order.cart().length > 0">
        <li *ngFor="let item of order.cart()" class="cart-item">
          <div>
            <h3>{{ item.name }}</h3>
            <p>{{ item.quantity }} × \${{ item.price.toFixed(2) }}</p>
          </div>

          <div class="quantity-controls">
            <button
              class="btn btn-outline-secondary btn-sm"
              (click)="changeQuantity(item.id, item.quantity - 1)"
            >
              -
            </button>
            <span>{{ item.quantity }}</span>
            <button
              class="btn btn-outline-secondary btn-sm"
              (click)="changeQuantity(item.id, item.quantity + 1)"
            >
              +
            </button>
            <button class="btn btn-link" (click)="order.removeItem(item.id)">
              Remove
            </button>
          </div>
        </li>
      </ul>

      <div class="cart-summary" *ngIf="order.cart().length > 0">
        <div class="summary-row">
          <span>Total</span>
          <strong>\${{ order.cartTotal().toFixed(2) }}</strong>
        </div>
        <button
          class="btn btn-success btn-block"
          [disabled]="order.loading()"
          (click)="submitOrder()"
        >
          Place Order
        </button>
      </div>
    </aside>
  `,
  styles: [
    `
      .cart-panel {
        border: 1px solid #e5e7eb;
        border-radius: 1rem;
        background: #f8fafc;
        padding: 1.25rem;
      }
      .cart-header {
        margin-bottom: 1rem;
      }
      .cart-status {
        color: #2563eb;
        margin-top: 0.5rem;
      }
      .empty-cart {
        color: #6b7280;
        padding: 1rem 0;
      }
      .cart-list {
        list-style: none;
        margin: 0;
        padding: 0;
        display: grid;
        gap: 1rem;
      }
      .cart-item {
        background: #ffffff;
        border: 1px solid #d1d5db;
        border-radius: 1rem;
        display: flex;
        justify-content: space-between;
        gap: 1rem;
        padding: 1rem;
      }
      .quantity-controls {
        display: grid;
        grid-template-columns: repeat(4, auto);
        gap: 0.5rem;
        align-items: center;
      }
      .summary-row {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 1rem;
        font-size: 1.05rem;
      }
      .btn-block {
        width: 100%;
      }
    `,
  ],
})
export class CartComponent {
  constructor(public order: OrderService) {}

  changeQuantity(itemId: string, quantity: number) {
    this.order.updateQuantity(itemId, quantity);
  }

  submitOrder() {
    this.order.placeOrder().subscribe({
      next: () => {},
      error: () => {
        // Error state is already tracked in the service.
      },
    });
  }
}
