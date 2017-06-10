using System;
using System.Management;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Collections;

using IP地址切换器.BusinessObjectLayer;
using IP地址切换器.BusinessLogicLayer;
using IP地址切换器.CommonComponent;

namespace IP地址切换器
{
    public partial class IPSwitcher : Form
    {
        private IP_BLL ipBLL = new IP_BLL();

        public IPSwitcher()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 页面初始化 初始化列表框,MAC查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IPSwitcher_Load(object sender, EventArgs e)
        {
            DataTable dt = ipBLL.SelectWays("IP", "WAYS");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cob_way.Items.Add(dt.Rows[i][0].ToString());
            }
            cob_way.SelectedIndex = 0;
            ModifyIP mIP = new ModifyIP();
            lb_Mac.Text = mIP.GetMAC();
        }
        /// <summary>
        /// 切换按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            IPAddress myIP = GetDataFromUI();
            ModifyIP mIP = new ModifyIP();
            if (mIP.ModIP(myIP))
            {
                MessageBox.Show("IP成功修改！");
            }
        }

        /// <summary>
        /// 列表框选项改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cob_way_SelectedIndexChanged(object sender, EventArgs e)
        {
            string way = cob_way.SelectedItem.ToString();
            DataRow dr = ipBLL.SelectAllIP("IP", "WAYS", way);
            DisplayIPFromDataRow(dr);
            //GetDataFromUI();
        }

        /// <summary>
        /// 添加按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bt_Add_Click(object sender, EventArgs e)
        {
            IPAddress myIP = GetDataFromUI();
            Hashtable ht = new Hashtable();
            ht = AddIPToHashTable(myIP);
            if (cob_way.SelectedItem != null && myIP.Ip == textBox1.Text && myIP.Subnetmask == textBox2.Text  && myIP.Gateway == textBox3.Text  && myIP.Dns1 == textBox4.Text && myIP.Dns2 == textBox5.Text )
            {
                MessageBox.Show("已经存在相同的方案！");
            }
            else
            {
                if (ipBLL.InsertIPRow("IP", ht))
                {
                    MessageBox.Show("添加成功");
                    cob_way.Items.Clear();
                    DataTable dt = ipBLL.SelectWays("IP", "WAYS");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        cob_way.Items.Add(dt.Rows[i][0].ToString());
                    }
                    cob_way.SelectedIndex = 0;
                }

            }
        }

        /// <summary>
        /// 修改按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bt_Mod_Click(object sender, EventArgs e)
        {
            string way = cob_way.SelectedItem.ToString();
            if (MessageBox.Show("确定要修改\"" + way + "\"吗？", "确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                IPAddress myIP = GetDataFromUI();
                Hashtable ht=new Hashtable ();
                ht = AddIPToHashTable(myIP);
                if (ipBLL.UpdateIPRow("IP", ht, "WAYS",way))
                {
                    MessageBox.Show("方案成功修改!");
                    cob_way.Items.Clear();
                    DataTable dt = ipBLL.SelectWays("IP", "WAYS");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        cob_way.Items.Add(dt.Rows[i][0].ToString());
                    }
                    cob_way.SelectedIndex = 0;
                }
                else
                {
                    MessageBox.Show("方案修改失败！");
                }
            }
        }
        
        /// <summary>
        /// 删除按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bt_Del_Click(object sender, EventArgs e)
        {
            string way = cob_way.SelectedItem.ToString();
            if (MessageBox.Show("你确定要删除\"" + way + "\"吗？", "确认删除？", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                IPAddress myIP = GetDataFromUI();
                Hashtable ht = new Hashtable();
                ht = AddIPToHashTable(myIP);
                if (ipBLL.DelIPRow("IP", "WAYS", way))
                {
                    MessageBox.Show("方案成功删除!");
                    cob_way.Items.Clear();
                    DataTable dt = ipBLL.SelectWays("IP", "WAYS");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        cob_way.Items.Add(dt.Rows[i][0].ToString());
                    }
                    cob_way.SelectedIndex = 0;
                }
                else
                {
                    MessageBox.Show("方案删除失败！");
                }
            }

        }
 
        /// <summary>
        /// 将IP类成员添加HashTable
        /// </summary>
        /// <returns></returns>
        private Hashtable AddIPToHashTable(IPAddress myIP)
        {
            Hashtable ht = new Hashtable();
            ht.Add("WAYS", SqlStringFormat.GetQuotedString (myIP.Way));
            ht.Add("IP", SqlStringFormat.GetQuotedString( myIP.Ip));
            ht.Add("MAC", SqlStringFormat.GetQuotedString( myIP.Mac));
            ht.Add("SUBNETMASK", SqlStringFormat.GetQuotedString( myIP.Subnetmask));
            ht.Add("GATEWAY", SqlStringFormat.GetQuotedString ( myIP.Gateway));
            ht.Add("DNS1", SqlStringFormat.GetQuotedString( myIP.Dns1));
            ht.Add("DNS2", SqlStringFormat.GetQuotedString ( myIP.Dns2));
            return ht;
        }
        /// <summary>
        /// 将DataRow中的数据绑定到IP类中并显示出来
        /// </summary>
        /// <param name="dr"></param>
        private void DisplayIPFromDataRow(DataRow dr)
        {
            if (!dr.IsNull(0))
            {
                textBox1.Text = dr["IP"].ToString();
                textBox2.Text = dr["SUBNETMASK"].ToString(); 
                textBox3.Text = dr["GATEWAY"].ToString(); 
                textBox4.Text = dr["DNS1"].ToString(); 
                textBox5.Text = dr["DNS2"].ToString();                 
            }
        }
        /// <summary>
        /// 从文本框获得数据
        /// </summary>
        private IPAddress GetDataFromUI()
        {
            IPAddress myIP = new IPAddress();    
            myIP.Ip = textBox1.Text;
            myIP.Subnetmask = textBox2.Text;
            myIP.Gateway = textBox3.Text;
            myIP.Dns1 = textBox4.Text;
            myIP.Dns2 = textBox5.Text;
            myIP.Way = cob_way.Text;
            myIP.Mac = lb_Mac.Text;
            return myIP;
        }
     
    }
}
