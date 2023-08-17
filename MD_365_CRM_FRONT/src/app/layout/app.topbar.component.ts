import { Component, ElementRef, ViewChild, OnInit } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { LayoutService } from './service/app.layout.service';
import { AppStateService } from '../services/app-state.service';
import { Router } from '@angular/router';

@Component({
    selector: 'app-topbar',
    templateUrl: './app.topbar.component.html',
})
export class AppTopBarComponent {
    items!: MenuItem[];

    @ViewChild('menubutton') menuButton!: ElementRef;

    @ViewChild('topbarmenubutton') topbarMenuButton!: ElementRef;

    @ViewChild('topbarmenu') menu!: ElementRef;

    constructor(
        public layoutService: LayoutService,
        private appState: AppStateService,
        private router: Router,
        private elementRef: ElementRef
    ) {
        this.handleInstallApp();
    }

    signOut() {
        this.appState.signOut();
        location.reload();
    }

    displayInstall: boolean = false;
    deferredPrompt: any;
    handleInstallApp() {
        this.displayInstall = false;
        window.addEventListener('beforeinstallprompt', (e: Event) => {
            e.preventDefault();
            this.deferredPrompt = e;
            this.displayInstall = true;
        });
    }

    installApp() {
        console.log('tst');
        this.displayInstall = false;
        this.deferredPrompt?.prompt();
        this.deferredPrompt?.userChoice.then((choiceResult: any) => {
            if (choiceResult.outcome === 'accepted') {
                console.log('User accepted the A2HS prompt');
            } else {
                console.log('User dismissed the A2HS prompt');
            }
            this.deferredPrompt = null;
        });
    }
}
