import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { RecurringTransaction } from '../../models/entities/recurring-transaction.model';
import { TransactionsByCategory } from '../../models/groupings/transactions-by-category.model';
import { numberToCurrency } from '../../services/currency.service';



@Component({
  selector: 'app-transaction-table',
  imports: [CommonModule],
  templateUrl: './transaction-table.html',
  styleUrl: './transaction-table.scss'
})
export class TransactionTable {
  
  public static PLANNING_MODE = "planning-mode";
  public PLANNING_MODE = TransactionTable.PLANNING_MODE;

  @Input()
  public mode!: string;

  @Input()
  public recurringTransactions? : RecurringTransaction[];

  @Input()
  public transactionsByCategory? : TransactionsByCategory[];

  public numberToCurrency = numberToCurrency;

  public getUnpaidRecurringTransactions() : RecurringTransaction[] {
    if(!this.recurringTransactions) {
      return [];
    }
    return this.recurringTransactions.filter(e => !e.payedThisMonth);
  }

  public getPayedRecurringTransactions() : RecurringTransaction[] {
    if(!this.recurringTransactions) {
      return [];
    }
    return this.recurringTransactions.filter(e => e.payedThisMonth);
  }

  private activeDropdowns = new Map<string, boolean>();
  public toggleTransactionList(id : string) {
    this.activeDropdowns.set(id, !(this.activeDropdowns.get(id) ?? false));
  }
  public getActiveClassForId(id : string) : string {
    return (this.activeDropdowns.get(id) ?? false) ? 'active' : '';
  }

  public openPayedDialog() {
    // TODO:
  }
}
