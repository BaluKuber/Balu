using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using S3GBusEntity;
using System.Data;


namespace S3GServiceLayer
{
    // NOTE: If you change the interface name "ISystemAdmin" here, you must also update the reference to "ISystemAdmin" in Web.config.
    [ServiceContract]
    public interface ISystemAdmin
    {
        //[OperationContract]
        //[FaultContract(typeof(ClsPubFaultException))]
        //int FunPubAddProductInt(SerializationMode SerMode, byte[] bytesObjProductMasterDataTable_SERLAY);

        //[OperationContract]
        //[FaultContract(typeof(ClsPubFaultException))]
        //byte[] FunPubQueryProduct(SerializationMode SerMode, byte[] bytesObjProductMasterDataTable_SERLAY);

        //[OperationContract]
        //[FaultContract(typeof(ClsPubFaultException))]
        //int FunPubUpdateProductInt(SerializationMode SerMode, byte[] bytesObjProductMasterDataTable_SERLAY);

        //[OperationContract]
        //[FaultContract(typeof(ClsPubFaultException))]
        //int FunPubDeleteProductInt(SerializationMode SerMode, byte[] bytesObjProductMasterDataTable_SERLAY);
        
    }
    //[DataContract]
    //public class ProductMaster
    //{
    //    [DataMember]
    //    public S3GBusEntity.SystemAdmin.S3G_ProductMasterDataTable ProductTable
    //    {
    //        get;
    //        set;
    //    }
    //}
}
