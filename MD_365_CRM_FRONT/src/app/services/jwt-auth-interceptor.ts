import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
import { AppStateService } from './app-state.service';
import { environment } from 'src/environments/environment';

@Injectable()
export class JWtAuthInterceptor implements HttpInterceptor {

  token!: string;

  private readonly publicUrls = [ //this list enlists the paths from which requests are not to be intercepted
    '/Auth/login',
    '/Auth/register',
    '/verification-code',
    'email-verification'
  ];

  constructor(private appState: AppStateService) {
    this.token = this.appState.getAuthState().token;
  }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // Check if the request URL is a public URL (doesn't need authorization)
    const isPublicUrl = this.publicUrls.some(url => request.url.includes(url));

    /*
    ? An extra check to verify if the user is authenticated and the request is sent to the appropriate server; not necessary at the moment.
    */
   const isLoggedIn = this.appState.getAuthState().isAuthenticated;
   const isToServer = request.url.startsWith(environment.apiBaseUrl);


    // Clone the request and add the Authorization header if it's not a public URL
    const modifiedRequest = isPublicUrl
      ? request // Skip adding the Authorization header for public URLs
      : request.clone({
          headers: request.headers
          .set('Authorization', `Bearer ${this.token}`)
          .set('Content-Type', 'application/json')
        });

    // Continue with the modified or original request
    return next.handle(modifiedRequest);
  }
}
