export type BusinessCategory = 'Bakery' | 'Restaurant' | 'Cafe' | 'Market' | 'Grocer' | 'Other';
export type BusinessStatus = 'Pending' | 'Approved' | 'Rejected' | 'Suspended';

export interface BusinessProfile {
  id: string;
  ownerUserId: string;
  ownerFullName: string;
  name: string;
  description: string;
  category: BusinessCategory;
  address: string;
  latitude: number;
  longitude: number;
  status: BusinessStatus;
  createdAt: string;
}

export interface UpsertBusinessRequest {
  name: string;
  description: string;
  category: BusinessCategory;
  address: string;
  latitude: number;
  longitude: number;
}
