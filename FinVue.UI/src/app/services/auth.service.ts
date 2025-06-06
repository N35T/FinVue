import { Injectable } from "@angular/core";
import { ApiService } from "./api.service";
import { map, Observable } from "rxjs";
import { User, UserAdapter } from "../models/entities/user.model";
import { environment } from "../environment/environment";

@Injectable({
    providedIn: 'root'
})
export class AuthService {

    private IDENTITY_PROVIDER_URL = environment.IDENTITY_PROVIDER_URL;

    constructor(private apiService: ApiService, private userAdapter : UserAdapter) {
    }

    public auth() : Observable<User> {
        return this.apiService.post<User>('/users', {})
            .pipe(map(e => this.userAdapter.adapt(e)));
    }

    public redirectToAuth() {
        window.location.href = this.IDENTITY_PROVIDER_URL;
    }

}