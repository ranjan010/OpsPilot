import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { TenantService } from '../services/tenant.service';
import { map, catchError, of } from 'rxjs';

export const tenantGuard: CanActivateFn = () => {
  const router = inject(Router);
  const tenantService = inject(TenantService);

  return tenantService.getMyWorkspaces().pipe(
    map(workspaces => {
      if (workspaces && workspaces.length > 0) {
        return true;
      }
      router.navigate(['/workspace/setup']);
      return false;
    }),
    catchError(() => {
      router.navigate(['/workspace/setup']);
      return of(false);
    })
  );
};
