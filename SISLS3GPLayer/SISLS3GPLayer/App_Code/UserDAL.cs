#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Program ID						: 
/// Program Description				: To capture Login details
/// LLD Reference					: 
/// Name of the Programmer			: Venkatesan S
/// Reference(Call/Pin)Description	: SIS/SL/09/N/063
/// Creation Date					: 09-Feb-2010
/// <Program Summary>
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.ObjectBuilder2;


namespace SmartLendLiteNxgDALHelper.ADM
{
    public class UserDAL
    {

        /// <summary>
        /// <see cref="Validate the user login"/>
        /// </summary>
        /// <param name="userLoginID">Pass User Login ID</param>
        /// <param name="password">Pass user Password</param>
        /// <returns>return type as int</returns>

        public int ValidateLogin(string userLoginID, string password, out int CompanyID, out int UserID, out string CompanyName,string CompanyCode)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase("connectionString");
                DbCommand command = db.GetStoredProcCommand("dbo.NXG_Validate_Login");
                db.AddInParameter(command, "@UserLoginID", DbType.String, userLoginID);
                db.AddInParameter(command, "@Password", DbType.String, password);
                db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int32));
                db.AddOutParameter(command, "@CompanyID", DbType.Int32, sizeof(Int32));
                db.AddOutParameter(command, "@UserID", DbType.Int32, sizeof(Int32));
                db.AddOutParameter(command, "@CompanyName", DbType.String, 50);
                db.AddInParameter(command, "@CompanyCode", DbType.String, CompanyCode);
                db.ExecuteNonQuery(command);
                int errorCode = (command.Parameters["@ErrorCode"].Value != DBNull.Value) ? (int)command.Parameters["@ErrorCode"].Value : 0;
                CompanyID = (command.Parameters["@CompanyID"].Value != DBNull.Value) ? (int)command.Parameters["@CompanyID"].Value : 0;
                UserID = (command.Parameters["@UserID"].Value != DBNull.Value) ? (int)command.Parameters["@UserID"].Value : 0;
                CompanyName = Convert.ToString((command.Parameters["@CompanyName"].Value != DBNull.Value) ? command.Parameters["@CompanyName"].Value : 0);
                CompanyCode = Convert.ToString((command.Parameters["@CompanyCode"].Value != DBNull.Value) ? command.Parameters["@CompanyCode"].Value : 0);
                return errorCode;
            }

            catch (Exception Objex)
            {
                throw Objex;
            }
        }
    }
}




