using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Net;

namespace com.wjlc.util
{
    public class ImageHelper
    {
        // ��ͼƬ���ˮӡ
        public static string AddWaterMart(string sourImaName, string wartMarkFile, int width = 740, int height = 740)
        {
            if (!File.Exists(sourImaName))
            {
                throw new ArgumentException("������ļ�������");
            }

            if (String.IsNullOrEmpty(wartMarkFile) || !File.Exists(wartMarkFile))
            {
                return sourImaName;
            }

            System.Drawing.Image image = System.Drawing.Image.FromFile(sourImaName);
            Bitmap water = new Bitmap(wartMarkFile);

            using (Graphics g = Graphics.FromImage(image))
            {

                System.Drawing.Imaging.ImageAttributes imageAttributes = new System.Drawing.Imaging.ImageAttributes();

                //�þ�������ˮӡͼƬ͸����
                float[][] colorMatrixElements = { 
               new float[] {0.3f,  0.0f,  0.0f,  0.0f, 0.0f},
               new float[] {0.0f,  0.3f,  0.0f,  0.0f, 0.0f},
               new float[] {0.0f,  0.0f,  0.3f,  0.0f, 0.0f},
               new float[] {0.0f,  0.0f,  0.0f,  0.3f, 0.0f},
               new float[] {0.0f,  0.0f,  0.0f,  0.0f, 0.3f}
                };

                System.Drawing.Imaging.ColorMatrix wmColorMatrix = new System.Drawing.Imaging.ColorMatrix(colorMatrixElements);

                imageAttributes.SetColorMatrix(wmColorMatrix, System.Drawing.Imaging.ColorMatrixFlag.Default, System.Drawing.Imaging.ColorAdjustType.Bitmap);

                g.DrawImage(
                      water,
                      new Rectangle(0, 0, width, height),
                      0, 0, water.Width, water.Height,
                      GraphicsUnit.Pixel,
                      imageAttributes
                    );

                g.Dispose();


                string extension = sourImaName.Substring(sourImaName.LastIndexOf(".")).ToLower();
                ImageFormat imgType = ImageFormat.Jpeg;

                switch (extension)
                {
                    case ".jpeg":
                    case ".jpg":
                    case ".gif":
                        imgType = ImageFormat.Jpeg;
                        extension = ".JPG";
                        break;
                    case ".png":
                        imgType = ImageFormat.Png;
                        extension = ".png";
                        break;
                    default:
                        imgType = ImageFormat.Jpeg;
                        extension = ".JPG";
                        break;
                }

                string newfile = Regex.Replace(sourImaName, @"\.\w+$", "_new" + extension);
                image.Save(newfile, imgType);
                image.Dispose();
                return newfile;
            }
        }
        public static string CreateResizeImage(string sourImgName, string desImgName, int imgSize, bool isOverride)
        {
            int iWidth = 0;
            int iHeight = 0;
            ImageFormat imgType = ImageFormat.Jpeg;

            FileInfo sourFile = new FileInfo(sourImgName);
            string extension = sourFile.Extension.ToLower();

            // ���ͼƬ�ļ��洢��ʽ
            switch (extension)
            {
                case ".jpeg":
                case ".jpg":
                case ".gif":
                    imgType = ImageFormat.Jpeg;
                    extension = ".jpg";
                    break;
                case ".png":
                    imgType = ImageFormat.Png;
                    extension = ".png";
                    break;
                default:
                    imgType = ImageFormat.Jpeg;
                    extension = ".jpg";
                    break;
            }

            // �޸�Ŀ���ļ��ĺ�׺��
            desImgName = Regex.Replace(desImgName, @"\.\w+$", extension);

            // �����ļ���·��
            FileInfo file = new FileInfo(desImgName);
            DirectoryInfo dinfo = new DirectoryInfo(file.DirectoryName);
            if (!dinfo.Exists)
                dinfo.Create();

            if (file.Exists)
            {
                if (isOverride)
                    file.Delete();
                else
                {
                    string filestr = file.Name.Replace(extension, "");
                    FileInfo[] files = dinfo.GetFiles(filestr + "*", SearchOption.TopDirectoryOnly);
                    int maxIndex = 0;
                    string regstr = @"[MSLo]_(?<index>\d+)";
                    for (int i = 0; i < files.Length; i++)
                    {
                        Match match = Regex.Match(files[i].Name, regstr);
                        if (match.Success)
                        {
                            try
                            {
                                int curIndex = int.Parse(match.Groups["index"].Value);
                                if (maxIndex < curIndex)
                                {
                                    maxIndex = curIndex;
                                }
                            }
                            catch { }
                        }
                    }
                    maxIndex = maxIndex + 1;

                    //desImgName = file.DirectoryName + "\\" + filestr + "_N" + maxIndex + extension;
                    desImgName = file.DirectoryName + "\\" + filestr + "_" + maxIndex + extension;
                }
            }


            // ����ͼƬ�����ųߴ�
            Image imgSource = Image.FromFile(sourImgName);
            if (imgSize == -1)
            {
                iWidth = imgSource.Width;
                iHeight = imgSource.Height;
            }
            else
            {
                if (imgSource.Width >= imgSource.Height)
                {
                    iWidth = imgSize;
                    iHeight = (int)(imgSize * ((float)imgSource.Height / imgSource.Width));
                }
                else
                {
                    iHeight = imgSize;
                    iWidth = (int)(imgSize * ((float)imgSource.Width / imgSource.Height));
                }
            }

            Bitmap bmTarget = new Bitmap(iWidth, iHeight, PixelFormat.Format32bppArgb);
            bmTarget.SetResolution(imgSource.HorizontalResolution, imgSource.VerticalResolution);
            bmTarget.MakeTransparent(Color.White);

            //��������
            Graphics g = Graphics.FromImage(bmTarget);

            try
            {
                //��ʼ������
                g.Clear(Color.White);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.CompositingQuality = CompositingQuality.GammaCorrected;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;

                // �ڻ����ϻ�ͼ
                g.DrawImage(imgSource, 0, 0, Convert.ToSingle(iWidth), Convert.ToSingle(iHeight));

                // �ͷ���Դ
                imgSource.Dispose();
                imgSource = (Image)bmTarget.Clone();
                imgSource.Save(desImgName, imgType);
            }
            catch
            {
                throw;
            }
            finally
            {
                imgSource.Dispose();
                g.Dispose();
                bmTarget.Dispose();
            }
            return desImgName;
        }

        /// <summary>
        /// ������ƷID�������ƷͼƬ�����·����������ͼƬ���ļ�����
        /// </summary>
        /// <param name="productID">��ƷID</param>
        /// <returns></returns>
        public static string ProductPictureRelativePath(int productID)
        {
            string full = productID.ToString("000000");
            return full.Substring(0, 3) + "/";

        }

        /// <summary>
        /// �����ļ���ָ������Ŀ¼��
        /// </summary>
        /// <param name="localpath">�ļ����Ŀ¼</param>
        /// <param name="imgUrlPath">url·��(http)</param>
        /// <returns></returns>
        public static string DownloadImage(string localpath, string imgUrlPath)
        {
            string filename = imgUrlPath.Substring(imgUrlPath.LastIndexOf("/") + 1);
            filename = String.Format("{0}{1}", localpath, filename);
            if (!imgUrlPath.StartsWith("http://"))
                imgUrlPath = "http://" + imgUrlPath;
            WebClient client = new WebClient();
            client.DownloadFile(imgUrlPath, filename);
            return filename;
        }

        /// <summary>
        /// ��ȡ��ƷĬ��ͼ
        /// </summary>
        /// <param name="images">ͼƬ�ַ���</param>
        /// <returns>ͼƬ·��</returns>
        public static string ProductDefaultImage(string images)
        {
            if (null != images && images.Length > 0)
            {
                string[] imgList = images.Split("|".ToCharArray());
                return (imgList.Length > 0 && imgList[0].Length > 0) ? "http://img.jxdyf.com/product/" + imgList[0] : "http://img.jxdyf.com/picture.jpg";
            }
            else
            {
                return "http://img.jxdyf.com/picture.jpg";
            }
        }
        /// <summary>
        /// ��ȡ��ƷĬ��ͼ
        /// </summary>
        /// <param name="images">ͼƬ�ַ���</param>
        /// <returns>ͼƬ·��</returns>
        public static string ProductDefaultImageRelativeUrl(string images)
        {
            if (null != images && images.Length > 0)
            {
                string[] imgList = images.Split("|".ToCharArray());
                return (imgList.Length > 0 && imgList[0].Length > 0) ? imgList[0] : string.Empty;
            }
            else
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// ��ȡ��ƷͼƬ
        /// </summary>
        /// <param name="images">ͼƬ�ַ���</param>
        /// <returns>ͼƬ·��</returns>
        public static List<string> ProductImages(string images)
        {
            string[] imgList = images.Split("|".ToCharArray());
            List<string> list = new List<string>();
            for (int i = 0; i < imgList.Length; i++)
            {
                if (!string.IsNullOrEmpty(imgList[i]))
                {
                    list.Add("http://img.jxdyf.com/product/" + imgList[i]);
                }
            }
            if (list.Count == 0)
            {
                list.Add("http://img.jxdyf.com/picture.jpg");
            }
            return list;
        }

        /// <summary>
        /// ��ȡ��ƷĬ��ͼ
        /// </summary>
        /// <param name="images">ͼƬ�ַ���</param>
        /// <returns>ͼƬ·��</returns>
        public static string UnionProductDefaultImage(string images)
        {
            string[] imgList = images.Split("|".ToCharArray());
            return (imgList.Length > 0 && imgList[0].Length > 0) ? "http://img.jxdyf.com/union/" + imgList[0] : "http://img.jxdyf.com/picture.jpg";
        }

        /// <summary>
        /// ���ͼƬ�б�
        /// </summary>
        /// <param name="images">ͼƬ�ַ���</param>
        /// <returns>ͼƬ·��</returns>
        public static string ProductDefaultImageList(string images, string alt)
        {
            StringBuilder sBuilder = new StringBuilder("<ul class=\"imgList jcarousel-skin-tango\">");
            string[] imgList = images.Split("|".ToCharArray());
            for (var i = 0; i < imgList.Length; i++)
            {
                if (i == 0)
                    sBuilder.Append("<li class=\"cur\">");
                else
                    sBuilder.Append("<li>");
                sBuilder.Append(string.Format("<img alt=\"{0}\" src=\"{1}\" ></li>", alt, (imgList[i].Length > 0) ? "http://img.jxdyf.com/product/" + imgList[i] : "http://img.jxdyf.com/picture.jpg"));
            }
            sBuilder.Append("</ul>");
            return sBuilder.ToString();
        }

        /// <summary>
        /// ���ͼƬ�б�(��ͼҳ��)
        /// </summary>
        /// <param name="images">ͼƬ�ַ���</param>
        /// <returns>ͼƬ·��</returns>
        public static string ProductBigImageList(string images, string alt)
        {
            StringBuilder sBuilder = new StringBuilder();
            string[] imgList = images.Split("|".ToCharArray());
            sBuilder.Append("<ul>");
            for (var i = 0; i < imgList.Length; i++)
            {
                if (i == 0)
                    sBuilder.Append("<li><a class=\"cur\">");
                else
                    sBuilder.Append("<li><a>");
                sBuilder.Append(string.Format("<img alt=\"{0}\" src=\"{1}\" gid=\"{2}\" ></a></li>", alt, (imgList[i].Length > 0) ? "http://img.jxdyf.com/product/" + imgList[i].Replace("_S", "_M") : "http://img.jxdyf.com/picture.jpg", (i + 1)));
            }
            sBuilder.Append("</ul>");
            return sBuilder.ToString();
        }

        /// <summary>
        /// ��ȡƷ��Ĭ��ͼ
        /// </summary>
        /// <param name="images">ͼƬ�ַ���</param>
        /// <returns>ͼƬ·��</returns>
        public static string BrandDefaultImage(string images)
        {
            string[] imgList = images.Split("|".ToCharArray());
            return (imgList.Length > 0 && imgList[0].Length > 0) ? "http://img.jxdyf.com/brand/" + imgList[0] : "http://img.jxdyf.com/picture.jpg";
        }
    }
}