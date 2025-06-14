import { Injectable } from "@angular/core";
import { Adapter } from "../adapter.model";

export class DateOnly {

    private _date : Date;

    constructor(date : string | Date) {
        if(typeof(date) === 'string') {
            this._date = new Date(date);
        }else {
            this._date = date;
        }
    }

    static fromDate(date : Date) : DateOnly {
        return new DateOnly(date);
    }

    public toString() : string  {
        return this._date.getFullYear() + "-" + ((this._date.getMonth() + 1) + "").padStart(2, '0') + "-" + (this._date.getDate() + "").padStart(2, '0');
    }

    toJSON(): string {
        return this.toString();
    }
}

@Injectable({
    providedIn: "root",
  })
  export class DateOnlyAdapter implements Adapter<DateOnly> {
  
    adapt (item: any): DateOnly {
        if (!item) 
            throw new Error("Can't map item to model " + item);
        // Expected format: 'yyyy-mm-dd'
        return new DateOnly(item);
    }
  }