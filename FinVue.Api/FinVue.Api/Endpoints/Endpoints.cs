using System.Runtime.CompilerServices;
using System.Security.Claims;
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
        MapUsers(app);
    }

    private static void MapTransactions(WebApplication app) {
        app.MapGet("/transactions/ofMonth",
            async ([FromQuery] int year, [FromQuery] int month, TransactionService transactionService) => {
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
            });

        app.MapGet("/transactions/recurring/ofMonth", async ([FromQuery] int year, [FromQuery] int month,
            RecurringTransactionService recurringTransactionService) => {
            var recurringOfMonth =
                await recurringTransactionService.GetAllRecurringTransactionsFromMonthAsync(year, (Month)month);

            return Results.Ok(recurringOfMonth);
        });

        app.MapPost("/transactions",
            async (TransactionService transactionService, HttpContext ctx, [FromBody] TransactionDto transactionDto) => {
                var currentUserId = ctx.GetUserId();
                if (currentUserId is null) {
                    return Results.Unauthorized();
                }
                var transaction = new Transaction {
                    Id = Guid.NewGuid().ToString(),
                    Name = transactionDto.Name,
                    ValueInCent = transactionDto.ValueInCent,
                    PayDate = transactionDto.PayDate,
                    Type = transactionDto.Type,
                    PaymentMethod = transactionDto.PaymentMethod,
                    CreationUserId = currentUserId,
                    PayingUserId = transactionDto.PayingUser.Id,
                    CategoryId = transactionDto.Category?.Id,
                    CreationDate = DateTime.Now,
                };
                var res = await transactionService.AddTransactionAsync(transaction);
                res.PayingUser = transactionDto.PayingUser;
                return Results.Ok(TransactionDto.FromModel(res));
        });

        app.MapPost("/transactions/recurring",
            async (RecurringTransactionService rtService, [FromBody] RecurringTransactionDto rtDto) => {
                var rt = new RecurringTransaction(
                    Guid.NewGuid().ToString(),
                    rtDto.Name,
                    rtDto.ValueInCent,
                    rtDto.MonthFrequency,
                    rtDto.Type,
                    rtDto.Category?.Id
                );
                var res = await rtService.AddRecurringTransactionAsync(rt);
                return Results.Ok(RecurringTransactionDto.FromModel(res));
            });

        app.MapPost("/transactions/fromRecurring",
            async(RecurringTransactionService rtService, UserService userService, HttpContext ctx, [FromBody] TransactionFromRecurringDto rtDto) => {
                var userId = ctx.GetUserId();
                if (userId is null) {
                    return Results.Unauthorized();
                }
                var user = await userService.GetUserFromIdAsync(userId);
                if (user is null) {
                    return Results.BadRequest();
                }
                
                var res = await rtService.MarkRecurringTransactionAsDone(rtDto.RecurringTransactionId, rtDto.PayDate, user);
                return Results.Ok(TransactionDto.FromModel(res));
            });
    }

    private static void MapCategories(WebApplication app) {
        app.MapPost("/categories", async (CategoryService categoryService, [FromBody]CategoryDto category) => {
            var res = await categoryService.AddCategoryAsync(new Category(Guid.NewGuid().ToString(), category.CategoryName,
                new Color(category.CategoryColor)));

            return Results.Ok(CategoryDto.FromModel(res));
        });
        app.MapGet("/categories", async (CategoryService categoryService) => {
            var res = (await categoryService.GetAllCategoriesAsync())
                .Select(CategoryDto.FromModel)
                .ToList();
            return Results.Ok(res);
        });
    }

    private static void MapUsers(WebApplication app) {
        app.MapGet("/users", async (UserService userService) => {
            var users = await userService.GetAllUsersAsync();
            return Results.Ok(users);
        });
        app.MapPost("/users", async (UserService userService, HttpContext ctx) => {
            var id = ctx.GetUserId();
            var name = ctx.GetUserName();
            if (id is null || name is null) {
                return Results.Unauthorized();
            }

            var user = await userService.GetUserFromIdAsync(id);
            if (user is not null) {
                return Results.Ok(user);
            }

            user = new User(id, name);
            var res = await userService.AddUserAsync(user);
            return Results.Ok(res);
        });
    }
    
    private static void MapStatistics(WebApplication app) {
        app.MapGet("/statistics/byYear", async ([FromQuery] int year, TransactionService transactionService) => {
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
        });

        app.MapGet("/statistics/byMonth",
            async ([FromQuery] int year, [FromQuery] int month, TransactionService transactionService) => {
                var totalIncome =
                    await transactionService.GetTotalSumFromYearAndMonthAsync(TransactionType.Einkommen, year, month);
                var totalOutcome =
                    await transactionService.GetTotalSumFromYearAndMonthAsync(TransactionType.Ausgaben, year, month);

                return Results.Ok(new MonthlyTransactionStatistics {
                    TotalIncome = totalIncome,
                    TotalOutcome = totalOutcome,
                });
            });
    }

    private static string? GetUserId(this HttpContext ctx) {
        return ctx.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
    private static string? GetUserName(this HttpContext ctx) {
        return ctx.User?.FindFirst(ClaimTypes.Name)?.Value;
    }
}