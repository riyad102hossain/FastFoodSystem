import { CommonModule } from '@angular/common';
import { Component, OnInit, signal } from '@angular/core';
import { CatalogService } from '../services/catalog.service';
import { OrderService } from '../../orders/services/order.service';
import { MenuItem } from '../../../core/models/menu-item.model';

@Component({
  selector: 'app-menu-list',
  standalone: true,
  imports: [CommonModule],
  template: `
    <section class="menu-section">
      <div class="menu-header">
        <h1>Fast Food Menu</h1>
        <p class="subtitle">Fresh, fast, and always ready to satisfy.</p>
      </div>

      <div *ngIf="addedToCartMessage()" class="toast">
        {{ addedToCartMessage() }}
      </div>

      <div *ngIf="catalog.loading()" class="loader">Loading menu...</div>
      <div *ngIf="catalog.error()" class="alert alert-danger">
        {{ catalog.error() }}
      </div>

      <div class="menu-grid">
        <article class="menu-card" *ngFor="let item of catalog.menuItems()">
          <div class="menu-card__top">
            <h2>{{ item.name }}</h2>
            <span
              class="badge"
              [class.badge-success]="item.isAvailable"
              [class.badge-secondary]="!item.isAvailable"
            >
              {{ item.isAvailable ? 'In Stock' : 'Sold Out' }}
            </span>
          </div>

          <p class="menu-description">{{ item.description }}</p>

          <div class="menu-footer">
            <strong class="price">\${{ item.price.toFixed(2) }}</strong>
            <button
              class="btn btn-primary"
              [disabled]="!item.isAvailable"
              (click)="addToCart(item)"
            >
              Add to Cart
            </button>
          </div>
        </article>
      </div>
    </section>
  `,
  styles: [
    `
      .menu-section {
        padding: 1rem 0;
      }
      .menu-header {
        margin-bottom: 1.5rem;
      }
      .subtitle {
        color: #6b7280;
        margin-top: 0.5rem;
      }
      .loader {
        margin-bottom: 1rem;
        font-weight: 600;
      }
      .menu-grid {
        display: grid;
        gap: 1.25rem;
        grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
      }
      .menu-card {
        background: #ffffff;
        border: 1px solid #e5e7eb;
        border-radius: 1rem;
        box-shadow: 0 10px 24px rgba(15, 23, 42, 0.05);
        display: flex;
        flex-direction: column;
        gap: 1rem;
        padding: 1.25rem;
      }
      .menu-card__top {
        display: flex;
        align-items: center;
        justify-content: space-between;
        gap: 1rem;
      }
      .menu-description {
        color: #4b5563;
        line-height: 1.6;
      }
      .menu-footer {
        display: flex;
        align-items: center;
        justify-content: space-between;
        gap: 1rem;
      }
      .price {
        font-size: 1.15rem;
      }
      .badge {
        border-radius: 999px;
        padding: 0.35rem 0.75rem;
        font-size: 0.8rem;
      }
      .badge-success {
        background-color: #dcfce7;
        color: #166534;
      }
      .badge-secondary {
        background-color: #e5e7eb;
        color: #374151;
      }
    `,
  ],
})
export class MenuListComponent implements OnInit {
  readonly addedToCartMessage = signal('');

  constructor(
    public catalog: CatalogService,
    private orderService: OrderService
  ) {}

  ngOnInit() {
    this.catalog.loadMenuItems().subscribe({
      error: () => {
        // State is handled in the service.
      },
    });
  }

  addToCart(item: MenuItem) {
    this.orderService.addItem({
      id: item.id,
      name: item.name,
      price: item.price,
      quantity: 1,
    });
    this.addedToCartMessage.set(`${item.name} added to cart!`);
    window.setTimeout(() => this.addedToCartMessage.set(''), 1600);
  }
}
