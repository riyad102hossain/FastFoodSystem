import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { CartComponent } from './features/orders/components/cart.component';
import { MenuListComponent } from './features/catalog/components/menu-list.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, MenuListComponent, CartComponent],
  template: `
    <div class="app-shell">
      <main class="content">
        <app-menu-list></app-menu-list>
      </main>
      <aside class="sidebar">
        <app-cart></app-cart>
      </aside>
    </div>
  `,
})
export class AppComponent {}
