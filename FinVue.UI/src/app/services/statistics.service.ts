import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { YearlyTransactionStatistics } from '../models/yearly-transaction-statistics.models';
import { MonthlyTransactionStatistics } from '../models/monthly-transaction-statistics.models';

@Injectable({
  providedIn: 'root'
})
export class StatisticService {

    constructor(private apiService : ApiService) {}

    public getYearlyStatistics(year: number) : Observable<YearlyTransactionStatistics> {
        return this.apiService.get<YearlyTransactionStatistics>('/statistics/byYear?year=' + year);
    }

    public getMonthlyStatistics(year: number, month: number) : Observable<MonthlyTransactionStatistics> {
        return this.apiService.get<MonthlyTransactionStatistics>('/statistics/byMonth?year=' + year + '&month='+ month);
    }

}
