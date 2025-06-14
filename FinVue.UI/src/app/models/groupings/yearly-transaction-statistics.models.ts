import { Injectable } from "@angular/core";
import { Adapter } from "../adapter.model";
import { SumByCategory, SumByCategoryAdapter } from "./sum-by-category.models";


export class YearlyTransactionStatistics {

    constructor(public incomePerMonth : number[], public outcomePerMonth : number[], public outcomeByCategory: SumByCategory[]) {}


    private incomeSum? : number;
    private incomeAvg? : number;

    private outcomeSum? : number;
    private outcomeAvg? : number;

    private profitByMonth? : number[];
    private profitSum? : number;
    private profitAvg? : number;

    private cumulatedProfitByMonth? : number[]

    public getTotalIncome() : number {
        return this.incomeSum ??= this.incomePerMonth.reduce((partialSum, a) => partialSum + a, 0);
    }
    public getAverageIncome() : number {
        return this.incomeAvg ??= this.getTotalIncome() / this.incomePerMonth.length;
    }

    public getTotalOutcome() : number {
        return this.outcomeSum ??= this.outcomePerMonth.reduce((partialSum, a) => partialSum + a, 0);
    }
    public getAverageOutcome() : number {
        return this.outcomeAvg ??= this.getTotalOutcome() / this.outcomePerMonth.length;
    }

    public getProfitByMonth() : number[] {
        if(this.profitByMonth) {
            return this.profitByMonth;
        }
        this.profitByMonth = Array(this.incomePerMonth.length);
        for(let i = 0; i < this.incomePerMonth.length; ++i)  {
            this.profitByMonth[i] = this.incomePerMonth[i]-this.outcomePerMonth[i];
        }
        return this.profitByMonth;
    }

    public getTotalProfit() : number {
        return this.profitSum ??= this.getProfitByMonth().reduce((partialSum, a) => partialSum + a, 0);
    }
    public getAverageProfit() : number {
        return this.profitAvg ??= this.getTotalProfit() / this.incomePerMonth.length;
    }

    public getCumulatedProfitByMonth() : number[] {
        if(this.cumulatedProfitByMonth) {
            return this.cumulatedProfitByMonth;
        }
        this.cumulatedProfitByMonth = Array(this.incomePerMonth.length);
        this.cumulatedProfitByMonth[0] = this.incomePerMonth[0] - this.outcomePerMonth[0];
        for(let i = 1; i < this.incomePerMonth.length; ++i)  {
            this.cumulatedProfitByMonth[i] = this.cumulatedProfitByMonth[i-1] + this.incomePerMonth[i]-this.outcomePerMonth[i];
        }
        return this.cumulatedProfitByMonth;
    }
}

@Injectable({
  providedIn: "root",
})
export class YearlyTransactionStatisticsAdapter implements Adapter<YearlyTransactionStatistics> {
    constructor(
        private sumByCategoryAdapter : SumByCategoryAdapter
    ) { }

    adapt (item: any): YearlyTransactionStatistics {
        if (!item) 
            throw new Error("Can't map item to model " + item);
        return new YearlyTransactionStatistics(
            item.incomePerMonth, 
            item.outcomePerMonth, 
            item.outcomeByCategory.map((e:any) => this.sumByCategoryAdapter.adapt(e)));
    }
}