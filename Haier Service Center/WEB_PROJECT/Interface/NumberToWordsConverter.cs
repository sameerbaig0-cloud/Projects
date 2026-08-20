namespace ServicePlatform.Interface
{
	public interface INumberToWordsConverter
	{
		string ConvertAmountToWords(decimal amount);
	}

	public class NumberToWordsConverter : INumberToWordsConverter
	{
		private static readonly string[] UnitsMap = { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten",
												  "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };

		private static readonly string[] TensMap = { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

		public string ConvertAmountToWords(decimal amount)
		{
			if (amount == 0)
				return "Zero";

			int intPart = (int)Math.Truncate(amount);
			int decimalPart = (int)((amount - intPart) * 100);

			string words = ConvertIntegerToWords(intPart);

			if (decimalPart > 0)
			{
				words += " and " + ConvertIntegerToWords(decimalPart) + " Paise";
			}

			return words + " Only";
		}

		private static string ConvertIntegerToWords(int number)
		{
			if (number == 0)
				return "";

			if (number < 0)
				return "Minus " + ConvertIntegerToWords(Math.Abs(number));

			string words = "";

			if ((number / 1000000) > 0)
			{
				words += ConvertIntegerToWords(number / 1000000) + " Million ";
				number %= 1000000;
			}

			if ((number / 100000) > 0)
			{
				words += ConvertIntegerToWords(number / 100000) + " Lakh ";
				number %= 100000;
			}


			if ((number / 1000) > 0)
			{
				words += ConvertIntegerToWords(number / 1000) + " Thousand ";
				number %= 1000;
			}

			if ((number / 100) > 0)
			{
				words += ConvertIntegerToWords(number / 100) + " Hundred ";
				number %= 100;
			}

			if (number > 0)
			{
				if (words != "")
					words += "and ";

				if (number < 20)
					words += UnitsMap[number];
				else
				{
					words += TensMap[number / 10];
					if ((number % 10) > 0)
						words += "-" + UnitsMap[number % 10];
				}
			}

			return words.Trim();
		}
	}



}
