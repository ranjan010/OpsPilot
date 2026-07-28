import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeRoutingModule } from './home-routing.module';
import { HomeComponent } from './home.component';

@NgModule({
  declarations: [],
  imports: [CommonModule, HomeRoutingModule, HomeComponent],
  exports: []
})
export class HomeModule {}
