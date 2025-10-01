import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, delay, finalize } from 'rxjs';
import { LoadingService } from '../shared/services/loading.service';
import { Router } from '@angular/router';

@Injectable()
export class LoadingInterceptor implements HttpInterceptor {
  constructor(private loadingService: LoadingService, private router: Router) {}

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    const currentUrl = this.router.url;

    // Skip showing spinner if route is /dashboard
    const skipLoading = currentUrl.includes('/dashboard');

    if (!skipLoading) {
      this.loadingService.loading();
    }

    return next.handle(req).pipe(
      delay(300),
      finalize(() => {
        if (!skipLoading) {
          this.loadingService.idle();
        }
      })
    );
  }
}
