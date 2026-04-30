import { Component } from '@angular/core';

@Component({
  selector: 'app-reservations',
  template: `
    <section>
      <div class="page-header">
        <h1>Rezervasyonlarım</h1>
        <p>Pickup code ve aktif rezervasyon kartları reservation API eklenince burada görünecek.</p>
      </div>

      <div class="panel empty-state">
        <strong>Henüz aktif rezervasyon yok.</strong>
        <span class="muted">Listing ve reserve akışı ikinci aşamada bağlanacak.</span>
      </div>
    </section>
  `
})
export class ReservationsComponent {}
