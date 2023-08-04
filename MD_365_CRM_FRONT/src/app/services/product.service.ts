import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { APIResponse } from '../Models/APIResponse';

@Injectable({
    providedIn: 'root',
})
export class ProductService {
    apiUrl = environment.apiBaseUrl;
    token = '';

    constructor(private http: HttpClient) {}

    getAllProducts() {
        const httpOptions = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
                Authorization: 'Bearer ' + this.token,
            }),
        };
        return this.http.get<APIResponse>(
            `${this.apiUrl}/Product/GetProducts`,
            httpOptions
        );
    }
    getProductById(id: string) {
        const httpOptions = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
                Authorization: 'Bearer ' + this.token,
            }),
        };
        return this.http.get<APIResponse>(
            `${this.apiUrl}/Product/GetProductById/${id}`,
            httpOptions
        );
    }
}
