import { Component, OnInit } from '@angular/core';
import { TabMenuModule } from 'primeng/tabmenu';


@Component({
  selector: 'app-user-settings',
  templateUrl: './user-settings.component.html',
  styleUrls: ['./user-settings.component.scss']
})
export class UserSettingsComponent implements OnInit {

  items!: any[];
  activeItem: any;

  constructor() {

  }

  ngOnInit(): void {
    this.items = [
      { label: 'User', icon: 'pi pi-fw pi-user', routerLink: "user-settings-info"},
      { label: 'Email', icon: 'pi pi-fw pi-at', routerLink: "user-settings-email" },
      { label: 'Password', icon: 'pi pi-fw pi-key', routerLink: "user-settings-password" }
    ];

    this.activeItem = this.items[0];
  }
}
