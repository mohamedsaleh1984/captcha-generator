﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace CaptchaLibrary
{

    public class CaptchaLibraryClass
    {
        private Random _Rand;
        private int _iCodeLength;
        private Bitmap _bitmap;
        private int _imageWidth;
        private int _imageHeight;

        public SolidBrush LinesColor
        { get; set; }
        public String ImageFilePath { get; private set; }

        public SolidBrush BackgroundColor { get; set; }
        public SolidBrush FontColor { get; set; }
        public String DirectoryPath { get; set; }

        public int CodeLength { get; set; }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="CodeLength">Length of Generated Code</param>
        /// <param name="DirectoryPath">Location where BMP files Get generated.</param>
        public CaptchaLibraryClass(int CodeLength = 5,
                                string DirectoryPath = "C:\\Images",
                                int ImageWidth = 200,
                                int ImageHeight = 50)
        {
            _Rand = new Random();
            this.CodeLength = CodeLength;
            _bitmap = null;
            this.DirectoryPath = DirectoryPath;
            ImageFilePath = "";
            GeneratedCode = "";
            _imageWidth = ImageWidth;
            _imageHeight = ImageHeight;
            ImageFont = new Font("Tahoma", 10 + _Rand.Next(14, 18));
            BackgroundColor = new SolidBrush(Color.Black);
            FontColor = new SolidBrush(Color.White);
            LinesColor = new SolidBrush(Color.Gray);
            NumOfLines = 10;
        }

        public int NumOfLines { get; set; }


        /// <summary>
        /// Get/Set Image Font
        /// </summary>
        public Font ImageFont { get; set; }

        /// <summary>
        /// Get Generate Text
        /// </summary>
        public String GeneratedCode { get; private set; }


        /// <summary>
        /// Reset all values for the class.
        /// </summary>
        public void Reset()
        {
            _Rand = new Random();
            _iCodeLength = 5;
            DirectoryPath = "";
        }

        /// <summary>
        /// Generate random points to place tge characters in the Bitmap.
        /// </summary>
        /// <returns></returns>
        private Point[] GetRandomPoints()
        {
            Point[] points = { new Point(_Rand.Next(10, 150), _Rand.Next(10, 150)),
                               new Point(_Rand.Next(10, 100), _Rand.Next(10, 100)) };
            return points;
        }

        /// <summary>
        /// Generate random text.
        /// </summary>
        /// <returns></returns>
        public string GenerateRandomText()
        {
            string randomText = "";
            string alphabets = "abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Random r = new Random();

            for (int j = 0; j <= CodeLength; j++)
                randomText = randomText + alphabets[r.Next(alphabets.Length)];

            GeneratedCode = randomText.ToString();
            return GeneratedCode;
        }

        /// <summary>
        /// Check if the given string matched with generated code.
        /// </summary>
        /// <param name="strInputFromUser"></param>
        /// <returns></returns>
        public bool IsValidCode(object strInputFromUser)
        {
            if (String.IsNullOrEmpty(GeneratedCode))
            {
                throw new Exception("");
            }

            return GeneratedCode.Equals(Convert.ToString(strInputFromUser)) ? true : false;
        }

        /// <summary>
        /// Generate an Image.
        /// </summary>
        /// <param name="strPath"></param>
        private void CreateImage()
        {
            string code = GenerateRandomText();

            Bitmap bitmap = new Bitmap(_imageWidth, _imageHeight, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bitmap);

            Rectangle rect = new Rectangle(0, 0, _imageWidth, _imageHeight);

            SolidBrush blkBrush = new SolidBrush(Color.Black);
            SolidBrush whtBrush = new SolidBrush(Color.White);

            int ipxlOffset = 0;

            g.DrawRectangle(new Pen(Color.Yellow), rect);

            g.FillRectangle(blkBrush, rect);

            for (int i = 0; i < code.Length; i++)
            {
                g.DrawString(code[i].ToString(),
                           ImageFont,
                            whtBrush, new PointF(10 + ipxlOffset, 10));

                ipxlOffset += 20;
            }

            DrawRandomLines(g);
        }

        /// <summary>
        /// Generate an Image and return Bitmap
        /// </summary>
        /// <returns></returns>
        public Bitmap CreateImageBitmap()
        {
            string code = GenerateRandomText();
            if (_bitmap != null)
                _bitmap.Dispose();

            _bitmap = new Bitmap(_imageWidth, _imageHeight, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(_bitmap);

            Rectangle rect = new Rectangle(0, 0, _imageWidth, _imageHeight);


            int ipxlOffset = 0;

            g.DrawRectangle(new Pen(Color.Yellow), rect);

            g.FillRectangle(BackgroundColor, rect);

            for (int i = 0; i < code.Length; i++)
            {
                g.DrawString(code[i].ToString(),
                           ImageFont,
                            FontColor,
                            new PointF(_imageWidth / 20 + ipxlOffset, _imageWidth / 20));

                ipxlOffset += _imageWidth / 10;
            }

            DrawRandomLines(g);

            return _bitmap;
        }

        /// <summary>
        /// Draw lines.
        /// </summary>
        /// <param name="g"></param>
        private void DrawRandomLines(Graphics g)
        {
            for (int i = 0; i < NumOfLines; i++)
            {
                g.DrawLines(new Pen(LinesColor, 2), GetRandomPoints());
            }
        }

        private String GenerateFileName()
        {
            string strFileName = DateTime.Now.Year.ToString() +
                                DateTime.Now.Month.ToString() +
                                DateTime.Now.Day.ToString() +
                                DateTime.Now.Hour.ToString() +
                                DateTime.Now.Minute.ToString() +
                                DateTime.Now.Second.ToString();

            strFileName += ".bmp";
            return strFileName;

        }
        public Boolean SaveBitmapToFile()
        {
            ImageFilePath = GenerateFileName();

            if (!Directory.Exists(DirectoryPath))
            {
                Directory.CreateDirectory(DirectoryPath);
            }


            String strFullFilePath = DirectoryPath + "\\" + ImageFilePath;

            if (File.Exists(strFullFilePath))
            {
                try
                {
                    File.Delete(strFullFilePath);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error creating file." + "\n" + ex.Message);
                }
            }

            bool bSuccessful = false;
            try
            {
                _bitmap.Save(strFullFilePath);
                bSuccessful = true;
                ImageFilePath = strFullFilePath;
            }
            catch (Exception)
            {
                //log error message.
            }


            return bSuccessful;
        }

    }
}