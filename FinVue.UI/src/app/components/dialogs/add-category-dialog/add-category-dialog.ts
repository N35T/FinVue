import { Component } from '@angular/core';
import { ISaveData } from '../base-dialog/base-dialog';
import { delay, Observable, of, throwError } from 'rxjs';
import { CategoryService } from '../../../services/category.service';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Category } from '../../../models/entities/category.model';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';

@Component({
  selector: 'app-add-category-dialog',
  imports: [MatInputModule, MatFormFieldModule, ReactiveFormsModule],
  templateUrl: './add-category-dialog.html',
  styleUrl: './add-category-dialog.scss'
})
export class AddCategoryDialog implements ISaveData {

    public addCategoryForm = new FormGroup({
        name: new FormControl('', Validators.required),
        color: new FormControl(this.getRandomColor(), Validators.required)
    });

    public getNameForm = () => this.addCategoryForm.get("name");
    public getColorForm = () => this.addCategoryForm.get("color");

    constructor(private categoryService : CategoryService) {}


    public onSave(): Observable<any> {
        if(!this.addCategoryForm.valid) {
            return throwError(() => new Error("Category Form is invalid!"));
        }

        const name = this.addCategoryForm.get("name")!.value;
        const color = this.addCategoryForm.get("color")!.value;
        return this.categoryService.addNewCategory(new Category("", name!, color!));
    }   

    private getRandomColor() : string {
        const randomChannel = () => 127 + Math.floor(Math.random() * 128); // 127–255 for light colors
        const r = randomChannel();
        const g = randomChannel();
        const b = randomChannel();
        return `#${((1 << 24) + (r << 16) + (g << 8) + b).toString(16).slice(1)}`;
    }

}
