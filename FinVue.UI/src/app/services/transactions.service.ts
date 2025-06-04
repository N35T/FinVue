import { Injectable } from "@angular/core";
import { ApiService } from "./api.service";
import { map, Observable } from "rxjs";
import { MonthlyTransactions, MonthlyTransactionsAdapter } from "../models/groupings/monthly-transactions.model";
import { RecurringTransaction, RecurringTransactionAdapter } from "../models/entities/recurring-transaction.model";

@Injectable({
    providedIn: 'root'
})
export class TransactionService {

    constructor(
        private apiService : ApiService,
        private monthlyTransactionsAdapter : MonthlyTransactionsAdapter,
        private recurringTransactionsAdapter : RecurringTransactionAdapter
    ) {}

    public getTransactionOfMonth(year: number, month: number) : Observable<MonthlyTransactions> {
        return this.apiService.get<MonthlyTransactions>('/transactions/ofMonth?year=' + year + '&month=' + month)
            .pipe(map(e => this.monthlyTransactionsAdapter.adapt(e)));
    }

    public getRecurringTransactionsOfMonth(year: number, month: number) : Observable<RecurringTransaction[]> {
        return this.apiService.get<RecurringTransaction[]>('/transactions/recurring/ofMonth?year=' + year + "&month=" + month)
            .pipe(map(item => item.map(e => this.recurringTransactionsAdapter.adapt(e))));
    }
}