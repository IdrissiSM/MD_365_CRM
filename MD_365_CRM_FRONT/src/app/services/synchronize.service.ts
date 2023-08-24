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

    synchronize() {
        return this.http.get<APIResponse>(`${this.apiUrl}/Synchronize`);
    }
}
