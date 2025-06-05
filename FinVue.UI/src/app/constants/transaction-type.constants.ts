import { Injectable } from "@angular/core";
import { Adapter } from "../models/adapter.model";

export enum TransactionType {
    Einkommen = 0, 
    Ausgaben = 1
}
export const _TRANSACTION_TYPES = ["Einkommen", "Ausgaben"];

export function transactionTypeToString(type : TransactionType) : string {
    return _TRANSACTION_TYPES[type];
}

export function stringToTransactionType(type : string) : TransactionType {
    const i = _TRANSACTION_TYPES.indexOf(type);
    return i === -1 ? TransactionType.Ausgaben : i
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