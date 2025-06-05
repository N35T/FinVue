import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTooltip } from '@angular/material/tooltip';
import { AddCategoryDialog } from '../dialogs/add-category-dialog/add-category-dialog';
import { BaseDialog } from '../dialogs/base-dialog/base-dialog';

@Component({
  selector: 'app-top-nav',
  imports: [MatTooltip],
  templateUrl: './top-nav.html',
  styleUrl: './top-nav.scss'
})
export class TopNav {

    constructor(private matDialog : MatDialog) {}

    public addTransaction() {

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
