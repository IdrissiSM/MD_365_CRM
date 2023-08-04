import {Component, OnInit} from '@angular/core';
import {IncidentService} from "../../../../services/Incident/incident.service";
import {firstValueFrom} from "rxjs";

@Component({
  selector: 'app-polar',
  templateUrl: './polar.component.html',
  styleUrls: ['./polar.component.scss']
})
export class PolarComponent implements OnInit{
    data: any;

    options: any;

    temp : number[] = [];

    constructor(private incidentService:IncidentService) {
    }
    async ngOnInit() {
        const documentStyle = getComputedStyle(document.documentElement);
        const textColor = documentStyle.getPropertyValue('--text-color');
        const surfaceBorder = documentStyle.getPropertyValue('--surface-border');
        await firstValueFrom(this.incidentService.caseTypeDataSubject)
            .then(value => {
                value.forEach(v => this.temp.push(v));
                console.log(this.temp);
            })
            .catch(reason => {
                console.log(reason);
            })
        this.data = {
            datasets: [
                {
                    data: this.temp,
                    backgroundColor: [
                        documentStyle.getPropertyValue('--green-500'),
                        documentStyle.getPropertyValue('--red-500'),
                        documentStyle.getPropertyValue('--yellow-500'),
                    ],
                    label: 'My dataset'
                }
            ],
            labels: ['QUESTION', 'PROBLEM', 'REQUEST']
        };

        this.options = {
            plugins: {
                legend: {
                    labels: {
                        color: textColor
                    }
                }
            },
            scales: {
                r: {
                    grid: {
                        color: surfaceBorder
                    }
                }
            }
        };
    }
}
