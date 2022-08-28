using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShoppingProject.Utility
{
    public static class SD
    {    //StoreProcedure
        public const string Proc_CoverType_Create = "SP_CreateCoverType";
        public const string Proc_CoverType_Update = "SP_UpdateCoverType";
        public const string Proc_CoverType_Delete = "SP_DeleteCoverType";
        public const string Proc_GetCoverType = "SP_GetCoverType";
        //Roles
        public const string Role_Admin = "Admin";
        public const string Role_Employee = "Employee User";
        public const string Role_Comapny = "Company User";
        public const string Role_Individual = "Individual User";

        //Session
        public const string Ss_Session = "CartCountSession";

        //OrderStatus
        public const string OrderStatusPending = "Pending";
        public const string OrderStatusApproved = "Approved";
        public const string OrderStatusInPrograss = "Progress";
        public const string OrderStatusCancelled = "Cancelled";
        public const string OrderStatusRefunded = "Refunded";

        //PaymentStatus
        public const string PaymentStatusPending = "Pending";
        public const string PaymentStatusApproved = "Approved";
        public const string PaymentStatusDelayPayment = "PaymentStatusDelay";
        public const string PaymentStatusRejected = "Rejected";
        public const string PaymentStatusRefunded = "Refunded";

        //Cart
        public static double GetPriceBasedOnQuantity(double quantity, double price, double price50, double price100)
        {
            if (quantity > 50)
                return price;
            else if (quantity > 100)
                return price50;
            else return price100;
        }
        public static string ConvertToRawHtml(String Source)
        {
            char[] array = new char[Source.Length];
            int arrayIndex = 0;
            bool inside = false;
            for (int i = 0; i < Source.Length; i++)
            {
                char let = Source[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = true;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }
}   }
