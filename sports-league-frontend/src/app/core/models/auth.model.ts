export interface LoginRequest {
  email: string;
  password: string;
}
 
export interface RegisterRequest {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
}
 
export interface AuthResponse {
  token: string;
  expiration: string;
  email: string;
  roles: string[];
}
