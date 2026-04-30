import { Component } from '@angular/core';

@Component({
  selector: 'app-admin-dashboard',
  template: `
    <section>
      <div class="page-header">
        <h1>Admin paneli</h1>
        <p>Onay bekleyen işletmeler ve sistem denetimi için minimum admin ekranı burada büyüyecek.</p>
      </div>

      <div class="panel empty-state">
        <strong>Onay kuyruğu hazır değil.</strong>
        <span class="muted">İşletme başvuru endpoint'i eklenince bu panel gerçek veriyle dolacak.</span>
      </div>
    </section>
  `
})
export class AdminDashboardComponent {}
