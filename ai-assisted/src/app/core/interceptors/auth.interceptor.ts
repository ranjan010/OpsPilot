import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { TenantService } from '../services/tenant.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const tenantService = inject(TenantService);
  const token = localStorage.getItem('auth-token');
  const tenantId = tenantService.activeTenantId;

  let headers: Record<string, string> = {};

  if (token) {
    headers['Authorization'] = `Bearer ${token}`;
  }

  if (tenantId) {
    headers['X-Tenant-Id'] = tenantId;
  }

  if (Object.keys(headers).length > 0) {
    const authReq = req.clone({
      setHeaders: headers
    });
    return next(authReq);
  }

  return next(req);
};
