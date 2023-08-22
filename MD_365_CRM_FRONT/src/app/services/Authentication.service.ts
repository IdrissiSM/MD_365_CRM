import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { AppStateService } from './app-state.service';
import { Login } from '../Models/LoginRequest';
import { Register } from '../Models/RegisterRequest';
import { AuthResponse } from '../Models/AuthResponse';
import { Observable } from 'rxjs';
import { VerifyEmailRequest } from '../Models/EmailVerificationRequest';
import { ConfirmEmail } from '../Models/EmailConfirmationRequest';
import { Contact } from '../Models/Contact';
import { ResetPassword } from '../Models/ResetPasswordRequest';
import { EmailVerificationResponse } from '../Models/EmailVerificationResponse';
import { UpdateProfileRequest } from '../Models/UpdateProfileRequest';
import { ChangePasswordRequest } from '../Models/ChangePasswordRequest';
import { APIResponse } from '../Models/APIResponse';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {
  apiUrl = environment.apiBaseUrl;

  constructor(private http: HttpClient, private appState: AppStateService) {}

  login(loginRequest: Login): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/Auth/login`, loginRequest);
  }

  register(registerRequest: Register): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/Auth/register`, registerRequest);
  }

  emailVerification(verifyEmailRequest: VerifyEmailRequest): Observable<EmailVerificationResponse> {
    return this.http.post<EmailVerificationResponse>(`${this.apiUrl}/Auth/email_verification`, verifyEmailRequest);
  }

  emailConfirmation(confirmEmailRequest: ConfirmEmail): Observable<Contact | null> {
    return this.http.post<Contact | null>(`${this.apiUrl}/Auth/email_confirmation`, confirmEmailRequest);
  }

  resetPassword(resetPasswordRequest: ResetPassword): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/Auth/reset_password`, resetPasswordRequest);
  }

  updateProfile(updateProfileRequest: UpdateProfileRequest): Observable<Contact | null> {
    return this.http.post<Contact | null>(`${this.apiUrl}/Auth/update_user_profile`, updateProfileRequest);
  }

  changePassword(changePasswordRequest: ChangePasswordRequest): Observable<APIResponse> {
    return this.http.post<APIResponse>(`${this.apiUrl}/Auth/change_password`, changePasswordRequest);
  }
}
