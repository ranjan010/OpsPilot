import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home.component';
import { authGuard } from '../../core/guards/auth.guard';
import { ROUTE_PATHS, buildRoutePath } from '../../core/constants/routes';

const routes: Routes = [
  {
    path: ROUTE_PATHS.home,
    component: HomeComponent,
    canActivate: [authGuard]
  },
  {
    path: buildRoutePath(ROUTE_PATHS.orders),
    component: HomeComponent,
    canActivate: [authGuard]
  },
  {
    path: buildRoutePath(ROUTE_PATHS.orders, ROUTE_PATHS.orderDetails),
    component: HomeComponent,
    canActivate: [authGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class HomeRoutingModule {}
