
/// <summary>
/// Summary description for WorkFlowSession
/// </summary>
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
public partial class WorkFlowSession 
{
	public WorkFlowSession( )
	{
		//
		// TODO: Add constructor logic here
		//
	}
    
    public WorkFlowSession(int statusId,int programId,string docNo,int branch_Id,int Lob_Id,int product_Id,string LasDocumentNo,int Document_Type)
    {
        WorkFlowStatusID = statusId;
        WorkFlowProgramId = programId;
        WorkFlowDocumentNo = docNo;
        BranchID = branch_Id;
        LOBId = Lob_Id;
        ProductId = product_Id;
        LastDocumentNo = LasDocumentNo;
        Document_Type = Document_Type;
    }
    #region Work Flow Properties

    // # introduced for Load Admin 
     public string PANUM 
       {
           get
           {
               return ((string)Utility.Load("PANUM", ""));
           }

           set
           {
               Utility.store("PANUM", value);
           }
       }

     public string SANUM 
       {
           get
           {
               return ((string)Utility.Load("SANUM", ""));
           }

           set
           {
               Utility.store("SANUM", value);
           }
       }

    public int WorkFlowStatusID
    {
        get
        {
            return ((int)Utility.Load("WorkFlowStatusID", ""));
        }

        set
        {
            Utility.store("WorkFlowStatusID", value);
        }

    }

    public int WorkFlowProgramId
    {
        get
        {
            return ((int)Utility.Load("WorkFlowProgramId", ""));
        }

        set
        {
            Utility.store("WorkFlowProgramId", value);
        }
    }

    public string WorkFlowDocumentNo
    {
        get
        {
            return ((string)Utility.Load("WorkFlowDocumentNo", ""));
        }

        set
        {
            Utility.store("WorkFlowDocumentNo", value);
        }
    }

    public string LastDocumentNo 
    {
        get
        {
            return ((string)Utility.Load("LastDocumentNo", ""));
        }
        set
        {
            Utility.store("LastDocumentNo", value);
        }
    }

    public int BranchID
    {
        get
        {
            return ((int)Utility.Load("BranchID", ""));
        }

        set
        {
            Utility.store("BranchID", value);
        }
    }

    public int LOBId
    {
        get
        {
            return ((int)Utility.Load("LOBId", ""));
        }

        set
        {
            Utility.store("LOBId", value);
        }
    }

    public int ProductId { 
        get
        {
            return ((int)Utility.Load("ProductId", ""));
        }

        set
        {
            Utility.store("ProductId", value);
        }
    }

    public List<DataRow> WFPageURL
    {
        get
        {
            return ((List<DataRow>)Utility.Load("WFNextPageURL", ""));
        }

        set
        {
            Utility.store("WFNextPageURL", value);
        }
    }

    public string WFNextPageURL
    {
        get
        {
            return ((string)Utility.Load("WFNextPageURL", ""));
        }

        set
        {
            Utility.store("WFNextPageURL", value);
        }
    }

    public DataTable WorkFlowScreens
    {
        get
        {
            return ((DataTable)Utility.Load("WorkFlowScreens", ""));
        }

        set
        {
            Utility.store("WorkFlowScreens", value);
        }
    }

    public string WorkFlowSequence
    {
        get
        {
            return ((string)Utility.Load("WorkFlowSequence", ""));
        }
        set
        {
            Utility.store("WorkFlowSequence", value);
        }
    }

    public int Document_Type
    {
        get
        {
            return ((int)Utility.Load("Document_Type", ""));
        }

        set
        {
            Utility.store("Document_Type", value);
        }
    }


    #endregion


  
}

public struct SessionValues
{
    public int LOBId { get; set; }
    public int BranchID { get; set; }
    public int ProductId { get; set; }
    public int SelecteDocument { get; set; }
    public int SelectedProgramId { get; set; }
    public string SelectedDocumentNo { get; set; }
    public string ReferenceDocNo { get; set; }
    public string WFSequenceID { get; set; }
}    





