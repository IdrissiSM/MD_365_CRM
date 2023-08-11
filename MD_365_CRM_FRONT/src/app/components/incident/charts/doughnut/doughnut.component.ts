import {Component, Input, OnInit} from '@angular/core';
import {IncidentService} from "../../../../services/Incident/incident.service";
import {firstValueFrom} from "rxjs";

@Component({
  selector: 'app-doughnut',
  templateUrl: './doughnut.component.html',
  styleUrls: ['./doughnut.component.scss']
})
export class DoughnutComponent implements OnInit{
    data: any;
    options: any;
    temp: number[] = [];
    constructor(private incidentService: IncidentService) {
    }
    async ngOnInit() {
        const documentStyle = getComputedStyle(document.documentElement);
        const textColor = documentStyle.getPropertyValue('--text-color');


        await firstValueFrom(this.incidentService.stateDateSubject).then(
            value => { this.temp = value;}

        );


        this.data = {
            labels: ['ACTIVE', 'RESOLVED', 'CANCELLED'],
            datasets: [
                {
                    data: this.temp,
                    backgroundColor: [documentStyle.getPropertyValue('--blue-500'),  documentStyle.getPropertyValue('--green-500'),documentStyle.getPropertyValue('--yellow-500')],
                    hoverBackgroundColor: [documentStyle.getPropertyValue('--blue-400'), documentStyle.getPropertyValue('--green-400'), documentStyle.getPropertyValue('--yellow-400')]
                }
            ]
        };


        this.options = {
            cutout: '60%',
            plugins: {
                legend: {
                    labels: {
                        color: textColor
                    }
                }
            }
        };
    }
}
