import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { WorkspaceSetupComponent } from './workspace-setup.component';

const routes: Routes = [
  {
    path: '',
    component: WorkspaceSetupComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class WorkspaceSetupRoutingModule {}
