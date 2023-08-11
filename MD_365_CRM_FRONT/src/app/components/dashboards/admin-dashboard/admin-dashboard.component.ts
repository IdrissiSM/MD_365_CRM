import {Component, OnInit} from '@angular/core';
import {Account, AccountStateCode} from "../../../Models/Account/Account.model";
import {DashboardService} from "../../../services/Dashboard/admin/dashboard.service";
import {ProductService} from "../../../services/product.service";
import {BestSellingProducts} from "../../../Models/BestSellingProducts";
import {AccountService} from "../../../services/Account/account.service";

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.scss']
})
export class AdminDashboardComponent implements OnInit{
    accounts!: Account[];
    items: any;
    chartData: any;
    chartOptions: any;
    currentChoice !: string;
    choices: any;
    statuses!: any[];

    //Stats
    nbCustomers !:number;
    nbOpportunities!:number;
    revenue !: number;
    nbIncident !: number;

    AccountStateCode = AccountStateCode;
    bestSellingProducts !: BestSellingProducts[];
    totalQuantity !: number;

    isLoading : boolean = true;

    constructor(private accountService : AccountService,private dashboardService : DashboardService,private productService : ProductService) {
    }

    ngOnInit() {
        this.choices = [
            { name: 'Customers', code: 'Customers' },
            { name: 'Opportunities', code: 'Opportunities' },
        ];

        this.currentChoice = 'Customers';

        this.accountService.getTopCustomers().subscribe({
            next: value => {
                if (value.success){
                    this.accounts = value.result;
                }
            },
            error: err => {
                console.log(err);
            }
        });

        this.statuses = [
            { label: 'ACTIVE', value: 0 },
            { label: 'INACTIVE', value: 1 }
        ];

        this.dashboardService.getStats().subscribe({
            next: value => {
                this.nbCustomers = value.result[0].value;
                this.nbOpportunities = value.result[1].value;
                this.revenue = value.result[2].value;
                this.nbIncident = value.result[3].value;
            },
            error: err => {
                console.log(err);
            }
        });

        this.productService.getBestSellingProducts().subscribe({
            next: value => {
                this.bestSellingProducts = value.result;
                this.totalQuantity = this.bestSellingProducts.reduce((accumulator, currentValue) => accumulator + currentValue.quantity, 0);
                this.isLoading = !this.isLoading;
            },
            error: err => {
                console.log(err);
            }
        });
    }

    getStateCodeSeverity(label: any) {
        switch (label) {
            case 'ACTIVE':
                return 'qualified';
            case 'INACTIVE':
                return 'unqualified';
            default:
                return 'renewal';
        }
    }

    getProductPercent(quantity : number){
        return (quantity / this.totalQuantity) * 100;
    }

    getProductPercentSeverity(quantity : number) {
        let productPercent = this.getProductPercent(quantity);
       if (productPercent < 25) return 'red';
       else if(productPercent < 50) return 'orange';
       else if(productPercent < 75) return  'cyan';
       else return  'green';
    }

    repeat(count: number): number[] {
        return Array(count).fill(0);
    }

}
