import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
import { AppStateService } from './app-state.service';

@Injectable()
export class MyHttpInterceptor implements HttpInterceptor {

  token = this.appState.getAuthState().token;

  private readonly publicUrls = [
    '/Auth/login',
    '/Auth/register',
    '/' // to stop the interceptor
  ];

  constructor(private appState: AppStateService) {

  }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // Check if the request URL is a public URL (doesn't need authorization)
    const isPublicUrl = this.publicUrls.some(url => request.url.includes(url));

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
