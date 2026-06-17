import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
  HttpErrorResponse,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';

@Injectable()
export class HttpErrorInterceptor implements HttpInterceptor {
  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        const message =
          error.error?.message ||
          error.statusText ||
          'A network error occurred. Please try again.';

        console.error('[API ERROR]', {
          url: request.url,
          status: error.status,
          message,
          error: error.error,
        });

        return throwError(() => error);
      })
    );
  }
}
