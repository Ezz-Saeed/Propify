import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../Services/auth.service';
import { catchError, map, of, switchMap, take } from 'rxjs';
import { Router } from '@angular/router';
import { IAuthUser } from '../Models/auth.user';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const router = inject(Router)
  let currentUser:IAuthUser | null = new IAuthUser()
  const token = localStorage.getItem('token');
  if(token)
    req = req.clone({
      setHeaders:{Authorization:`Bearer ${token}`}
  })

  authService.currentUser$.pipe(
    map(user=>{
      if(user){
        currentUser = user
      }else{

      }

    })
  )

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
        // router.navigateByUrl('/login')
      }
      return of();
    })
  );
};
