import { Component, OnInit } from '@angular/core';
import { ISaveData } from '../base-dialog/base-dialog';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { TransactionService } from '../../../services/transactions.service';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { Transaction } from '../../../models/entities/transaction.model';
import { _TRANSACTION_TYPES, stringToTransactionType, TransactionType, transactionTypeToString } from '../../../constants/transaction-type.constants';
import { _PAYMENT_METHOD, PaymentMethod, paymentMethodToString, stringToPaymentMethod } from '../../../constants/payment-method.constants';
import { Category } from '../../../models/entities/category.model';
import { User } from '../../../models/entities/user.model';
import { CategoryService } from '../../../services/category.service';
import { UserService } from '../../../services/user.service';
import { MatSelectModule } from '@angular/material/select';
import { AsyncPipe, CommonModule } from '@angular/common';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MAT_DATE_LOCALE, provideNativeDateAdapter } from '@angular/material/core';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { RecurringTransaction } from '../../../models/entities/recurring-transaction.model';
import { DateOnly } from '../../../models/entities/date.model';

@Component({
  selector: 'app-transaction-dialog',
  imports: [MatInputModule, MatFormFieldModule, ReactiveFormsModule, MatSelectModule, MatDatepickerModule, AsyncPipe, CurrencyMaskModule, MatSlideToggleModule, CommonModule],
  providers: [{provide: MAT_DATE_LOCALE, useValue: 'de-DE'}, provideNativeDateAdapter()],
  templateUrl: './transaction-dialog.html',
  styleUrl: './transaction-dialog.scss'
})
export class TransactionDialog implements ISaveData, OnInit {

    public addTransactionForm = new FormGroup({
        isRecurring: new FormControl(false),
        monthFrequency: new FormControl(1, Validators.required),
        name: new FormControl('', Validators.required),
        value: new FormControl(0, Validators.required),
        payDate: new FormControl(new Date(), Validators.required),
        type: new FormControl(transactionTypeToString(TransactionType.Ausgaben), Validators.required),
        paymentMethod: new FormControl(paymentMethodToString(PaymentMethod.Bar), Validators.required),
        payingUser: new FormControl('', Validators.required),
        category: new FormControl('', Validators.required),
    });

    public isRecurring() : boolean {
        return this.getIsRecurringForm().value ?? false
    }

    public getIsRecurringForm = () => this.addTransactionForm.get('isRecurring')!;
    public getMonthFrequencyForm = () => this.addTransactionForm.get('monthFrequency')!;
    public getNameForm = () => this.addTransactionForm.get("name")!;
    public getValueForm = () => this.addTransactionForm.get("value")!;
    public getPayDateForm = () => this.addTransactionForm.get("payDate")!;
    public getTypeForm = () => this.addTransactionForm.get("type")!;
    public getPaymentMethodForm = () => this.addTransactionForm.get("paymentMethod")!;
    public getPayingUserForm = () => this.addTransactionForm.get("payingUser")!;
    public getCategoryForm = () => this.addTransactionForm.get("category")!;

    public transactionTypes = _TRANSACTION_TYPES;
    public paymentMethods = _PAYMENT_METHOD;
    public users = new BehaviorSubject<User[]>([]);
    public categories = new BehaviorSubject<Category[]>([]);

    public today = new Date();

    constructor(private transactionService : TransactionService, private userService : UserService, private categoryService : CategoryService) {}

    ngOnInit(): void {
        this.userService.getUsers().subscribe(e => this.users.next(e)); 
        this.categoryService.getCategories().subscribe(e => {this.categories.next(e); console.log(e)}); 
    }

    onChangeMode() {
        if(!this.isRecurring()) {
            this.getMonthFrequencyForm().enable();
            this.getPayDateForm().disable();
            this.getPayingUserForm().disable();
            this.getPaymentMethodForm().disable();
        }else {
            this.getMonthFrequencyForm().disable();
            this.getPayDateForm().enable();
            this.getPayingUserForm().enable();
            this.getPaymentMethodForm().enable();
        }
    }

    onSave(): Observable<any> {
        if(!this.addTransactionForm.valid || this.categories.getValue().length === 0) {
            return throwError(() => new Error("Transaction Form is invalid!"));
        }
        return this.isRecurring() ? this.createRecurringTransaction() : this.createTransaction();
    }

    private createTransaction() : Observable<any> {
        if(this.users.getValue().length === 0) {
            return throwError(() => new Error("Transaction Form is invalid!"));
        }

        const name = this.getNameForm().value;
        const valueInCent = this.getValueForm().value!*100;
        const payDate = DateOnly.fromDate(this.getPayDateForm().value!);
        const type = stringToTransactionType(this.getTypeForm().value!);
        const paymentMethod = stringToPaymentMethod(this.getPaymentMethodForm().value!);
        const payingUser = this.users.getValue().filter(e => e.username === this.getPayingUserForm().value)?.at(0);
        const category = this.categories.getValue().filter(e => e.categoryName === this.getCategoryForm().value)?.at(0);

        const transaction = new Transaction("", name!, valueInCent, new Date(), payDate, type, paymentMethod, payingUser!, undefined, category, undefined);
        console.log(transaction.toString())

        return this.transactionService.addTransaction(transaction);
    }

    private createRecurringTransaction() : Observable<any> {
        const name = this.getNameForm().value;
        const monthFrequency = this.getMonthFrequencyForm().value;
        const valueInCent = this.getValueForm().value!*100;
        const type = stringToTransactionType(this.getTypeForm().value!);
        const category = this.categories.getValue().filter(e => e.categoryName === this.getCategoryForm().value)?.at(0);

        const rt = new RecurringTransaction("", name!, valueInCent, monthFrequency!, type, false, category);

        return this.transactionService.addRecurringTransaction(rt);
    }

}
