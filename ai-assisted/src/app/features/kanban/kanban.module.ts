import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { KanbanComponent } from './kanban.component';
import { KanbanRoutingModule } from './kanban-routing.module';

@NgModule({
  imports: [CommonModule, KanbanRoutingModule, KanbanComponent]
})
export class KanbanModule {}
