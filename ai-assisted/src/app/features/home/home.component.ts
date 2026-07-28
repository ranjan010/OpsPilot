import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TenantService } from '../../core/services/tenant.service';
import { WorkspaceService } from '../../core/services/workspace.service';
import { OrganizationOverview, TenantRole } from '../../core/models/organization.model';
import { Project } from '../../core/models/project.model';
import { Ticket } from '../../core/models/ticket.model';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  overview: OrganizationOverview | null = null;
  projects: Project[] = [];
  tickets: Ticket[] = [];
  isLoading = true;
  errorMessage = '';

  constructor(private tenantService: TenantService, private workspaceService: WorkspaceService) {}

  ngOnInit(): void {
    this.loadOverview();
    this.loadWorkspaceData();
  }

  loadOverview(): void {
    this.isLoading = true;
    this.tenantService.getOverview().subscribe({
      next: (data) => {
        this.overview = data;
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = err?.error?.message || 'Failed to load workspace metrics.';
        this.isLoading = false;
      }
    });
  }

  loadWorkspaceData(): void {
    this.workspaceService.getProjects().subscribe(projects => this.projects = projects.slice(0, 3));
    this.workspaceService.getTickets().subscribe(tickets => this.tickets = tickets.slice(0, 3));
  }

  getRoleLabel(role: TenantRole | undefined): string {
    switch (role) {
      case TenantRole.Admin: return 'Admin';
      case TenantRole.Manager: return 'Manager';
      case TenantRole.Agent: return 'Agent';
      default: return 'Member';
    }
  }
}
