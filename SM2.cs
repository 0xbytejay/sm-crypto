using System.Globalization;

namespace SMCrypto;

// See https://aka.ms/new-console-template for more information



using System.Text;

//扩展方法




public class SM2
{
     public  enum CipherMode
    {
        C1C2C3=0,
        C1C3C2=1,
    }


 
 
 public static string doDecrypt(string encryptData,string  privateKey,CipherMode cipherMode=CipherMode.C1C3C2)
 {
     BigInteger privateKeyI = new BigInteger(privateKey, 16);
     string c3 = encryptData.Substring(128, 64);
     string c2 = encryptData.Substring(128 + 64);
     if (cipherMode == CipherMode.C1C2C3)
     {
         c3 = encryptData.Substring(encryptData.Length - 64);
         c2 = encryptData.Substring(128, encryptData.Length - 128 - 64);
     }

     byte[] msg =SMUtils.hexToArray(c2);
     var sss = encryptData.Substring(0, 128);
     ECPointFp c1 = SMUtils.getGlobalCurve().decodePointHex("04" + encryptData.Substring(0, 128));
     ECPointFp p = c1.multiply(privateKeyI);
     byte[] x2 = SMUtils.hexToArray(p.getX().toBigInteger().ToString(16).PadLeft(64));
     byte[] y2 = SMUtils.hexToArray(p.getY().toBigInteger().ToString(16).PadLeft(64));
     var ct = 1;
     var offset = 0;
     // var t = [] ;// 256 位
     byte[] z = x2.Concat(y2).ToArray();
     byte[] n = new[] {(byte)(ct >> 24 & 0x00ff),(byte)(ct >> 16 & 0x00ff),(byte)(ct >> 8 & 0x00ff),(byte)(ct & 0x00ff) };
     var t = SM3.SM3Array(z.Concat(n).ToArray());
     ct++;
     offset = 0;
     for (int i = 0, len = msg.Length; i < len; i++) {
         // t = Ha1 || Ha2 || Ha3 || Ha4
         if (offset == t.Length)
         {
             t = SM3.SM3Array(z.Concat(n).ToArray());;
             ct++;
             offset = 0;
         }
         // c2 = msg ^ t
         msg[i] ^= (byte)(t[offset++] & 0xff);
     }
     // c3 = hash(x2 || msg || y2)
     byte[] tt = new byte[0];
     var checkC3 = SMUtils.arrayToHex(SM3.SM3Array(tt.Concat(x2).Concat(msg).Concat(y2).ToArray()));
     return Encoding.UTF8.GetString(msg);
 }

 
}



