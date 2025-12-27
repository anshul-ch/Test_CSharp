using System;

namespace Test_Charp.QuickMart_Profit_Calculator
{
    /// <summary>
    /// Represents a sales transaction, including details about the customer, item, amounts, 
    /// and profit or loss information.
    /// </summary>
    public class SaleTransaction
    {
        /// <summary>
        /// Represents a single retail sale transaction.
        /// <para>
        /// This entity captures details about the customer, item, and financial metrics 
        /// required to calculate profit or loss.
        /// </para>
        /// </summary>
        /// 
        public string InvoiceNo { get; set; }
        public string CustomerName { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal PurchaseAmount { get; set; }
        public decimal SellingAmount { get; set; }
        public string ProfitOrLossStatus { get; set; }
        public decimal ProfitOrLossAmount { get; set; }
        public decimal ProfitMarginPercent { get; set; }
    }

    public static class TransactionState
    {
        /// <summary>
        /// Represents the most recent sale transaction processed by the system.
        /// </summary>
        public static SaleTransaction LastTransaction;
        public static bool HasLastTransaction = false;
    }

    public class IniciateTransaction
    {
        /// <summary>
        /// Prompts the user to enter details for a new sales transaction and saves the transaction as the most recent
        /// entry.
        /// </summary>
        /// <remarks>This method collect transaction information, including invoice number, customer name, item name, 
        /// quantity, purchase amount, and selling amount. It validates user input for required fields 
        /// and numeric constraints. After collecting the data, calculates the profit or loss status and margin, 
        /// and updates the application's transaction state to reflect
        /// the new transaction.</remarks>
        public void CreateNewTransaction()
        {
            Console.WriteLine("\n--- Enter Transaction Details ---");

            SaleTransaction newTrans = new SaleTransaction();

            while (true)
            {
                Console.Write("Enter Invoice No: ");
                string inputId = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(inputId))
                {
                    newTrans.InvoiceNo = inputId;
                    break;
                }
                Console.WriteLine("Invoice No cannot be empty.");
            }

            Console.Write("Enter Customer Name: ");
            newTrans.CustomerName = Console.ReadLine();

            Console.Write("Enter Item Name: ");
            newTrans.ItemName = Console.ReadLine();

            while (true)
            {
                Console.Write("Enter Quantity: ");
                if (int.TryParse(Console.ReadLine(), out int qty) && qty > 0)
                {
                    newTrans.Quantity = qty;
                    break;
                }
                Console.WriteLine("Invalid input. Quantity must be greater than 0.");
            }

            while (true)
            {
                Console.Write("Enter Purchase Amount (total): ");
                if (decimal.TryParse(Console.ReadLine(), out decimal purchase) && purchase > 0)
                {
                    newTrans.PurchaseAmount = purchase;
                    break;
                }
                Console.WriteLine("Invalid input. Purchase Amount must be greater than 0.");
            }

            while (true)
            {
                Console.Write("Enter Selling Amount (total): ");
                if (decimal.TryParse(Console.ReadLine(), out decimal selling) && selling >= 0)
                {
                    newTrans.SellingAmount = selling;
                    break;
                }
                Console.WriteLine("Invalid input. Selling Amount must be non-negative.");
            }

            if (newTrans.SellingAmount > newTrans.PurchaseAmount)
            {
                newTrans.ProfitOrLossStatus = "PROFIT";
                newTrans.ProfitOrLossAmount = newTrans.SellingAmount - newTrans.PurchaseAmount;
            }
            else if (newTrans.SellingAmount < newTrans.PurchaseAmount)
            {
                newTrans.ProfitOrLossStatus = "LOSS";
                newTrans.ProfitOrLossAmount = newTrans.PurchaseAmount - newTrans.SellingAmount;
            }
            else
            {
                newTrans.ProfitOrLossStatus = "BREAK-EVEN";
                newTrans.ProfitOrLossAmount = 0;
            }

            if (newTrans.PurchaseAmount > 0)
            {
                newTrans.ProfitMarginPercent = (newTrans.ProfitOrLossAmount / newTrans.PurchaseAmount) * 100;
            }
            else
            {
                newTrans.ProfitMarginPercent = 0;
            }

            TransactionState.LastTransaction = newTrans;
            TransactionState.HasLastTransaction = true;

            Console.WriteLine("Transaction saved successfully.");
            Console.WriteLine($"Status: {newTrans.ProfitOrLossStatus}");
            Console.WriteLine($"Profit/Loss Amount: {newTrans.ProfitOrLossAmount:F2}");
            Console.WriteLine($"Profit Margin (%): {newTrans.ProfitMarginPercent:F2}");
        }
    }

    public class Transactionviewtransaction
    {
        /// <summary>
        /// Displays the details of the most recent sale transaction to the console.
        /// </summary>
        /// <remarks>If there is no previous transaction available, a message is displayed indicating that
        /// no transaction exists. This method outputs transaction details such as invoice number, customer, item,
        /// amounts, status, and profit information.</remarks>
        public void ViewLastTransaction()
        {
            if (!TransactionState.HasLastTransaction)
            {
                Console.WriteLine("No transaction available. Please create a new transaction first.");
                return;
            }

            SaleTransaction trans = TransactionState.LastTransaction;

            Console.WriteLine("\n-------------- Last Transaction --------------");
            Console.WriteLine($"InvoiceNo: {trans.InvoiceNo}");
            Console.WriteLine($"Customer: {trans.CustomerName}");
            Console.WriteLine($"Item: {trans.ItemName}");
            Console.WriteLine($"Quantity: {trans.Quantity}");
            Console.WriteLine($"Purchase Amount: {trans.PurchaseAmount:F2}");
            Console.WriteLine($"Selling Amount: {trans.SellingAmount:F2}");
            Console.WriteLine($"Status: {trans.ProfitOrLossStatus}");
            Console.WriteLine($"Profit/Loss Amount: {trans.ProfitOrLossAmount:F2}");
            Console.WriteLine($"Profit Margin (%): {trans.ProfitMarginPercent:F2}");
            Console.WriteLine("--------------------------------------------------------------------------------------------------");
        }
    }

    public class TransactionCalculator
    {
        /// <summary>
        /// Calculates and updates the profit or loss status, amount, and margin for the most recent sale transaction.
        /// </summary>
        /// <remarks>If there is no existing transaction, displays a message and performs no
        /// calculation. The method updates the last transaction's profit or loss fields based on the difference between
        /// the selling and purchase amounts, and outputs the results</remarks>
        public void CalculateProfitLoss()
        {
            if (!TransactionState.HasLastTransaction)
            {
                Console.WriteLine("No transaction available. Please create a new transaction first.");
                return;
            }

            SaleTransaction trans = TransactionState.LastTransaction;

            if (trans.SellingAmount > trans.PurchaseAmount)
            {
                trans.ProfitOrLossStatus = "PROFIT";
                trans.ProfitOrLossAmount = trans.SellingAmount - trans.PurchaseAmount;
            }
            else if (trans.SellingAmount < trans.PurchaseAmount)
            {
                trans.ProfitOrLossStatus = "LOSS";
                trans.ProfitOrLossAmount = trans.PurchaseAmount - trans.SellingAmount;
            }
            else
            {
                trans.ProfitOrLossStatus = "BREAK-EVEN";
                trans.ProfitOrLossAmount = 0;
            }

            if (trans.PurchaseAmount > 0)
            {
                trans.ProfitMarginPercent = (trans.ProfitOrLossAmount / trans.PurchaseAmount) * 100;
            }
            else
            {
                trans.ProfitMarginPercent = 0;
            }

            Console.WriteLine("\n--- Recomputed Profit/Loss ---");
            Console.WriteLine($"Status: {trans.ProfitOrLossStatus}");
            Console.WriteLine($"Profit/Loss Amount: {trans.ProfitOrLossAmount:F2}");
            Console.WriteLine($"Profit Margin (%): {trans.ProfitMarginPercent:F2}");
        }
    }

    class Program
    {
        /// <summary>
        /// Serves as the entry point for the QuickMart Traders application.
        /// </summary>
        static void Main(string[] args)
        {
            IniciateTransaction newTransaction = new IniciateTransaction();
            Transactionviewtransaction viewTransaction = new Transactionviewtransaction();
            TransactionCalculator calculateTransaction = new TransactionCalculator();

            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\n================== QuickMart Traders ==================");
                Console.WriteLine("1. Create New Transaction (Enter Purchase & Selling Details)");
                Console.WriteLine("2. View Last Transaction");
                Console.WriteLine("3. Calculate Profit/Loss (Recompute & Print)");
                Console.WriteLine("4. Exit");
                Console.Write("Enter your option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        newTransaction.CreateNewTransaction();
                        break;
                    case "2":
                        viewTransaction.ViewLastTransaction();
                        break;
                    case "3":
                        calculateTransaction.CalculateProfitLoss();
                        break;
                    case "4":
                        exit = true;
                        Console.WriteLine("Thank you. Application closed normally.");
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }
    }
}