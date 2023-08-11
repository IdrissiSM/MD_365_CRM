import { OnInit } from '@angular/core';
import { Component } from '@angular/core';
import { LayoutService } from './service/app.layout.service';

@Component({
    selector: 'app-menu',
    templateUrl: './app.menu.component.html',
})
export class AppMenuComponent implements OnInit {
    model: any[] = [];

    constructor(public layoutService: LayoutService) {}

    ngOnInit() {
        this.model = [
            {
                label: 'Home',
                items: [
                    {
                        label: 'Dashboard',
                        icon: 'pi pi-fw pi-home',
                        routerLink: ['/admin-dashboard'],
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
            {
                label: 'Users',
                items: [
                    {
                        label: 'Users',
                        icon: 'pi pi-fw pi-users',
                        routerLink: ['/users'],
                    },
                ],
            },
        ];
    }
}
