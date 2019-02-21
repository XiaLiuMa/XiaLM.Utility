using AForge.Imaging;
using AForge.Imaging.Filters;
using System.Drawing;
using System.Drawing.Imaging;

namespace XiaLM.AForge.PokerDetection
{
    /// <summary>
    /// 扑克识别器
    /// </summary>
    public class PokerRecognizer
    {

        ///// <summary>
        ///// 区分花色
        ///// </summary>
        ///// <returns></returns>
        //public char Test()
        //{
        //    char color = 'B';
        //    // 开始，锁像素
        //    BitmapData imageData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
        //        ImageLockMode.ReadOnly, bmp.PixelFormat);
        //    int totalRed = 0;
        //    int totalBlack = 0;

        //    // 统计红与黑
        //    try
        //    {
        //        UnmanagedImage img = new UnmanagedImage(imageData);

        //        int height = img.Height;
        //        int width = img.Width;
        //        int pixelSize = (img.PixelFormat == PixelFormat.Format24bppRgb) ? 3 : 4;
        //        byte* p = (byte*)img.ImageData.ToPointer();

        //        // 逐行
        //        for (int y = 0; y < height; y++)
        //        {
        //            // 逐像素
        //            for (int x = 0; x < width; x++, p += pixelSize)
        //            {
        //                int r = (int)p[RGB.R]; // 红
        //                int g = (int)p[RGB.G]; // 绿
        //                int b = (int)p[RGB.B]; // 蓝

        //                if (r > g + b)  // 红 > 绿 + 蓝
        //                    totalRed++;  // 认为是红色

        //                if (r <= g + b && r < 50 && g < 50 && b < 50) // 红绿蓝均小于50
        //                    totalBlack++; // 认为是黑色
        //            }
        //        }
        //    }
        //    finally
        //    {
        //        bmp.UnlockBits(imageData); // 解锁
        //    }

        //    if (totalRed > totalBlack) // 红色占优
        //        color = 'R'; // 设置颜色为红，否则默认黑色
        //    return color;
        //}


        ///// <summary>
        ///// 获取右上角部分(包括点数和花色)
        ///// </summary>
        ///// <returns></returns>
        //public Bitmap GetTopRightPart()
        //{
        //    if (image == null)
        //        return null;
        //    Crop crop = new Crop(new Rectangle(image.Width - 37, 10, 30, 60));

        //    return crop.Apply(image);
        //}


        ///// <summary>
        ///// 是否是人物牌(包括JQK)
        ///// </summary>
        ///// <param name="bmp"></param>
        ///// <returns></returns>
        //private bool IsFaceCard(Bitmap bmp)
        //{
        //    FiltersSequence commonSeq = new FiltersSequence();
        //    commonSeq.Add(Grayscale.CommonAlgorithms.BT709);
        //    commonSeq.Add(new BradleyLocalThresholding());
        //    commonSeq.Add(new DifferenceEdgeDetector());

        //    Bitmap temp = this.commonSeq.Apply(bmp);
        //    ExtractBiggestBlob extractor = new ExtractBiggestBlob();
        //    temp = extractor.Apply(temp); // 提取最大图块

        //    if (temp.Width > bmp.Width / 2)  // 如果宽度大于整个牌的一般宽
        //        return true; // 人物牌
        //    return false;  // 数字牌
        //}

        //private Suit ScanSuit(Bitmap suitBmp, char color)
        //{
        //    Bitmap temp = commonSeq.Apply(suitBmp);
        //    //Extract biggest blob on card
        //    ExtractBiggestBlob extractor = new ExtractBiggestBlob();
        //    temp = extractor.Apply(temp);  //Biggest blob is suit blob so extract it
        //    Suit suit = Suit.NOT_RECOGNIZED;

        //    //Determine type of suit according to its color and width
        //    if (color == 'R')
        //        suit = temp.Width >= 55 ? Suit.Diamonds : Suit.Hearts;
        //    if (color == 'B')
        //        suit = temp.Width <= 48 ? Suit.Spades : Suit.Clubs;

        //    return suit;
        //}

        //private Suit ScanFaceSuit(Bitmap bmp, char color)
        //{
        //    Bitmap clubs, diamonds, spades, hearts; // 花色模板 4 
        //                                            // 载入模板资源
        //    clubs = PlayingCardRecognition.Properties.Resources.Clubs;
        //    diamonds = PlayingCardRecognition.Properties.Resources.Diamonds;
        //    spades = PlayingCardRecognition.Properties.Resources.Spades;
        //    hearts = PlayingCardRecognition.Properties.Resources.Hearts;

        //    // 用0.8的相似度阈值初始化模板匹配类
        //    ExhaustiveTemplateMatching templateMatching = new ExhaustiveTemplateMatching(0.8f);
        //    Suit suit = Suit.NOT_RECOGNIZED;

        //    if (color == 'R') // 如果是红色
        //    {
        //        if (templateMatching.ProcessImage(bmp, hearts).Length > 0)
        //            suit = Suit.Hearts; //匹配红桃
        //        if (templateMatching.ProcessImage(bmp, diamonds).Length > 0)
        //            suit = Suit.Diamonds; // 匹配方块
        //    }
        //    else // 如果是黑色
        //    {
        //        if (templateMatching.ProcessImage(bmp, spades).Length > 0)
        //            suit = Suit.Spades; // 匹配黑桃
        //        if (templateMatching.ProcessImage(bmp, clubs).Length > 0)
        //            suit = Suit.Clubs; // 匹配梅花
        //    }
        //    return suit;
        //}

        //private Rank ScanRank(Bitmap cardImage)
        //{
        //    Rank rank = Rank.NOT_RECOGNIZED;

        //    int total = 0;
        //    Bitmap temp = commonSeq.Apply(cardImage); // 应用滤镜
        //    BlobCounter blobCounter = new BlobCounter();
        //    blobCounter.FilterBlobs = true;
        //    // 过滤小图块
        //    blobCounter.MinHeight = blobCounter.MinWidth = 30;
        //    blobCounter.ProcessImage(temp);

        //    total = blobCounter.GetObjectsInformation().Length; // 获取总数
        //    rank = (Rank)total; // 转换成大小（枚举类型）

        //    return rank;
        //}

        //private Rank ScanFaceRank(Bitmap bmp)
        //{
        //    Bitmap j, k, q; // 人物牌人物模板 4      // 载入资源
        //    j = PlayingCardRecognition.Properties.Resources.J;
        //    k = PlayingCardRecognition.Properties.Resources.K;
        //    q = PlayingCardRecognition.Properties.Resources.Q;


        //    // 用0.75进行初始化
        //    ExhaustiveTemplateMatching templateMatchin =
        //              new ExhaustiveTemplateMatching(0.75f);
        //    Rank rank = Rank.NOT_RECOGNIZED;

        //    if (templateMatchin.ProcessImage(bmp, j).Length > 0) // J
        //        rank = Rank.Jack;
        //    if (templateMatchin.ProcessImage(bmp, k).Length > 0)// K
        //        rank = Rank.King;
        //    if (templateMatchin.ProcessImage(bmp, q).Length > 0)// Q
        //        rank = Rank.Queen;

        //    return rank;
        //}
    }
}
