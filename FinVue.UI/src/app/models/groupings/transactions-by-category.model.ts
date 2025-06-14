import { Injectable } from "@angular/core";
import { Adapter } from "../adapter.model";
import { Transaction, TransactionAdapter } from "../entities/transaction.model";

export class TransactionsByCategory {

    constructor(public categoryName : string, public transactions : Transaction[], public totalSum : number, public categoryColor : string) {}
}

@Injectable({
  providedIn: "root",
})
export class TransactionsByCategoryAdapter implements Adapter<TransactionsByCategory> {

    constructor(
        private transactionAdapter : TransactionAdapter
    ) {}

    adapt (item: any): TransactionsByCategory {
        if (!item) 
            throw new Error("Can't map item to model " + item);
        return new TransactionsByCategory(
            item.categoryName,
            item.transactions.map((e : any) => this.transactionAdapter.adapt(e)),
            item.totalSum,
            item.categoryColor
        );
    }
}