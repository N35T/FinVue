import { Component, Input } from '@angular/core';
import { ProfitCardModel } from '../../models/ui/profit-card.model';
import { numberToCurrency } from '../../services/currency.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-profit-card',
  imports: [CommonModule],
  templateUrl: './profit-card.html',
  styleUrl: './profit-card.scss'
})
export class ProfitCard {

  @Input()
  public title!: string;

  @Input()
  public reverseColors?: boolean = false;

  @Input()
  public model! : ProfitCardModel;

  public numberToCurrency = numberToCurrency;

  public getColorClass(value: number) {
    if (value < 0) {
      return this.reverseColors ? "color-primary" : "color-red";
    }
    return this.reverseColors ? "color-red" : "color-primary";
  }
}
