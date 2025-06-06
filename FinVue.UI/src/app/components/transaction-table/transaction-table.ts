import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { RecurringTransaction } from '../../models/entities/recurring-transaction.model';
import { TransactionsByCategory } from '../../models/groupings/transactions-by-category.model';
import { numberToCurrency } from '../../services/currency.service';
import { LoadingSpinner } from '../loading-spinner/loading-spinner';
import { MatDialog } from '@angular/material/dialog';
import { BaseDialog } from '../dialogs/base-dialog/base-dialog';
import { RecurringDoneDialog } from '../dialogs/recurring-done-dialog/recurring-done-dialog';
import { transactionTypeToString } from '../../constants/transaction-type.constants';
import { paymentMethodToString } from '../../constants/payment-method.constants';
import { CurrentDate } from '../../models/ui/current-date.model';



@Component({
    selector: 'app-transaction-table',
    imports: [CommonModule, LoadingSpinner],
    templateUrl: './transaction-table.html',
    styleUrl: './transaction-table.scss'
})
export class TransactionTable {

    public static PLANNED_MODE = "planning-mode";
    public PLANNED_MODE = TransactionTable.PLANNED_MODE;

    @Input()
    public mode!: string;

    @Input()
    public recurringTransactions?: RecurringTransaction[];

    @Input()
    public transactionsByCategory?: TransactionsByCategory[];

    @Input()
    public loading = false;

    @Input()
    public tableDate!: CurrentDate;

    @Output()
    public transactionsUpdated: EventEmitter<any> = new EventEmitter();

    public numberToCurrency = numberToCurrency;

    public transactionTypeToString = transactionTypeToString;
    public paymentMethodToString = paymentMethodToString;

    constructor(private matDialog: MatDialog) { }

    public getUnpaidRecurringTransactions(): RecurringTransaction[] {
        if (!this.recurringTransactions) {
            return [];
        }
        return this.recurringTransactions.filter(e => !e.payedThisMonth);
    }

    public getPayedRecurringTransactions(): RecurringTransaction[] {
        if (!this.recurringTransactions) {
            return [];
        }
        return this.recurringTransactions.filter(e => e.payedThisMonth);
    }

    private activeDropdowns = new Map<string, boolean>();
    public toggleTransactionList(id: string) {
        this.activeDropdowns.set(id, !(this.activeDropdowns.get(id) ?? false));
    }
    public isTransactionCategoryOpen(id: string): boolean {
        return this.activeDropdowns.get(id) ?? false;
    }

    public openPayedDialog(rt: RecurringTransaction) {
        const dialogRef = this.matDialog.open(BaseDialog, {
            width: '500px',
            data: {
                component: RecurringDoneDialog,
                componentData: {
                    rt: rt,
                    currentDate : this.tableDate
                },
                componentTitle: "Geplante Transaktion abschließen"
            }
        });

        dialogRef.afterClosed()
            .subscribe(data => {
                if(!data || data.action !== BaseDialog.ACTION_SAVE) {
                    return;
                }
                this.transactionsUpdated?.emit();
            })
    }
}
