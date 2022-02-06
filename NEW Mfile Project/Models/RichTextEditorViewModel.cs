using NReco.PdfGenerator;
using PdfSharp.Pdf;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Web.Mvc;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace NEW_Mfile_Project.Models
{
    public class RichTextEditorViewModel
    {
        public static byte[] GeneratePDF(string htmlContent, string htmlContent1)
        {
            byte[] pdfBytes = new byte[0];
            try
            {
                var htmlToPdf = new HtmlToPdfConverter();
                htmlToPdf.PageFooterHtml = "<div style='text-align: right; '>Page: <span class='page'></span></div>";
              
                pdfBytes = htmlToPdf.GeneratePdf(htmlContent, null);
            }
            catch (Exception ex)
            {

                CreateLogFiles.ErrorLog("PDF Generation " + " " + ex.Message + " " + ex.StackTrace);

                MemoryStream msBody = new MemoryStream();

                PdfDocument pdf = PdfGenerator.GeneratePdf(htmlContent1, (PdfSharp.PageSize)PageSize.A4,20, null, null, null);
               
                pdf.Save(msBody, false);

                //pdf.Save("D:/test.pdf");

                pdfBytes =  msBody.ToArray();
            }

            return pdfBytes;
        }
    }
}