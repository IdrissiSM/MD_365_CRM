import { Opportunity } from 'src/app/Models/Opportunity';
import { Product } from 'src/app/Models/Product';
import { Incident } from './Models/Incident/Incident';
import { APIResponse } from 'src/app/Models/APIResponse';
import { Component, OnInit } from '@angular/core';
import { PrimeNGConfig } from 'primeng/api';
import { OpportunityService } from './services/opportunity.service';
import { IncidentService } from './services/Incident/incident.service';
import { ProductService } from './services/product.service';
import { AppStateService } from './services/app-state.service';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
})
export class AppComponent implements OnInit {
    constructor(private primengConfig: PrimeNGConfig) {}

    ngOnInit() {
        this.primengConfig.ripple = true;
    }

    clearCache(): void {
        if ('caches' in window) {
            caches.keys().then((cacheNames) => {
                cacheNames.forEach((cacheName) => {
                    caches.delete(cacheName);
                });
            });
        }
    }
}
