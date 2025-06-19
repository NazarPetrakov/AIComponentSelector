import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';
import { catchError, map, of } from 'rxjs';

export const adminGuard: CanActivateFn = (route, state) => {
  const accountService = inject(AccountService);
  const toastr = inject(ToastrService);
  const router = inject(Router);

  if (accountService.user() && accountService.user()?.role === 'Admin') {
    return true;
  }
  return accountService.setMe().pipe(
    map((user) => {
      if (user?.role === 'Admin') {
        return true;
      } else {
        toastr.error('You have no permission to access this page.');
        router.navigateByUrl('/');
        return false;
      }
    }),
    catchError((err) => {
      toastr.error('Authorization failed.');
      router.navigateByUrl('/');
      return of(false);
    })
  );
};
