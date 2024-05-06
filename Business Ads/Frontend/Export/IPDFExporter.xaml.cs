﻿// <copyright file="IPDFExporter.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Frontend
{
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Net.Mail;
    using System.Windows;
    using CsvHelper;
    using Frontend.Export;
    using ScottPlot.Panels;
    using Syncfusion.Pdf;
    using Syncfusion.Pdf.Graphics;
    using static Frontend.ExportWindow;

    /// <summary>
    /// Interaction logic for Window1.xaml.
    /// </summary>
    ///
    public interface IPDFExporter
    {
        void ExportPDF(
            AdvertisementStats stats,
            User user,
            int fontSize,
            int fontIndex,
            int colorIndex,
            bool impressionsChecked,
            bool clicksChecked,
            bool buysChecked,
            bool timeChecked,
            bool ctrChecked,
            bool dateChecked,
            bool signatureChecked,
            bool recipientChecked,
            string recipientInput,
            bool emailButtonChecked,
            bool downloadButtonChecked);
    }

    public interface ICSVExporter
    {
        void ExportCSV(
            AdvertisementStats stats,
            bool impressionsChecked,
            bool clicksChecked,
            bool buysChecked,
            bool ctrChecked,
            bool timeChecked,
            string outputPath,
            string emailRecipient);
    }

    public interface IEmailSender
    {
        Task SendDocEmailAsync(string recipient, string filename);
    }

    public class ExportManager : IPDFExporter, ICSVExporter, IEmailSender
    {
        public void ExportPDF(
            AdvertisementStats stats,
            User user,
            int fontSize,
            int fontIndex,
            int colorIndex,
            bool impressionsChecked,
            bool clicksChecked,
            bool buysChecked,
            bool timeChecked,
            bool ctrChecked,
            bool dateChecked,
            bool signatureChecked,
            bool recipientChecked,
            string recipientInput,
            bool emailButtonChecked,
            bool downloadButtonChecked)
        {
            PdfDocument document = new PdfDocument();
            PdfPage page = document.Pages.Add();
            PdfGraphics graphics = page.Graphics;
            PdfBrush color = new PdfSolidBrush(System.Drawing.Color.Black);
            int height = 20;
            PdfStandardFont font = new PdfStandardFont(PdfFontFamily.TimesRoman, 12);
            if (fontIndex == 1)
            {
                font = font = new PdfStandardFont(PdfFontFamily.Helvetica, 12);
            }

            if (fontIndex == 2)
            {
                font = font = new PdfStandardFont(PdfFontFamily.Courier, 12);
            }

            try
            {
                float parsedFontSize = fontSize;
            }
            catch
            {
                throw new Exception("Wrong Size Input");
            }

            if (colorIndex == 1)
            {
                color = new PdfSolidBrush(System.Drawing.Color.Gray);
            }

            if (colorIndex == 2)
            {
                color = new PdfSolidBrush(System.Drawing.Color.Red);
            }

            graphics.DrawString("Advertisement Statistics Report", font, color, new PointF((page.Size.Width / 2f) - 100, 10));

            if (impressionsChecked == true)
            {
                graphics.DrawString("\nAmount of Impressions: " + stats.Views.ToString(), font, color, new PointF(10, height));
                height += 20;
            }

            if (clicksChecked == true)
            {
                graphics.DrawString("\nAmount of Clicks: " + stats.Clicks.ToString(), font, color, new PointF(10, height));
                height += 20;
            }

            if (buysChecked == true)
            {
                graphics.DrawString("\nAmount of Purchases: " + stats.Buys.ToString(), font, color, new PointF(10, height));
                height += 20;
            }

            if (timeChecked == true)
            {
                graphics.DrawString("\nTotal Time Viewed: " + stats.Time.ToString(), font, color, new PointF(10, height));
                height += 20;
            }

            if (ctrChecked == true)
            {
                graphics.DrawString("\nClick Through Ratio: " + ((float)(stats.Clicks / stats.Views)).ToString(), font, color, new PointF(10, height));
                height += 20;
            }

            if (dateChecked == true)
            {
                graphics.DrawString("Created on:", font, color, new PointF(20, page.Size.Height - 120));
                graphics.DrawString(DateTime.Now.ToString(), font, color, new PointF(20, page.Size.Height - 100));
            }

            if (signatureChecked == true)
            {
                graphics.DrawString("Signature:", font, color, new PointF(page.Size.Width - 200, page.Size.Height - 120));
                graphics.DrawString(user.Name, font, color, new PointF(page.Size.Width - 200, page.Size.Height - 100));
            }

            if (recipientChecked == true)
            {
                graphics.DrawString("Intended Recipient:", font, color, new PointF((page.Size.Width / 2f) - 50, page.Size.Height - 120));
                graphics.DrawString(recipientInput, font, color, new PointF((page.Size.Width / 2f) - 50, page.Size.Height - 100));
            }

            if (emailButtonChecked == true)
            {
                string outputPath = "C:\\Users\\User\\Downloads\\output.pdf";
                document.Save(outputPath);
                this.SendDocEmailAsync(recipientInput, "C:\\Users\\User\\Downloads\\output.pdf");
            }

            if (downloadButtonChecked == true)
            {
                string outputPath = "C:\\Users\\User\\Downloads\\output.pdf";
                document.Save(outputPath);
            }

            document.Close(true);
        }

        public void ExportCSV(
            AdvertisementStats stats,
            bool impressionsChecked,
            bool clicksChecked,
            bool buysChecked,
            bool ctrChecked,
            bool timeChecked,
            string outputPath,
            string emailRecipient)
        {
            using (var writer = new StreamWriter(outputPath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                List<AdvertisementStats> records = new List<AdvertisementStats> { stats };

                foreach (var record in records)
                {
                    if (impressionsChecked)
                    {
                        csv.WriteField(record.Views);
                    }

                    if (clicksChecked)
                    {
                        csv.WriteField(record.Clicks);
                    }

                    if (buysChecked)
                    {
                        csv.WriteField(record.Buys);
                    }

                    if (ctrChecked)
                    {
                        csv.WriteField((float)record.Views / record.Clicks);
                    }

                    if (timeChecked)
                    {
                        csv.WriteField(record.Time);
                    }

                    csv.NextRecord();
                }
            }

            if (!string.IsNullOrEmpty(emailRecipient))
            {
                this.SendDocEmailAsync(emailRecipient, outputPath);
            }
        }

        public Task SendDocEmailAsync(string recipient, string filename)
        {
            var senderEmail = "osvathrobert03@gmail.com";
            var password = "daes ndml ukpj qvuj";

            var subject = "Statistics Report";
            var message = "Attached are the requested documents";

            var client = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(senderEmail, password),
                EnableSsl = true,
            };

            var mail = new MailMessage(
                from: senderEmail,
                to: recipient,
                subject: subject,
                body: message);

            mail.Attachments.Add(new Attachment(filename));
            return client.SendMailAsync(mail);
        }
    }

    public class AdvertisementStats
    {
        public AdvertisementStats(int views, int clicks, int buys, int time)
        {
            this.Views = views;
            this.Clicks = clicks;
            this.Buys = buys;
            this.Time = time;
        }

        public int Views { get; set; }

        public int Clicks { get; set; }

        public int Buys { get; set; }

        public int Time { get; set; }
    }

    public class User
    {
        public User(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }
    }

    public partial class ExportWindow : Window
    {
        public Window MainWindow;
        ExportManager exportManager;

        AdvertisementStats stats = new AdvertisementStats(1000, 100, 5, 30);
        User user = new User("Andrew Stone");

        public ExportWindow()
        {
            this.InitializeComponent();

            this.Closed += (sender, eventData) =>
            {
                this.MainWindow.Show();
            };
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            int fontIndex = this.FontBox.SelectedIndex;
            int colorIndex = this.ColorBox.SelectedIndex;
            int fontSize = int.Parse(this.SizeInput.Text);
            bool impressionsChecked = this.ImpressionsCheck.IsChecked == true;
            bool clicksChecked = this.ClicksCheck.IsChecked == true;
            bool buysChecked = this.BuysCheck.IsChecked == true;
            bool timeChecked = this.TimeCheck.IsChecked == true;
            bool ctrChecked = this.CTRCheck.IsChecked == true;
            bool dateChecked = this.DateCheck.IsChecked == true;
            bool signatureChecked = this.SignatureCheck.IsChecked == true;
            bool recipientChecked = this.RecipientCheck.IsChecked == true;
            string recipientInput = this.RecipientInput.Text;
            bool emailButtonChecked = this.EmailButton1.IsChecked == true;
            bool downloadButtonChecked = this.DownloadButton1.IsChecked == true;
            string outputPath = "C:\\Users\\User\\Downloads\\output.csv";
            string emailRecipient = this.EmailInput1.Text;

            if (this.Radio7.IsChecked == true)
            {
                this.exportManager.ExportPDF(this.stats,
                    this.user,
                    fontSize,
                    fontIndex,
                    colorIndex,
                    impressionsChecked,
                    clicksChecked,
                    buysChecked,
                    timeChecked,
                    ctrChecked,
                    dateChecked,
                    signatureChecked,
                    recipientChecked,
                    recipientInput,
                    emailButtonChecked,
                    downloadButtonChecked);
            }

            if (this.Radio5.IsChecked == true)
            {
                this.exportManager.ExportCSV(
                    this.stats,
                    impressionsChecked,
                    clicksChecked,
                    buysChecked,
                    ctrChecked,
                    timeChecked,
                    outputPath,
                    emailRecipient);
            }

            this.ConfirmExport();
        }

        public void ConfirmExport()
        {
            ExportSucces exportSucces = new ()
            {
                MainWindow = this.MainWindow,
            };
            exportSucces.Show();
            this.Hide();
        }

        private void ReturnButton1_Click(object sender, RoutedEventArgs e)
        {
            this.MainWindow.Show();
            this.Close();
        }
    }
}