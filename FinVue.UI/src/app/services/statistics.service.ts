import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { ApiService } from './api.service';
import { YearlyTransactionStatistics, YearlyTransactionStatisticsAdapter } from '../models/groupings/yearly-transaction-statistics.models';
import { MonthlyTransactionStatistics, MonthlyTransactionStatisticsAdapter } from '../models/groupings/monthly-transaction-statistics.models';

@Injectable({
  providedIn: 'root'
})
export class StatisticService {

    constructor(
        private apiService : ApiService, 
        private yearStatsAdapter : YearlyTransactionStatisticsAdapter,
        private monthStatsAdapter : MonthlyTransactionStatisticsAdapter
    ) {}

    public getYearlyStatistics(year: number) : Observable<YearlyTransactionStatistics> {
        return this.apiService.get<YearlyTransactionStatistics>('/statistics/byYear?year=' + year)
            .pipe(map(e => this.yearStatsAdapter.adapt(e)));
    }

    public getMonthlyStatistics(year: number, month: number) : Observable<MonthlyTransactionStatistics> {
        return this.apiService.get<MonthlyTransactionStatistics>('/statistics/byMonth?year=' + year + '&month='+ month)
            .pipe(map(e => this.monthStatsAdapter.adapt(e)));
    }

    public getYearOfOldestTransaction() : Observable<number> {
        return this.apiService.get<number>('/statistics/oldest');
    }

}
