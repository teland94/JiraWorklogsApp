import { AdalService } from 'adal-angular4';
import { Component, AfterViewInit, OnInit, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html'
})
export class NavigationComponent implements OnInit {
  name: string;
  showHide: boolean;

  @Output()
  toggleSidebar = new EventEmitter<void>();

  constructor(private adalService: AdalService) {
    this.showHide = true;
  }

  ngOnInit() {

  }

  changeShowStatus() {
    this.showHide = !this.showHide;
  }

  logout() {
    this.adalService.logOut();
  }
}
