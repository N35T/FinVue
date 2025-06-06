import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { TopNav } from './components/top-nav/top-nav';
import { SideNav } from './components/side-nav/side-nav';
import { AuthService } from './services/auth.service';
import { UserService } from './services/user.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, TopNav, SideNav],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App implements OnInit {
    protected title = 'FinVue.UI';

    constructor(private authService : AuthService, private userService : UserService) {}

    ngOnInit(): void {
        this.authService.auth()
            .subscribe(user => {
                this.userService.setCurrentUser(user);
            });
    }
}
