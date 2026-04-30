export type UserRole = 'Customer' | 'Business' | 'Admin';

export interface UserProfile {
  id: string;
  fullName: string;
  email: string;
  phone?: string | null;
  role: UserRole;
}

export interface AuthResponse {
  accessToken: string;
  expiresAt: string;
  user: UserProfile;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  fullName: string;
  email: string;
  password: string;
  phone?: string | null;
  role: UserRole;
}
