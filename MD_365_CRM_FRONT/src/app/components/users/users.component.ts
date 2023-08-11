import { Component, ElementRef, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Table } from 'primeng/table';
import { APIResponse } from 'src/app/Models/APIResponse';
import { User } from 'src/app/Models/User';
import { BlacklistedUser } from 'src/app/Models/blacklisted-user';
import { UsersService } from 'src/app/services/users.service';

@Component({
    selector: 'app-users',
    templateUrl: './users.component.html',
    styleUrls: ['./users.component.scss']
})
export class UsersComponent {
    loading: boolean = true;
    users: User[] = [];
    blacklistedUsers: BlacklistedUser[] = []
    totalUsers = 0
    totalBlacklistedUsers = 0
    genderChart : any
    stateChart: any
    pieOptions: any

    @ViewChild('filter') filter!: ElementRef;

    constructor(
        private usersService: UsersService,
        private router: Router
    ) { }

    ngOnInit() {
        this.getUsers();
        this.getBlacklistedUsers();
    }

    initCharts() {
        const documentStyle = getComputedStyle(document.documentElement);
        const textColor = documentStyle.getPropertyValue('--text-color');
        this.initGenderChart(documentStyle);
        this.initStateChart(documentStyle);
        this.pieOptions = {
            cutout: '50%',
            plugins: {
                legend: {
                    labels: {
                        color: textColor,
                    },
                },
            },
        };
    }

    initGenderChart(documentStyle: CSSStyleDeclaration) {
        const genderCodeCount = (this.users as any[]).reduce(
            (acc, user) => {
                if (acc.hasOwnProperty(user.gendercode)) {
                    acc[user.gendercode] += 1;
                } else {
                    acc[user.gendercode] = 1;
                }
                return acc;
            },
            {}
        );
        for (let i = 1; i <= 2; i++) {
            if (!(i in genderCodeCount)) {
                genderCodeCount[i] = 0;
            }
        }
        this.genderChart = {
            labels: this.genders.map((gender) => gender.label),
            datasets: [
                {
                    data: Object.values(genderCodeCount),
                    backgroundColor: [
                        documentStyle.getPropertyValue('--yellow-500'),
                        documentStyle.getPropertyValue('--green-500')
                    ],
                    hoverBackgroundColor: [
                        documentStyle.getPropertyValue('--yellow-400'),
                        documentStyle.getPropertyValue('--green-400')
                    ],
                },
            ],
        };
    }
    
    initStateChart(documentStyle: CSSStyleDeclaration) {
        const stateCodeCount = (this.users as any[]).reduce(
            (acc, user) => {
                if (acc.hasOwnProperty(user.stateCode)) {
                    acc[user.stateCode] += 1;
                } else {
                    acc[user.stateCode] = 1;
                }
                return acc;
            },
            {}
        );
        for (let i = 0; i <= 3; i++) {
            if (!(i in stateCodeCount)) {
                stateCodeCount[i] = 0;
            }
        }
        this.stateChart = {
            labels: this.states.map((state) => state.label),
            datasets: [
                {
                    data: Object.values(stateCodeCount),
                    backgroundColor: [
                        documentStyle.getPropertyValue('--blue-500'),
                        documentStyle.getPropertyValue('--red-500')
                    ],
                    hoverBackgroundColor: [
                        documentStyle.getPropertyValue('--blue-400'),
                        documentStyle.getPropertyValue('--red-400')
                    ],
                },
            ],
        };
    }

    // GET All
    getUsers() {
        this.usersService
            .getAllUsers()
            .subscribe((response: APIResponse) => {
                if (response.success) {
                    this.users = response.result;
                    this.totalUsers = this.users.length
                    this.initCharts()
                    this.loading = false;
                } else {
                    console.log(response.errorMessages);
                }
            });
    }
    getBlacklistedUsers() {
        this.usersService
            .getAllBlacklistedUsers()
            .subscribe((response: APIResponse) => {
                if (response.success) {
                    this.blacklistedUsers = response.result;
                    this.totalBlacklistedUsers = this.blacklistedUsers.length
                    this.loading = false;
                } else {
                    console.log(response.errorMessages);
                }
            });
    }

    addToBlacklist(email: string){
        const blacklisted: BlacklistedUser = {
            email: email
        } 
        this.usersService
            .addToBlacklist(blacklisted)
            .subscribe((response: APIResponse) => {
                if (response.success) {
                    this.ngOnInit()
                } else {
                    console.log(response.errorMessages);
                }
            });
    }
    removeFromBlacklist(email: string){
        this.usersService
            .removeFromBlacklist(email)
            .subscribe((response: APIResponse) => {
                if (response.success) {
                    this.ngOnInit()
                } else {
                    console.log(response.errorMessages);
                }
            });
    }
    deleteUser(id: any){
        this.usersService
            .deleteUser(id)
            .subscribe((response: APIResponse) => {
                if (response.success) {
                    this.ngOnInit()
                } else {
                    console.log(response.errorMessages);
                }
            });
    }

    onGlobalFilter(table: Table, event: Event) {
        table.filterGlobal(
            (event.target as HTMLInputElement).value,
            'contains'
        );
    }

    clear(table: Table) {
        table.clear();
        this.filter.nativeElement.value = '';
    }

    getFirstNWords(text: string, n: number): string {
        const wordsArray = text?.split(' ');
        const firstNWords = wordsArray?.slice(0, n);
        return firstNWords?.join(' ');
    }

    getGenderLabel(genderCode: number): string {
        const gender = this.genders.find(
            (gender: any) => gender.value === genderCode
        );
        return gender ? gender.label : '';
    }
    getStateLabel(stateCode: number): string {
        const state = this.states.find(
            (state: any) => state.value == stateCode
        );
        return state ? state.label : '';
    }

    genders = [
        { value: 1, label: "Male" },
        { value: 2, label: "Female" }
    ]
    states = [
        { value: 0, label: "Active" },
        { value: 1, label: "Inactive" }
    ]
}
