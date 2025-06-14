import { Injectable } from "@angular/core";
import { ApiService } from "./api.service";
import { map, Observable } from "rxjs";
import { User, UserAdapter } from "../models/entities/user.model";

@Injectable({
    providedIn: 'root'
})
export class UserService {

    private user? : User;

    constructor(
        private apiService : ApiService,
        private userAdapter : UserAdapter
    ) {}

    public getUsers() : Observable<User[]> {
        return this.apiService.get<User[]>('/users')
            .pipe(map(item => item.map(e => this.userAdapter.adapt(e))));
    }

    public getCurrentUser() : User {
        if(!this.user) {
            throw new Error("Session user not accessible!")
        }
        return this.user;
    }

    public setCurrentUser(user : User) {
        this.user = user;
    }
}