import { Component, OnInit, isDevMode } from '@angular/core';
import { AdalService } from 'adal-angular4';
import { Spinkit } from 'ng-http-loader';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'app';

  private azureAdConfig = {
    tenant: 'teland01mail.onmicrosoft.com',
    clientId: '3907a040-eb02-4ddd-aa8c-da94e79a21ce',
    cacheLocation: 'localStorage'
  };

  public spinkit = Spinkit;

  constructor(private adalService: AdalService) {
    this.adalService.init(this.azureAdConfig);
  }

  ngOnInit(): void {
    this.adalService.handleWindowCallback();

    if (!this.adalService.userInfo.authenticated || !this.adalService.userInfo.loginCached) {
      this.adalService.login();
    }
  }
}
