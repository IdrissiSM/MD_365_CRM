import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { APIResponse } from '../../Models/APIResponse';
import { firstValueFrom, Observable, Subject } from 'rxjs';
import { Incident } from '../../Models/Incident/Incident';
import { environment } from 'src/environments/environment';

@Injectable({
    providedIn: 'root',
})
export class IncidentService {
    private incidents!: Incident[];
    private Q1: Date[] = [
        new Date(new Date().getFullYear(), 0, 1),
        new Date(new Date().getFullYear(), 2, 31),
    ];
    private Q2: Date[] = [
        new Date(new Date().getFullYear(), 3, 1),
        new Date(new Date().getFullYear(), 5, 30),
    ];
    private Q3: Date[] = [
        new Date(new Date().getFullYear(), 6, 1),
        new Date(new Date().getFullYear(), 8, 30),
    ];
    private Q4: Date[] = [
        new Date(new Date().getFullYear(), 9, 1),
        new Date(new Date().getFullYear(), 11, 31),
    ];
    private basicData: number[] = [0, 0, 0, 0];
    private stateDate: number[] = [0, 0, 0];
    private caseTypeData: number[] = [0, 0, 0];
    private groupedIncident!: any[];
    public incidentsSubject = new Subject<Incident[]>();
    public basicDataSubject = new Subject<number[]>();
    public stateDateSubject = new Subject<number[]>();
    public caseTypeDataSubject = new Subject<number[]>();
    public groupedIncidentSubject = new Subject<any[]>();
    private errorMeassge!: string;

    private baseUrl: string = 'https://localhost:7118/api/Incident/';
    apiUrl = environment.apiBaseUrl;
    token: string = '';
    constructor(private http: HttpClient) {}
    async getIncidents(contactId: string) {
        let isSuccess: boolean = false;
        await firstValueFrom(
            this.http.get<APIResponse>(`${this.baseUrl}${contactId}`)
        )
            .then((value) => {
                this.incidents = value.result;
                isSuccess = !isSuccess;
                this.incidentsSubject.next(this.incidents);
            })
            .catch((reason) => {
                this.errorMeassge = reason.error.errorMessages[0];
            });

        if (isSuccess) {
            this.CalculateBasicData();
            this.CalculateStateData();
            this.CalculateCaseTypeData();
            return Promise.resolve();
        } else {
            return Promise.reject(this.errorMeassge);
        }
    }

    async GetGroupedByContactIncidents() {
        let isSuccess: boolean = false;
        await firstValueFrom(
            this.http.get<APIResponse>(
                `${this.baseUrl}GetGroupedByContactIncidents`
            )
        )
            .then((value) => {
                this.groupedIncident = value.result;
                isSuccess = !isSuccess;
                this.groupedIncidentSubject.next(this.groupedIncident);
            })
            .catch((reason) => {
                this.errorMeassge = reason.errorMeassge.errorMeassge[0];
            });
        if (isSuccess) {
            return Promise.resolve();
        } else {
            return Promise.reject(this.errorMeassge);
        }
    }

    getIncidentById(incidentId: string) {
        return this.http.get<APIResponse>(
            `${this.baseUrl}GetIncidentById/${incidentId}`
        );
    }
    CalculateBasicData() {
        this.basicData = [0, 0, 0];
        this.incidents.forEach((i) => {
            if (i.createdon != null) {
                const temp = new Date(i.createdon);
                if (temp > this.Q1[0] && temp < this.Q1[1]) {
                    this.basicData[0]++;
                } else if (temp > this.Q2[0] && temp < this.Q2[1]) {
                    this.basicData[1]++;
                } else if (temp > this.Q3[0] && temp < this.Q3[1]) {
                    this.basicData[2]++;
                } else if (temp > this.Q4[0] && temp < this.Q4[1]) {
                    this.basicData[3]++;
                }
            }
        });
        this.basicDataSubject.next(this.basicData);
    }

    CalculateStateData() {
        this.stateDate = [0, 0, 0];
        this.incidents.forEach((i) => {
            switch (i.statecode) {
                case 0:
                    this.stateDate[0]++;
                    break;
                case 1:
                    this.stateDate[1]++;
                    break;
                case 2:
                    this.stateDate[2]++;
                    break;
            }
        });
        this.stateDateSubject.next(this.stateDate);
    }

    CalculateCaseTypeData() {
        this.caseTypeData = [0, 0, 0];
        this.incidents.forEach((i) => {
            switch (i.casetypecode) {
                case 1:
                    this.caseTypeData[0]++;
                    break;
                case 2:
                    this.caseTypeData[1]++;
                    break;
                case 3:
                    this.caseTypeData[2]++;
                    break;
            }
        });
        this.caseTypeDataSubject.next(this.caseTypeData);
    }

    getIncidentsCache(contactId: string) {
        const httpOptions = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
                Authorization: 'Bearer ' + this.token,
            }),
        };
        return this.http.get<APIResponse>(
            `${this.apiUrl}/Incident/${contactId}`,
            httpOptions
        );
    }

    getIncidentByIdCache(incidentId: string) {
        const httpOptions = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
                Authorization: 'Bearer ' + this.token,
            }),
        };
        return this.http.get<APIResponse>(
            `${this.apiUrl}/Incident/GetIncidentById/${incidentId}`,
            httpOptions
        );
    }
}
