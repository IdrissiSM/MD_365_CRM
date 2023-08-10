import { OnInit } from '@angular/core';
import { Component } from '@angular/core';
import { LayoutService } from './service/app.layout.service';
import { AppStateService } from '../services/app-state.service';

@Component({
    selector: 'app-menu',
    templateUrl: './app.menu.component.html',
})
export class AppMenuComponent implements OnInit {
    model: any[] = [];
    role = ""

    constructor(public layoutService: LayoutService, private appStateService: AppStateService) {
        this.role = this.appStateService.authState.roles
        console.log(this.role === "Admin")
    }

    ngOnInit() {
        this.model = [
            {
                label: 'Home',
                items: [
                    {
                        label: 'Dashboard',
                        icon: 'pi pi-fw pi-home',
                        routerLink: ['/home'],
                    },
                ],
            },
            {
                label: 'Opportunities',
                items: [
                    {
                        label: 'Opportunities',
                        icon: 'pi pi-fw pi-briefcase',
                        routerLink: ['/opportunities'],
                    },
                ],
            },
            {
                label: 'Incidents',
                items: [
                    {
                        label: 'Incidents',
                        icon: 'pi pi-fw pi-wrench',
                        routerLink: ['/incidents'],
                    },
                ],
            },
            {
                label: 'Products',
                items: [
                    {
                        label: 'Products',
                        icon: 'pi pi-fw pi-box',
                        routerLink: ['/products'],
                    },
                ],
            },
            this.role === "Admin" ?{
                label: 'Users',
                items: [
                    {
                        label: 'Users',
                        icon: 'pi pi-fw pi-users',
                        routerLink: ['/users'],
                    },
                ],
            }: null,
        ];
    }
}
