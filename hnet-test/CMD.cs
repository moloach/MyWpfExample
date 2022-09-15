using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNetTest
{
    /// <summary>
    /// 命令实体模型
    /// </summary>
    public class CMD
    {
        EnumType.DataEncode _DataEncode = EnumType.DataEncode.Hex;
        string _strCMD = "";
        byte[] _byteCMD = null;

        public CMD(EnumType.DataEncode DataEncode, byte[] data)
        {
            _DataEncode = DataEncode;
            _byteCMD = data;
            switch (_DataEncode)
            {
                case EnumType.DataEncode.Hex:
                    foreach (byte b in _byteCMD)
                    {
                        _strCMD += string.Format("{0:X2} ", b);
                    }
                    _strCMD = _strCMD.TrimEnd();
                    break;
                case EnumType.DataEncode.ASCII:
                    _strCMD = new ASCIIEncoding().GetString(_byteCMD);
                    break;
                case EnumType.DataEncode.UTF8:
                    _strCMD = new UTF8Encoding().GetString(_byteCMD);
                    break;
                case EnumType.DataEncode.GB2312:
                    _strCMD = Encoding.GetEncoding("GB2312").GetString(_byteCMD);
                    break;
            }
        }

        /// <summary>
        /// 命令类型
        /// </summary>
        public EnumType.DataEncode ContentType
        {
            get
            {
                return _DataEncode;
            }
        }

        /// <summary>
        /// 命令类型
        /// </summary>
        public string ContentTypeName
        {
            get
            {
                return _DataEncode.ToString();
            }
        }

        /// <summary>
        /// 命令字符串
        /// </summary>
        public string Text
        {
            get
            {
                return _strCMD;
            }
        }

        /// <summary>
        /// 命令字节数组
        /// </summary>
        public byte[] Bytes
        {
            get
            {
                return _byteCMD;
            }
        }
    }
}
