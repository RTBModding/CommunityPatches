using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using HarmonyLib;

using SharpDX;
using SharpDX.Windows;

using RaceBits;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Configuration;
using System.Diagnostics;

[HarmonyPatch]
class MainFormPatch
{
    static Type TargetType()
    {
        return AccessTools.TypeByName("cv");
        /*
            cv: global::cu.b("Initialising Globals", true);
            s: cv.d.AllowDrop = true;
            cv.by.ai: if (cx.i().RObjectsHandler.BObjects.Count == 0)
            cv.by.ai.ag: 
                this.ae();
		        this.ab();

                foreach (ObjectList objectList in this.af())
                list.Add(objectList.ObjectType);
        */
    }

    static MethodBase TargetMethod()
    {
        return AccessTools.Method(TargetType(), "s");
    }
    static void Postfix(object __instance, RenderForm ___d)
    {

        var by = AccessTools.Field(__instance.GetType(), "by")?.GetValue(__instance);


        ___d.AllowDrop = true;

        ___d.DragEnter += (s, e) =>
         {
             if (e.Data.GetDataPresent(DataFormats.FileDrop))
                 e.Effect = DragDropEffects.Copy;
             else
                 e.Effect = DragDropEffects.None;
         };


        ___d.DragDrop += (s, e) =>
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            string file = files[0];

            if (files.Length != 1)
            {
                MessageBox.Show("You can not import more than one file at once.");
                return;
            }


            XPack myxpack = Globals.Venue.MyPack;
            string extension = IsFileAllowed(file);
            string name = Path.GetFileNameWithoutExtension(file);

            BObject? obj = ImportPatchFile(file);
            if (obj == null)
            {
                MessageBox.Show("File import failed");
                return;
            }

            obj.LODIn = 1000f;
            obj.Name = name;
            obj.ObjectType = "PatchObjects";
            obj.Purpose = BObject.Purposes.Object;
            obj.SeparationGroupId = -1;
            obj.Icon = CreateImage128(name);
            obj.mbRequiresRecalc = true;
            myxpack.RecreateBObjectsUsingBObject(obj);

            AccessTools.Method(by?.GetType(), "ag")?.Invoke(by, null);

            // TODO: Save icon

            //Globals.Venue.RObjectsHandler.ObjectLists.Update();
            MessageBox.Show("File imported. Reimport possible but requires project reload");

            Debugger.Break();

            string projF = Globals.Venue.ProjectFolder;
            string xpF = Globals.Venue.MyPack.XPackFolder;
            Console.WriteLine(projF, xpF);

            

            //myxpack.VerifyAndSetReferences();



        };


        /*
        ___d.Click += (s, e) =>
            {
                MessageBox.Show("Clicked");
            };
        */
    }



    /// <summary>
    /// Checks if file is of extension and returns an empty sting or the fileextension
    /// </summary>
    public static string IsFileAllowed(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            return "";
        string[] allowedFileExtensions = { "fbx", "col" };
        string extension = Path.GetExtension(filePath);
        if (string.IsNullOrEmpty(extension))
            return "";
        // Remove the leading '.' and compare case-insensitively
        extension = extension.TrimStart('.');
        var t = allowedFileExtensions.Any(ext =>
            ext.Equals(extension, StringComparison.OrdinalIgnoreCase));

        if (!t) return "";

        return extension.ToLower();
    }

    public static BObject? ImportPatchFile(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return null;

        string extension = Path.GetExtension(path).ToLower().TrimStart('.');
        if (string.IsNullOrEmpty(extension))
            return null;

        XPack myxpack = Globals.Venue.MyPack;

        if (extension == "fbx")
        {
            return myxpack.ImportFBX(path, @"patchImport\");
        }
        else if (extension == "col")
        {
            return myxpack.ImportCollada(path);
        }

        return null;

    }

    public static System.Drawing.Image CreateImage128(string text)
    {
        const int size = 128;
        const int padding = 12; // margin inside the icon

        var bmp = new System.Drawing.Bitmap(size, size, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

        using (var g = System.Drawing.Graphics.FromImage(bmp))
        {
            g.Clear(System.Drawing.Color.Black);

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            var layout = new System.Drawing.RectangleF(
                padding,
                padding,
                size - padding * 2,
                size - padding * 2);

            // Start large and shrink until it fits
            float fontSize = 96f;
            System.Drawing.Font? font = null;

            while (fontSize > 6)
            {
                font?.Dispose();
                font = new System.Drawing.Font(
                    "Segoe UI",
                    fontSize,
                    System.Drawing.FontStyle.Bold,
                    System.Drawing.GraphicsUnit.Pixel);

                var measured = g.MeasureString(text, font, (int)layout.Width);

                if (measured.Width <= layout.Width &&
                    measured.Height <= layout.Height)
                {
                    break;
                }

                fontSize -= 2f;
            }

            using (font)
            using (var brush = new System.Drawing.SolidBrush(System.Drawing.Color.White))
            using (var format = new System.Drawing.StringFormat
            {
                Alignment = System.Drawing.StringAlignment.Center,
                LineAlignment = System.Drawing.StringAlignment.Center,
                Trimming = System.Drawing.StringTrimming.EllipsisCharacter,
                FormatFlags = System.Drawing.StringFormatFlags.NoClip
            })
            {
                g.DrawString(text, font, brush, layout, format);
            }
        }
        string path = Globals.Venue.MyPack.XPackFolder + "\\Objects\\patchImport\\" + text + ".jpg";
        Console.WriteLine(path);
        Debugger.Break();
        bmp.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);

        return bmp; // caller must Dispose()
    }

}

