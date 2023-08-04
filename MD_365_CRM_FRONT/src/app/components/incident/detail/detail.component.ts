import { Component, OnInit } from '@angular/core';
import {
    CaseOriginCode,
    CaseTypeCode,
    Incident,
    PriorityCode,
    RouteCase,
    ServicesStage,
    StateCode,
} from '../../../Models/Incident/Incident';
import { IncidentService } from '../../../services/Incident/incident.service';
import { ActivatedRoute } from '@angular/router';
import { firstValueFrom } from 'rxjs';

@Component({
    selector: 'app-detail',
    templateUrl: './detail.component.html',
    styleUrls: ['./detail.component.scss'],
})
export class DetailComponent implements OnInit {
    incident!: Incident;
    incidentId!: string;
    loading: boolean = false;

    StateCode = StateCode;
    PriorityCode = PriorityCode;

    RouteCase = RouteCase;
    ServicesStage = ServicesStage;
    CaseTypeCode = CaseTypeCode;
    CaseOriginCode = CaseOriginCode;

    constructor(
        private incidentService: IncidentService,
        private route: ActivatedRoute
    ) {}

    async ngOnInit() {
        this.route.params.subscribe({
            next: (params) => {
                this.incidentId = params['id'];
            },
            error: (err) => {
                console.log(err);
            },
        });

        this.loading = !this.loading;
        this.incidentService.getIncidentById(this.incidentId).subscribe({
            next: (value) => {
                this.incident = value.result;
                this.loading = !this.loading;
            },
            error: (err) => {
                console.log(err);
                this.loading = !this.loading;
            },
        });
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
    getRouteCaseSeverity(label: any) {
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
    getServicesStageSeverity(servicestage: string) {
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
}
