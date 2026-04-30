import { Component, OnInit, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../../core/auth/auth.service';
import { BusinessCategory, BusinessProfile, UpsertBusinessRequest } from '../../../core/business/business.models';
import { BusinessService } from '../../../core/business/business.service';

@Component({
  selector: 'app-business-dashboard',
  imports: [ReactiveFormsModule],
  templateUrl: './business-dashboard.component.html'
})
export class BusinessDashboardComponent implements OnInit {
  private readonly auth = inject(AuthService);
  private readonly businessService = inject(BusinessService);
  private readonly formBuilder = inject(FormBuilder);

  protected readonly business = signal<BusinessProfile | null>(null);
  protected readonly isLoading = signal(true);
  protected readonly isSubmitting = signal(false);
  protected readonly message = signal<string | null>(null);
  protected readonly error = signal<string | null>(null);
  protected readonly categories: BusinessCategory[] = ['Bakery', 'Restaurant', 'Cafe', 'Market', 'Grocer', 'Other'];

  protected readonly form = this.formBuilder.nonNullable.group({
    name: ['', [Validators.required, Validators.minLength(2)]],
    description: ['', [Validators.required, Validators.minLength(10)]],
    category: ['Bakery' as BusinessCategory, [Validators.required]],
    address: ['', [Validators.required, Validators.minLength(5)]],
    latitude: [41.0082, [Validators.required]],
    longitude: [28.9784, [Validators.required]]
  });

  ngOnInit(): void {
    this.loadBusiness();
  }

  submit(): void {
    if (this.form.invalid || this.isSubmitting()) {
      return;
    }

    this.isSubmitting.set(true);
    this.message.set(null);
    this.error.set(null);

    const request = this.toRequest();
    const action = this.business()
      ? this.businessService.updateMine(request)
      : this.businessService.apply(request);

    action.subscribe({
      next: (business) => {
        this.business.set(business);
        this.patchForm(business);
        this.message.set(business.status === 'Approved' ? 'Isletme profili guncellendi.' : 'Basvuru admin onayina gonderildi.');
        this.isSubmitting.set(false);
      },
      error: (response) => {
        this.error.set(response?.error?.message ?? 'Isletme bilgileri kaydedilemedi.');
        this.isSubmitting.set(false);
      }
    });
  }

  private loadBusiness(): void {
    this.businessService.getMine().subscribe({
      next: (business) => {
        this.business.set(business);
        this.patchForm(business);
        this.isLoading.set(false);
      },
      error: (response) => {
        if (response?.status !== 404) {
          this.error.set(response?.error?.message ?? 'Isletme bilgileri yuklenemedi.');
        }
        this.isLoading.set(false);
      }
    });
  }

  private patchForm(business: BusinessProfile): void {
    this.form.patchValue({
      name: business.name,
      description: business.description,
      category: business.category,
      address: business.address,
      latitude: business.latitude,
      longitude: business.longitude
    });
  }

  private toRequest(): UpsertBusinessRequest {
    const value = this.form.getRawValue();
    return {
      name: value.name,
      description: value.description,
      category: value.category,
      address: value.address,
      latitude: Number(value.latitude),
      longitude: Number(value.longitude)
    };
  }

  protected isBusinessAccount(): boolean {
    const role = this.auth.currentUser()?.role;
    return role === 'Business' || role === 'Admin';
  }
}
