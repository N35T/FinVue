import { Component, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CurrentDateService } from '../../services/current-date-service';
import { CurrentDate } from '../../models/ui/current-date.model';

@Component({
  selector: 'app-month-page',
  imports: [],
  templateUrl: './month-page.html',
  styleUrl: './month-page.scss'
})
export class MonthPage {

  constructor(private activatedRoute : ActivatedRoute, public currentDateService: CurrentDateService) {
    this.activatedRoute.params.subscribe((params) => {
        this.currentDateService.currentDate.set(new CurrentDate(params['selectedYear'], params['selectedMonth']));
    })
  }
}
