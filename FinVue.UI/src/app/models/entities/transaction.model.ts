import { Injectable } from "@angular/core";
import { PaymentMethod, PaymentMethodAdapter } from "../../constants/payment-method.constants";
import { TransactionType, TransactionTypeAdapter } from "../../constants/transaction-type.constants";
import { User, UserAdapter } from "./user.model";
import { Adapter } from "../adapter.model";

export class Transaction {

    constructor(
        public id : string,
        public name : string,
        public valueInCent : number,
        public creationDate : Date,
        public payDate : Date,
        public type : TransactionType,
        public paymentMethod : PaymentMethod,
        public payingUser : User,
        public creationUser? : User,
    ) {}
}

@Injectable({
  providedIn: "root",
})
export class TransactionAdapter implements Adapter<Transaction> {

    constructor(
        private transactionTypeAdapter : TransactionTypeAdapter,
        private paymentMethodAdapter : PaymentMethodAdapter,
        private userAdapter : UserAdapter
    ) {}

    adapt (item: any): Transaction {
        if (!item) 
            throw new Error("Can't map item to model " + item);
        return new Transaction(
            item.id,
            item.name,
            item.valueInCent,
            item.creationDate,
            item.payDate,
            this.transactionTypeAdapter.adapt(item.type),
            this.paymentMethodAdapter.adapt(item.paymentMethod),
            this.userAdapter.adapt(item.payingUser),
            item.creationUser ? this.userAdapter.adapt(item.creationUser) : undefined
        );
    }
}