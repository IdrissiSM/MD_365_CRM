import { NgModule } from '@angular/core';
import {
    CommonModule,
    HashLocationStrategy,
    LocationStrategy,
} from '@angular/common';
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { AppLayoutModule } from './layout/app.layout.module';
import { NotfoundComponent } from './demo/components/notfound/notfound.component';
import { ProductService } from './demo/service/product.service';
import { CountryService } from './demo/service/country.service';
import { CustomerService } from './demo/service/customer.service';
import { EventService } from './demo/service/event.service';
import { IconService } from './demo/service/icon.service';
import { NodeService } from './demo/service/node.service';
import { PhotoService } from './demo/service/photo.service';
import { PrimeNGModule } from './prime-ng/PrimeNG.module';
import { PasswordModule } from 'primeng/password';
import { CheckboxModule } from 'primeng/checkbox';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ConfirmationService, MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { OpportunitiesComponent } from './components/opportunities/opportunities.component';
import { OpportunityComponent } from './components/opportunity/opportunity.component';
import { ProductsComponent } from './components/products/products.component';
import { ProductDetailsComponent } from './components/product-details/product-details.component';
import { IncidentComponent } from './components/incident/incident.component';
import { DetailComponent } from './components/incident/detail/detail.component';
import { BasicComponent } from './components/incident/charts/basic/basic.component';
import { DoughnutComponent } from './components/incident/charts/doughnut/doughnut.component';
import { PolarComponent } from './components/incident/charts/polar/polar.component';
import { AdminDashboardComponent } from './components/dashboards/admin-dashboard/admin-dashboard.component';
import {MenuModule} from "primeng/menu";
import {AvatarModule} from "primeng/avatar";
import {BasicComponent2} from "./components/dashboards/admin-dashboard/charts/basic2/basic.component";
import { VerticalBarComponent } from './components/dashboards/admin-dashboard/charts/vertical-bar/vertical-bar.component';
import { BubbleComponent } from './components/dashboards/admin-dashboard/charts/bubble/bubble.component';
import {NgChartsModule} from "ng2-charts";


@NgModule({
    declarations: [
        AppComponent,
        NotfoundComponent,
        OpportunitiesComponent,
        OpportunityComponent,
        ProductsComponent,
        ProductDetailsComponent,
        IncidentComponent,
        DetailComponent,
        BasicComponent,
        DoughnutComponent,
        PolarComponent,
        AdminDashboardComponent,
        BasicComponent2,
        VerticalBarComponent,
        BubbleComponent
    ],
    imports: [
        AppRoutingModule,
        AppLayoutModule,
        FormsModule,
        PrimeNGModule,
        PasswordModule,
        CheckboxModule,
        FormsModule,
        ReactiveFormsModule,
        CommonModule,
        MenuModule,
        AvatarModule,
        NgChartsModule
    ],
    providers: [
        { provide: LocationStrategy, useClass: HashLocationStrategy },
        CountryService,
        CustomerService,
        EventService,
        IconService,
        NodeService,
        PhotoService,
        ProductService,
        ConfirmationService,
        MessageService,
        DialogService,
    ],
    bootstrap: [AppComponent],
})
export class AppModule {}
