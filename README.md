# sm-crypto
国密算法sm2、sm3移植自https://github.com/JuneAndGreen/sm-crypto
在此感谢原作者
# GitHub主页
https://github.com/Mole365/sm-crypto
# NuGet主页
https://www.nuget.org/packages/SMCrypto
# 安装
```
dotnet add package SMCrypto --version 1.0.2
```
# 使用

## sm2

### 加密解密
```c#
// js加密后C#解密正常，C#互相加解密正常，暂不支持C#加密js解密
using SMCrypto;

SM2Key sm2Key= SM2.GenerateKeyPairHex();
string encryptData = SM2.DoEncrypt(msgString,sm2Key.PubKey,SM2.CipherMode.C1C3C2);
string decryptData = SM2.DoDecrypt(encryptData,sm2Key.PriKey,SM2.CipherMode.C1C3C2).ToString();

```

## sm3
```c#
using SMCrypto;

string hashData = SM3.StrSum("abc"); // 杂凑
```

