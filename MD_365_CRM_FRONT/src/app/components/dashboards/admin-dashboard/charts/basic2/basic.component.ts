import {Component, OnInit} from '@angular/core';
import {IncidentService} from "../../../../../services/Incident/incident.service";
import {firstValueFrom} from "rxjs";

@Component({
  selector: 'app-basic2',
  templateUrl: './basic.component.html',
  styleUrls: ['./basic.component.scss']
})
export class BasicComponent2 implements OnInit{
    basicData: any;

    basicOptions: any;

    data : number[] = [];
    labels : string[] = [];

    constructor(private incidentService : IncidentService) {

    }


    async ngOnInit() {
        const documentStyle = getComputedStyle(document.documentElement);
        const textColor = documentStyle.getPropertyValue('--text-color');
        const textColorSecondary = documentStyle.getPropertyValue('--text-color-secondary');
        const surfaceBorder = documentStyle.getPropertyValue('--surface-border');
        this.incidentService.GetGroupedByContactIncidents();
        await firstValueFrom(this.incidentService.groupedIncidentSubject)
            .then(value => {
            value.forEach(v => {
                this.labels.push(v.name );
                this.data.push(v.count);
            })
            console.log(this.data);
        })
        this.basicData = {
            labels: this.labels,
            datasets: [
                {
                    label: 'Incidents',
                    data: this.data,
                    backgroundColor: ['rgba(255, 159, 64, 0.2)', 'rgba(75, 192, 192, 0.2)', 'rgba(54, 162, 235, 0.2)', 'rgba(153, 102, 255, 0.2)'],
                    borderColor: ['rgb(255, 159, 64)', 'rgb(75, 192, 192)', 'rgb(54, 162, 235)', 'rgb(153, 102, 255)'],
                    borderWidth: 1
                }
            ]
        };

        this.basicOptions = {
            plugins: {
                legend: {
                    labels: {
                        color: textColor
                    }
                }
            },
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: {
                        color: textColorSecondary
                    },
                    grid: {
                        color: surfaceBorder,
                        drawBorder: false
                    }
                },
                x: {
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
