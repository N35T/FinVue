export class CurrentDate {

    public year: number;
    public month?: number;

    constructor(year?: number, month? : number) {
        if(!year) {
            year = new Date().getFullYear();
        }
        this.year = year;
        this.month = month;
    }
}