import { Injectable } from "@angular/core";
import { Adapter } from "../adapter.model";

export class User {

    constructor(public id : string, public username : string) {}
}

@Injectable({
  providedIn: "root",
})
export class UserAdapter implements Adapter<User> {

    adapt (item: any): User {
        if (!item) 
            throw new Error("Can't map item to model " + item);
        return new User(item.id, item.username);
    }
}