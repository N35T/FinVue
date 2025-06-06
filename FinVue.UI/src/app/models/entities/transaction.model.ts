import { Injectable } from "@angular/core";
import { PaymentMethod, PaymentMethodAdapter } from "../../constants/payment-method.constants";
import { TransactionType, TransactionTypeAdapter } from "../../constants/transaction-type.constants";
import { User, UserAdapter } from "./user.model";
import { Adapter } from "../adapter.model";
import { Category, CategoryAdapter } from "./category.model";
import { DateOnly, DateOnlyAdapter } from "./date.model";

export class Transaction {

    constructor(
        public id : string,
        public name : string,
        public valueInCent : number,
        public creationDate : Date,
        public payDate : DateOnly,
        public type : TransactionType,
        public paymentMethod : PaymentMethod,
        public payingUser : User,
        public creationUser? : User,
        public category? : Category,
        public recurringTransactionId? : string
    ) {}
}

@Injectable({
  providedIn: "root",
})
export class TransactionAdapter implements Adapter<Transaction> {

    constructor(
        private transactionTypeAdapter : TransactionTypeAdapter,
        private paymentMethodAdapter : PaymentMethodAdapter,
        private userAdapter : UserAdapter,
        private categoryAdapter : CategoryAdapter,
        private dateOnlyAdapter : DateOnlyAdapter
    ) {}

    adapt (item: any): Transaction {
        if (!item) 
            throw new Error("Can't map item to model " + item);
        return new Transaction(
            item.id,
            item.name,
            item.valueInCent,
            item.creationDate,
            this.dateOnlyAdapter.adapt(item.payDate),
            this.transactionTypeAdapter.adapt(item.type),
            this.paymentMethodAdapter.adapt(item.paymentMethod),
            this.userAdapter.adapt(item.payingUser),
            item.creationUser ? this.userAdapter.adapt(item.creationUser) : undefined,
            item.category ? this.categoryAdapter.adapt(item.category) : undefined,
            item.recurringTransactionId
        );
    }
}