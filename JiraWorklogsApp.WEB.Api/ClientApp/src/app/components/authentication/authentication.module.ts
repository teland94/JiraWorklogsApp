import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';

import { NotFoundComponent } from './404/not-found.component';
import { LoginComponent } from './login/login.component';
import { SignupComponent } from './signup/signup.component';

import { AuthenticationRoutes } from './authentication.routing';


@NgModule({
  imports: [ 
    CommonModule,
    RouterModule.forChild(AuthenticationRoutes)
  ],
  declarations: [
    NotFoundComponent,
    LoginComponent,
    SignupComponent
  ]
})

export class AuthenticationModule {}
