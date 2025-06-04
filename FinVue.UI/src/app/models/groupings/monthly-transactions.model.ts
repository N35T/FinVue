import { Injectable } from "@angular/core";
import { TransactionsByCategory, TransactionsByCategoryAdapter } from "./transactions-by-category.model";
import { Adapter } from "../adapter.model";

export class MonthlyTransactions {

    constructor(public incomeTransactions : TransactionsByCategory[], public outcomeTransactions : TransactionsByCategory[]) {}
}

@Injectable({
  providedIn: "root",
})
export class MonthlyTransactionsAdapter implements Adapter<MonthlyTransactions> {

    constructor(
        private transactionAdapter : TransactionsByCategoryAdapter
    ) {}

    adapt (item: any): MonthlyTransactions {
        if (!item) 
            throw new Error("Can't map item to model " + item);
        return new MonthlyTransactions(
            item.incomeTransactions.map((e:any) => this.transactionAdapter.adapt(e)),
            item.outcomeTransactions.map((e:any) => this.transactionAdapter.adapt(e))
        );
    }
}