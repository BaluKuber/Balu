using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using S3GBusEntity;
using S3GDALayer;
using System.ServiceModel.Activation;

namespace S3GServiceLayer
{
    // To Implement Service Compatablity
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    // NOTE: If you change the class name "SystemAdmin" here, you must also update the reference to "SystemAdmin" in Web.config.
    public class SystemAdmin : ISystemAdmin
    {
        
        #region ISystemAdmin Members

        //public byte[] FunPubQueryProduct(SerializationMode SerMode, byte[] bytesObjProductMasterDataTable_SERLAY)
        //{
        //    S3GBusEntity.SystemAdmin.S3G_ProductMaster_ViewDataTable dtProductMaster;
        //    //ClsPubProductMaster ObjPubProductMaster = new ClsPubProductMaster();
        //    //dtProductMaster = ObjPubProductMaster.FunPubQueryProductMasterList(SerMode,bytesObjProductMasterDataTable_SERLAY);
        //    byte[] bytesProdMaster= ClsPubSerialize.Serialize(dtProductMaster, SerializationMode.Binary);
        //    return bytesProdMaster;
        //}

   

        //#region ISystemAdmin Members


        //public ProductMaster FunPubQueryProduct_New()
        //{
        //    ProductMaster PdMaster = new ProductMaster();
        //    S3GBusEntity.SystemAdmin.S3G_ProductMasterDataTable dtProductMaster = new S3GBusEntity.SystemAdmin.S3G_ProductMasterDataTable();
        //    ClsPubProductMaster ObjPubProductMaster = new ClsPubProductMaster();
        //    dtProductMaster = ObjPubProductMaster.FunPubQueryProductMasterList();
        //    PdMaster.ProductTable = dtProductMaster;
        //    return PdMaster;
        //}

        //#endregion


        //public int FunPubAddProductInt(SerializationMode SerMode, byte[] bytesObjProductMasterDataTable_SERLAY)
        //{
        //    //ClsPubProductMaster ObjPubProductMaster = new ClsPubProductMaster();
        //   // return ObjPubProductMaster.FunProductMasterInsertInt(SerMode, bytesObjProductMasterDataTable_SERLAY);
        //}

        //public int FunPubUpdateProductInt(SerializationMode SerMode, byte[] bytesObjProductMasterDataTable_SERLAY)
        //{
        //   // ClsPubProductMaster ObjPubProductMaster = new ClsPubProductMaster();
        //   // return ObjPubProductMaster.FunProductMasterUpdateInt(SerMode, bytesObjProductMasterDataTable_SERLAY);
        //}

        //public int FunPubDeleteProductInt(SerializationMode SerMode, byte[] bytesObjProductMasterDataTable_SERLAY)
        //{
        //    //ClsPubProductMaster ObjPubProductMaster = new ClsPubProductMaster();
        //    //return ObjPubProductMaster.FunProductMasterDeleteInt(SerMode, bytesObjProductMasterDataTable_SERLAY);
        //}

        #endregion
    }
}
