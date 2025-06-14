import { Injectable } from "@angular/core";
import { Adapter } from "../adapter.model";

export class SumByCategory {

    constructor(public categoryName : string, public totalSum : number, public categoryColor: string) {}
}


@Injectable({
  providedIn: "root",
})
export class SumByCategoryAdapter implements Adapter<SumByCategory> {

    adapt (item: any): SumByCategory {
        if (!item) 
            throw new Error("Can't map item to model " + item);
        return new SumByCategory(item.categoryName, item.totalSum, item.categoryColor);
    }
}