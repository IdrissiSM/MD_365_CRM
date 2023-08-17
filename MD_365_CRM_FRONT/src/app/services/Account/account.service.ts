import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {APIResponse} from "../../Models/APIResponse";

@Injectable({
  providedIn: 'root'
})
export class AccountService {

   private  readonly  baseUrl : string = "https://localhost:7118/api/Account/";

  constructor(private http : HttpClient) { }

  getTopCustomers() {
      return this.http.get<APIResponse>(`${this.baseUrl}GetAllAccountsOrderedByRevenue`);
  }
}
