import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CurrentDateService } from '../../services/current-date-service';
import { CurrentDate } from '../../models/ui/current-date.model';
import { ProfitCard } from '../../components/profit-card/profit-card';
import { Chart, registerables } from 'chart.js';
import { ProfitCardModel } from '../../models/ui/profit-card.model';
import { BehaviorSubject } from 'rxjs';
import { StatisticService } from '../../services/statistics.service';
import { SumByCategory } from '../../models/groupings/sum-by-category.models';
import { AsyncPipe } from '@angular/common';
import { YearlyTransactionStatistics } from '../../models/groupings/yearly-transaction-statistics.models';
import { numberToCurrency } from '../../services/currency.service';

@Component({
  selector: 'app-year-page',
  imports: [ProfitCard, AsyncPipe],
  templateUrl: './year-page.html',
  styleUrl: './year-page.scss'
})
export class YearPage implements OnInit {

  public profitChart : any;
  public categoryChart : any;

  public incomeProfitCard: BehaviorSubject<ProfitCardModel> = new BehaviorSubject(this.getDefaultProfitCardModel());
  public outcomeProfitCard : BehaviorSubject<ProfitCardModel> = new BehaviorSubject(this.getDefaultProfitCardModel());
  public profitProfitCard : BehaviorSubject<ProfitCardModel> = new BehaviorSubject(this.getDefaultProfitCardModel());

  constructor(private activatedRoute : ActivatedRoute, public currentDateService : CurrentDateService, private statsService : StatisticService) {
    this.activatedRoute.params.subscribe((params) => {
        this.currentDateService.currentDate.set(new CurrentDate(params['selectedYear']));
    })
  }


  ngOnInit(): void {
    Chart.register(...registerables);
    this.createCharts();
    this.statsService.getYearlyStatistics(this.currentDateService.currentDate().year).subscribe((e : YearlyTransactionStatistics) => {
      this.updateProfitCards(e);
      this.updateChartData(e);
    })
  }

  private createCharts() {
    this.createProfitChart();
    this.createCategoryChart();

  }

  private updateProfitCards(stats : YearlyTransactionStatistics) {
    this.incomeProfitCard.next(new ProfitCardModel(stats.getTotalIncome(), stats.getAverageIncome()));
    this.outcomeProfitCard.next(new ProfitCardModel(stats.getTotalOutcome(), stats.getAverageOutcome()));
    this.profitProfitCard.next(new ProfitCardModel(stats.getTotalProfit(), stats.getAverageProfit()));
  }

  private updateChartData(stats: YearlyTransactionStatistics) {
    this.profitChart.data = this.createProfitChartData(stats.incomePerMonth, stats.outcomePerMonth, stats.getProfitByMonth(), stats.getCumulatedProfitByMonth());
    this.categoryChart.data = this.createCategoryChartData(stats.outcomeByCategory);
  }

  private createProfitChart() {
    const options = {
      plugins: {
        legend: {
          display: true
        },
        tooltip: {
          callbacks: {
            label: function(context: any): any {
              const label = context.dataset.label || '';
              const value = context.parsed.y;
              return `${label}: ${numberToCurrency(value)}`;
            }
          }
        }
      },
      scales: {
        y: {
          ticks: {
            callback: function(value: any): any {
              return numberToCurrency(value, true);
            }
          }
        }
      }
    };
    
    const data = this.createProfitChartData([0], [0], [0], [0]);

    this.profitChart = new Chart("profit-chart", {
        type: "line",
        data: data,
        options: options
    });

  }

  private createCategoryChart() {
    const data = this.createCategoryChartData([new SumByCategory("Kategorie", 100, '#20201d')])
    const options = {
      plugins: {
        tooltip: {
          callbacks: {
            label: function(context: any) {
              const label = context.label || ''; // X-axis label
              const value = context.dataset.data[context.dataIndex]; // Y value
              return `${label}: ${numberToCurrency(value)}`;
            }
          }
        }
      }
    };

    new Chart("category-sums-chart", {
      type: "pie",
      data: data,
      options: options
    });
  }

  private createProfitChartData(income: number[], outcome: number[], profit: number[], cumProfit: number[]) : any {
    const months = ["Jan", "Feb", "Mär", "Mai", "Apr", "Jun", "Jul", "Aug", "Sep", "Okt", "Nov", "Dez"];
    return {
      labels: months,
      datasets: [{
          data: outcome.map(e => e / 100),
          borderColor: "#ac4d4d",
          fill: false,
          label: "Ausgaben"
      },{
          data: income.map(e => e / 100),
          borderColor: "#81a975",
          fill: false,
          label: "Einkommen"
      },{
          data: profit.map(e => e / 100),
          borderColor: "#ede3e390",
          fill: false,
          label: "Profit pro Monat"
      },{
          data: cumProfit.map(e => e / 100),
          borderColor: "#33aad6",
          fill: false,
          label: "Kumulierter Profit"
      }]
   };
  }

  private createCategoryChartData(outcomeSumsByCategory : SumByCategory[]) : any {
    return {
      labels: ["Kategorien"],
      datasets: [{
          label: outcomeSumsByCategory.map(e => e.categoryName),
          data: outcomeSumsByCategory.map(e => e.totalSum / 100),
          backgroundColor: outcomeSumsByCategory.map(e => e.categoryColor),
          hoverOffset: 4
      }]
    };
  }

  public getDefaultProfitCardModel() {
    return ProfitCardModel.defaultModel();
  }
}