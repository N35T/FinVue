import { Transaction } from "../entities/transaction.model";

export class TransactionsByCategory {

    constructor(public categoryName : string, public transactions : Transaction[], public totalSum : number, public categoryColor : string) {}
}