import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { CreateOrganizationRequest, Organization, OrganizationOverview } from '../models/organization.model';

@Injectable({ providedIn: 'root' })
export class TenantService {
  private readonly baseUrl = 'https://localhost:58958/api/organizations';
  private readonly tenantStorageKey = 'opspilot-tenant-id';

  private activeTenantSubject = new BehaviorSubject<Organization | null>(null);
  public activeTenant$ = this.activeTenantSubject.asObservable();

  constructor(private http: HttpClient) {
    const savedTenantId = localStorage.getItem(this.tenantStorageKey);
    if (savedTenantId) {
      // Restore cached tenant id initial stub
      this.activeTenantSubject.next({ id: savedTenantId, name: '', slug: '', role: 0, joinedAt: '' });
    }
  }

  getMyWorkspaces(): Observable<Organization[]> {
    return this.http.get<Organization[]>(`${this.baseUrl}/my`).pipe(
      tap(workspaces => {
        if (workspaces.length > 0) {
          const currentId = localStorage.getItem(this.tenantStorageKey);
          const found = workspaces.find(w => w.id === currentId);
          if (found) {
            this.setActiveTenant(found);
          } else {
            this.setActiveTenant(workspaces[0]);
          }
        } else {
          this.activeTenantSubject.next(null);
        }
      })
    );
  }

  createOrganization(request: CreateOrganizationRequest): Observable<Organization> {
    return this.http.post<Organization>(this.baseUrl, request).pipe(
      tap(org => {
        this.setActiveTenant(org);
      })
    );
  }

  getOverview(): Observable<OrganizationOverview> {
    return this.http.get<OrganizationOverview>(`${this.baseUrl}/overview`);
  }

  setActiveTenant(org: Organization): void {
    localStorage.setItem(this.tenantStorageKey, org.id);
    this.activeTenantSubject.next(org);
  }

  get activeTenantId(): string | null {
    return localStorage.getItem(this.tenantStorageKey);
  }

  clearTenant(): void {
    localStorage.removeItem(this.tenantStorageKey);
    this.activeTenantSubject.next(null);
  }
}
