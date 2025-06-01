import { Component } from '@angular/core';
import { ActivatedRoute, RouterOutlet } from '@angular/router';
import { TopNav } from './components/top-nav/top-nav';
import { SideNav } from './components/side-nav/side-nav';
import { CurrentDateService } from './services/current-date-service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, TopNav, SideNav],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected title = 'FinVue.UI';
}
