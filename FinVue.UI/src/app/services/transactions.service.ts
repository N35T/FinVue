import { Injectable } from "@angular/core";
import { ApiService } from "./api.service";
import { Observable } from "rxjs";
import { MonthlyTransactions } from "../models/groupings/monthly-transactions.model";
import { RecurringTransaction } from "../models/entities/recurring-transaction.model";

@Injectable({
    providedIn: 'root'
})
export class CurrentDateService {

    constructor(private apiService : ApiService) {}

    public getTransactionOfMonth(year: number, month: number) : Observable<MonthlyTransactions> {
        return this.apiService.get<MonthlyTransactions>('/transactions/ofMonth?year=' + year + '&month=' + month);
    }

    public getRecurringTransactionsOfMonth(year: number, month: number) : Observable<RecurringTransaction[]> {
        return this.apiService.get<RecurringTransaction[]>('/transactions/recurring/ofMonth');
    }
}