import { Injectable } from "@angular/core";
import { Adapter } from "../adapter.model";

export class Category {

    constructor(public id : string, public categoryName: string, public categoryColor : string) {}
}


@Injectable({
    providedIn: 'root'
})
export class CategoryAdapter implements Adapter<Category> {
    
    adapt (item: any): Category {
        if (!item) 
            throw new Error("Can't map item to model " + item);
        return new Category(
            item.id,
            item.categoryName,
            item.categoryColor
        );
    }
}