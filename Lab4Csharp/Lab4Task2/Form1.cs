﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace Lab4Task2
{
    public partial class Form1 : Form
    {
        private static readonly Regex regexForImage = new Regex("^(.)((bmp)|(gif)|(tiff?)|(jpe?g)|(png))$",
            RegexOptions.IgnoreCase);

        private static readonly string RESOURCES_PATH = Path.Combine(Path.GetDirectoryName(
                    Assembly.GetExecutingAssembly().Location), "Resources");

        public Form1()
        {
            InitializeComponent();
        }

        public static BitmapSource ConvertBitmap(Bitmap source)
        {
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                          source.GetHbitmap(),
                          IntPtr.Zero,
                          Int32Rect.Empty,
                          BitmapSizeOptions.FromEmptyOptions());
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            var dirInfo = new DirectoryInfo(RESOURCES_PATH);
            var images = new Dictionary<Image, FileInfo>();

            foreach (var file in dirInfo.GetFiles("*.*"))
            {
                try
                {
                    if (regexForImage.IsMatch(file.Extension))
                    {
                        images.Add(Image.FromFile(file.FullName), file);
                    }
                }
                catch (OutOfMemoryException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            foreach (Bitmap bitmapImage in images.Keys)
            {
                bitmapImage.RotateFlip(RotateFlipType.RotateNoneFlipX);
                var frame = BitmapFrame.Create(ConvertBitmap(bitmapImage));
                var encoder = new GifBitmapEncoder();
                encoder.Frames.Add(frame);

                images.TryGetValue(bitmapImage, out FileInfo file);
                var imgPath = Path.Combine(file.DirectoryName, $"{file.Name.Split('.')[0]}-mirrored.gif");
                try
                {
                    using var fs = new FileStream(imgPath, FileMode.CreateNew);
                    encoder.Save(fs);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}