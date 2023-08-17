import { OnInit } from '@angular/core';
import { Component } from '@angular/core';
import { LayoutService } from './service/app.layout.service';
import { AppStateService } from '../services/app-state.service';
import jwt_decode from 'jwt-decode';
import { Contact } from '../Models/Contact';


@Component({
    selector: 'app-menu',
    templateUrl: './app.menu.component.html',
})
export class AppMenuComponent implements OnInit {
    model: any[] = [];
    role = '';
    decodedJWT: any;
    authState!: any;

    constructor(
        public layoutService: LayoutService,
        private appStateService: AppStateService
    ) {
        // this.role = this.appStateService.authState.roles;
        // console.log(this.role === 'Admin');
        this.decodedJWT= jwt_decode(this.appStateService.authState.token);
        this.role = this.decodedJWT.roles;
        
    }

    ngOnInit() {

        this.authState = this.appStateService.getAuthState();

        this.model = [
            this.role === 'Admin'
                ? {
                      label: 'Dashboard',
                      items: [
                          {
                              label: 'Dashboard',
                              icon: 'pi pi-fw pi-home',
                              routerLink: ['/admin-dashboard'],
                          },
                      ],
                  }
                : {
                    label: 'Dashboard',
                    items: [
                        {
                            label: 'Dashboard',
                            icon: 'pi pi-fw pi-home',
                            routerLink: ['/dashboard'],
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
            this.role === 'Admin'
                ? {
                      label: 'Users',
                      items: [
                          {
                              label: 'Users',
                              icon: 'pi pi-fw pi-users',
                              routerLink: ['/users'],
                          },
                      ],
                  }
                : null,
            
            
        ];
    }
}
