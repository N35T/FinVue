import { AfterContentInit, Component, OnInit, signal } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTooltip } from '@angular/material/tooltip';
import { AddCategoryDialog } from '../dialogs/add-category-dialog/add-category-dialog';
import { BaseDialog } from '../dialogs/base-dialog/base-dialog';
import { TransactionDialog } from '../dialogs/transaction-dialog/transaction-dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { StatisticService } from '../../services/statistics.service';
import { BehaviorSubject } from 'rxjs';
import { AsyncPipe, CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { CurrentDateService } from '../../services/current-date-service';
import { FormControl, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-top-nav',
  imports: [MatTooltip, MatFormFieldModule, ReactiveFormsModule , MatSelectModule, AsyncPipe, CommonModule],
  templateUrl: './top-nav.html',
  styleUrl: './top-nav.scss'
})
export class TopNav implements OnInit {

    public yearsDropDown = new FormControl();

    public years = new BehaviorSubject<number[]>([]);

    constructor(private matDialog : MatDialog, private statisticService : StatisticService, private router: Router, public currentDateService: CurrentDateService) {}

    ngOnInit(): void {
        this.statisticService.getYearOfOldestTransaction().subscribe((oldestYear: number) => {
            let currentYear = new Date().getFullYear();
            const years = [];
            while(currentYear >= (oldestYear ?? 2000)) {
                years.push(currentYear);
                currentYear--;
            }
            this.years.next(years);
            this.yearsDropDown.setValue(+this.currentDateService.currentDate().year)
        }); 
    }

    public navigateToYear(year: number) {
        this.router.navigateByUrl("/year/" + year);
    }

    public addTransaction() {
        const dialogRef = this.matDialog.open(BaseDialog, {
            width: '500px',
            data: {
                component: TransactionDialog,
                componentTitle: "Neue Transaktion"
            }
        });
    }

    public addCategory() {
        const dialogRef = this.matDialog.open(BaseDialog, {
            width: '500px',
            data: {
                component: AddCategoryDialog,
                componentTitle: "Neue Kategorie"
            }
        });
    }
}
