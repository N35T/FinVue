import { Component, Inject, Input, OnInit } from '@angular/core';
import { ISaveData } from '../base-dialog/base-dialog';
import { Observable, throwError } from 'rxjs';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { MAT_DATE_LOCALE, provideNativeDateAdapter } from '@angular/material/core';
import { TransactionService } from '../../../services/transactions.service';
import { TransactionFromRecurring } from '../../../models/groupings/transaction-from-recurring.models';
import { RecurringTransaction } from '../../../models/entities/recurring-transaction.model';
import { MatInputModule } from '@angular/material/input';
import { DateOnly } from '../../../models/entities/date.model';
import { CurrentDate } from '../../../models/ui/current-date.model';

@Component({
  selector: 'app-recurring-done-dialog',
  imports: [MatFormFieldModule, ReactiveFormsModule, MatDatepickerModule, CurrencyMaskModule, MatInputModule],
  providers: [{provide: MAT_DATE_LOCALE, useValue: 'de-DE'}, provideNativeDateAdapter()],
  templateUrl: './recurring-done-dialog.html',
  styleUrl: './recurring-done-dialog.scss'
})
export class RecurringDoneDialog implements ISaveData, OnInit {

    @Input()
    public rt! : RecurringTransaction;

    @Input()
    private currentDate! : CurrentDate;

    public markTransactionForm = new FormGroup({
        name: new FormControl({value: '', disabled: true}),
        value: new FormControl({value: 0, disabled: true }),
        payDate: new FormControl(new Date(), Validators.required),
    });

    public maxDate! : Date;
    public minDate! : Date;

    public getNameForm = () => this.markTransactionForm.get("name")!;
    public getValueForm = () => this.markTransactionForm.get("value")!;
    public getPayDateForm = () => this.markTransactionForm.get("payDate")!;
    
    constructor(
        private transactionService : TransactionService,
    ) {
    }
    ngOnInit(): void {
        this.minDate = new Date(this.currentDate.year, this.currentDate.month!-1, 1);
        this.maxDate = new Date(this.currentDate.year, this.currentDate.month!, 0);
        this.maxDate = this.minOfDates(this.maxDate, new Date());

        this.getNameForm().setValue(this.rt.name);
        this.getValueForm().setValue(this.rt.valueInCent / 100);
        this.getPayDateForm().setValue(this.maxDate);
    }

    minOfDates(date1 : any, date2 : any) : Date {
        return new Date(Math.min(date1, date2));
    }


    onSave(): Observable<any> {
        if(!this.markTransactionForm.valid) {
            return throwError(() => new Error("Mark Transaction Form is invalid!"));
        }

        const payDate = DateOnly.fromDate(this.getPayDateForm().value!);
        
        const tFromRt = new TransactionFromRecurring(this.rt.id, payDate);
        return this.transactionService.markRecurringTransactionAsDone(tFromRt);
    }

}
