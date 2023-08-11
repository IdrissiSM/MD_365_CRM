import { ProductsComponent } from './components/products/products.component';
import { OpportunitiesComponent } from './components/opportunities/opportunities.component';
import { NgModule, Component } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AppLayoutComponent } from './layout/app.layout.component';
import { OpportunityComponent } from './components/opportunity/opportunity.component';
import { ProductDetailsComponent } from './components/product-details/product-details.component';
import { DetailComponent } from './components/incident/detail/detail.component';
import { IncidentComponent } from './components/incident/incident.component';
import {AdminDashboardComponent} from "./components/dashboards/admin-dashboard/admin-dashboard.component";

const routes: Routes = [
    {
        path: '',
        component: AppLayoutComponent,
        children: [
            { path: 'opportunities', component: OpportunitiesComponent },
            { path: 'opportunity', component: OpportunityComponent },
            { path: 'products', component: ProductsComponent },
            { path: 'product-details', component: ProductDetailsComponent },
            { path: 'incidents', component: IncidentComponent },
            { path: 'incidents/detail/:id', component: DetailComponent },
            {path: 'admin-dashboard' ,component: AdminDashboardComponent}
        ],
    },
];
@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule],
})
export class AppRoutingModule {}
