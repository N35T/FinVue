import { Routes } from '@angular/router';
import { YearPage } from './pages/year-page/year-page';
import { MonthPage } from './pages/month-page/month-page';

export const routes: Routes = [
    {
        path: '',
        component: YearPage
    },
    {
        path: 'year',
        component: YearPage
    },
    {
        path: 'year/:selectedYear',
        component: YearPage
    },
    {
        path: 'year/:selectedYear/month/:selectedMonth',
        component: MonthPage
    }
];
