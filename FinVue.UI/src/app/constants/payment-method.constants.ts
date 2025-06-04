import { Injectable } from "@angular/core";
import { Adapter } from "../models/adapter.model";

export enum PaymentMethod {
    Bar = "Bar", Girokarte = "Girokarte", Kreditkarte = "Kreditkarte", Überweisung = "Überweisung", PayPal = "PayPal"
}

@Injectable({
  providedIn: "root",
})
export class PaymentMethodAdapter implements Adapter<PaymentMethod> {

    adapt (item: any): PaymentMethod {
        if(item == 0) return PaymentMethod.Bar;
        else if(item == 1) return PaymentMethod.Girokarte;
        else if(item == 2) return PaymentMethod.Kreditkarte;
        else if(item == 3) return PaymentMethod.Überweisung;
        else if(item == 4) return PaymentMethod.PayPal;
        else throw new Error("Cannot map Payment method type " + item);
    }
}