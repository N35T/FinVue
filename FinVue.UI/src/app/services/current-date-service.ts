import { Injectable, signal } from '@angular/core';
import { CurrentDate } from '../models/current-date.model';

@Injectable({
  providedIn: 'root'
})
export class CurrentDateService {

  public currentDate = signal(new CurrentDate());

}
