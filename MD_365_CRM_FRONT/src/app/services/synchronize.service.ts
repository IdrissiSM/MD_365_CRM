import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { APIResponse } from '../Models/APIResponse';

@Injectable({
    providedIn: 'root',
})
export class SynchronizeService {

    apiUrl = environment.apiBaseUrl;

    constructor(private http: HttpClient) {}

    synchronizeOpportunities(contactId: string) {
        return this.http.get<APIResponse>(`${this.apiUrl}/Synchronize/opportunities/${contactId}`);
    }

    synchronizeIncidents(contactId: string) {
        return this.http.get<APIResponse>(`${this.apiUrl}/Synchronize/incidents/${contactId}`);
    }

    synchronizeProducts(contactId: string) {
        return this.http.get<APIResponse>(`${this.apiUrl}/Synchronize/products/${contactId}`);
    }

    synchronizeProfile(contactId: string) {
        return this.http.get<APIResponse>(`${this.apiUrl}/Synchronize/profile/${contactId}`);
    }
}
