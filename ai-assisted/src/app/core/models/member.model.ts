import { TenantRole } from './organization.model';

export interface Member {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  role: TenantRole;
  joinedAt: string;
}
