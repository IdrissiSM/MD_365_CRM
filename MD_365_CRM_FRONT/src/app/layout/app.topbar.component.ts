import { APIResponse } from './../Models/APIResponse';
import { Component, ElementRef, ViewChild } from '@angular/core';
import {
    ConfirmationService,
    MessageService,
    ConfirmEventType,
} from 'primeng/api';
import { MenuItem } from 'primeng/api';
import { LayoutService } from './service/app.layout.service';
import { AppStateService } from '../services/app-state.service';
import { SynchronizeService } from '../services/synchronize.service';

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
        private synchronizeService: SynchronizeService,
        private confirmationService: ConfirmationService,
        private messageService: MessageService
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

    isSynchronizing: boolean = false;
    synchronize() {
        this.synchronizeService
            .synchronize()
            .subscribe((response: APIResponse) => {
                if (response.success) {
                    this.messageService.add({
                        severity: 'success',
                        summary: 'Confirmed',
                        detail: 'Synchronized successfully !',
                    });
                    this.isSynchronizing = false;
                }
            });
    }

    confirmSynchronization() {
        this.confirmationService.confirm({
            message: 'Are you sure that you want to synchronize ?',
            header: 'Confirmation',
            icon: 'pi pi-exclamation-triangle',
            accept: () => {
                this.isSynchronizing = true;
                this.synchronize();
            },
            reject: (type: ConfirmEventType) => {
                switch (type) {
                    case ConfirmEventType.REJECT:
                        this.messageService.add({
                            severity: 'error',
                            summary: 'Rejected',
                            detail: 'You have rejected',
                        });
                        break;
                    case ConfirmEventType.CANCEL:
                        this.messageService.add({
                            severity: 'warn',
                            summary: 'Cancelled',
                            detail: 'You have cancelled',
                        });
                        break;
                }
            },
        });
    }
}
