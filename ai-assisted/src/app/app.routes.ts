import { Routes } from '@angular/router';
import { LayoutComponent } from './shared/layout.component';
import { ROUTE_PATHS } from './core/constants/routes';
import { authGuard } from './core/guards/auth.guard';
import { tenantGuard } from './core/guards/tenant.guard';

export const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    canActivate: [authGuard, tenantGuard],
    children: [
      {
        path: '',
        loadChildren: () => import('./features/home/home.module').then((m) => m.HomeModule),
        pathMatch: 'full'
      },
      {
        path: 'projects',
        loadComponent: () => import('./features/projects/projects.component').then((m) => m.ProjectsComponent)
      },
      {
        path: 'tickets',
        loadComponent: () => import('./features/tickets/tickets.component').then((m) => m.TicketsComponent)
      },
      {
        path: 'kanban',
        loadChildren: () => import('./features/kanban/kanban.module').then((m) => m.KanbanModule)
      },
      {
        path: 'members',
        loadChildren: () => import('./features/members/members.module').then((m) => m.MembersModule)
      },
      {
        path: 'settings',
        loadComponent: () => import('./features/settings/settings.component').then((m) => m.SettingsComponent)
      }
    ]
  },
  {
    path: 'workspace/setup',
    canActivate: [authGuard],
    loadChildren: () => import('./features/workspace/workspace-setup.module').then((m) => m.WorkspaceSetupModule)
  },
  {
    path: ROUTE_PATHS.login,
    loadChildren: () => import('./features/login/login.module').then((m) => m.LoginModule)
  },
  {
    path: ROUTE_PATHS.register,
    loadChildren: () => import('./features/register/register.module').then((m) => m.RegisterModule)
  },
  {
    path: '**',
    redirectTo: ROUTE_PATHS.login
  }
];
