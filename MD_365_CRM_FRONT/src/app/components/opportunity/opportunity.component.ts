import { Opportunity } from './../../Models/Opportunity';
import { Component } from '@angular/core';
import { OpportunityService } from 'src/app/services/opportunity.service';
import { ActivatedRoute } from '@angular/router';
import { APIResponse } from 'src/app/Models/APIResponse';

@Component({
    selector: 'app-opportunity',
    templateUrl: './opportunity.component.html',
    styleUrls: ['./opportunity.component.scss'],
})
export class OpportunityComponent {
    loading: boolean = true;
    opportunityId!: string;
    opportunity?: Opportunity;

    constructor(
        private opportunityService: OpportunityService,
        private route: ActivatedRoute
    ) {}

    ngOnInit() {
        this.route.queryParams.subscribe((params) => {
            this.opportunityId = params['id'];
        });
        this.getOpportunity();
    }

    getOpportunity() {
        this.opportunityService
            .getOpportunityById(this.opportunityId)
            .subscribe((response: APIResponse) => {
                console.log(response);
                if (response.success) {
                    this.opportunity = response.result;
                    this.loading = false;
                } else {
                    console.log(response.errorMessages);
                }
            });
    }

    getStateLabel(value: any) {
        return this.states.find((state) => state.value == value)?.label;
    }
    getStateStyle(value: any) {
        return this.states.find((state) => state.value == value)?.style;
    }
    getStatusLabel(value: any) {
        return this.statuses.find((status) => status.value == value)?.label;
    }
    getStatusStyle(value: any) {
        return this.statuses.find((status) => status.value == value)?.style;
    }
    getRatingLabel(value: any) {
        return this.ratings.find((status) => status.value == value)?.label;
    }
    getRatingStyle(value: any) {
        return this.ratings.find((status) => status.value == value)?.style;
    }
    states = [
        { value: 0, label: 'Open', style: 'renewal' },
        { value: 1, label: 'Won', style: 'qualified' },
        { value: 2, label: 'Lost', style: 'unqualified' },
    ];
    ratings = [
        { value: 1, label: 'Hot', style: 'unqualified' },
        { value: 2, label: 'Warm', style: 'proposal' },
        { value: 3, label: 'Cold', style: 'new' },
    ];
    statuses = [
        { value: 1, label: 'In-Progress', state: 0, style: 'renewal' },
        { value: 2, label: 'On-Hold', state: 0, style: 'new' },
        { value: 3, label: 'Won', state: 1, style: 'qualified' },
        { value: 4, label: 'Canceled', state: 2, style: 'proposal' },
        { value: 5, label: 'Out-Sold', state: 2, style: 'unqualified' },
    ];
}
