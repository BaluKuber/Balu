using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;


/// <summary>
/// This class is used to store and retrive session data in S3G project
/// </summary>
public class S3GSession
{
    public S3GSession()
	{
		
	}

    public S3GSession(string strDatformat,string strCurrencyName,string strCurrencyCode,int intCurrencyId,int intPINDigits,string strPINType,int intGpsPrefix,int intGpsSuffix, string strCompanyCountry)
    
    {
        ProDateFormatRW = strDatformat;
        ProCurrencyNameRW = strCurrencyName;
        ProCurrencyCodeRW = strCurrencyCode;
        ProCurrencyIdRW = intCurrencyId;
        ProPINCodeDigitsRW=intPINDigits;
        ProPINCodeTypeRW = strPINType;
        ProGpsPrefixRW = intGpsPrefix;
        ProGpsSuffixRW = intGpsSuffix;
        if (strCompanyCountry != "")
        {
            ProCompanyCountry = strCompanyCountry;
        }
    }

    
   public string ProDateFormatRW
    {
        get
        {
            return ((string)Utility.Load("DateFormat", ""));
        }

        set
        {
            Utility.store("DateFormat", value);
        }

    }
    public string ProCurrencyCodeRW
    {
        get
        {
            return ((string)Utility.Load("CurrencyCode", ""));
        }

        set
        {
            Utility.store("CurrencyCode", value);
        }

    }
    public string ProPINCodeTypeRW
    {
        get
        {
            return ((string)Utility.Load("PINCode_Type", ""));
        }

        set
        {
            Utility.store("PINCode_Type", value);
        }

    }
    public int ProPINCodeDigitsRW
    {
        get
        {
            return (Convert.ToInt32(Utility.Load("PINCode_Digits", "")));
        }

        set
        {
            Utility.store("PINCode_Digits", value);
        }

    }
    public string ProCurrencyNameRW
    {
        get
        {
            return ((string)Utility.Load("CurrencyName", ""));
        }

        set
        {
            Utility.store("CurrencyName", value);
        }

    }
    public int ProCurrencyIdRW
    {
        get
        {
            return ((int)Utility.Load("Currencyid", ""));
        }

        set
        {
            Utility.store("Currencyid", value);
        }

    }

    public int ProGpsPrefixRW
    {
        get
        {
            return ((int)Utility.Load("GpsPrefix", ""));
        }

        set
        {
            Utility.store("GpsPrefix", value);
        }

    }
    public int ProGpsSuffixRW
    {
        get
        {
            return ((int)Utility.Load("GpsSuffix", ""));
        }

        set
        {
            Utility.store("GpsSuffix", value);
        }

    }
    public string ProCompanyCountry
    {
        get
        {
            return ((string)Utility.Load("CompanyCountry", ""));
        }

        set
        {
            Utility.store("CompanyCountry", value);
        }

    }
   
}
