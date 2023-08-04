import { Injectable } from '@angular/core';
import {
    ActivatedRouteSnapshot,
    CanActivate,
    Router,
    RouterStateSnapshot,
    UrlTree,
} from '@angular/router';
import { Observable } from 'rxjs';
import { AppStateService } from '../../services/app-state.service';

@Injectable({
    providedIn: 'root',
})
export class LoggedInGuard implements CanActivate {
    /**
     *
     */
    constructor(private router: Router, private appState: AppStateService) {}

    canActivate(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
    ):
        | Observable<boolean | UrlTree>
        | Promise<boolean | UrlTree>
        | boolean
        | UrlTree {
        return !this.appState.authState.isAuthenticated
            ? true
            : this.router.createUrlTree(['']);
    }
}
