using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace HNetTest
{
    public partial class FrmCMD : Form
    {
        public CMD NewCMD = null;

        public FrmCMD(CMD cmd)
        {
            InitializeComponent();
            txtContent.SetCMD(cmd);
            SetType(txtContent.EncodeType);
        }

        public FrmCMD()
        {
            InitializeComponent();
            SetType(EnumType.DataEncode.Hex);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            NewCMD = txtContent.GetCMD();
            if (NewCMD != null)
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void SetType(EnumType.DataEncode EncodeType)
        {
            switch (EncodeType)
            {
                case EnumType.DataEncode.Hex:
                    MSI_Hex.Checked = true;
                    CM_Check.Enabled = true;

                    MSI_ASCII.Enabled = true;
                    MSI_UTF8.Enabled = true;
                    MSI_GB2312.Enabled = true;

                    MSI_ASCII.Checked = false;
                    MSI_UTF8.Checked = false;
                    MSI_GB2312.Checked = false;
                    break;
                case EnumType.DataEncode.ASCII:
                    MSI_ASCII.Enabled = false;
                    MSI_UTF8.Enabled = false;
                    MSI_GB2312.Enabled = false;
                    CM_Check.Enabled = false;

                    MSI_Hex.Checked = false;
                    MSI_ASCII.Checked = true;
                    break;
                case EnumType.DataEncode.UTF8:
                    MSI_ASCII.Enabled = false;
                    MSI_UTF8.Enabled = false;
                    MSI_GB2312.Enabled = false;
                    CM_Check.Enabled = false;

                    MSI_Hex.Checked = false;
                    MSI_UTF8.Checked = true;
                    break;
                case EnumType.DataEncode.GB2312:
                    MSI_ASCII.Enabled = false;
                    MSI_UTF8.Enabled = false;
                    MSI_GB2312.Enabled = false;
                    CM_Check.Enabled = false;

                    MSI_Hex.Checked = false;
                    MSI_GB2312.Checked = true;
                    break;
            }
            txtContent.EncodeType = EncodeType;
        }

        #region 菜单操作
        private void MSI_Hex_Click(object sender, EventArgs e)
        {
            SetType(EnumType.DataEncode.Hex);
        }

        private void MSI_ASCII_Click(object sender, EventArgs e)
        {
            SetType(EnumType.DataEncode.ASCII);
        }

        private void MSI_UTF8_Click(object sender, EventArgs e)
        {
            SetType(EnumType.DataEncode.UTF8);
        }

        private void MSI_GB2312_Click(object sender, EventArgs e)
        {
            SetType(EnumType.DataEncode.GB2312);
        }
        
        /// <summary>
        /// 清空
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CM_Clear_Click(object sender, EventArgs e)
        {
            txtContent.Clear();
        }
        #endregion

        #region 校验码计算
        /// <summary>
        /// 校验和
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MS_CheckSum_Click(object sender, EventArgs e)
        {
            CMD cmd=txtContent.GetCMD();
            if (cmd != null)
            {
                byte check = BytesCheck.GetXOR(cmd.Bytes);
                txtContent.AppendText(Convert.ToString(check, 16).PadLeft(2, '0'));
            }
        }

        /// <summary>
        /// Modbus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MS_CRC16_Click(object sender, EventArgs e)
        {
             CMD cmd=txtContent.GetCMD();
             if (cmd != null)
             {
                 AppendToContent(BytesCheck.GetCRC16(cmd.Bytes, true));
             }
        }


        private void AppendToContent(byte[] check)
        {
            foreach (byte b in check)
            {
                txtContent.AppendText(string.Format("{0:X2}", b));
            }
        }
        #endregion

    }
}
