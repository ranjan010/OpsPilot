import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterLinkActive, RouterOutlet, Router } from '@angular/router';
import { AuthService } from '../core/services/auth.service';
import { TenantService } from '../core/services/tenant.service';
import { Organization } from '../core/models/organization.model';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterLinkActive, RouterOutlet],
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent implements OnInit {
  private readonly authService = inject(AuthService);
  private readonly tenantService = inject(TenantService);
  private readonly router = inject(Router);

  activeTenant: Organization | null = null;
  workspaces: Organization[] = [];

  ngOnInit(): void {
    this.tenantService.activeTenant$.subscribe(tenant => {
      this.activeTenant = tenant;
    });

    this.tenantService.getMyWorkspaces().subscribe(list => {
      this.workspaces = list;
    });
  }

  selectWorkspace(org: Organization): void {
    this.tenantService.setActiveTenant(org);
    window.location.reload();
  }

  switchWorkspace(): void {
    this.router.navigate(['/workspace/setup']);
  }

  logout(event: Event): void {
    event.preventDefault();
    this.tenantService.clearTenant();
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
