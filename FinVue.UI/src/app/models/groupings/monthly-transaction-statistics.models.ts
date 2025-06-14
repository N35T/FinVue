import { Injectable } from "@angular/core";
import { Adapter } from "../adapter.model";

export class MonthlyTransactionStatistics {

    constructor(public totalIncome : number, public totalOutcome : number) {}
    
}

@Injectable({
  providedIn: "root",
})
export class MonthlyTransactionStatisticsAdapter implements Adapter<MonthlyTransactionStatistics> {

    adapt (item: any): MonthlyTransactionStatistics {
        if (!item) 
            throw new Error("Can't map item to model " + item);
        return new MonthlyTransactionStatistics(
            item.totalIncome,
            item.totalOutcome
        );
    }
}