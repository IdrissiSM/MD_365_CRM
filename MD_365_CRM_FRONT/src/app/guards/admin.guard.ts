import { Injectable } from '@angular/core';
import {
    ActivatedRouteSnapshot,
    CanActivate,
    Router,
    RouterStateSnapshot,
    UrlTree,
} from '@angular/router';
import { Observable } from 'rxjs';
import { AppStateService } from '../services/app-state.service';

@Injectable({
    providedIn: 'root',
})
export class AdminGuard implements CanActivate {
    constructor(
        private router: Router,
        private appSateService: AppStateService
    ) {}

    canActivate(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
    ): boolean {
        const userRole = this.appSateService.authState.roles;
        if (userRole === 'Admin') {
            return true;
        } else {
            this.router.navigate(['dashboard']);
            return false;
        }
    }
}
