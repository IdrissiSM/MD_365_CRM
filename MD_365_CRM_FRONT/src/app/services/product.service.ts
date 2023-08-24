import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { APIResponse } from '../Models/APIResponse';
import { AppStateService } from './app-state.service';

@Injectable({
    providedIn: 'root',
})
export class ProductService {
    apiUrl = environment.apiBaseUrl;
    contactId: string;

    constructor(
        private http: HttpClient,
        private appStateService: AppStateService
    ) {
        this.contactId = this.appStateService.authState.contactid;
    }

    getAllProducts() {
        return this.http.get<APIResponse>(
            `${this.apiUrl}/Product/GetProducts/${this.contactId}`
        );
    }
    getProductById(id: string) {
        return this.http.get<APIResponse>(
            `${this.apiUrl}/Product/GetProductById/${id}`
        );
    }
    getBestSellingProducts() {
        return this.http.get<APIResponse>(
            `${this.apiUrl}/Product/GetBestSellingProducts`
        );
    }
}
