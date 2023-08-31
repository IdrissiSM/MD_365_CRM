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
import { Router } from '@angular/router';
import { DataSyncService } from '../services/data-sync.service';
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

	isSynchronizing: boolean = false;

	synCompleted = false;

	displayInstall = false;

	syncComplete = false;

	syncMessages = {
		opportunities: { succeededMessage: 'Opportunities synchronized successfully', inProgressMessage: 'Opportunities are being synchronized ...', failedMessage: 'Failed to synchronize the opportunities' },
		incidents: { succeededMessage: 'Incidents synchronized successfully', inProgressMessage: 'Incidents are being synchronized ...', failedMessage: 'Failed to synchronize the incidents' },
		products: { succeededMessage: 'Products synchronized successfully', inProgressMessage: 'Products are being synchronized ...', failedMessage: 'Failed to synchronize the products' },
		profile: { succeededMessage: 'Profile synchronized successfully', inProgressMessage: 'Profile is being synchronized ...', failedMessage: 'Failed to synchronize the Profile' },
	};

	syncIcons = {
		checking: { icon: 'pi-spin pi-spinner', color: 'slateblue' },
		failed: { icon: 'pi-times', color: 'red' },
		succeeded: { icon: 'pi-check', color: 'green' }
	}

	syncItems = [
		{ ...this.syncIcons.checking, message: this.syncMessages.opportunities.inProgressMessage, completed: false },
		{ ...this.syncIcons.checking, message: this.syncMessages.incidents.inProgressMessage, completed: false },
		{ ...this.syncIcons.checking, message: this.syncMessages.products.inProgressMessage, completed: false },
		{ ...this.syncIcons.checking, message: this.syncMessages.profile.inProgressMessage, completed: false },
	]


	constructor(
		public layoutService: LayoutService,
		private appState: AppStateService,
		private appStateService: AppStateService,
		private synchronizeService: SynchronizeService,
		private confirmationService: ConfirmationService,
		private messageService: MessageService,
		private dataSync: DataSyncService
	) {
		this.displayInstall = this.appStateService.displayInstall,
			this.dataSync.dataHolder = 'new value'
	}

	signOut() {
		this.appState.signOut();
		location.reload();
	}

	// displayInstall: boolean = false;
	// deferredPrompt: any;
	// handleInstallApp() {
	//     this.displayInstall = false;
	//     window.addEventListener('beforeinstallprompt', (e: Event) => {
	//         e.preventDefault();
	//         this.deferredPrompt = e;
	//         this.displayInstall = true;
	//     });
	// }

	installApp() {
		console.log('tst');
		this.appStateService.deferredPrompt?.prompt();
		this.appStateService.deferredPrompt?.userChoice.then((choiceResult: any) => {
			if (choiceResult.outcome === 'accepted') {
				this.displayInstall = false;
				console.log('User accepted the A2HS prompt');
			} else {
				console.log('User dismissed the A2HS prompt');
			}
			this.appStateService.deferredPrompt = null;
		});
	}

	// synchronize() {
	//     this.synchronizeService
	//         .synchronize(this.appState.authState.contactId)
	//         .subscribe((response: APIResponse) => {
	//             if (response.success) {
	//                 this.messageService.add({
	//                     severity: 'success',
	//                     summary: 'Confirmed',
	//                     detail: 'Synchronized successfully !',
	//                 });
	//                 this.isSynchronizing = false;
	//             } else {
	//                 this.messageService.add({
	//                     severity: 'error',
	//                     summary: 'Failed',
	//                     detail: 'Synchronization failed !',
	//                 });
	//                 this.isSynchronizing = false;
	//             }
	//         }, (error) => {
	//             console.log(error);
	//             this.messageService.add({
	//                 severity: 'error',
	//                 summary: 'Failed',
	//                 detail: 'Synchronization failed !',
	//             });
	//             this.isSynchronizing = false;
	//         });
	// }

	checkIfSyncIsComplete(): boolean {
		return this.syncItems.every(item => item.completed);
	}

	synchronize() {
		this.synchronizeService.synchronizeOpportunities(this.appState.authState.contactId).subscribe((response: APIResponse) => {
			if (response.success) {
				this.syncItems[0].message = this.syncMessages.opportunities.succeededMessage;
				this.syncItems[0].icon = this.syncIcons.succeeded.icon;
				this.syncItems[0].color = this.syncIcons.succeeded.color;
				this.syncItems[0].completed = true;
				this.syncComplete = this.checkIfSyncIsComplete();
			} else {
				this.syncItems[0].message = this.syncMessages.opportunities.failedMessage;
				this.syncItems[0].icon = this.syncIcons.failed.icon;
				this.syncItems[0].color = this.syncIcons.failed.color;
				this.syncItems[0].completed = true;
				this.syncComplete = this.checkIfSyncIsComplete();
			}
		}, (error) => {
			console.log(error);
			this.syncItems[0].message = this.syncMessages.opportunities.failedMessage;
			this.syncItems[0].icon = this.syncIcons.failed.icon;
			this.syncItems[0].color = this.syncIcons.failed.color;
			this.syncItems[0].completed = true;
			this.syncComplete = this.checkIfSyncIsComplete();
		});
		this.synchronizeService.synchronizeIncidents(this.appState.authState.contactId).subscribe((response: APIResponse) => {
			if (response.success) {
				this.syncItems[1].message = this.syncMessages.incidents.succeededMessage;
				this.syncItems[1].icon = this.syncIcons.succeeded.icon;
				this.syncItems[1].color = this.syncIcons.succeeded.color;
				this.syncItems[1].completed = true;
				this.syncComplete = this.checkIfSyncIsComplete();
			} else {
				this.syncItems[1].message = this.syncMessages.incidents.failedMessage;
				this.syncItems[1].icon = this.syncIcons.failed.icon;
				this.syncItems[1].color = this.syncIcons.failed.color;
				this.syncItems[1].completed = true;
				this.syncComplete = this.checkIfSyncIsComplete();
			}
		}, (error) => {
			console.log(error);
			this.syncItems[1].message = this.syncMessages.incidents.failedMessage;
			this.syncItems[1].icon = this.syncIcons.failed.icon;
			this.syncItems[1].color = this.syncIcons.failed.color;
			this.syncItems[1].completed = true;
			this.syncComplete = this.checkIfSyncIsComplete();
		});
		this.synchronizeService.synchronizeProducts(this.appState.authState.contactId).subscribe((response: APIResponse) => {
			if (response.success) {
				this.syncItems[2].message = this.syncMessages.products.succeededMessage;
				this.syncItems[2].icon = this.syncIcons.succeeded.icon;
				this.syncItems[2].color = this.syncIcons.succeeded.color;
				this.syncItems[2].completed = true;
				this.syncComplete = this.checkIfSyncIsComplete();
			} else {
				this.syncItems[2].message = this.syncMessages.products.failedMessage;
				this.syncItems[2].icon = this.syncIcons.failed.icon;
				this.syncItems[2].color = this.syncIcons.failed.color;
				this.syncItems[2].completed = true;
				this.syncComplete = this.checkIfSyncIsComplete();
			}
		}, (error) => {
			console.log(error);
			this.syncItems[2].message = this.syncMessages.products.failedMessage;
			this.syncItems[2].icon = this.syncIcons.failed.icon;
			this.syncItems[2].color = this.syncIcons.failed.color;
			this.syncItems[2].completed = true;
			this.syncComplete = this.checkIfSyncIsComplete();
		});
		this.synchronizeService.synchronizeOpportunities(this.appState.authState.contactId).subscribe((response: APIResponse) => {
			if (response.success) {
				this.syncItems[3].message = this.syncMessages.profile.succeededMessage;
				this.syncItems[3].icon = this.syncIcons.succeeded.icon;
				this.syncItems[3].color = this.syncIcons.succeeded.color;
				this.syncItems[3].completed = true;
				this.syncComplete = this.checkIfSyncIsComplete();
			} else {
				this.syncItems[3].message = this.syncMessages.profile.failedMessage;
				this.syncItems[3].icon = this.syncIcons.failed.icon;
				this.syncItems[3].color = this.syncIcons.failed.color;
				this.syncItems[3].completed = true;
				this.syncComplete = this.checkIfSyncIsComplete();
			}
		}, (error) => {
			console.log(error);
			this.syncItems[3].message = this.syncMessages.profile.failedMessage;
			this.syncItems[3].icon = this.syncIcons.failed.icon;
			this.syncItems[3].color = this.syncIcons.failed.color;
			this.syncItems[3].completed = true;
			this.syncComplete = this.checkIfSyncIsComplete();
		})
	}

	confirmSynchronization() {
		this.confirmationService.confirm({
			message: 'Are you sure that you want to synchronize ?',
			header: 'Confirmation',
			icon: 'pi pi-exclamation-triangle',
			accept: () => {
				this.isSynchronizing = true;
				this.syncItems = [
					{ ...this.syncIcons.checking, message: this.syncMessages.opportunities.inProgressMessage, completed: false },
					{ ...this.syncIcons.checking, message: this.syncMessages.incidents.inProgressMessage, completed: false },
					{ ...this.syncIcons.checking, message: this.syncMessages.products.inProgressMessage, completed: false },
					{ ...this.syncIcons.checking, message: this.syncMessages.profile.inProgressMessage, completed: false },
				]
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
