import { HomeComponent } from './components/home/home.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { LayoutComponent } from './components/layout/layout.component';
import { SettingsComponent } from './components/settings/settings.component';
import { ReportComponent } from './components/report/report.component';
import { AdalGuard } from 'adal-angular4';

export const routes: Routes = [
{
    path: '',
    component: LayoutComponent,
    children: [
        { path: '', redirectTo: '/home', pathMatch: 'full' },
        {
          path: 'home',
          component: HomeComponent,
          data: {
            title: 'Home Page',
            canActivate: [AdalGuard],
            urls: [{ title: 'Home', url: '/home' }, { title: 'Home Page' }]
          }
        },
        {
          path: 'report',
          component: ReportComponent,
          data: {
            title: 'Reports',
            canActivate: [AdalGuard],
            urls: [{ title: 'Reports', url: '/report' }, { title: 'Reports' }]
          }
        },
        {
          path: 'settings',
          component: SettingsComponent,
          data: {
            title: 'Settings',
            canActivate: [AdalGuard],
            urls: [{ title: 'Settings', url: '/settings' }, { title: 'Settings' }]
          }
        }
    ]
},
{
    path: '**',
    redirectTo: '404'
}];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }

