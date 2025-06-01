import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CurrentDateService } from '../../services/current-date-service';
import { CurrentDate } from '../../models/current-date.model';

@Component({
  selector: 'app-year-page',
  imports: [],
  templateUrl: './year-page.html',
  styleUrl: './year-page.scss'
})
export class YearPage {

  constructor(private activatedRoute : ActivatedRoute, public currentDateService : CurrentDateService) {
    this.activatedRoute.params.subscribe((params) => {
        this.currentDateService.currentDate.set(new CurrentDate(params['selectedYear']));
    })
  }


}
