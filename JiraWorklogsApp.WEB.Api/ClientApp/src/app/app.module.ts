import { JiraConnectionsService } from './services/jira-connections.service';
import * as $ from 'jquery';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CommonModule, DatePipe } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { CustomFormsModule } from 'ngx-custom-validators';
import { NgHttpLoaderModule } from 'ng-http-loader';
import { ToastrModule } from 'ngx-toastr';

import { BlankComponent } from './components/blank/blank.component';
import { SIDEBAR_TOGGLE_DIRECTIVES } from './components/shared/sidebar.directive';
import { NavigationComponent } from './components/shared/header-navigation/navigation.component';
import { SidebarComponent } from './components/shared/sidebar/sidebar.component';
import { RightSidebarComponent } from './components/shared/right-sidebar/rightsidebar.component';
import { BreadcrumbComponent } from './components/shared/breadcrumb/breadcrumb.component';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SpinnerComponent } from './components/shared/spinner.component';
import { HomeComponent } from './components/home/home.component';
import { SettingsComponent } from './components/settings/settings.component';
import { ValidationErrorComponent } from './components/validation-error/validation-error.component';
import { ModalModule } from 'ngx-bootstrap/modal';
import { ReportComponent } from './components/report/report.component';
import { ReportService } from './services/report.service';
import { AdalService, AdalGuard, AdalInterceptor } from 'adal-angular4';

@NgModule({
  declarations: [
    AppComponent,
    SpinnerComponent,
    BlankComponent,
    NavigationComponent,
    BreadcrumbComponent,
    SidebarComponent,
    RightSidebarComponent,
    SIDEBAR_TOGGLE_DIRECTIVES,
    HomeComponent,
    SettingsComponent,
    ValidationErrorComponent,
    ReportComponent
  ],
  imports: [
    CommonModule,
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    HttpClientModule,
    AppRoutingModule,
    ReactiveFormsModule,
    Ng2SmartTableModule,
    ModalModule.forRoot(),
    BsDatepickerModule.forRoot(),
    CustomFormsModule,
    NgHttpLoaderModule.forRoot(),
    ToastrModule.forRoot()
  ],
  providers: [
    JiraConnectionsService,
    ReportService,
    DatePipe,
    AdalService, AdalGuard,
    { provide: HTTP_INTERCEPTORS, useClass: AdalInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
