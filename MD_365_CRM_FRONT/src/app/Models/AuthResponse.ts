export interface AuthResponse {
  isAuthenticated: boolean,
  token: string,
  message: string,
  role: string
}