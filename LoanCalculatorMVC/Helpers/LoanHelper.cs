using LoanCalculatorMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanCalculatorMVC.Helpers
{
    public class LoanHelper
    {
        public Loan GetPayments(Loan loan)
        {

            // Calculate a monthly payment
            loan.Payment = CalcPayment(loan.Amount, loan.Rate, loan.Term);

            // Create loop from 1 to the term
            var balance = loan.Amount;
            var totalInterest = 0.0m;
            var monthlyInterest = 0.0m;
            var monthlyPrincipal = 0.0m;
            var monthlyRate = CalcMonthlyRate(loan.Rate);

            // Loop over each month until we reach the term of the loan
            for (int month = 1; month <= loan.Term; month++)
            {
                monthlyInterest = CalcMonthlyInterest(balance, monthlyRate);
                totalInterest += monthlyInterest;
                monthlyPrincipal = loan.Payment - monthlyInterest;
                balance -= monthlyPrincipal;

                LoanPayment loanPayment = new();

                loanPayment.Month = month;
                loanPayment.Payment = loan.Payment;
                loanPayment.MonthlyPrincipal = monthlyPrincipal;
                loanPayment.MonthlyInterest = monthlyInterest;
                loanPayment.TotalInterest = totalInterest;
                loanPayment.Balance = balance;

                // Push payments into the loan Model
                loan.Payments.Add(loanPayment);
            }

            loan.TotalInterest = totalInterest;
            loan.TotalCost = loan.Amount + totalInterest;

            // Return the loan

            return loan;
        }

        private decimal CalcPayment(decimal amount, decimal rate, int term)
        {
            var monthlyRate = CalcMonthlyRate(rate);
            var rateDbl = Convert.ToDouble(monthlyRate);
            var amountDbl = Convert.ToDouble(amount);
            var paymentDbl = (amountDbl * rateDbl) / (1 - Math.Pow(1 + rateDbl, -term));

            return Convert.ToDecimal(paymentDbl);
        }

        private decimal CalcMonthlyRate(decimal rate)
        {
            return rate / 1200;
        }

        private decimal CalcMonthlyInterest(decimal balance, decimal monthlyRate)
        {
            return balance * monthlyRate;
        }
    }
}
