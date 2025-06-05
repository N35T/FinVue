import { Injectable } from "@angular/core";
import { Adapter } from "../models/adapter.model";

export enum PaymentMethod {
    Bar = 0, Girokarte = 1, Kreditkarte = 2, Überweisung = 3, PayPal = 4
}

export const _PAYMENT_METHOD = ["Bar", "Girokarte", "Kreditkarte", "Überweisung", "PayPal"];

export function paymentMethodToString(method : PaymentMethod) : string{
    return _PAYMENT_METHOD[method];
}

export function stringToPaymentMethod(type : string) : PaymentMethod {
    const i = _PAYMENT_METHOD.indexOf(type);
    return i === -1 ? PaymentMethod.Überweisung : i
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