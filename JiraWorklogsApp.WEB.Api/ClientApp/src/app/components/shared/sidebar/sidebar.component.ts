import { Component, AfterViewInit, OnInit } from '@angular/core';
import { AdalService } from 'adal-angular4';

@Component({
  selector: 'ap-sidebar',
  templateUrl: './sidebar.component.html'

})
export class SidebarComponent implements AfterViewInit {
  //this is for the open close
  isActive: boolean = true;
  showMenu: string = '';
  showSubMenu: string = '';

  constructor(private adalService: AdalService) {

  }

  addExpandClass(element: any) {
    if (element === this.showMenu) {
      this.showMenu = '0';
    } else {
      this.showMenu = element;
    }
    this.hideSidebar();
  }
  addActiveClass(element: any) {
    if (element === this.showSubMenu) {
      this.showSubMenu = '0';
    } else {
      this.showSubMenu = element;
    }
  }
  eventCalled() {
    this.isActive = !this.isActive;

  }
  // End open close
  ngAfterViewInit() {
    $(function () {

      $(".sidebartoggler").on('click', function () {
        if ($("body").hasClass("mini-sidebar")) {
          $("body").trigger("resize");
          $(".scroll-sidebar, .slimScrollDiv").css("overflow", "hidden").parent().css("overflow", "visible");
          $("body").removeClass("mini-sidebar");
          $('.navbar-brand span').show();
          //$(".sidebartoggler i").addClass("ti-menu");
        }
        else {
          $("body").trigger("resize");
          $(".scroll-sidebar, .slimScrollDiv").css("overflow-x", "visible").parent().css("overflow", "visible");
          $("body").addClass("mini-sidebar");
          $('.navbar-brand span').hide();
          //$(".sidebartoggler i").removeClass("ti-menu");
        }
      });

    });
  }

  logout() {
    this.adalService.logOut();
  }

  private hideSidebar() {
    document.querySelector('body').classList.toggle('show-sidebar');
    document.querySelector('.nav-toggler i').classList.toggle('ti-menu');
  }
}
