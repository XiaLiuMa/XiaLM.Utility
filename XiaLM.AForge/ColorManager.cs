using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;

namespace XiaLM.AForge
{
    public class ColorManager
    {
        private const long CELLS_PER_LINE = 10;
        private const float MARGIN = 12;
        private const float CELL_WIDTH = 160;
        private const float CELL_HEIGHT = 64;
        private const float COLOR_LEFT_MARGIN = 8;
        private const float COLOR_TOP_MARGIN = 8;
        private const float COLOR_CELL_WIDTH = 48;
        private const float COLOR_CELL_HEIGHT = 32;
        private const float TEXT_TOP_MARGIN = COLOR_TOP_MARGIN + COLOR_CELL_HEIGHT + 2;

        /// <summary>
        /// 保存全部颜色信息到bmp图片
        /// </summary>
        /// <param name="filePath"></param>
        public void SaveAllColorToBmp(string filePath)
        {
            List<Color> vColors = new List<Color>();
            Type t = typeof(Color);
            PropertyInfo[] vProps = t.GetProperties();
            foreach (PropertyInfo propInfo in vProps)
            {
                if (MemberTypes.Property == propInfo.MemberType &&
                    typeof(Color) == propInfo.PropertyType)
                {
                    Color tmpColor = (Color)propInfo.GetValue(null, null);
                    vColors.Add(tmpColor);
                }
            }

            Bitmap bmpColor = new Bitmap((int)(CELLS_PER_LINE * CELL_WIDTH + MARGIN * 2), (int)((vColors.Count / CELLS_PER_LINE + 1) * CELL_HEIGHT + MARGIN * 2));
            using (Graphics grp = Graphics.FromImage(bmpColor))
            {
                grp.Clear(Color.Black);

                for (int i = 0; i < vColors.Count; i++)
                {
                    float nLeftBase = MARGIN + i % CELLS_PER_LINE * CELL_WIDTH;
                    float nTopBase = MARGIN + i / CELLS_PER_LINE * CELL_HEIGHT;

                    grp.DrawRectangle(new Pen(Color.White), nLeftBase, nTopBase, CELL_WIDTH, CELL_HEIGHT);

                    grp.FillRectangle(new SolidBrush(vColors[i]),
                                      nLeftBase + COLOR_LEFT_MARGIN, nTopBase + COLOR_TOP_MARGIN,
                                      COLOR_CELL_WIDTH, COLOR_CELL_HEIGHT);

                    grp.DrawString(vColors[i].Name,
                                   new Font("宋体", 9, FontStyle.Regular),
                                   new SolidBrush(Color.White),
                                   nLeftBase + COLOR_LEFT_MARGIN, nTopBase + TEXT_TOP_MARGIN);
                }
            }
            bmpColor.Save(filePath);
        }
    }
}
