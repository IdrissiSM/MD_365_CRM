import { AppStateService } from 'src/app/services/app-state.service';
import { OpportunityService } from './../../services/opportunity.service';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { ConfirmationService, MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { APIResponse } from 'src/app/Models/APIResponse';
import { Opportunity } from 'src/app/Models/Opportunity';
import { Table } from 'primeng/table';
import { NavigationExtras, Router } from '@angular/router';

interface expandedRows {
    [key: string]: boolean;
}

@Component({
    selector: 'app-opportunities',
    templateUrl: './opportunities.component.html',
    styleUrls: ['./opportunities.component.scss'],
})
export class OpportunitiesComponent {
    opportunities: Opportunity[] = [];

    expandedRows: expandedRows = {};

    activityValues: number[] = [0, 100];

    isExpanded: boolean = false;

    idFrozen: boolean = false;

    loading: boolean = true;

    contactid?: string;

    repeat(count: number): number[] {
        return Array(count).fill(0);
    }

    @ViewChild('filter') filter!: ElementRef;

    constructor(
        private opportunityService: OpportunityService,
        private confirmationService: ConfirmationService,
        private messageService: MessageService,
        private dialogService: DialogService,
        private appStateService: AppStateService,
        private router: Router
    ) {}

    ngOnInit() {
        this.contactid = this.appStateService.authState.contactid;
        this.getOpportunities();
    }

    // GET
    getOpportunities() {
        this.opportunityService
            .getAllOpportunities()
            .subscribe((response: APIResponse) => {
                // console.log(response);
                if (response.success) {
                    this.opportunities = response.result;
                    this.loading = false;
                    this.initRatingChart();
                    this.initStateChart();
                    this.groupRevenueByMonth();
                    this.initRevenueChart();
                } else {
                    console.log(response.errorMessages);
                }
            });
    }

    opportunityDetails(opportunityId: number) {
        const navigationExtras: NavigationExtras = {
            queryParams: { id: opportunityId },
        };
        this.router.navigate(['/opportunity'], navigationExtras);
    }

    expandAll() {
        if (!this.isExpanded) {
            this.opportunities.forEach((opportunity) =>
                opportunity && opportunity.opportunityId
                    ? (this.expandedRows[opportunity.opportunityId] = true)
                    : ''
            );
        } else {
            this.expandedRows = {};
        }
        this.isExpanded = !this.isExpanded;
    }

    formatCurrency(value: number) {
        return value.toLocaleString('en-US', {
            style: 'currency',
            currency: 'USD',
        });
    }
    onGlobalFilter(table: Table, event: Event) {
        table.filterGlobal(
            (event.target as HTMLInputElement).value,
            'contains'
        );
    }

    clear(table: Table) {
        table.clear();
        this.filter.nativeElement.value = '';
    }

    getStateLabel(value: number) {
        return this.states.find((state) => state.value == value)?.label;
    }
    getStateStyle(value: number) {
        return this.states.find((state) => state.value == value)?.style;
    }
    getStatusLabel(value: number) {
        return this.statuses.find((status) => status.value == value)?.label;
    }
    getStatusStyle(value: number) {
        return this.statuses.find((status) => status.value == value)?.style;
    }
    getRatingLabel(value: number) {
        return this.ratings.find((status) => status.value == value)?.label;
    }
    getRatingStyle(value: number) {
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

    // Charts --------------------------------------------------------------

    documentStyle = getComputedStyle(document.documentElement);
    textColor = this.documentStyle.getPropertyValue('--text-color');
    textColorSecondary = this.documentStyle.getPropertyValue(
        '--text-color-secondary'
    );
    surfaceBorder = this.documentStyle.getPropertyValue('--surface-border');

    getMonthName(month: number): string {
        const monthNames: string[] = [
            'January',
            'February',
            'March',
            'April',
            'May',
            'June',
            'July',
            'August',
            'September',
            'October',
            'November',
            'December',
        ];
        return monthNames[month];
    }

    months!: any;
    estimatedRevenue!: any;
    actualRevenue!: any;

    groupRevenueByMonth() {
        const revenueByMonthMap: Map<
            number,
            { estimatedRevenue: number; actualRevenue: number }
        > = new Map();
        const currentYear = new Date().getFullYear();

        this.opportunities.forEach((opportunity) => {
            const estimatedCloseDate = new Date(opportunity.estimatedCloseDate);
            const month = estimatedCloseDate.getMonth();
            const estimatedRevenue = opportunity.estimatedValue;
            const actualRevenue = opportunity.actualValue;

            if (estimatedCloseDate.getFullYear() === currentYear) {
                if (revenueByMonthMap.has(month)) {
                    const existingData = revenueByMonthMap.get(month)!;
                    existingData.estimatedRevenue += estimatedRevenue;
                    existingData.actualRevenue += actualRevenue;
                } else {
                    revenueByMonthMap.set(month, {
                        estimatedRevenue,
                        actualRevenue,
                    });
                }
            }
        });
        const sortedRevenueByMonthMap = new Map(
            [...revenueByMonthMap.entries()].sort((a, b) => a[0] - b[0])
        );
        const months: string[] = [];
        const estimatedRevenue: number[] = [];
        const actualRevenue: number[] = [];

        sortedRevenueByMonthMap.forEach((revenueData, month) => {
            months.push(this.getMonthName(month));
            estimatedRevenue.push(revenueData.estimatedRevenue);
            actualRevenue.push(revenueData.actualRevenue);
        });
        this.months = months;
        this.estimatedRevenue = estimatedRevenue;
        this.actualRevenue = actualRevenue;
        // console.log(this.months, this.estimatedRevenue, this.actualRevenue);
    }

    revenueChartData!: any;
    revenueChartOptions!: any;
    initRevenueChart() {
        this.revenueChartData = {
            labels: this.months,
            datasets: [
                {
                    label: 'Estimated Revenue',
                    backgroundColor:
                        this.documentStyle.getPropertyValue('--primary-300'),
                    borderColor:
                        this.documentStyle.getPropertyValue('--primary-300'),
                    data: this.estimatedRevenue,
                },
                {
                    label: 'Actual Revenue',
                    backgroundColor:
                        this.documentStyle.getPropertyValue('--primary-500'),
                    borderColor:
                        this.documentStyle.getPropertyValue('--primary-500'),
                    data: this.actualRevenue,
                },
            ],
        };
        this.revenueChartOptions = {
            plugins: {
                legend: {
                    labels: {
                        fontColor: this.textColor,
                    },
                },
            },
            scales: {
                x: {
                    ticks: {
                        color: this.textColorSecondary,
                        font: {
                            weight: 500,
                        },
                    },
                    grid: {
                        display: false,
                        drawBorder: false,
                    },
                },
                y: {
                    ticks: {
                        color: this.textColorSecondary,
                    },
                    grid: {
                        color: this.surfaceBorder,
                        drawBorder: false,
                    },
                },
            },
        };
    }

    ratingChartData: any;
    ratingChartOptions: any;
    nbrOpenRatingOpp: number = 0;
    nbrWonRatingOpp: number = 0;
    nbrLostRatingOpp: number = 0;
    initRatingChart() {
        this.opportunities.forEach((opp) => {
            switch (opp.opportunityRatingCode) {
                case 1:
                    this.nbrOpenRatingOpp++;
                    break;
                case 2:
                    this.nbrWonRatingOpp++;
                    break;
                case 3:
                    this.nbrLostRatingOpp++;
                    break;
                default:
                    break;
            }
        });
        this.ratingChartData = {
            labels: ['Hot', 'Warm', 'Cold'],
            datasets: [
                {
                    data: [
                        this.nbrOpenRatingOpp,
                        this.nbrWonRatingOpp,
                        this.nbrLostRatingOpp,
                    ],
                    backgroundColor: [
                        this.documentStyle.getPropertyValue('--red-500'),
                        this.documentStyle.getPropertyValue('--orange-500'),
                        this.documentStyle.getPropertyValue('--cyan-500'),
                    ],
                    hoverBackgroundColor: [
                        this.documentStyle.getPropertyValue('--red-400'),
                        this.documentStyle.getPropertyValue('--orange-400'),
                        this.documentStyle.getPropertyValue('--cyan-400'),
                    ],
                },
            ],
        };
        this.ratingChartOptions = {
            plugins: {
                legend: {
                    labels: {
                        usePointStyle: true,
                        color: this.textColor,
                    },
                },
            },
        };
    }

    stateChartData: any;
    stateChartOptions: any;
    nbrOpenStateOpp: number = 0;
    nbrWonStateOpp: number = 0;
    nbrLostStateOpp: number = 0;
    initStateChart() {
        this.opportunities.forEach((opp) => {
            switch (opp.stateCode) {
                case 0:
                    this.nbrOpenStateOpp++;
                    break;
                case 1:
                    this.nbrWonStateOpp++;
                    break;
                case 2:
                    this.nbrLostStateOpp++;
                    break;
                default:
                    break;
            }
        });
        this.stateChartData = {
            labels: ['Open', 'Won', 'Lost'],
            datasets: [
                {
                    data: [
                        this.nbrOpenStateOpp,
                        this.nbrWonStateOpp,
                        this.nbrLostStateOpp,
                    ],
                    backgroundColor: [
                        this.documentStyle.getPropertyValue('--cyan-500'),
                        this.documentStyle.getPropertyValue('--green-500'),
                        this.documentStyle.getPropertyValue('--red-500'),
                    ],
                    hoverBackgroundColor: [
                        this.documentStyle.getPropertyValue('--cyan-400'),
                        this.documentStyle.getPropertyValue('--green-400'),
                        this.documentStyle.getPropertyValue('--red-400'),
                    ],
                },
            ],
        };
        this.stateChartOptions = {
            plugins: {
                legend: {
                    labels: {
                        usePointStyle: true,
                        color: this.textColor,
                    },
                },
            },
        };
    }
}
