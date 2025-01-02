using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;


public static partial class LicenceEnc
{

    public static String Encrypt(String EncryptText)
    {
        Dictionary<String, String> Dict = new Dictionary<String, String>();
        Dict.Add("0", "C");
        Dict.Add("1", "F");
        Dict.Add("2", "G");
        Dict.Add("3", "Z");
        Dict.Add("4", "R");
        Dict.Add("5", "P");
        Dict.Add("6", "T");
        Dict.Add("7", "B");
        Dict.Add("8", "H");
        Dict.Add("9", "Q");

        char[] EncryptArray = EncryptText.ToCharArray();
        String Enc = "";

        for (int i = 0; i < EncryptArray.Length; i++)
        {
            Enc += GetValue(Dict, EncryptArray[i].ToString());
        }
        return DoubleEncrypt(Enc);
    }


    public static String DoubleEncrypt(String Encrypt)
    {
        char[] EncryptArray = Encrypt.ToCharArray();

        String DEnc = "";
        //4-6A3W-85X4-2

        String CompleteEncrypedText = "";
        for (int i = 0; i < EncryptArray.Length; i++)
        {
            DEnc = DecryptLogic(EncryptArray[i].ToString());// GetValue(Alphabets, EncryptArray[i].ToString());
            String[] Logic = { "4", "-6", "A", "3", "W", "-8", "5", "X", "4", "-2" };
            for (int j = 0; j < Logic.Length; j++)
            {
                try
                {
                    int Got = Convert.ToInt32(DEnc);
                    int Log = Convert.ToInt32(Logic[j]);
                    int Act = Got + Log;
                    if (Act > 26)
                        Act = Log;
                    CompleteEncrypedText += EncryptLogic(Act.ToString());
                }
                catch
                {
                    CompleteEncrypedText += Logic[j];
                }
            }
        }
        return CompleteEncrypedText;
    }
    public static String EncryptLogic(String Enc)
    {

        Dictionary<String, String> Alphabets = new Dictionary<String, String>();
        Alphabets.Add("1", "A");
        Alphabets.Add("2", "B");
        Alphabets.Add("3", "C");
        Alphabets.Add("4", "D");
        Alphabets.Add("5", "E");
        Alphabets.Add("6", "F");
        Alphabets.Add("7", "G");
        Alphabets.Add("8", "H");
        Alphabets.Add("9", "I");
        Alphabets.Add("10", "J");
        Alphabets.Add("11", "K");
        Alphabets.Add("12", "L");
        Alphabets.Add("13", "M");
        Alphabets.Add("14", "N");
        Alphabets.Add("15", "O");
        Alphabets.Add("16", "P");
        Alphabets.Add("17", "Q");
        Alphabets.Add("18", "R");
        Alphabets.Add("19", "S");
        Alphabets.Add("20", "T");
        Alphabets.Add("21", "U");
        Alphabets.Add("22", "V");
        Alphabets.Add("23", "W");
        Alphabets.Add("24", "X");
        Alphabets.Add("25", "Y");
        Alphabets.Add("26", "Z");

        if (GetValue(Alphabets, Enc) == "The given key was not present in the dictionary.")
            Enc = Convert.ToInt32(Alphabets.Count + Convert.ToInt32(Enc)).ToString();
        return GetValue(Alphabets, Enc);
    }
    //Decryption by using every character 
    //Code Start
    //public String Decrpyt(String DecryptText)
    //{
    //    //String Decrypted = "";
    //    String Dec = DoubleDecrpyt(DecryptText);

    //    if (Dec == "Invalid Key!")
    //        return Dec;


    //    String PreviousKey, CurrentKey = "";


    //    //for (int i = 0; i < Dec.Length; i++)
    //    //{
    //        if (Dec.Length == 10)
    //        {
    //            Dec = Dec.Remove(2, 1).Remove(3, 1).Remove(5, 1);
    //        }
    //        else if (Dec.Length == 20)
    //        {
    //            Dec = Dec.Remove(2, 1).Remove(3, 1).Remove(5, 1).Remove(9, 1).Remove(10, 1).Remove(12, 1);
    //        }
    //        else if (Dec.Length == 30)
    //        {
    //            Dec = Dec.Remove(2, 1).Remove(3, 1).Remove(5, 1).Remove(9, 1).Remove(10, 1).Remove(12, 1).Remove(16, 1).Remove(17, 1).Remove(19, 1);
    //        }
    //        else if (Dec.Length == 40)
    //        {
    //            Dec = Dec.Remove(2, 1).Remove(3, 1).Remove(5, 1).Remove(9, 1).Remove(10, 1).Remove(12, 1).Remove(16, 1).Remove(17, 1).Remove(19, 1).Remove(23, 1).Remove(24, 1).Remove(26, 1);
    //        }
    //    //}


    //    Dictionary<String, String> Dict = new Dictionary<String, String>();
    //    Dict.Add("C", "0");
    //    Dict.Add("F", "1");
    //    Dict.Add("G", "2");
    //    Dict.Add("Z", "3");
    //    Dict.Add("R", "4");
    //    Dict.Add("P", "5");
    //    Dict.Add("T", "6");
    //    Dict.Add("B", "7");
    //    Dict.Add("H", "8");
    //    Dict.Add("Q", "9");



    //    char[] DecArray = Dec.ToCharArray();

    //    String One = "", Two = "", Three = "", Four = "";

    //    for (int i = 0; i < DecArray.Length; i++)
    //    {
    //        if (i <= 6)
    //        {
    //            PreviousKey = GetValue(Dict, DecArray[0].ToString());
    //            CurrentKey = GetValue(Dict, DecArray[i].ToString());
    //            if (PreviousKey != CurrentKey)
    //            {
    //                return "Invalid Key!";
    //            }
    //            One = PreviousKey;
    //        }
    //        else if (i > 6 && i <= 13)
    //        {
    //            if (i == 7)
    //                PreviousKey = "";
    //            PreviousKey = GetValue(Dict, DecArray[7].ToString());
    //            CurrentKey = GetValue(Dict, DecArray[i].ToString());
    //            if (PreviousKey != CurrentKey)
    //            {
    //                return "Invalid Key!";
    //            }
    //            Two = PreviousKey;
    //        }
    //        else if (i > 13 && i <= 20)
    //        {
    //            if (i == 14)
    //                PreviousKey = "";
    //            PreviousKey = GetValue(Dict, DecArray[14].ToString());
    //            CurrentKey = GetValue(Dict, DecArray[i].ToString());
    //            if (PreviousKey != CurrentKey)
    //            {
    //                return "Invalid Key!";
    //            }
    //            Three = PreviousKey;
    //        }
    //        else if (i > 20 && i <= 28)
    //        {
    //            if (i == 22)
    //                PreviousKey = "";
    //            PreviousKey = GetValue(Dict, DecArray[22].ToString());
    //            CurrentKey = GetValue(Dict, DecArray[i].ToString());
    //            if (PreviousKey != CurrentKey)
    //            {
    //                return "Invalid Key!";
    //            }
    //            Four = PreviousKey;
    //        }
    //    }
    //    return One + Two + Three + Four;
    //}
    //private String DoubleDecrpyt(String DecryptText)
    //{
    //    if (DecryptText.Length == 40)
    //    {
    //      //  DecryptText = DecryptText.ToCharArray().GetValue(0).ToString() + DecryptText.ToCharArray().GetValue(10).ToString() + DecryptText.ToCharArray().GetValue(20).ToString() + DecryptText.ToCharArray().GetValue(30).ToString();
    //    }
    //    else if (DecryptText.Length == 30)
    //    {
    //      //  DecryptText = DecryptText.ToCharArray().GetValue(0).ToString() + DecryptText.ToCharArray().GetValue(10).ToString() + DecryptText.ToCharArray().GetValue(20).ToString();
    //    }
    //    else if (DecryptText.Length == 20)
    //    {
    //      //  DecryptText = DecryptText.ToCharArray().GetValue(0).ToString() + DecryptText.ToCharArray().GetValue(10).ToString();
    //    }
    //    else if (DecryptText.Length == 10)
    //    {
    //      //  DecryptText = DecryptText.ToCharArray().GetValue(0).ToString();
    //    }
    //    else
    //    {
    //        return "Invalid Key!";
    //    }


    //    char[] DecryptArray = DecryptText.ToCharArray();
    //    String Dec = "";

    //    String CompleteDecrypedText = "";
    //    for (int i = 0; i < DecryptArray.Length; i++)
    //    {
    //        Dec = DecryptLogic(DecryptArray[i].ToString());
    //        String[] Logic = { "-4", "6", "A", "-3", "W", "8", "-5", "X", "-4", "2" };
    //        try
    //        {
    //            int Got = Convert.ToInt32(Dec);
    //            int Log=0;
    //            if (i <= 9) {
    //                Log = Convert.ToInt32(Logic[i]);
    //            }
    //            else if (i > 9 && i <= 19) {
    //                Log = Convert.ToInt32(Logic[i -10]);
    //            }
    //            else if (i > 19 && i <= 29)
    //            {
    //                Log = Convert.ToInt32(Logic[i - 20]);
    //            }
    //            else if (i > 29 && i <= 40)
    //            {
    //                Log = Convert.ToInt32(Logic[i - 30]);
    //            } 
    //            int Act = Got + Log;
    //            if (Act == 0) {
    //                Act = 26;
    //            }
    //            else if (Act > 26) {
    //                Act = Act - 26;
    //            }

    //            CompleteDecrypedText += EncryptLogic(Act.ToString());
    //        }
    //        catch
    //        {
    //            if (i <= 9)
    //            {
    //                CompleteDecrypedText += Logic[i];
    //            }
    //            else if (i > 9 && i <= 19) {
    //                CompleteDecrypedText += Logic[i-10];
    //            }
    //            else if (i > 19 && i <= 29)
    //            {
    //                CompleteDecrypedText += Logic[i - 20];
    //            }
    //            else if (i > 29 && i <= 40)
    //            {
    //                CompleteDecrypedText += Logic[i - 30];
    //            }
    //        }
    //    }
    //    return CompleteDecrypedText;
    //}
    //Code End
    ////Decryption by using every character
    public static String DecryptLogic(String Enc)
    {

        Dictionary<String, String> Alphabets = new Dictionary<String, String>();
        Alphabets.Add("A", "1");
        Alphabets.Add("B", "2");
        Alphabets.Add("C", "3");
        Alphabets.Add("D", "4");
        Alphabets.Add("E", "5");
        Alphabets.Add("F", "6");
        Alphabets.Add("G", "7");
        Alphabets.Add("H", "8");
        Alphabets.Add("I", "9");
        Alphabets.Add("J", "10");
        Alphabets.Add("K", "11");
        Alphabets.Add("L", "12");
        Alphabets.Add("M", "13");
        Alphabets.Add("N", "14");
        Alphabets.Add("O", "15");
        Alphabets.Add("P", "16");
        Alphabets.Add("Q", "17");
        Alphabets.Add("R", "18");
        Alphabets.Add("S", "19");
        Alphabets.Add("T", "20");
        Alphabets.Add("U", "21");
        Alphabets.Add("V", "22");
        Alphabets.Add("W", "23");
        Alphabets.Add("X", "24");
        Alphabets.Add("Y", "25");
        Alphabets.Add("Z", "26");

        if (GetValue(Alphabets, Enc) == "The given key was not present in the dictionary.")
            Enc = Convert.ToInt32(Alphabets.Count + Convert.ToInt32(Enc)).ToString();
        return GetValue(Alphabets, Enc);
    }


    // Basic Method
    #region Decryption Logic
    public static String Decrpyt(String DecryptText)
    {
        String Decrypted = "";
        String Dec = DoubleDecrpyt(DecryptText);

        if (Dec == "Invalid Key!")
            return Dec;





        Dictionary<String, String> Dict = new Dictionary<String, String>();
        Dict.Add("C", "0");
        Dict.Add("F", "1");
        Dict.Add("G", "2");
        Dict.Add("Z", "3");
        Dict.Add("R", "4");
        Dict.Add("P", "5");
        Dict.Add("T", "6");
        Dict.Add("B", "7");
        Dict.Add("H", "8");
        Dict.Add("Q", "9");

        char[] DecArray = Dec.ToCharArray();

        for (int i = 0; i < DecArray.Length; i++)
        {
            Decrypted += GetValue(Dict, DecArray[i].ToString());
        }
        return Decrypted;
    }
    public static String DoubleDecrpyt(String DecryptText)
    {
        if (DecryptText.Length == 40)
        {
            DecryptText = DecryptText.ToCharArray().GetValue(0).ToString() + DecryptText.ToCharArray().GetValue(10).ToString() + DecryptText.ToCharArray().GetValue(20).ToString() + DecryptText.ToCharArray().GetValue(30).ToString();
        }
        else if (DecryptText.Length == 30)
        {
            DecryptText = DecryptText.ToCharArray().GetValue(0).ToString() + DecryptText.ToCharArray().GetValue(10).ToString() + DecryptText.ToCharArray().GetValue(20).ToString();
        }
        else if (DecryptText.Length == 20)
        {
            DecryptText = DecryptText.ToCharArray().GetValue(0).ToString() + DecryptText.ToCharArray().GetValue(10).ToString();
        }
        else if (DecryptText.Length == 10)
        {
            DecryptText = DecryptText.ToCharArray().GetValue(0).ToString();
        }
        else
        {
            return "Invalid Key!";
        }


        char[] DecryptArray = DecryptText.ToCharArray();
        String Dec = "";

        String CompleteDecrypedText = "";
        for (int i = 0; i < DecryptArray.Length; i++)
        {
            Dec = DecryptLogic(DecryptArray[i].ToString());
            String[] Logic = { "-4", "6", "A", "-3", "W", "8", "-5", "X", "-4", "2" };
            try
            {
                int Got = Convert.ToInt32(Dec);
                int Log = Convert.ToInt32(Logic[0]);
                int Act = Got + Log;
                if (Act == 0)
                    Act = 26;
                CompleteDecrypedText += EncryptLogic(Act.ToString());
            }
            catch
            {
                CompleteDecrypedText += Logic[0];
            }
        }
        return CompleteDecrypedText;
    }
    #endregion


    // Common Mehtod
    public static String GetValue(Dictionary<String, String> Dict, String a)
    {
        try
        {
            return Dict[a].ToString();
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
}

