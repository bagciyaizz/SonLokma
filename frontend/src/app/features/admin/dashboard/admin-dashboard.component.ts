import { Component, OnInit, inject, signal } from '@angular/core';
import { AuthService } from '../../../core/auth/auth.service';
import { BusinessProfile } from '../../../core/business/business.models';
import { BusinessService } from '../../../core/business/business.service';

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html'
})
export class AdminDashboardComponent implements OnInit {
  private readonly auth = inject(AuthService);
  private readonly businessService = inject(BusinessService);

  protected readonly pendingBusinesses = signal<BusinessProfile[]>([]);
  protected readonly isLoading = signal(true);
  protected readonly error = signal<string | null>(null);

  ngOnInit(): void {
    this.loadPending();
  }

  approve(business: BusinessProfile): void {
    this.businessService.approve(business.id).subscribe({
      next: () => this.removeBusiness(business.id),
      error: (response) => this.error.set(response?.error?.message ?? 'Onay islemi tamamlanamadi.')
    });
  }

  reject(business: BusinessProfile): void {
    this.businessService.reject(business.id).subscribe({
      next: () => this.removeBusiness(business.id),
      error: (response) => this.error.set(response?.error?.message ?? 'Red islemi tamamlanamadi.')
    });
  }

  protected isAdmin(): boolean {
    return this.auth.currentUser()?.role === 'Admin';
  }

  private loadPending(): void {
    if (!this.isAdmin()) {
      this.isLoading.set(false);
      return;
    }

    this.businessService.getPending().subscribe({
      next: (businesses) => {
        this.pendingBusinesses.set(businesses);
        this.isLoading.set(false);
      },
      error: (response) => {
        this.error.set(response?.error?.message ?? 'Onay kuyruğu yuklenemedi.');
        this.isLoading.set(false);
      }
    });
  }

  private removeBusiness(id: string): void {
    this.pendingBusinesses.update((businesses) => businesses.filter((business) => business.id !== id));
  }
}
