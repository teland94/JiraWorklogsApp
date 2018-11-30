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
    tenant: 'teland94hotmail.onmicrosoft.com',
    clientId: '35106b69-8750-44d2-a181-73451ec96464',
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
