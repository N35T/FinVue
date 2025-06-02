export class ProfitCardModel {

    constructor(public total: number, public average?: number) {}

    static defaultModel() : ProfitCardModel {
        return new ProfitCardModel(0,0)
    }
}