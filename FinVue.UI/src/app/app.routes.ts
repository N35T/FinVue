import { Routes } from '@angular/router';
import { YearPage } from './pages/year-page/year-page';
import { MonthPage } from './pages/month-page/month-page';

export const routes: Routes = [
    {
        path: '',
        loadComponent: () => import('./pages/year-page/year-page').then(m => m.YearPage)
    },
    {
        path: 'year',
        loadComponent: () => import('./pages/year-page/year-page').then(m => m.YearPage)
    },
    {
        path: 'year/:selectedYear',
        loadComponent: () => import('./pages/year-page/year-page').then(m => m.YearPage)
    },
    {
        path: 'year/:selectedYear/month/:selectedMonth',
        loadComponent: () => import('./pages/month-page/month-page').then(m => m.MonthPage)
    }
];
