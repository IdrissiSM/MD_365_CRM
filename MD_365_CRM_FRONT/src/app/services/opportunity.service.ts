import { Injectable } from '@angular/core';
import { APIResponse } from '../Models/APIResponse';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Opportunity } from '../Models/Opportunity';

@Injectable({
    providedIn: 'root',
})
export class OpportunityService {
    apiUrl = environment.apiBaseUrl;
    token = '';

    constructor(private http: HttpClient) {}

    getAllOpportunities() {
        const httpOptions = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
                Authorization: 'Bearer ' + this.token,
            }),
        };
        return this.http.get<APIResponse>(
            `${this.apiUrl}/Opportunity`,
            httpOptions
        );
    }

    getOpportunityById(opportunityId: string) {
        const httpOptions = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
                Authorization: 'Bearer ' + this.token,
            }),
        };
        return this.http.get<APIResponse>(
            `${this.apiUrl}/Opportunity/${opportunityId}`,
            httpOptions
        );
    }
}
