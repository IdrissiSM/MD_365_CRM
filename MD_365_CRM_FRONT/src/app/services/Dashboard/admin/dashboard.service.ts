import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {APIResponse} from "../../../Models/APIResponse";
import {firstValueFrom, Subject} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
    private  readonly  baseUrl : string = "https://localhost:7118/api/Dashboard/";
    private opportunitiesScore : any[] = [];
    public opportunitiesScoreSubject = new Subject<any[]>();

  constructor(private  http : HttpClient) { }

  getStats() {
  return this.http.get<APIResponse>(`${this.baseUrl}GetStats`);
  }

  getOpportunitiesValues(){
      return this.http.get<APIResponse>(`${this.baseUrl}GetOpportunitiesValues`);
  }

  async getOpportunitiesScore(month : string) {
      let isSuccess = true;
      await firstValueFrom(this.http.get<APIResponse>(`${this.baseUrl}GetOpportunitiesScoreByMonth/${month}`))
          .then(value => {
              if(value.success) {
                  this.opportunitiesScore = value.result;
                  this.opportunitiesScore = [];
                  value.result.forEach((v : any ) => {
                      this.opportunitiesScore.push({x : new Date(v['estimatedCloseDate']).getDate(),y : v['closeProbability'],r : v['closeProbability'] / 2})
                  });
                  this.opportunitiesScoreSubject.next(this.opportunitiesScore);
              }
          })
          .catch(reason => {
              isSuccess = false;
              console.log(reason);
          });


      if (isSuccess){
          return Promise.resolve(this.opportunitiesScore);
      }
      else {
          return Promise.resolve();
      }
  }

}
