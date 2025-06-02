import { TransactionsByCategory } from "./transactions-by-category.model";

export class MonthlyTransactions {

    constructor(public incomeTransactions : TransactionsByCategory[], public outcomeTransactions : TransactionsByCategory[]) {}
}