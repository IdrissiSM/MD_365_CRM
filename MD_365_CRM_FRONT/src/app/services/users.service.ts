import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { APIResponse } from '../Models/APIResponse';
import { BlacklistedUser } from '../Models/blacklisted-user';

@Injectable({
  providedIn: 'root'
})
export class UsersService {
  apiUrl = environment.apiBaseUrl;

  constructor(private http: HttpClient) {}

  getAllUsers() {
      return this.http.get<APIResponse>(
          `${this.apiUrl}/Users/GetUsers`
      );
  }

  getAllBlacklistedUsers() {
    return this.http.get<APIResponse>(
        `${this.apiUrl}/Blacklist/GetBlacklistedUsers`
    );
  }
  addToBlacklist(blacklisted: BlacklistedUser){
    return this.http.post<APIResponse>(
      `${this.apiUrl}/Blacklist/AddToBlacklist`, blacklisted
    );
  }
  removeFromBlacklist(email: string){
    return this.http.delete<APIResponse>(
      `${this.apiUrl}/Blacklist/RemoveFromBlacklist/${email}`
    );
  }
  deleteUser(id: any){
    return this.http.delete<APIResponse>(
      `${this.apiUrl}/Users/DeleteUser/${id}`
    );
  }
}
