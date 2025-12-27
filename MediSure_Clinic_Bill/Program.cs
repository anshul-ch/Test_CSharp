using System;

namespace Test_Charp.MediSure_Clinic_Bill
{

    #region ClassProperties
    public class PatientBill
    {
        /// <summary>
        /// Represents the core entity for a patient's bill.
        /// <para>
        /// This class acts as a data container for both user inputs (Name, Fees) 
        /// and calculated financial metrics (Gross, Discount, Final Payable).
        /// </para>
        /// </summary>
        
        public string BillId { get; set; }
        public string PatientName { get; set; }
        public bool HasInsurance { get; set; }
        public decimal ConsultationFee { get; set; }
        public decimal LabCharges { get; set; }
        public decimal MedicineCharges { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalPayable { get; set; }
    }
    #endregion
    public static class BillState
    {
        /// <summary>
        /// Gets or sets the most recently generated patient bill.
        /// </summary>
        
        public static PatientBill LastBill;
        public static bool HasLastBill = false;
    }

    #region CreateNewBill
    public class BillIniciate
    {
        /// <summary>
        /// Creates bill for new Patients:
        /// <item>Captures and validates user input from the console.</item>
        /// <item>Calculates Gross, Discount, and Final amounts.</itemm
        /// </summary>


        public void CreateNewBill()
        {
            Console.WriteLine("\n--- Enter Patient Details ---");

            PatientBill newBill = new PatientBill();

            while (true)
            {
                Console.Write("Enter Bill Id: ");
                string inputId = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(inputId))
                {
                    newBill.BillId = inputId;
                    break;
                }
                Console.WriteLine("Bill Id cannot be empty.");
            }

            // Patient Name
            Console.Write("Enter Patient Name: ");
            newBill.PatientName = Console.ReadLine();

            // Insurance Status
            Console.Write("Is the patient insured? (Y/N): ");
            string insuranceInput = Console.ReadLine();

            if (insuranceInput != null && insuranceInput.ToUpper() == "Y")
            {
                newBill.HasInsurance = true;
            }
            else
            {
                newBill.HasInsurance = false;
            }

           // Consultant Fee
            while (true)
            {
                Console.Write("Enter Consultation Fee: ");
                if (decimal.TryParse(Console.ReadLine(), out decimal fee) && fee > 0)
                {
                    newBill.ConsultationFee = fee;
                    break;
                }
                Console.WriteLine("Invalid input. Consultation Fee must be a number greater than 0.");
            }

            // Lab Charges
            while (true)
            {
                Console.Write("Enter Lab Charges: ");
                if (decimal.TryParse(Console.ReadLine(), out decimal lab) && lab >= 0)
                {
                    newBill.LabCharges = lab;
                    break;
                }
                Console.WriteLine("Invalid input. Lab Charges must be a non-negative number.");
            }

            // Medicine Charges
            while (true)
            {
                Console.Write("Enter Medicine Charges: ");
                if (decimal.TryParse(Console.ReadLine(), out decimal meds) && meds >= 0)
                {
                    newBill.MedicineCharges = meds;
                    break;
                }
                Console.WriteLine("Invalid input. Medicine Charges must be a non-negative number.");
            }

            // Calculations
            newBill.GrossAmount = newBill.ConsultationFee + newBill.LabCharges + newBill.MedicineCharges;

            if (newBill.HasInsurance)
            {
                newBill.DiscountAmount = newBill.GrossAmount * 0.10m;
            }
            else
            {
                newBill.DiscountAmount = 0;
            }

            newBill.FinalPayable = newBill.GrossAmount - newBill.DiscountAmount;

            BillState.LastBill = newBill;
            BillState.HasLastBill = true;

            Console.WriteLine("Bill created successfully.");
            Console.WriteLine($"Gross Amount: {newBill.GrossAmount:F2}");
            Console.WriteLine($"Discount Amount: {newBill.DiscountAmount:F2}");
            Console.WriteLine($"Final Payable: {newBill.FinalPayable:F2}");
        }
    }
    #endregion


    #region ViewLastBill
    public class ViewBill
    {
        /// <summary>
        /// Displays the most recently created bill:
        /// </summary>
        public void ViewLastBill()
        {
            if (!BillState.HasLastBill)
            {
                Console.WriteLine("No bill available. Please create a new bill first.");
                return;
            }

            PatientBill bill = BillState.LastBill;

            Console.WriteLine("\n----------- Last Bill -----------");
            Console.WriteLine($"BillId: {bill.BillId}");
            Console.WriteLine($"Patient: {bill.PatientName}");
            Console.WriteLine($"Insured: {(bill.HasInsurance ? "Yes" : "No")}");
            Console.WriteLine($"Consultation Fee: {bill.ConsultationFee:F2}");
            Console.WriteLine($"Lab Charges: {bill.LabCharges:F2}");
            Console.WriteLine($"Medicine Charges: {bill.MedicineCharges:F2}");
            Console.WriteLine($"Gross Amount: {bill.GrossAmount:F2}");
            Console.WriteLine($"Discount Amount: {bill.DiscountAmount:F2}");
            Console.WriteLine($"Final Payable: {bill.FinalPayable:F2}");
            Console.WriteLine("---------------------------------");
        }
    }
    #endregion


    #region ClearLastBill
    public class BillClean
    {
        /// <summary>
        /// Clears the record of the last processed bill from the shared state.
        /// </summary>
        /// <remarks>Call this method to remove the most recently processed bill. After
        /// calling this method, attempts to access the last bill will indicate that no bill is currently
        /// stored.</remarks>
        public void ClearLastBill()
        {
            
            BillState.LastBill = null;
            BillState.HasLastBill = false;
            Console.WriteLine("Last bill cleared.");
        }
    }
    #endregion


    class Program
    {
        /// <summary>
        /// Serves as the entry point for the MediSure Clinic Billing application.
        /// </summary>
        static void Main(string[] args)
        {
            // Instantiate separate classes for each function
            BillIniciate newbill = new BillIniciate();
            ViewBill viewbill = new ViewBill();
            BillClean clearbill = new BillClean();

            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\n================== MediSure Clinic Billing ==================");
                Console.WriteLine("1. Create New Bill (Enter Patient Details)");
                Console.WriteLine("2. View Last Bill");
                Console.WriteLine("3. Clear Last Bill");
                Console.WriteLine("4. Exit");
                Console.Write("Enter your option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        newbill.CreateNewBill();
                        break;
                    case "2":
                        viewbill.ViewLastBill();
                        break;
                    case "3":
                        clearbill.ClearLastBill();
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