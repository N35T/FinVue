import { CommonModule } from '@angular/common';
import { Component, Input, signal } from '@angular/core';
import { CurrentDateService } from '../../services/current-date-service';
import { RouterModule } from '@angular/router';
import { MONTHS } from '../../constants/month.constants';


@Component({
  selector: 'app-side-nav',
  imports: [CommonModule, RouterModule],
  templateUrl: './side-nav.html',
  styleUrl: './side-nav.scss'
})
export class SideNav {
  
  public monthNames = MONTHS;

  get currentYear() { return this.currentDateService.currentDate().year; }
  get currentMonth() { return this.currentDateService.currentDate().month; }

  constructor(public currentDateService: CurrentDateService) {}


  getMonthClasses(monthName:string) {
    let classList = "";
    const now = new Date();
    const monthIndex = this.monthNames.indexOf(monthName);
    if (this.currentYear > now.getFullYear() || this.currentYear == now.getFullYear() && monthIndex > now.getMonth()) {
        classList += "low-opac";
    }
    if (this.currentMonth && monthIndex + 1 == this.currentMonth) {
        classList += " color-primary";
    }
    return classList;
  }

  buildLink(monthName?:string) {
    if (!monthName) {
      return "/year/" + this.currentYear;
    }
    const monthIndex = this.monthNames.indexOf(monthName);
    return "/year/" + this.currentYear + "/month/" + (monthIndex + 1);
  }
}
