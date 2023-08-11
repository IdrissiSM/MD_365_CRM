import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Table } from 'primeng/table';
import {
    CaseOriginCode,
    CaseTypeCode,
    Incident,
    PriorityCode,
    ServicesStage,
    StateCode,
} from '../../Models/Incident/Incident';
import { IncidentService } from '../../services/Incident/incident.service';
import { Router } from '@angular/router';

@Component({
    selector: 'app-incident',
    templateUrl: './incident.component.html',
    styleUrls: ['./incident.component.scss'],
})
export class IncidentComponent implements OnInit {
    incidents!: Incident[];
    loading: boolean = false;
    @ViewChild('filter') filter!: ElementRef;
    statuses!: any[];
    ServiceStages!: any[];
    CaseOriginCodes!: any[];
    priorities!: any[];

    caseTypesCodes!: any[];
    StateCode = StateCode;
    ServicesStage = ServicesStage;
    CaseOriginCode = CaseOriginCode;
    PriorityCode = PriorityCode;
    CaseTypeCode = CaseTypeCode;

    constructor(
        private incidentService: IncidentService,
        private router: Router
    ) {}

    ngOnInit() {
        this.statuses = [
            { label: 'ACTIVE', value: 0 },
            { label: 'RESOLVED', value: 1 },
            { label: 'CANCELLED', value: 2 },
        ];
        this.ServiceStages = [
            { label: 'IDENTIFY', value: 0 },
            { label: 'RESEARCH', value: 1 },
            { label: 'RESOLVE', value: 2 },
        ];
        this.CaseOriginCodes = [
            { label: 'PHONE', value: 1 },
            { label: 'EMAIL', value: 2 },
            { label: 'WEB', value: 3 },
            { label: 'FACEBOOK', value: 2483 },
            { label: 'TWITTER', value: 3986 },
        ];
        this.caseTypesCodes = [
            { label: 'QUESTION', value: 1 },
            { label: 'PROBLEM', value: 2 },
            { label: 'REQUEST', value: 3 },
        ];
        this.priorities = [
            { label: 'HIGH', value: 1 },
            { label: 'NORMAL', value: 2 },
            { label: 'LOW', value: 3 },
        ];
        //It is a temp solution until brahim adds his task
        this.loading = !this.loading;
        this.incidentService.incidentsSubject.subscribe({
            next: (value) => {
                this.incidents = value;
            },
            error: (err) => {
                console.log(err);
            },
        });
        this.incidentService
            .getIncidents('51c9e452-5930-ee11-bdf4-6045bd905df9')
            .then((value) => {
                this.loading = !this.loading;
            })
            .catch((reason) => {
                this.loading = !this.loading;
            });
    }

    clear(table: Table) {
        table.clear();
        this.filter.nativeElement.value = '';
    }

    onGlobalFilter(table: Table, event: Event) {
        table.filterGlobal(
            (event.target as HTMLInputElement).value,
            'contains'
        );
    }

    getSeverity(stateCode: any) {
        switch (stateCode) {
            case 'ACTIVE':
                return 'success';
            case 'CANCELLED':
                return 'danger';
            case 'RESOLVED':
                return 'info';
            default:
                return 'danger';
        }
    }

    getServicesStageSeverity(servicestage: any) {
        switch (servicestage) {
            case 'IDENTIFY':
                return 'proposal';
            case 'RESEARCH':
                return 'new';
            case 'RESOLVE':
                return 'qualified';
            default:
                return 'renewal';
        }
    }

    getCaseOriginIcon(label: string) {
        switch (label) {
            case 'PHONE':
                return 'pi-phone';
            case 'EMAIL':
                return 'pi-inbox';
            case 'WEB':
                return 'pi-link';
            case 'FACEBOOK':
                return 'pi-facebook';
            case 'TWITTER':
                return 'pi-twitter';
            default:
                return 'danger';
        }
    }

    getPrioritySeverity(label: any) {
        switch (label) {
            case 'HIGH':
                return 'unqualified';
            case 'NORMAL':
                return 'new';
            case 'LOW':
                return 'qualified';
            default:
                return 'proposal';
        }
    }

    getStateCodeSeverity(label: any) {
        switch (label) {
            case 'ACTIVE':
                return 'new';
            case 'RESOLVED':
                return 'qualified';
            case 'CANCELLED':
                return 'unqualified';
            default:
                return 'renewal';
        }
    }

    getCaseOriginIconSeverity(label: any) {
        switch (label) {
            case 'PHONE':
                return 'renewal';
            case 'EMAIL':
                return 'qualified';
            case 'WEB':
                return 'unqualified';
            case 'FACEBOOK':
                return 'new';
            case 'TWITTER':
                return 'negotiation';
            default:
                return 'renewal';
        }
    }

    getCaseCodeSeverity(label: any) {
        switch (label) {
            case 'QUESTION':
                return 'qualified';
            case 'PROBLEM':
                return 'unqualified';
            case 'REQUEST':
                return 'new';
            default:
                return 'proposal';
        }
    }

    GoToIncidentDetail(incidentId: string) {
        console.log(incidentId);
        this.router
            .navigate(['/incidents', 'detail', incidentId])
            .catch((reason) => {});
    }
    repeat(count: number): number[] {
        return Array(count).fill(0);
    }
}
