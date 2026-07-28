import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WorkspaceService } from '../../core/services/workspace.service';
import { Member } from '../../core/models/member.model';
import { TenantRole } from '../../core/models/organization.model';

@Component({
  standalone: true,
  selector: 'app-members',
  imports: [CommonModule],
  templateUrl: './members.component.html',
  styleUrls: ['./members.component.scss']
})
export class MembersComponent implements OnInit {
  members: Member[] = [];
  isLoading = true;
  errorMessage = '';

  constructor(private workspaceService: WorkspaceService) {}

  ngOnInit(): void {
    this.loadMembers();
  }

  loadMembers(): void {
    this.isLoading = true;
    this.errorMessage = '';
    this.workspaceService.getMembers().subscribe({
      next: (data) => {
        this.members = data;
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = err?.error?.message || 'Failed to load members.';
        this.isLoading = false;
      }
    });
  }

  getRoleLabel(role: TenantRole): string {
    switch (role) {
      case TenantRole.Admin:   return 'Admin';
      case TenantRole.Manager: return 'Manager';
      case TenantRole.Agent:   return 'Agent';
      default:                  return 'Member';
    }
  }

  getRoleClass(role: TenantRole): string {
    switch (role) {
      case TenantRole.Admin:   return 'role-admin';
      case TenantRole.Manager: return 'role-manager';
      case TenantRole.Agent:   return 'role-agent';
      default:                  return 'role-agent';
    }
  }

  getInitials(member: Member): string {
    return `${member.firstName.charAt(0)}${member.lastName.charAt(0)}`.toUpperCase();
  }

  trackById(_: number, member: Member): string {
    return member.id;
  }
}
