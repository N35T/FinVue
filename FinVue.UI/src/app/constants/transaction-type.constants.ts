import { Injectable } from "@angular/core";
import { Adapter } from "../models/adapter.model";

export enum TransactionType {
    Einkommen = "Einkommen", 
    Ausgaben = "Ausgaben"
}


@Injectable({
  providedIn: "root",
})
export class TransactionTypeAdapter implements Adapter<TransactionType> {

    adapt (item: any): TransactionType {
        if(item == 0) return TransactionType.Einkommen;
        else if(item == 1) return TransactionType.Ausgaben;
        else throw new Error("Cannot map Transaction type " + item);
    }
}