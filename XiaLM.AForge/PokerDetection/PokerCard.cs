using AForge;
using System.Drawing;

namespace XiaLM.AForge.PokerDetection
{
    public class PokerCard
    {
        private Rank rank; //点数
        private Suit suit; // 花色
        private Bitmap image; // 提取出的图像
        private System.Drawing.Point[] corners;// 四角点

        public System.Drawing.Point[] Corners
        {
            get { return this.corners; }
        }
        public Rank Rank
        {
            set { this.rank = value; }
        }
        public Suit Suit
        {
            set { this.suit = value; }
        }
        public Bitmap Image
        {
            get { return this.image; }
        }
        // 构造函数
        public PokerCard(Bitmap cardImg, IntPoint[] cornerIntPoints)
        {
            this.image = cardImg;

            // 将AForge.IntPoint数组转化为System.Drawing.Point数组
            int total = cornerIntPoints.Length;
            corners = new System.Drawing.Point[total];

            for (int i = 0; i < total; i++)
            {
                this.corners[i].X = cornerIntPoints[i].X;
                this.corners[i].Y = cornerIntPoints[i].Y;
            }
        }
    }

    /// <summary>
    /// 点数
    /// </summary>
    public enum Rank
    {
        NOT_RECOGNIZED = 0,
        Ace,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King
    }

    /// <summary>
    /// 花色
    /// </summary>
    public enum Suit
    {
        NOT_RECOGNIZED = 0,
        Hearts,
        Diamonds,
        Spades,
        Clubs
    }

}
