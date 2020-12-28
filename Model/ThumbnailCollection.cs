using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using OpenCvSharp;

namespace Model
{
    public class ThumbnailCollection
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly Lazy<ThumbnailCollection> instance =
            new Lazy<ThumbnailCollection>(() => new ThumbnailCollection());

        public static ThumbnailCollection Instance  { get { return instance.Value; } }

        private ThumbnailCollection()
        {
            thumbnailList = new List<Thumbnail>();
        }

        private List<Thumbnail> thumbnailList;
        //public List<Thumbnail> ThumbnailList { get => thumbnailList; set => thumbnailList = value; }

        public void AddThumbnail(Thumbnail thumbnail)
        {
            thumbnailList.Add(thumbnail);
        }

        public void RemoveThumbnail(Thumbnail thumbnail)
        {
            thumbnailList.Remove(thumbnail);
        }

        public void RemoveAllThumbnails()
        {
            thumbnailList = null;
        }

        public int GetSize()
        {
            return thumbnailList.Count;
        }
        public Thumbnail GetThumbnailWithIndex(int i)
        {
            return thumbnailList[i];
        }
        public List<Thumbnail> GetThumbnailList()
        {
            return thumbnailList;
        }
        public List<Mat> GetThumbnailListOnlyImage()
        {
            List<Mat> imageList = new List<Mat>();
            foreach (var item in thumbnailList)
            {
                imageList.Add(item.Image);
            }
            return imageList;
        }
        public List<string> GetThumbnailListOnlyFileName()
        {
            List<string> imageList = new List<string>();
            foreach (var item in thumbnailList)
            {
                imageList.Add(item.FileName);
            }
            return imageList;
        }
    }
}
