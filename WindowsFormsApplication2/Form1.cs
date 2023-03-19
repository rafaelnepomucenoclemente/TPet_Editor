using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic.Devices;
using System.Collections;
using System.Diagnostics;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.VisualBasic;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        //Дополнительная структура
        private byte[] Bytes;
        private uint[] Uint32;
        private string[] String1S;
        //Структура TPet
        private ushort[] ID;
        private ushort[] Icons;
        private byte[] RecallKind1;
        private byte[] RecallKind2;
        private ushort[] RecallValue1;
        private ushort[] RecallValue2;
        private byte[] ConditionType;
        private uint[] ConditionValue;
        private ushort[] MonID;
        //Дополнительная структура
        private int[] String1längeI;
        private ushort AnzahlNPC;
        public Form1()
        {
            InitializeComponent();
        }

        private void MenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.OpenFileDialog1.Filter = "Tcd Files (*.tcd)|*.tcd|All Files (*.*)|*.*";
            this.OpenFileDialog1.Title = "Open Tcd File";
            this.OpenFileDialog1.FileName = "";
            if (this.OpenFileDialog1.ShowDialog() != DialogResult.OK)
                return;
            Computer myComputer = new Computer();
            this.Bytes = myComputer.FileSystem.ReadAllBytes(this.OpenFileDialog1.FileName);
            this.Laden();
        }

        private void ReDims()
        {
            ushort num = (ushort)500;
            this.Uint32 = new uint[checked((int)num + 300 + 1)];
            this.String1S = new string[checked((int)num + 300 + 1)];
            this.String1längeI = new int[checked((int)num + 300 + 1)];
            this.ID = new ushort[checked((int)num + 300 + 1)];
            this.Icons = new ushort[checked((int)num + 300 + 1)];
            this.RecallKind1 = new byte[checked((int)num + 300 + 1)];
            this.RecallKind2 = new byte[checked((int)num + 300 + 1)];
            this.RecallValue1 = new ushort[checked((int)num + 300 + 1)];
            this.RecallValue2 = new ushort[checked((int)num + 300 + 1)];
            this.ConditionType = new byte[checked((int)num + 300 + 1)];
            this.ConditionValue = new uint[checked((int)num + 300 + 1)];
            this.MonID = new ushort[checked((int)num + 300 + 1)];

        }

        private void Speichern()
        {
            this.SaveFileDialog1.Filter = "Tcd File (*.tcd)|*.tcd|All File (*.*)|*.*";
            this.SaveFileDialog1.Title = "Save Tcd";
            this.SaveFileDialog1.FileName = "";
            if (this.SaveFileDialog1.ShowDialog() != DialogResult.OK)
                return;
            BinaryWriter binaryWriter = new BinaryWriter((Stream)File.Open(this.SaveFileDialog1.FileName, FileMode.Create),Encoding.Default);
            int index2 = 0;
            binaryWriter.Write(this.AnzahlNPC);
            while (index2 != (int)this.AnzahlNPC)
            {
                binaryWriter.Write(this.ID[index2]);
                binaryWriter.Write(this.Icons[index2]);
                binaryWriter.Write(this.RecallKind1[index2]);
                binaryWriter.Write(this.RecallKind2[index2]);
                binaryWriter.Write(this.RecallValue1[index2]);
                binaryWriter.Write(this.RecallValue2[index2]);
                binaryWriter.Write(this.ConditionType[index2]);
                binaryWriter.Write(this.ConditionValue[index2]);
                binaryWriter.Write(this.MonID[index2]);
                checked { ++index2; }
            }
            binaryWriter.Close();
            int num = (int)Interaction.MsgBox((object)"Saved", MsgBoxStyle.OkOnly, (object)null);
        }

        private void Laden()
        {
         // StreamReader   new StreamReader(@"E:\database.txt", System.Text.Encoding.Default));
            BinaryReader binaryReader = new BinaryReader((Stream)new MemoryStream(this.Bytes),Encoding.Default);
            int index1 = 0;
            binaryReader.BaseStream.Position = 0L;
            this.AnzahlNPC = binaryReader.ReadUInt16();
            this.ReDims();
            while (index1 != (int)this.AnzahlNPC)
            {
                this.ID[index1] = binaryReader.ReadUInt16();
                this.Icons[index1] = binaryReader.ReadUInt16();
                this.RecallKind1[index1] = binaryReader.ReadByte();
                this.RecallKind2[index1] = binaryReader.ReadByte();
                this.RecallValue1[index1] = binaryReader.ReadUInt16();
                this.RecallValue2[index1] = binaryReader.ReadUInt16();
                this.ConditionType[index1] = binaryReader.ReadByte();
                this.ConditionValue[index1] = binaryReader.ReadUInt32();
                this.MonID[index1] = binaryReader.ReadUInt16();
                checked { ++index1; }
            }
            this.AnzahlNPC = checked((ushort)index1);
            this.Listen();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Speichern();
        }

        private void InsertinBox(int PortalZahl)
        {
            this.PetIDTextBox.Text = Conversions.ToString((ushort)this.ID[PortalZahl]);
            this.IconsTextBox.Text = Conversions.ToString((ushort)this.Icons[PortalZahl]);
            this.RecallKind1TextBox.Text = Conversions.ToString((byte)this.RecallKind1[PortalZahl]);
            this.RecallKind2TextBox.Text = Conversions.ToString((byte)this.RecallKind2[PortalZahl]);
            this.RecallValue1TextBox.Text = Conversions.ToString((ushort)this.RecallValue1[PortalZahl]);
            this.RecallValue2TextBox.Text = Conversions.ToString((ushort)this.RecallValue2[PortalZahl]);
            this.ConditionTypeTextBox.Text = Conversions.ToString((byte)this.ConditionType[PortalZahl]);
            this.ConditionValueTextBox.Text = Conversions.ToString((uint)this.ConditionValue[PortalZahl]);
            this.MonIDTextBox.Text = Conversions.ToString((ushort)this.MonID[PortalZahl]);
        }

        private void Listen()
        {
            for (string str = Conversions.ToString(0); (double)this.AnzahlNPC >= Conversions.ToDouble(str) + 1.0; str = Conversions.ToString(Conversions.ToDouble(str) + 1.0))
                this.listBox1.Items.Add((object)(str + ": " + this.ID[Conversions.ToInteger(str)]));
        }

        private void listBox1_SelectedValueChanged_1(object sender, EventArgs e)
        {
            string[] strArray = new string[11];
            this.InsertinBox(Conversions.ToInteger(((string[])NewLateBinding.LateGet(this.listBox1.SelectedItem, (System.Type)null, "Split", new object[1]
      {
        (object) Convert.ToChar(":")
      }, (string[])null, (System.Type[])null, (bool[])null))[0]));
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string[] strArray = new string[21];
            string str = ((string[])NewLateBinding.LateGet(this.listBox1.SelectedItem, (System.Type)null, "Split", new object[1]
      {
        (object) Convert.ToChar(":")
      }, (string[])null, (System.Type[])null, (bool[])null))[0];
           
           this.ID[Conversions.ToInteger(str)] = Conversions.ToUShort(this.PetIDTextBox.Text);
           this.Icons[Conversions.ToInteger(str)] = Conversions.ToUShort(this.IconsTextBox.Text);
           this.RecallKind1[Conversions.ToInteger(str)] = Conversions.ToByte(this.RecallKind1TextBox.Text);
           this.RecallKind2[Conversions.ToInteger(str)] = Conversions.ToByte(this.RecallKind2TextBox.Text);
           this.RecallValue1[Conversions.ToInteger(str)] = Conversions.ToUShort(this.RecallValue1TextBox.Text);
           this.RecallValue2[Conversions.ToInteger(str)] = Conversions.ToUShort(this.RecallValue2TextBox.Text);
           this.ConditionType[Conversions.ToInteger(str)] = Conversions.ToByte(this.ConditionTypeTextBox.Text);
           this.ConditionValue[Conversions.ToInteger(str)] = Conversions.ToUInteger(this.ConditionValueTextBox.Text);
           this.MonID[Conversions.ToInteger(str)] = Conversions.ToUShort(this.MonIDTextBox.Text);
            int num = (int)Interaction.MsgBox((object)"Saved", MsgBoxStyle.Information, (object)null);
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Add_NPC();
        }

        public void Add_NPC()
        {
            this.AnzahlNPC = checked((ushort)((int)this.AnzahlNPC + 1));
            this.listBox1.Items.Add((object)(Conversions.ToString(checked((int)this.AnzahlNPC - 1)) + ": " + this.String1S[checked((int)this.AnzahlNPC - 1)]));
        }

        private void listBox2_SelectedValueChanged_1(object sender, EventArgs e)
        {
            string[] strArray = new string[11];
            this.InsertinBox(Conversions.ToInteger(((string[])NewLateBinding.LateGet(this.listBox2.SelectedItem, (System.Type)null, "Split", new object[1]
      {
        (object) Convert.ToChar(":")
      }, (string[])null, (System.Type[])null, (bool[])null))[0]));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.listBox2.Hide();
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            if (Operators.CompareString(this.textBox1.Text, "", false) == 0)
            {
                this.listBox1.Show();
                this.listBox2.Hide();
            }
            else
            {
                this.listBox2.Items.Clear();
                try
                {
                    foreach (object obj1 in this.listBox1.Items)
                    {
                        object objectValue = RuntimeHelpers.GetObjectValue(obj1);
                        object Instance = objectValue;
                        // ISSUE: variable of the null type
                        object local1 = null;
                        string MemberName = "Contains";
                        object[] objArray1 = new object[1];
                        object[] objArray2 = objArray1;
                        int index = 0;
                        TextBox textBox1 = this.textBox1;
                        string text = textBox1.Text;
                        objArray2[index] = (object)text;
                        object[] objArray3 = objArray1;
                        object[] Arguments = objArray3;
                        // ISSUE: variable of the null type
                        object local2 = null;
                        // ISSUE: variable of the null type
                        object local3 = null;
                        bool[] flagArray = new bool[1]
            {
              true
            };
                        bool[] CopyBack = flagArray;
                        object obj2 = NewLateBinding.LateGet(Instance, (System.Type)local1, MemberName, Arguments, (string[])local2, (System.Type[])local3, CopyBack);
                        if (flagArray[0])
                            textBox1.Text = (string)Conversions.ChangeType(RuntimeHelpers.GetObjectValue(objArray3[0]), typeof(string));
                        if (Conversions.ToBoolean(obj2))
                            this.listBox2.Items.Add(RuntimeHelpers.GetObjectValue(objectValue));
                    }
                }
                finally
                {
                    IEnumerator enumerator = null;
                    if (enumerator is IDisposable)
                        (enumerator as IDisposable).Dispose();
                }
                if (this.listBox2.Items.Count < 1)
                    return;
                this.listBox2.Show();
                this.listBox1.Hide();
            }
        }

        private void menuStrip1_ItemClicked_1(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void smazatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.AnzahlNPC = checked((ushort)((int)this.AnzahlNPC - 1));
            
            this.listBox1.Items.Remove((object)(Conversions.ToString(checked((int)this.AnzahlNPC - 1)) + ": " + this.String1S[checked((int)this.AnzahlNPC - 1)]));
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void SkillPoint_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void xko_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            string[] strArray = new string[21];
            string str = ((string[])NewLateBinding.LateGet(this.listBox1.SelectedItem, (System.Type)null, "Split", new object[1]
      {
        (object) Convert.ToChar(":")
      }, (string[])null, (System.Type[])null, (bool[])null))[0];

            this.ID[Conversions.ToInteger(str)] = Conversions.ToUShort(this.PetIDTextBox.Text);
           this.Icons[Conversions.ToInteger(str)] = Conversions.ToUShort(this.IconsTextBox.Text);
           this.RecallKind1[Conversions.ToInteger(str)] = Conversions.ToByte(this.RecallKind1TextBox.Text);
           this.RecallKind2[Conversions.ToInteger(str)] = Conversions.ToByte(this.RecallKind2TextBox.Text);
           this.RecallValue1[Conversions.ToInteger(str)] = Conversions.ToUShort(this.RecallValue1TextBox.Text);
           this.RecallValue2[Conversions.ToInteger(str)] = Conversions.ToUShort(this.RecallValue2TextBox.Text);
           this.ConditionType[Conversions.ToInteger(str)] = Conversions.ToByte(this.ConditionTypeTextBox.Text);
           this.ConditionValue[Conversions.ToInteger(str)] = Conversions.ToUInteger(this.ConditionValueTextBox.Text);
           this.MonID[Conversions.ToInteger(str)] = Conversions.ToUShort(this.MonIDTextBox.Text);
            int num = (int)Interaction.MsgBox((object)"Saved", MsgBoxStyle.Information, (object)null);
        }

        private void IconsTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        }
    }

