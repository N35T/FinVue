export function numberToCurrency(num: any, noComma = false) {
    return Intl.NumberFormat('de-DE',{currency:"EUR", style:"currency", maximumFractionDigits: noComma ? 0 : 2}).format(num);
}