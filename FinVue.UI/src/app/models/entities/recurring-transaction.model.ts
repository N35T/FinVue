import { TransactionType } from "../../constants/transaction-type.constants";

export class RecurringTransaction {

    constructor(
        public id: string,
        public name : string,
        public valueInCent : number,
        public monthFrequency : number,
        public type : TransactionType,
        public payedThisMonth : boolean,
    ) {}
}