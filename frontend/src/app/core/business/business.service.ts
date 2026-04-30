import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '../../../environments/environment';
import { BusinessProfile, UpsertBusinessRequest } from './business.models';

@Injectable({ providedIn: 'root' })
export class BusinessService {
  private readonly http = inject(HttpClient);

  getMine() {
    return this.http.get<BusinessProfile>(`${environment.apiUrl}/businesses/me`);
  }

  apply(request: UpsertBusinessRequest) {
    return this.http.post<BusinessProfile>(`${environment.apiUrl}/businesses`, request);
  }

  updateMine(request: UpsertBusinessRequest) {
    return this.http.put<BusinessProfile>(`${environment.apiUrl}/businesses/me`, request);
  }

  getPending() {
    return this.http.get<BusinessProfile[]>(`${environment.apiUrl}/admin/businesses/pending`);
  }

  approve(id: string) {
    return this.http.post<BusinessProfile>(`${environment.apiUrl}/admin/businesses/${id}/approve`, {});
  }

  reject(id: string) {
    return this.http.post<BusinessProfile>(`${environment.apiUrl}/admin/businesses/${id}/reject`, {});
  }
}
