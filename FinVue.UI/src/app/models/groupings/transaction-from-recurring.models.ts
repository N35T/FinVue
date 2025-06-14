import { DateOnly } from "../entities/date.model";

export class TransactionFromRecurring {

    constructor(public recurringTransactionId: string, public payDate : DateOnly) {}
}