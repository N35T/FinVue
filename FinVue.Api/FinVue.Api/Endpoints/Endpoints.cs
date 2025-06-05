using FinVue.Api.DataTransferObjects;
using FinVue.Core.DataTransferObjects;
using FinVue.Core.Entities;
using FinVue.Core.Enums;
using FinVue.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FinVue.Api.Endpoints;

public static class Endpoints {

    public static void UseCustomEndpoints(this WebApplication app) {
        MapTransactions(app);
        MapCategories(app);
        MapStatistics(app);
    }

    private static void MapTransactions(WebApplication app) {
        app.MapGet("/transactions/ofMonth", async ([FromQuery]int year, [FromQuery]int month, TransactionService transactionService) => {
                var incomeTransactions =
                    await transactionService.GetAllTransactionsFromMonthAndYearAndTypeGroupByCategoryAsync(month, year,
                        TransactionType.Einkommen);
                var outcomeTransactions =
                    await transactionService.GetAllTransactionsFromMonthAndYearAndTypeGroupByCategoryAsync(month, year,
                        TransactionType.Ausgaben);
                
                return Results.Ok(new MonthlyTransactions() {
                    IncomeTransactions = incomeTransactions,
                    OutcomeTransactions = outcomeTransactions
                });
            })
            .WithName("GetTransactionsOfMonth")
            .WithGroupName("Transactions")
            .WithOpenApi();
        
        app.MapGet("/transactions/recurring/ofMonth", async ([FromQuery]int year, [FromQuery]int month, RecurringTransactionService recurringTransactionService) => {
                var recurringOfMonth = await recurringTransactionService.GetAllRecurringTransactionsFromMonthAsync(year, (Month)month);
                
                return Results.Ok(recurringOfMonth);
            })
            .WithName("GetRecurringTransactionsOfMonth")
            .WithGroupName("Transactions")
            .WithOpenApi();
        
    }

    private static void MapCategories(WebApplication app) {
        app.MapPost("/categories", async (CategoryService categoryService, [FromBody]CategoryDto category) => {
            var res = await categoryService.AddCategoryAsync(new Category(Guid.NewGuid().ToString(), category.CategoryName,
                new Color(category.CategoryColor)));

            return Results.Ok(res);
        });
    }
    
    private static void MapStatistics(WebApplication app) {
        app.MapGet("/statistics/byYear", async ([FromQuery]int year, TransactionService transactionService) => {
                var incomeByMonth = 
                    await transactionService.GetTotalSumFromYearAndAllRelevantMonthsAsync(TransactionType.Einkommen, year);
                var outcomeByMonth =
                    await transactionService.GetTotalSumFromYearAndAllRelevantMonthsAsync(TransactionType.Ausgaben, year);
                var outcomeByCategory =
                    await transactionService.GetTotalSumsFromYearGroupedByCategoryAsync(year, TransactionType.Ausgaben);

                var maxLength = Math.Max(Math.Max(incomeByMonth.Count, outcomeByMonth.Count), 1);
                while (incomeByMonth.Count < maxLength) {
                    incomeByMonth.Add(0);
                }

                while (outcomeByMonth.Count < maxLength) {
                    outcomeByMonth.Add(0);
                }

                return Results.Ok(new YearlyTransactionStatistics {
                    IncomePerMonth = incomeByMonth.ToArray(),
                    OutcomePerMonth = outcomeByMonth.ToArray(),
                    OutcomeByCategory = outcomeByCategory.ToArray()
                });
            })
        .WithName("GetYearlyStatistics")
        .WithGroupName("Statistics")
        .WithOpenApi();
        
        app.MapGet("/statistics/byMonth", async ([FromQuery]int year, [FromQuery]int month, TransactionService transactionService) => {
                var totalIncome = 
                    await transactionService.GetTotalSumFromYearAndMonthAsync(TransactionType.Einkommen, year, month);
                var totalOutcome =
                    await transactionService.GetTotalSumFromYearAndMonthAsync(TransactionType.Ausgaben, year, month);

                return Results.Ok(new MonthlyTransactionStatistics {
                    TotalIncome = totalIncome,
                    TotalOutcome = totalOutcome,
                });
            })
            .WithName("GetMonthlyStatistics")
            .WithGroupName("Statistics")
            .WithOpenApi();
    }
}