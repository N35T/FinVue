import { Component, OnInit, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CurrentDateService } from '../../services/current-date-service';
import { CurrentDate } from '../../models/ui/current-date.model';
import { MONTHS } from '../../constants/month.constants';
import { ProfitCard } from '../../components/profit-card/profit-card';
import { TransactionTable } from '../../components/transaction-table/transaction-table';
import { ProfitCardModel } from '../../models/ui/profit-card.model';
import { BehaviorSubject, Subject } from 'rxjs';
import { AsyncPipe, CommonModule } from '@angular/common';
import { RecurringTransaction } from '../../models/entities/recurring-transaction.model';
import { TransactionsByCategory } from '../../models/groupings/transactions-by-category.model';
import { TransactionService } from '../../services/transactions.service';
import { MonthlyTransactions } from '../../models/groupings/monthly-transactions.model';
import { StatisticService } from '../../services/statistics.service';
import { MonthlyTransactionStatistics } from '../../models/groupings/monthly-transaction-statistics.models';

@Component({
  selector: 'app-month-page',
  imports: [ProfitCard, TransactionTable, AsyncPipe, CommonModule],
  templateUrl: './month-page.html',
  styleUrl: './month-page.scss'
})
export class MonthPage {

  public PLANNED_MODE = TransactionTable.PLANNED_MODE;

  public incomeProfitCard: BehaviorSubject<ProfitCardModel> = new BehaviorSubject(this.getDefaultProfitCardModel());
  public outcomeProfitCard : BehaviorSubject<ProfitCardModel> = new BehaviorSubject(this.getDefaultProfitCardModel());
  public profitProfitCard : BehaviorSubject<ProfitCardModel> = new BehaviorSubject(this.getDefaultProfitCardModel());

  public recurringLoading = new BehaviorSubject<boolean>(true);
  public recurringTransactions : BehaviorSubject<RecurringTransaction[]> = new BehaviorSubject<RecurringTransaction[]>([]);;
  public incomeLoading = new BehaviorSubject<boolean>(true);
  public incomeTransactions : BehaviorSubject<TransactionsByCategory[]> = new BehaviorSubject<TransactionsByCategory[]>([]);
  public outcomeLoading = new BehaviorSubject<boolean>(true);
  public outcomeTransactions : BehaviorSubject<TransactionsByCategory[]> = new BehaviorSubject<TransactionsByCategory[]>([]);

  constructor(private activatedRoute : ActivatedRoute, public currentDateService: CurrentDateService, private transactionService : TransactionService, private statsService : StatisticService) {
    this.activatedRoute.params.subscribe((params) => {
        this.currentDateService.currentDate.set(new CurrentDate(params['selectedYear'], params['selectedMonth']));
        this.fetchData();
    })
  }
  
  private fetchData() {
    this.transactionService.getRecurringTransactionsOfMonth(this.currentDateService.currentDate().year, this.currentDateService.currentDate().month!)
      .subscribe((e : RecurringTransaction[]) => {
        this.recurringTransactions.next(e);
        this.recurringLoading.next(false);
      });

    this.transactionService.getTransactionOfMonth(this.currentDateService.currentDate().year, this.currentDateService.currentDate().month!)
      .subscribe((e : MonthlyTransactions) => {
        this.incomeTransactions.next(e.incomeTransactions);
        this.incomeLoading.next(false);
        this.outcomeTransactions.next(e.outcomeTransactions);
        this.outcomeLoading.next(false);
      })
    
    this.statsService.getMonthlyStatistics(this.currentDateService.currentDate().year, this.currentDateService.currentDate().month!)
      .subscribe((e : MonthlyTransactionStatistics) => {
        this.updateProfitCards(e);
      })
  }

  private updateProfitCards(stats : MonthlyTransactionStatistics) {
    this.incomeProfitCard.next(new ProfitCardModel(stats.totalIncome));
    this.outcomeProfitCard.next(new ProfitCardModel(stats.totalOutcome));
    this.profitProfitCard.next(new ProfitCardModel(stats.totalIncome - stats.totalOutcome));
  }

  public getTitle() : string {
    const year = this.currentDateService.currentDate().year;
    const month = this.currentDateService.currentDate().month;
    if(!month || month > 12 || month < 1) {
      return year + "";
    }
    return MONTHS[month - 1] + " - " + year;
  }

  private activeTables = new Map<string, boolean>();
  public switchTableTo(name : string) {
    for(const key of this.activeTables.keys()) {
      this.activeTables.set(key, false);
    }
    this.activeTables.set(name, true);
  }
  public isTableActive(name : string, defaultVal: boolean = false) : boolean {
    if(this.activeTables.get(name) === undefined) {
      this.activeTables.set(name, defaultVal);
    }
    return this.activeTables.get(name)!;
  }

  public getDefaultProfitCardModel() {
    return new ProfitCardModel(0);
  }
}
