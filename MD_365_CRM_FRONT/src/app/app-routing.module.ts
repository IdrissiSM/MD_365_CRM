import { ProductsComponent } from './components/products/products.component';
import { OpportunitiesComponent } from './components/opportunities/opportunities.component';
import { NgModule, Component } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AppLayoutComponent } from './layout/app.layout.component';
import { OpportunityComponent } from './components/opportunity/opportunity.component';
import { ProductDetailsComponent } from './components/product-details/product-details.component';
import { DetailComponent } from './components/incident/detail/detail.component';
import { IncidentComponent } from './components/incident/incident.component';
import { LoginComponent } from './components/login/login.component';
import { EmailVerificationComponent } from './components/email-verification/email-verification.component';
import { RegisterComponent } from './components/register/register.component';
import { VerificationCodeComponent } from './components/verification-code/verification-code.component';
import { AuthGuard } from './guards/auth.guard';
import { LoggedInGuard } from './guards/logged-in.guard';
import { UsersComponent } from './components/users/users.component';
import { AdminGuard } from './guards/admin.guard';
import { UserDashboardComponent } from './components/dashboards/user-dashboard/user-dashboard.component';
import { ResetPasswordComponent } from './components/reset-password/reset-password.component';
import {AdminDashboardComponent} from "./components/dashboards/admin-dashboard/admin-dashboard.component";
import { UserSettingsComponent } from './components/user-settings/user-settings.component';
import { UserSettingsDataComponent } from './components/user-settings/user-settings-data/user-settings-data.component';
import { UserSettingsPasswordComponent } from './components/user-settings/user-settings-password/user-settings-password.component';
import { UserSettingsEmailComponent } from './components/user-settings/user-settings-email/user-settings-email.component';


const routes: Routes = [
    {
        path: '',
        component: AppLayoutComponent,
        canActivate: [AuthGuard],
        children: [
            { path: 'opportunities', component: OpportunitiesComponent },
            { path: 'opportunity', component: OpportunityComponent },
            { path: 'products', component: ProductsComponent },
            { path: 'product-details', component: ProductDetailsComponent },
            { path: 'incidents', component: IncidentComponent },
            { path: 'incidents/detail/:id', component: DetailComponent },
            { path: 'users',component: UsersComponent,canActivate: [AdminGuard] },
            { path: 'dashboard', component: UserDashboardComponent },
            {path: 'admin-dashboard' ,component: AdminDashboardComponent, canActivate: [AdminGuard]},
            {
                path:'user-settings',
                component: UserSettingsComponent,
                canActivate: [AuthGuard],
                children: [
                    {path: '', redirectTo: 'user-settings-info', pathMatch: 'full'},
                    {path: 'user-settings-info', component: UserSettingsDataComponent},
                    {path: 'user-settings-password', component: UserSettingsPasswordComponent},
                    {path: 'user-settings-email', component: UserSettingsEmailComponent}
                ]
            },
        ],
    },
    {
        path: 'login',
        component: LoginComponent,
        canActivate: [LoggedInGuard],
    },
    {
        path: 'email-verification',
        component: EmailVerificationComponent,
        canActivate: [LoggedInGuard],
    },
    {
        path: 'register',
        component: RegisterComponent,
        canActivate: [LoggedInGuard],
    },
    {
        path: 'verification-code',
        component: VerificationCodeComponent,
        canActivate: [LoggedInGuard],
    },
    {
        path: 'reset-password',
        component: ResetPasswordComponent,
        canActivate: [LoggedInGuard],
    },
];
@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule],
})
export class AppRoutingModule {}
