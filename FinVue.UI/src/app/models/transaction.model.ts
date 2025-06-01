import { PaymentMethod } from "../constants/payment-method.constants";
import { TransactionType } from "../constants/transaction-type.constants";
import { User } from "./user.model";

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