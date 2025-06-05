import { Injectable } from "@angular/core";
import { ApiService } from "./api.service";
import { Category, CategoryAdapter } from "../models/entities/category.model";
import { map, Observable } from "rxjs";

@Injectable({
    providedIn: 'root'
})
export class CategoryService {
    
    constructor(private apiService : ApiService, private categoryAdapter : CategoryAdapter) {}

    public addNewCategory(category : Category) : Observable<Category> {
        return this.apiService.post('/categories', category)
            .pipe(map(e => this.categoryAdapter.adapt(e)));
    }
}
  