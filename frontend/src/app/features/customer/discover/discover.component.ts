import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

interface DemoListing {
  business: string;
  category: string;
  title: string;
  price: string;
  pickup: string;
  stock: number;
}

@Component({
  selector: 'app-discover',
  imports: [RouterLink],
  templateUrl: './discover.component.html'
})
export class DiscoverComponent {
  protected readonly listings: DemoListing[] = [
    {
      business: 'Köşe Fırın',
      category: 'Fırın',
      title: 'Günün sürpriz fırın paketi',
      price: '89 TL',
      pickup: '19:00 - 21:00',
      stock: 5
    },
    {
      business: 'Nar Lokanta',
      category: 'Restoran',
      title: 'Akşam yemeği kurtarma paketi',
      price: '129 TL',
      pickup: '20:00 - 21:30',
      stock: 3
    },
    {
      business: 'Mahalle Market',
      category: 'Market',
      title: 'Taze ürün sürpriz çantası',
      price: '99 TL',
      pickup: '18:30 - 20:00',
      stock: 7
    }
  ];
}
