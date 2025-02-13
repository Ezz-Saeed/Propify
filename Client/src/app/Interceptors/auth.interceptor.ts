import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../Services/auth.service';
import { catchError, of, switchMap, take } from 'rxjs';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const token = localStorage.getItem('token');
  if(token)
    req = req.clone({
      setHeaders:{Authorization:`Bearer ${token}`}
  })

  return next(req).pipe(
    catchError(error=>{
      if(error.status === 401){
        return authService.getNewToken().pipe(
          take(1),
          switchMap(res=>{
            localStorage.setItem('token', res.token);
            req = req.clone({
              setHeaders:{Authorization: `Bearer ${res.token}`}
            });
            return next(req);
          })
        )
      }
      return of();
    })
  );
};
