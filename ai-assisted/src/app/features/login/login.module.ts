import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { LoginRoutingModule } from './login-routing.module';
import { LoginComponent } from './login.component';

@NgModule({
  declarations: [],
  imports: [CommonModule, ReactiveFormsModule, LoginRoutingModule, LoginComponent],
  exports: []
})
export class LoginModule {}
