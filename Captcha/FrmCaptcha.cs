﻿using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using CaptchaLibrary;

namespace Captcha
{
    public partial class FrmCaptcha : Form
    {
        private CaptchaLibrary.CaptchaLibraryClass cls;
        private string code;

        public FrmCaptcha()
        {
            InitializeComponent();
            cls = new CaptchaLibraryClass(ImageWidth:300,ImageHeight:200);
            cls.BackgroundColor = new SolidBrush(Color.AliceBlue);
            cls.NumOfLines = 5;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GenerateImage();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            if (txtInput.Text.Equals(code))
                MessageBox.Show("Correct", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("Not Correct", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            GenerateImage();
        }

        private void GenerateImage()
        {
            if (pictureBox1.Image != null)
                pictureBox1.Image.Dispose();

            Image bitmap = cls.CreateImageBitmap();
             code = cls.GeneratedCode;
            pictureBox1.Image = bitmap;
        }
    }

}