import { Component, Inject } from '@angular/core';
import { ISaveData } from '../base-dialog/base-dialog';
import { Observable, throwError } from 'rxjs';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { MAT_DATE_LOCALE, provideNativeDateAdapter } from '@angular/material/core';
import { TransactionService } from '../../../services/transactions.service';
import { TransactionFromRecurring } from '../../../models/groupings/transaction-from-recurring.models';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { RecurringTransaction } from '../../../models/entities/recurring-transaction.model';

@Component({
  selector: 'app-recurring-done-dialog',
  imports: [MatFormFieldModule, ReactiveFormsModule, MatDatepickerModule, CurrencyMaskModule],
  providers: [{provide: MAT_DATE_LOCALE, useValue: 'de-DE'}, provideNativeDateAdapter()],
  templateUrl: './recurring-done-dialog.html',
  styleUrl: './recurring-done-dialog.scss'
})
export class RecurringDoneDialog implements ISaveData {
    
    public markTransactionForm;

    public today = new Date();

    public getPayDateForm = () => this.markTransactionForm.get("payDate")!;
    
    constructor(
        private transactionService : TransactionService,
        @Inject(MAT_DIALOG_DATA) private data : { rt: RecurringTransaction }
    ) {
        this.markTransactionForm = new FormGroup({
            name: new FormControl(this.data.rt.name),
            value: new FormControl(this.data.rt.valueInCent / 100),
            payDate: new FormControl(new Date(), Validators.required),
        });
    }


    onSave(): Observable<any> {
        if(!this.markTransactionForm.valid) {
            return throwError(() => new Error("Mark Transaction Form is invalid!"));
        }

        const payDate = this.getPayDateForm().value!;
        
        const tFromRt = new TransactionFromRecurring(this.data.rt.id, payDate);
        return this.transactionService.markRecurringTransactionAsDone(tFromRt);
    }

}
