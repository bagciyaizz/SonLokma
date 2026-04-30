import { Component } from '@angular/core';

@Component({
  selector: 'app-business-dashboard',
  template: `
    <section>
      <div class="page-header">
        <h1>İşletme paneli</h1>
        <p>İşletme başvurusu, paket oluşturma ve pickup kod doğrulama akışı bir sonraki aşamada buraya eklenecek.</p>
      </div>

      <div class="grid cards">
        <article class="panel metric">
          <span class="muted">Aktif paket</span>
          <strong>0</strong>
        </article>
        <article class="panel metric">
          <span class="muted">Bugünkü rezervasyon</span>
          <strong>0</strong>
        </article>
        <article class="panel metric">
          <span class="muted">Kurtarılan paket</span>
          <strong>0</strong>
        </article>
      </div>
    </section>
  `
})
export class BusinessDashboardComponent {}
