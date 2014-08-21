using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UICDS_Basics
{
    /// <summary>
    /// Various utility methods for the UICDS_Basics classes.
    /// </summary>
    public class Utilities
    {

        public class DegreesMinutesSeconds
        {
            public DegreesMinutesSeconds(double d, double m, double s)
            {
                degrees = DecimalFromDouble(d);
                minutes = DecimalFromDouble(m);
                seconds = DecimalFromDouble(s);
            }

            public decimal degrees { get; set; }
            public decimal minutes { get; set; }
            public decimal seconds { get; set; }
        };

        /// <summary>
        /// Convert decimal degrees into degrees, minute and seconds. Based on
        /// http://kiwigis.blogspot.com/2009/05/convert-decimal-degrees-to-degrees.html
        /// </summary>
        /// <param name="coordinate"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static DegreesMinutesSeconds DDtoDMS(double coordinate)
        {
            int sign = Math.Sign(coordinate);

            // Work with a positive number
            coordinate = Math.Abs(coordinate);

            // Get d/m/s components
            double d = Math.Floor(coordinate);
            coordinate -= d;
            coordinate *= 60;
            double m = Math.Floor(coordinate);
            coordinate -= m;
            coordinate *= 60;
            double s = Math.Round(coordinate);

            return new DegreesMinutesSeconds(d * sign, m, s);
        }

        // Convert the double argument; catch exceptions that are thrown.
        public static decimal DecimalFromDouble(double argument)
        {
            decimal decValue;

            // Convert the double argument to a decimal value.
            try
            {
                decValue = (decimal)argument;
            }
            catch (Exception ex)
            {
                decValue = GetExceptionType(ex);
            }

            return decValue;
        }

        // Get the exception type name; remove the namespace prefix.
        public static Decimal GetExceptionType(Exception ex)
        {
            string exceptionType = ex.GetType().ToString();
            try
            {
                return Decimal.Parse(exceptionType.Substring(
                    exceptionType.LastIndexOf('.') + 1));
            }
            catch (ArgumentNullException)
            {
                return 0.0M;
            }
            catch (FormatException)
            {
                return 0.0M;
            }
            catch (OverflowException)
            {
                return 0.0M;
            }
        }




    }
}
