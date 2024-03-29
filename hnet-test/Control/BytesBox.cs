﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HNetTest.Control
{
    public class BytesBox : TextBox
    {
        public BytesBox()
        {
        }

        #region 属性
        /// <summary>
        /// 编码是否正在转换
        /// </summary>
        bool EncodeChanging = false;
        EnumType.DataEncode _EncodeType = EnumType.DataEncode.Hex;
        [Description("可设置Hex,ASCII"), Category("输入格式设置")]
        public EnumType.DataEncode EncodeType
        {
            set
            {
                EncodeChanging = true;
                byte[] data = null;
                if (this.Text.Length > 0)
                {
                    switch (_EncodeType)
                    {
                        case EnumType.DataEncode.Hex:
                            string[] HexStr = this.Text.Trim().Split(' ');
                            data = new byte[HexStr.Length];
                            for (int i = 0; i < HexStr.Length; i++)
                            {
                                data[i] = (byte)(Convert.ToInt32(HexStr[i], 16));
                            }
                            break;
                        case EnumType.DataEncode.ASCII:
                            data = new ASCIIEncoding().GetBytes(this.Text);
                            break;
                        case EnumType.DataEncode.UTF8:
                            data = new UTF8Encoding().GetBytes(this.Text);
                            break;
                        case EnumType.DataEncode.GB2312:
                            data = Encoding.GetEncoding("GB2312").GetBytes(this.Text);
                            break;
                    }
                    switch (value)
                    {
                        case EnumType.DataEncode.Hex:
                            if (this.Text.Length > 0)
                            {
                                StringBuilder sb = new StringBuilder();
                                for (int i = 0; i < data.Length; i++)
                                {
                                    sb.AppendFormat("{0:x2} ", data[i]);
                                }
                                this.Text = sb.ToString().Trim().ToUpper();
                            }
                            break;
                        case EnumType.DataEncode.ASCII:
                            this.Text = new ASCIIEncoding().GetString(data);
                            break;
                        case EnumType.DataEncode.UTF8:
                            this.Text = new UTF8Encoding().GetString(data);
                            break;
                        case EnumType.DataEncode.GB2312:
                            this.Text = Encoding.GetEncoding("GB2312").GetString(data);
                            break;
                    }
                }
                _EncodeType = value;
                this.SelectionStart = this.Text.Length;
                EncodeChanging = false;
            }
            get
            {
                return _EncodeType;
            }
        }
        #endregion

        #region 输入控制
        protected override void OnTextChanged(EventArgs e)
        {
            if (EncodeChanging == false && _EncodeType == EnumType.DataEncode.Hex)
            {//Hex输入
                string Content = this.Text.Replace(" ", "");//获取去除空格后的字符内容
                int SpaceCount = Content.Length / 2;
                int StartIndex = 2;
                for (int i = 0; i < SpaceCount; i++)
                {
                    Content = Content.Insert(StartIndex, " ");
                    StartIndex = StartIndex + 3;
                }
                this.Text = Content.TrimEnd().ToUpper();
                this.SelectionStart = this.Text.Length;
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (e.KeyChar == 0x08//退格键
                || e.KeyChar == 0x03//拷贝
                || e.KeyChar == 0x18//剪切
                )
            {
                e.Handled = false;
                return;
            }

            if (e.KeyChar == 0x16)//粘贴
            {//粘贴前数据格式检查
                if (CheckPaste())
                {
                    e.Handled = false;
                    return;
                }
            }

            switch (_EncodeType)
            {
                case EnumType.DataEncode.Hex:
                    if ((e.KeyChar >= '0' && e.KeyChar <= '9')//数字0-9键   
                     || (e.KeyChar >= 'A' && e.KeyChar <= 'F')//字母A-F 
                     || (e.KeyChar >= 'a' && e.KeyChar <= 'f')//字母a-f 
                     )
                    {
                        e.Handled = false;
                    }
                    else
                    {
                        e.Handled = true;
                    }
                    break;
                case EnumType.DataEncode.ASCII:
                    if ((e.KeyChar >= 0x20 && e.KeyChar <= 0x7E)
                     || e.KeyChar == 0x0D)//回车键
                    {
                        e.Handled = false;
                    }
                    else
                    {
                        e.Handled = true;
                    }
                    break;
                case EnumType.DataEncode.UTF8:
                case EnumType.DataEncode.GB2312:
                    e.Handled = false;
                    break;
            }
        }

        /// <summary>
        /// 粘贴数据格式检查
        /// </summary>
        /// <returns></returns>
        private bool CheckPaste()
        {
            try
            {
                char[] PasteChar = Clipboard.GetDataObject().GetData(DataFormats.Text).ToString().ToCharArray();

                switch (_EncodeType)
                {
                    case EnumType.DataEncode.Hex:
                        foreach (char data in PasteChar)
                        {
                            if (!((data >= '0' && data <= '9')//数字0-9键   
                             || (data >= 'A' && data <= 'F')//字母A-F 
                             || (data >= 'a' && data <= 'f')//字母a-f
                             || data == 0x20))//空格
                            {
                                MessageBox.Show("粘贴数据含有非法字符，只能包含数字0-9,大写英文字母A-F,小写英文字母a-f以及空格！", "非法的粘贴", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }
                        }
                        break;
                    case EnumType.DataEncode.ASCII:
                        foreach (char data in PasteChar)
                        {
                            if (!((data >= 0x20 && data <= 0x7E)
                             || data == 0x0A
                             || data == 0x0D))//回车键
                            {
                                MessageBox.Show("粘贴数据含有非法字符，只能包含ASCII码字符！", "非法的粘贴", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }
                        }
                        break;
                    case EnumType.DataEncode.UTF8:
                    case EnumType.DataEncode.GB2312:
                        break;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 获取命令对象
        /// </summary>
        /// <returns></returns>
        public CMD GetCMD()
        {
            try
            {
                CMD Cmd = null;
                if (this.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("不允许内容为空！");
                    return null;
                }

                byte[] data;
                switch (_EncodeType)
                {
                    case EnumType.DataEncode.Hex:
                        string[] HexStr = this.Text.Trim().Split(' ');
                        data = new byte[HexStr.Length];
                        for (int i = 0; i < HexStr.Length; i++)
                        {
                            data[i] = (byte)(Convert.ToInt32(HexStr[i], 16));
                        }
                        Cmd = new CMD(EnumType.DataEncode.Hex, data);
                        break;
                    case EnumType.DataEncode.ASCII:
                        data = new ASCIIEncoding().GetBytes(this.Text.Trim());
                        Cmd = new CMD(EnumType.DataEncode.ASCII, data);
                        break;
                    case EnumType.DataEncode.UTF8:
                        data = new UTF8Encoding().GetBytes(this.Text);
                        Cmd = new CMD(EnumType.DataEncode.UTF8, data);
                        break;
                    case EnumType.DataEncode.GB2312:
                        data = Encoding.GetEncoding("GB2312").GetBytes(this.Text);
                        Cmd = new CMD(EnumType.DataEncode.GB2312, data);
                        break;
                }
                return Cmd;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 设置命令对象
        /// </summary>
        /// <param name="Cmd"></param>
        public void SetCMD(CMD Cmd)
        {
            try
            {
                this.EncodeType = Cmd.ContentType;
                this.Text = Cmd.Text;
                this.SelectionStart = this.Text.Length;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
    }
}
