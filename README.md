# OpsPilot Product Documentation

<img width="1916" height="917" alt="image" src="https://github.com/user-attachments/assets/77baa4aa-6b94-442a-970c-8ea8ce40b59a" />

<img width="1917" height="907" alt="image" src="https://github.com/user-attachments/assets/4b42e50c-d66b-4dd3-a5c0-5eac81caaa45" />

<img width="1907" height="917" alt="image" src="https://github.com/user-attachments/assets/24e11730-fb10-407f-bbe2-a3c2dbdfeaee" />

<img width="1902" height="907" alt="image" src="https://github.com/user-attachments/assets/6c0714c4-2528-4ae5-900b-4b2b3edfb667" />


## 1. Product Overview
OpsPilot is a multi-tenant SaaS platform for client support and work management. It is designed for small service teams that need a workspace-based system for projects, tickets, members, and tenant-scoped reporting.

## 2. Current Scope Completed
### Authentication and Identity
- User registration and login endpoints are available through AuthController.
- JWT-based authentication is configured with issuer, audience, and signing key settings.
- Access and refresh tokens are issued through the application token service.

### Multi-Tenant Workspace Management
- Organizations are modeled as tenant workspaces and can be created by authenticated users.
- Workspace membership is stored in OrganizationMember entities.
- Tenant context is resolved from the X-Tenant-Id header and validated against the current user's organization memberships.
- Tenant-scoped entities are isolated via EF Core query filters and save-time organization assignment.

### Core Domain Model
- Organizations, projects, tickets, ticket comments, tags, audit logs, and refresh tokens are modeled as part of the domain layer.
- Tenant-scoped entities implement the ITenantScoped contract to enforce workspace boundaries consistently.

### CQRS and Application Layer
- The application layer uses MediatR for commands and queries.
- Validation is wired into the MediatR pipeline using FluentValidation.
- Organization overview and workspace listing queries are implemented for dashboard-style usage.

### API Surface
- Authentication endpoints: /api/auth/login, /api/auth/register, /api/auth/me
- Organization endpoints: /api/organizations, /api/organizations/my, /api/organizations/overview
- Project endpoints: /api/projects
- Ticket endpoints: /api/tickets

## 3. Database Design Notes
The database design is centered on a multi-tenant model:
- Users table stores account-level identity and credentials.
- Organizations table stores tenant workspaces.
- OrganizationMembers joins users to organizations with role-based membership.
- Projects, tickets, comments, tags, and audit logs all carry OrganizationId to ensure tenant isolation.
- TicketTag supports many-to-many tagging for tickets.

## 4. Implementation Status
### Completed
- Clean Architecture structure for Domain, Application, Infrastructure, and API layers
- Multi-tenant entities and base abstractions
- EF Core tenant-aware persistence behavior
- Organization creation and workspace overview flow
- Project creation and list endpoints
- Ticket creation and list endpoints
- Product documentation artifact

### Next Planned Enhancements
- Ticket comments and audit activity feed
- Member invitation and role management
- Dashboard metrics and reporting widgets
- Angular integration for projects and tickets screens

## 5. Verification
The backend implementation was validated by building the solution and resolving the remaining compile issues introduced by the new endpoint additions.
