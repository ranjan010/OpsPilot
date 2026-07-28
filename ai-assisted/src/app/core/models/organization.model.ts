export enum TenantRole {
  Admin = 0,
  Manager = 1,
  Agent = 2
}

export interface Organization {
  id: string;
  name: string;
  slug: string;
  role: TenantRole;
  joinedAt: string;
}

export interface OrganizationOverview {
  id: string;
  name: string;
  slug: string;
  userRole: TenantRole;
  totalMembers: number;
  totalProjects: number;
  totalOpenTickets: number;
  createdAt: string;
}

export interface CreateOrganizationRequest {
  name: string;
  slug: string;
}
