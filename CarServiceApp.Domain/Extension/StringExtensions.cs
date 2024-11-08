namespace CarServiceApp.Domain.Extension;

public static class RuDateAndMoneyConverter
{
    private static readonly string zero = "ноль";
    private static readonly string firstMale = "один";
    private static readonly string firstFemale = "одна";
    private static readonly string firstFemaleAccusative = "одну";
    private static readonly string firstMaleGenetive = "одно";
    private static readonly string secondMale = "два";
    private static readonly string secondFemale = "две";
    private static readonly string secondMaleGenetive = "двух";
    private static readonly string secondFemaleGenetive = "двух";
    private static readonly string[] from3till19 = { string.Empty, "три", "четыре", "пять", "шесть", "семь", "восемь", "девять", "десять", "одиннадцать", "двенадцать", "тринадцать", "четырнадцать", "пятнадцать", "шестнадцать", "семнадцать", "восемнадцать", "девятнадцать" };
    private static readonly string[] from3till19Genetive = { string.Empty, "трех", "четырех", "пяти", "шести", "семи", "восеми", "девяти", "десяти", "одиннадцати", "двенадцати", "тринадцати", "четырнадцати", "пятнадцати", "шестнадцати", "семнадцати", "восемнадцати", "девятнадцати" };
    private static readonly string[] tens = [string.Empty, "двадцать", "тридцать", "сорок", "пятьдесят", "шестьдесят", "семьдесят", "восемьдесят", "девяносто"];
    private static readonly string[] tensGenetive = [string.Empty, "двадцати", "тридцати", "сорока", "пятидесяти", "шестидесяти", "семидесяти", "восьмидесяти", "девяноста"];
    private static readonly string[] hundreds = [string.Empty, "сто", "двести", "триста", "четыреста", "пятьсот", "шестьсот", "семьсот", "восемьсот", "девятьсот"];
    private static readonly string[] hundredsGenetive = [string.Empty, "ста", "двухсот", "трехсот", "четырехсот", "пятисот", "шестисот", "семисот", "восемисот", "девятисот"];
    private static readonly string[] thousands = [string.Empty, "тысяча", "тысячи", "тысяч"];
    private static readonly string[] thousandsAccusative = [string.Empty, "тысячу", "тысячи", "тысяч"];
    private static readonly string[] millions = [string.Empty, "миллион", "миллиона", "миллионов"];
    private static readonly string[] billions = [string.Empty, "миллиард", "миллиарда", "миллиардов"];
    private static readonly string[] trillions = [string.Empty, "трилион", "трилиона", "триллионов"];
    private static readonly string[] rubles = [string.Empty, "рубль", "рубля", "рублей"];
    private static readonly string[] copecks = [string.Empty, "копейка", "копейки", "копеек"];

    public enum TextCase
    {
        Nominative/*Кто? Что?*/, Genitive/*Кого? Чего?*/, Dative/*Кому? Чему?*/, Accusative/*Кого? Что?*/, Instrumental/*Кем? Чем?*/, Prepositional/*О ком? О чём?*/
    }

    private static bool IsPluralGenitive(int digits)
    {
        if (digits >= 5 || digits == 0)
        {
            return true;
        }

        return false;
    }

    private static bool IsSingularGenitive(int digits)
    {
        if (digits >= 2 && digits <= 4)
        {
            return true;
        }

        return false;
    }

    private static string MakeText(int digitsValue, string[] hundreds, string[] tens, string[] _from3till19, string secondValue, string firstValue, string[] power)
    {
        var str = string.Empty;
        var digits = digitsValue;

        if (digits >= 100)
        {
            str += hundreds[digits / 100] + " ";
            digits %= 100;
        }

        if (digits >= 20)
        {
            str += tens[digits / 10 - 1] + " ";
            digits %= 10;
        }

        if (digits >= 3)
        {
            str += _from3till19[digits - 2] + " ";
        }
        else if (digits == 2)
        {
            str += secondValue + " ";
        }
        else if (digits == 1)
        {
            str += firstValue + " ";
        }

        if (digitsValue != 0 && power.Length > 0)
        {
            digits = LastDigit(digitsValue);

            if (IsPluralGenitive(digits))
            {
                str += power[3] + " ";
            }
            else if (IsSingularGenitive(digits))
            {
                str += power[2] + " ";
            }
            else
            {
                str += power[1] + " ";
            }
        }

        return str;
    }

    /// <summary>
    /// реализовано для падежей: именительный (nominative), родительный (Genitive),  винительный (accusative).
    /// </summary>
    /// <param name="sourceNumber"></param>
    /// <param name="textCase"></param>
    /// <param name="isMale"></param>
    /// <param name="firstCapital"></param>
    /// <returns></returns>
    public static string NumeralsToTxt(long sourceNumber, TextCase textCase, bool isMale, bool firstCapital)
    {
        var str = string.Empty;
        var number = sourceNumber;
        var power = 0;

        if (number >= (long)Math.Pow(10, 15) || number < 0)
        {
            return string.Empty;
        }

        while (number > 0)
        {
            var remainder = (int)(number % 1000);
            number /= 1000;

            switch (power)
            {
                case 12:
                    str = MakeText(remainder, hundreds, tens, from3till19, secondMale, firstMale, trillions) + str;
                    break;
                case 9:
                    str = MakeText(remainder, hundreds, tens, from3till19, secondMale, firstMale, billions) + str;
                    break;
                case 6:
                    str = MakeText(remainder, hundreds, tens, from3till19, secondMale, firstMale, millions) + str;
                    break;
                case 3:
                    switch (textCase)
                    {
                        case TextCase.Accusative:
                            str = MakeText(remainder, hundreds, tens, from3till19, secondFemale, firstFemaleAccusative, thousandsAccusative) + str;
                            break;
                        default:
                            str = MakeText(remainder, hundreds, tens, from3till19, secondFemale, firstFemale, thousands) + str;
                            break;
                    }

                    break;
                default:
                    string[] powerArray = { };
                    switch (textCase)
                    {
                        case TextCase.Genitive:
                            str = MakeText(remainder, hundredsGenetive, tensGenetive, from3till19Genetive, isMale ? secondMaleGenetive : secondFemaleGenetive, isMale ? firstMaleGenetive : firstFemale, powerArray) + str;
                            break;
                        case TextCase.Accusative:
                            str = MakeText(remainder, hundreds, tens, from3till19, isMale ? secondMale : secondFemale, isMale ? firstMale : firstFemaleAccusative, powerArray) + str;
                            break;
                        default:
                            str = MakeText(remainder, hundreds, tens, from3till19, isMale ? secondMale : secondFemale, isMale ? firstMale : firstFemale, powerArray) + str;
                            break;
                    }

                    break;
            }

            power += 3;
        }

        if (sourceNumber == 0)
        {
            str = zero + " ";
        }

        if (str != string.Empty && firstCapital)
        {
            str = string.Concat(str[..1].ToUpper(), str.AsSpan(1));
        }

        return str.Trim();
    }

    /// <summary>
    /// Десять тысяч рублей 67 копеек.
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="firstCapital"></param>
    /// <returns></returns>
    public static string CurrencyToTxt(double amount, bool firstCapital)
    {
        //Десять тысяч рублей 67 копеек
        var rublesAmount = (long)Math.Floor(amount);
        var copecksAmount = ((long)Math.Round(amount * 100)) % 100;
        var lastRublesDigit = LastDigit(rublesAmount);
        var lastCopecksDigit = LastDigit(copecksAmount);

        var str = NumeralsToTxt(rublesAmount, TextCase.Nominative, true, firstCapital) + " ";

        if (IsPluralGenitive(lastRublesDigit))
        {
            str += rubles[3] + " ";
        }
        else if (IsSingularGenitive(lastRublesDigit))
        {
            str += rubles[2] + " ";
        }
        else
        {
            str += rubles[1] + " ";
        }

        str += $"{copecksAmount:00} ";

        if (IsPluralGenitive(lastCopecksDigit))
        {
            str += copecks[3] + " ";
        }
        else if (IsSingularGenitive(lastCopecksDigit))
        {
            str += copecks[2] + " ";
        }
        else
        {
            str += copecks[1] + " ";
        }

        return str.Trim();
    }

    private static int LastDigit(long amount)
    {
        if (amount >= 100)
        {
            amount %= 100;
        }

        if (amount >= 20)
        {
            amount %= 10;
        }

        return (int)amount;
    }
}

public static class StringExtensions
{
    /// <summary>
    /// Метод, который преобразует первую букву строки в верхний регистр, а остальные буквы в нижний регистр.
    /// </summary>
    /// <param name="str">Входная строка</param>
    /// <returns>Преобразованная строка</returns>
    public static string ToFirstUpper(this string str)
    {
        // Проверяем, что строка не пустая или не состоит из пробельных символов
        if (!string.IsNullOrWhiteSpace(str))
        {
            // Преобразуем первый символ строки в верхний регистр и добавляем остальную часть строки в нижнем регистре
            return str[..1].ToUpper() + (str.Length > 1 ? str[1..].ToLower() : "");
        }
        else
        {
            // Возвращаем пустую строку, если входная строка пустая или состоит только из пробельных символов
            return string.Empty;
        }
    }


    /// <summary>
    /// Метод, который преобразует полное имя (имя и фамилию) в сокращенное имя (только первые буквы имени и фамилии с точкой).
    /// </summary>
    /// <param name="fullName">Полное имя (имя и фамилия)</param>
    /// <returns>Сокращенное имя</returns>
    public static string ConvertToShortName(this string fullName)
    {
        // Проверяем, что строка не пустая или не состоит из пробельных символов
        if (string.IsNullOrWhiteSpace(fullName))
        {
            return null; // Возвращаем null, если входная строка пустая или состоит только из пробельных символов
        }

        // Разделяем полное имя на составляющие (имя и фамилию) по пробелам
        string[] arrName = fullName.Split(' ');
        string shortName = string.Empty; // Создаем переменную для сокращенного имени и инициализируем ее пустой строкой

        // Проходимся по каждой части имени и фамилии
        foreach (var namePart in arrName.Where(part => !string.IsNullOrWhiteSpace(part)))
        {
            // Если сокращенное имя пусто, то присваиваем ему первую часть имени или фамилии
            if (string.IsNullOrEmpty(shortName))
            {
                shortName = namePart;
            }
            else
            {
                // Если сокращенное имя уже не пусто, то добавляем к нему первую букву следующей части имени с точкой
                shortName += " " + namePart.ElementAt(0) + ".";
            }
        }

        // Возвращаем сокращенное имя
        return shortName;
    }

    /// <summary>
    /// Метод, который преобразует полное имя (имя и фамилию) в массив строк длиной не более трех элементов.
    /// </summary>
    /// <param name="fullName">Полное имя (имя и фамилия)</param>
    /// <returns>Массив строк, содержащий имя, фамилию и, возможно, отчество</returns>
    public static string[] ConvertToStringNames(this string fullName)
    {
        // Проверяем, что строка не пустая или не состоит из пробельных символов
        if (string.IsNullOrWhiteSpace(fullName))
        {
            return null; // Возвращаем null, если входная строка пустая или состоит только из пробельных символов
        }

        // Разделяем полное имя на составляющие (имя, фамилию и, возможно, отчество) по пробелам
        string[] arrayName = fullName.Split(' ');
        string[] arrayFullName = new string[3]; // Создаем массив для хранения имени, фамилии и отчества

        // Проходимся по каждой части имени, фамилии и отчества
        for (int i = 0; i < arrayName.Length && i < 3; i++)
        {
            // Если часть имени, фамилии или отчества не пуста, то добавляем ее в массив, обрезая пробельные символы по краям
            if (!string.IsNullOrWhiteSpace(arrayName[i]))
            {
                arrayFullName[i] = arrayName[i].Trim();
            }
        }

        // Возвращаем массив с именем, фамилией и, возможно, отчеством
        return arrayFullName;
    }
}