//using SignLib;
//using SignLib.Certificates;
//using SignLib.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Permissions;
using iTextSharp.text.pdf;
using iTextSharp.text;
using org.bouncycastle.crypto;
using org.bouncycastle.pkcs;
using System.Collections;
using org.bouncycastle.crypto;
using iTextSharp.text.xml.xmp;

namespace S3GPdfSign
{
    public class Cert
    {
        #region Attributes

        private string path = "";
        private string password = "";
        private AsymmetricKeyParameter akp;
        private org.bouncycastle.x509.X509Certificate[] chain;

        #endregion

        #region Accessors
        public org.bouncycastle.x509.X509Certificate[] Chain
        {
            get { return chain; }
        }
        public AsymmetricKeyParameter Akp
        {
            get { return akp; }
        }

        public string Path
        {
            get { return path; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        #endregion

        #region Helpers

        private void processCert()
        {
            string alias = null;
            PKCS12Store pk12;

            //First we'll read the certificate file
            pk12 = new PKCS12Store(new FileStream(this.Path, FileMode.Open, FileAccess.Read), this.password.ToCharArray());

            //then Iterate throught certificate entries to find the private key entry
            IEnumerator i = pk12.aliases();
            while (i.MoveNext())
            {
                alias = ((string)i.Current);
                if (pk12.isKeyEntry(alias))
                    break;
            }

            this.akp = pk12.getKey(alias).getKey();
            X509CertificateEntry[] ce = pk12.getCertificateChain(alias);
            this.chain = new org.bouncycastle.x509.X509Certificate[ce.Length];
            for (int k = 0; k < ce.Length; ++k)
                chain[k] = ce[k].getCertificate();

            chain[0].getSignature();

        }
        #endregion

        #region Constructors
        public Cert()
        {

        }

        public Cert(string cpath)
        {
            this.path = cpath;
            this.processCert();
        }

        public Cert(string cpath, string cpassword)
        {
            this.path = cpath;
            this.Password = cpassword;
            this.processCert();
        }

        #endregion

    }

    public class MetaData
    {
        private Hashtable info = new Hashtable();

        public Hashtable Info
        {
            get { return info; }
            set { info = value; }
        }

        public Hashtable getMetaData()
        {
            return this.info;
        }
        public byte[] getStreamedMetaData()
        {
            MemoryStream os = new System.IO.MemoryStream();
            XmpWriter xmp = new XmpWriter(os, this.info);
            xmp.Close();
            return os.ToArray();
        }
    }

    public class PDFSigner
    {
        private string inputPDF = "";
        private string outputPDF = "";
        private Cert myCert;
        private MetaData ObjMetaData;

        public PDFSigner(string input, string output)
        {
            this.inputPDF = input;
            this.outputPDF = output;
        }

        public PDFSigner(string input, string output, Cert cert)
        {
            this.inputPDF = input;
            this.outputPDF = output;
            this.myCert = cert;
        }

        public PDFSigner(string input, string output, MetaData md)
        {
            this.inputPDF = input;
            this.outputPDF = output;
            this.ObjMetaData = md;
        }

        public PDFSigner(string input, string output, Cert cert, MetaData md)
        {
            this.inputPDF = input;
            this.outputPDF = output;
            this.myCert = cert;
            this.ObjMetaData = md;
        }

        public void Verify()
        {

        }

        public void Sign(string SigReason, string SigContact, string SigLocation, bool visible, PdfReader reader, string SignPosition)
        {
            //Activate MultiSignatures
            PdfStamper st = PdfStamper.CreateSignature(reader, new FileStream(this.outputPDF, FileMode.Create, FileAccess.Write), '\0', null, true);
            //To disable Multi signatures uncomment this line : every new signature will invalidate older ones !

            st.MoreInfo = ObjMetaData.getMetaData();
            st.XmpMetadata = ObjMetaData.getStreamedMetaData();
            PdfSignatureAppearance sap = st.SignatureAppearance;

            sap.SetCrypto(this.myCert.Akp, this.myCert.Chain, null, PdfSignatureAppearance.WINCER_SIGNED);
            if (SigReason != "")
                sap.Reason = SigReason;
            //sap.Image = Image.GetInstance("C:\\Users\\007750\\Documents\\Visual Studio 2012\\WebSites\\Dig_Sign\\signature_image.png");
            if (SigContact != "")
                sap.Contact = SigContact;

            if (SigLocation != "")
                sap.Location = SigLocation;

            if (SignPosition.ToUpper() == "RIGHT")
                sap.SetVisibleSignature(new iTextSharp.text.Rectangle(435, 100, 575, 50), reader.NumberOfPages, null); // Right
            else
                sap.SetVisibleSignature(new iTextSharp.text.Rectangle(40, 50, 200, 100), reader.NumberOfPages, null); // Left

            st.Close();
        }
    }

    public class PDFDigiSign
    {
        public void DigiPDFSign(string InputFile, string OutputFile, string SignPosition, string SignReason = "", string SignAuth = "Jagannath Sabat", string SignLocation = "")
        {
            S3GPdfSign.Cert myCert = null;

            PdfReader reader = new PdfReader(InputFile);

            System.Configuration.AppSettingsReader AppReader = new System.Configuration.AppSettingsReader();
            string strPfxFilePath = (string)AppReader.GetValue("PFXFILEPATH", typeof(string));
            string strPfxPassword = (string)AppReader.GetValue("PFXPASSWORD", typeof(string));

            myCert = new S3GPdfSign.Cert(strPfxFilePath, strPfxPassword);

            S3GPdfSign.MetaData MyMD = new S3GPdfSign.MetaData();
            MyMD.Info = reader.Info;

            S3GPdfSign.PDFSigner pdfs = new S3GPdfSign.PDFSigner(InputFile, OutputFile, myCert, MyMD);

            pdfs.Sign(SignReason, SignAuth, SignLocation, true, reader, SignPosition);
        }
    }
}
