import {ChangeDetectorRef, Component, OnInit} from '@angular/core';
import { ChartConfiguration, ChartData, ChartEvent, ChartType } from 'chart.js';
import {DashboardService} from "../../../../../services/Dashboard/admin/dashboard.service";
import {firstValueFrom} from "rxjs";

@Component({
  selector: 'app-bubble',
  templateUrl: './bubble.component.html',
  styleUrls: ['./bubble.component.scss']
})
export class BubbleComponent implements OnInit{

    dataset : any[] = [];
    public bubbleChartOptions: ChartConfiguration['options'];
    public bubbleChartType!: ChartType;
    public bubbleChartData!: ChartData<'bubble'>;
    minValue : number = 0;
    months: any[]  = [];
    selectedCity : any;
    isLoading : boolean = true;

    constructor(private dashboardService : DashboardService,private changeDetectorRef: ChangeDetectorRef) {
    }

    async ngOnInit() {
        this.months = [
            { name: 'January', code: '01' },
            { name: 'February', code: '02' },
            { name: 'March', code: '03' },
            { name: 'April', code: '04' },
            { name: 'May', code: '05' },
            { name: 'June', code: '06' },
            { name: 'July', code: '07' },
            { name: 'August', code: '08' },
            { name: 'September', code: '09' },
            { name: 'October', code: '10' },
            { name: 'November', code: '11' },
            { name: 'December', code: '12' },
        ];

        await this.dashboardService.getOpportunitiesScore(new Date().getMonth() + 1 >= 10 ? `${new Date().getMonth() + 1 >= 10}` : `0${new Date().getMonth() + 1}`)
            .then((value) => {
                if(value != null){
                    this.dataset = value;
                    this.isLoading = !this.isLoading;
                }

            });

    this.bubbleChartOptions = {
            maintainAspectRatio: false,
            scales: {
                x: {
                    title : {
                        display: true,
                        text: 'Estimated close date'
                    },
                    min: 0,
                    max: 30,
                    ticks: {},
                },
                y: {
                    title : {
                        display: true,
                        text: 'Probability'
                    },
                    min: 0,
                    max: 120,
                    ticks: {},
                },
            },


        };
    this.bubbleChartType = 'bubble';
    this.bubbleChartData= {
            labels: [],
            datasets: [
                {
                    data: this.dataset,
                    label: 'Opportunities',
                    backgroundColor: this.dataset.map(i => i.y > 80 ? '#91d0a3' : i.y > 60 ? '#c3edf5' : i.y > 40 ? '#ebe0ff':'#feddc7'),
                    hoverBackgroundColor: this.dataset.map(i => i.y > 80 ? '#7dc391' : i.y > 60 ? '#9de7f5' : i.y > 40 ? '#c1aeef':'#ffa05d'),
                    hoverBorderColor: this.dataset.map(i => i.y > 80 ? '#4bc0c0' : i.y > 60 ? '#4bc0c0' : i.y > 40 ? '#cbb2ff':'#ffa05d'),
                },
            ],
        };
    }


    loadOpportunitiesScoreForSelectedMonth() {
        this.dashboardService.getOpportunitiesScore(this.selectedCity.code)
            .then(value => {
               if(value != null){
                   this.dataset = value;
                   this.bubbleChartData= {
                       labels: [],
                       datasets: [
                           {
                               data: value,
                               label: 'Opportunities',
                               backgroundColor: this.dataset.map(i => i.y >= 80 ? '#91d0a3' : i.y >= 60 ? '#c3edf5' : i.y >= 40 ? '#ebe0ff':'#feddc7'),
                               hoverBackgroundColor: this.dataset.map(i => i.y >= 80 ? '#7dc391' : i.y >= 60 ? '#9de7f5' : i.y >= 40 ? '#c1aeef':'#ffa05d'),
                               hoverBorderColor: this.dataset.map(i => i.y >= 80 ? '#4bc0c0' : i.y >= 60 ? '#4bc0c0' : i.y >= 40 ? '#cbb2ff':'#ffa05d'),
                           },
                       ],
                   };
               }
            })
            .catch(reason => {});
    }

    public bubbleChartLegend = true;



    // events
    public chartClicked({
                            event,
                            active,
                        }: {
        event: ChartEvent;
        active: object[];
    }): void {
        console.log(event, active);
    }

    public chartHovered({
                            event,
                            active,
                        }: {
        event: ChartEvent;
        active: object[];
    }): void {
        console.log(event, active);
    }





}
