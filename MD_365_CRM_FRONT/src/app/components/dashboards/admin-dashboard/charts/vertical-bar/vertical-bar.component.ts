import {Component, Input, OnInit} from '@angular/core';
import {firstValueFrom} from "rxjs";
import {DashboardService} from "../../../../../services/Dashboard/admin/dashboard.service";

@Component({
  selector: 'app-vertical-bar',
  templateUrl: './vertical-bar.component.html',
  styleUrls: ['./vertical-bar.component.scss']
})
export class VerticalBarComponent implements OnInit{
    data: any;
     dataset1: any [] = [];
     dataset2: any[] = [];
     labels : any[] = [];

    options: any;

    constructor(private dashboardService : DashboardService) {
    }

    async ngOnInit() {
        const documentStyle = getComputedStyle(document.documentElement);
        const textColor = documentStyle.getPropertyValue('--text-color');
        const textColorSecondary = documentStyle.getPropertyValue('--text-color-secondary');
        const surfaceBorder = documentStyle.getPropertyValue('--surface-border');

        await firstValueFrom(this.dashboardService.getOpportunitiesValues())
            .then(value => {
                value.result.forEach((v : any) => {
                    this.dataset1.push(v['estimatedValue']);
                    this.dataset2.push(v['actualValue']);
                    this.labels.push(v['month']);
                });
                console.log(this.dataset1);
            })
            .catch(reason => {
                console.log(reason);
            });


        this.data = {
            labels: this.labels,
            datasets: [
                {
                    label: 'Estimated Revenue',
                    backgroundColor: documentStyle.getPropertyValue('--primary-300'),
                    borderColor: documentStyle.getPropertyValue('--primary-300'),
                    data: this.dataset1
                },
                {
                    label: 'Actual Revenue',
                    backgroundColor: documentStyle.getPropertyValue('--primary-500'),
                    borderColor: documentStyle.getPropertyValue('--primary-500'),
                    data: this.dataset2
                }
            ]
        };

        this.options = {
            maintainAspectRatio: false,
            aspectRatio: 0.8,
            plugins: {
                legend: {
                    labels: {
                        color: textColor
                    }
                }
            },
            scales: {
                x: {
                    ticks: {
                        color: textColorSecondary,
                        font: {
                            weight: 500
                        }
                    },
                    grid: {
                        color: surfaceBorder,
                        drawBorder: false
                    }
                },
                y: {
                    ticks: {
                        color: textColorSecondary
                    },
                    grid: {
                        color: surfaceBorder,
                        drawBorder: false
                    }
                }

            }
        };
    }
}
