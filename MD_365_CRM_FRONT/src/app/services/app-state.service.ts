import { Injectable, OnInit } from '@angular/core';
import jwt_decode from 'jwt-decode';
import { Contact } from '../Models/Contact';

@Injectable({
  providedIn: 'root',
})
export class AppStateService {
  constructor() {
    this.getAuthState();
  }

  authState!: any;
  registrationStep: number = -1;
  resetPasswordStep: number = -1;
  contact!: Contact;
  otp: number = -1;

  getAuthState() {
    const authStateString = localStorage.getItem('authState') ?? sessionStorage.getItem('authState');
    this.authState = authStateString // get the authStateString if not null pass to the next condition
      && new Date(JSON.parse(authStateString!).expiresOn).getTime() > new Date().getTime() ? // verify if it had expired
      JSON.parse(authStateString)
      : {
          isAuthenticated: false,
        };
    return this.authState;
  }

  setAuthStateLocal(token: string) {
    const decodedJWT: any = jwt_decode(token);
    this.authState = {
      isAuthenticated: true,
      uid: decodedJWT.uid,
      email: decodedJWT.sub,
      roles: decodedJWT.roles,
      token: token,
      contactId: decodedJWT.contactid,
      expiresOn: new Date(decodedJWT.exp * 1000),
    };
    localStorage.removeItem('authState');
    localStorage.setItem('authState', JSON.stringify(this.authState));
  }

  setAuthStateSession(token: string) {
    const decodedJWT: any = jwt_decode(token);
    this.authState = {
      isAuthenticated: true,
      uid: decodedJWT.uid,
      email: decodedJWT.sub,
      roles: decodedJWT.roles,
      token: token,
      contactId: decodedJWT.contactid,
      expiresOn: new Date(decodedJWT.exp * 1000),
    };
    sessionStorage.removeItem('authState');
    sessionStorage.setItem('authState', JSON.stringify(this.authState));
  }
}
