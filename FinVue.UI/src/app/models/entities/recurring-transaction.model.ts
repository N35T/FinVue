import { Injectable } from "@angular/core";
import { TransactionType, TransactionTypeAdapter } from "../../constants/transaction-type.constants";
import { Adapter } from "../adapter.model";
import { Category, CategoryAdapter } from "./category.model";

export class RecurringTransaction {

    constructor(
        public id: string,
        public name : string,
        public valueInCent : number,
        public monthFrequency : number,
        public type : TransactionType,
        public payedThisMonth : boolean,
        public category? : Category
    ) {}
}

@Injectable({
  providedIn: "root",
})
export class RecurringTransactionAdapter implements Adapter<RecurringTransaction> {

    constructor(
        private transactionTypeAdapter : TransactionTypeAdapter,
        private categoryAdapter : CategoryAdapter
    ) {}

    adapt (item: any): RecurringTransaction {
        if (!item) 
            throw new Error("Can't map item to model " + item);
        return new RecurringTransaction(
            item.id,
            item.name,
            item.valueInCent,
            item.monthFrequency,
            this.transactionTypeAdapter.adapt(item.type),
            item.payedThisMonth,
            item.category ? this.categoryAdapter.adapt(item.category) : undefined
        );
    }
}