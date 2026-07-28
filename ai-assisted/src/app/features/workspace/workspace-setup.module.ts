import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { WorkspaceSetupRoutingModule } from './workspace-setup-routing.module';
import { WorkspaceSetupComponent } from './workspace-setup.component';

@NgModule({
  declarations: [WorkspaceSetupComponent],
  imports: [CommonModule, ReactiveFormsModule, WorkspaceSetupRoutingModule],
  exports: [WorkspaceSetupComponent]
})
export class WorkspaceSetupModule {}
