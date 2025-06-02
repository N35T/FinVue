import { Component, Input } from '@angular/core';
import { ProfitCardModel } from '../../models/ui/profit-card.model';

@Component({
  selector: 'app-profit-card',
  imports: [],
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

}
