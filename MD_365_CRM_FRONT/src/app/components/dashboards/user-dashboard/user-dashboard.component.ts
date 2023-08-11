import { Incident } from './../../../Models/Incident/Incident';
import { ProductOpportunity } from './../../../Models/ProductOpportunity';
import { Component } from '@angular/core';
import { APIResponse } from 'src/app/Models/APIResponse';
import { Opportunity } from 'src/app/Models/Opportunity';
import { AppStateService } from 'src/app/services/app-state.service';
import { OpportunityService } from 'src/app/services/opportunity.service';
import { IncidentService } from 'src/app/services/Incident/incident.service';
import { NavigationExtras, Router } from '@angular/router';

@Component({
    selector: 'app-user-dashboard',
    templateUrl: './user-dashboard.component.html',
    styleUrls: ['./user-dashboard.component.scss'],
})
export class UserDashboardComponent {
    products: any;
    items: any;
    chartData: any;
    chartOptions: any;
    currentChoice!: string;
    opportunities: Opportunity[] = [];
    lastOpportunities: Opportunity[] = [];
    opprtunitiesProducts: ProductOpportunity[] = [];
    mostBoughtProducts: ProductOpportunity[] = [];
    totalProductsQuantity: number = 0;
    incidents: Incident[] = [];
    contactid?: string;
    loading: boolean = true;
    totalEstimatedRevenue: number = 0.0;
    totalActualRevenue: number = 0.0;

    constructor(
        private opportunityService: OpportunityService,
        private appStateService: AppStateService,
        private router: Router,
        private incidentService: IncidentService
    ) {}

    ngOnInit() {
        this.contactid = this.appStateService.authState.contactid;
        this.getOpportunities();
        this.getIncidents();
    }

    // GET
    getOpportunities() {
        this.opportunityService
            .getAllOpportunities()
            .subscribe((response: APIResponse) => {
                if (response.success) {
                    this.opportunities = response.result;
                    this.loading = false;
                    this.getOpportunitiesStatistics();
                    this.getOpprtunitiesProducts();
                    this.getTotalEstimatedREvenue();
                    this.getLastOpportunities();
                    this.groupRevenueByMonth();
                    this.initRevenueChart();
                } else {
                    console.log(response.errorMessages);
                }
            });
    }

    openRatingOpp: any = 0;
    wonRatingOpp: any = 0;
    lostRatingOpp: any = 0;
    getOpportunitiesStatistics() {
        this.opportunities.forEach((opp) => {
            switch (opp.opportunityRatingCode) {
                case 1:
                    this.openRatingOpp++;
                    break;
                case 2:
                    this.wonRatingOpp++;
                    break;
                case 3:
                    this.lostRatingOpp++;
                    break;
                default:
                    break;
            }
        });
        this.openRatingOpp = (
            (this.openRatingOpp / this.opportunities.length) *
            100
        ).toFixed(2);
        this.wonRatingOpp = (
            (this.wonRatingOpp / this.opportunities.length) *
            100
        ).toFixed(2);
        this.lostRatingOpp = (
            (this.lostRatingOpp / this.opportunities.length) *
            100
        ).toFixed(2);
    }

    getOpprtunitiesProducts() {
        this.opportunities.forEach((opportunity) => {
            opportunity.product_opportunities.forEach((product_opportunity) => {
                const product = this.opprtunitiesProducts.find(
                    (op) =>
                        op._productId_value ==
                        product_opportunity._productId_value
                );
                if (product) {
                    product.quantity += product_opportunity.quantity;
                } else {
                    this.opprtunitiesProducts.push(product_opportunity);
                }
                this.totalProductsQuantity += product_opportunity.quantity;
            });
        });
        this.mostBoughtProducts = this.opprtunitiesProducts
            .slice()
            .sort((a, b) => b.quantity - a.quantity)
            .slice(0, 7);
    }

    getLastOpportunities() {
        this.lastOpportunities = this.opportunities
            .slice()
            .sort((a, b) => {
                const dateA = new Date(a.createdOn);
                const dateB = new Date(b.createdOn);
                return dateB.getTime() - dateA.getTime();
            })
            .slice(0, 4);
    }

    getTotalEstimatedREvenue() {
        this.opportunities.forEach((opportunity) => {
            this.totalEstimatedRevenue += opportunity.estimatedValue;
            this.totalActualRevenue += opportunity.actualValue;
        });
    }

    resolvedIncidents: number = 0;
    getIncidents() {
        this.incidentService.incidentsSubject.subscribe({
            next: (value) => {
                console.log(value);
                this.incidents = value;
                value.forEach((i) => {
                    if (i.statecode == 1) this.resolvedIncidents++;
                });
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

    opportunityDetails(opportunityId: string) {
        const navigationExtras: NavigationExtras = {
            queryParams: { id: opportunityId },
        };
        this.router.navigate(['/opportunity'], navigationExtras);
    }

    productDetails(id: string) {
        const navigationExtras: NavigationExtras = {
            queryParams: { id: id },
        };
        this.router.navigate(['/product-details'], navigationExtras);
    }

    onChoiceChange(event: any) {
        this.currentChoice = event.value['name'];
        this.items = {};
    }

    getStateIcon(value: number) {
        const icon = this.states.find((s) => s.value == value)?.icon;
        console.log(icon);
        return icon;
    }

    getStateColor(value: number) {
        const color = this.states.find((s) => s.value == value)?.color;
        console.log(color);
        return color;
    }

    states = [
        { value: 0, label: 'Open', icon: 'pi pi-hourglass', color: '#A855F7' },
        {
            value: 1,
            label: 'Won',
            icon: 'pi pi-check-circle',
            color: '#22C55E',
        },
        {
            value: 2,
            label: 'Lost',
            icon: 'pi pi-times-circle',
            color: '#FF3D32',
        },
    ];

    getDtate(date: string) {
        return new Date(date);
    }

    getProductPercentage(quantity: number) {
        return `${(quantity / this.totalProductsQuantity) * 100}%`;
    }

    getProductPercentageColor(quantity: number, type: string) {
        const percentage = (quantity / this.totalProductsQuantity) * 100;
        if (percentage > 0 && percentage < 25) return `${type}-red-500`;
        if (percentage > 25 && percentage < 50) return `${type}-orange-500`;
        if (percentage > 50 && percentage < 75) return `${type}-cyan-500`;
        if (percentage > 75 && percentage < 100) return `${type}-green-500`;
        else return '';
    }
    repeat(count: number): number[] {
        return Array(count).fill(0);
    }

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

    skeltonOpportunities: any = [{}, {}, {}, {}];
}
