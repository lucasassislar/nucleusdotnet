using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.ConsoleEngine {
    public class ConsoleGraphics {
        private char[,] canvas;

        private Size size;

        public ConsoleGraphics(int width, int height) {
            canvas = new char[width, height];
            size = new Size(width, height);
        }

        public void Render() {
            Console.SetCursorPosition(0, 0);
            string buffer = "";
            for (int y = 0; y < size.Height; y++) {
                for (int x = 0; x < size.Width; x++) {
                    buffer += canvas[x, y];

                    canvas[x, y] = ' '; // clear canvas
                }
            }

            Console.Write(buffer);
        }

        private void EndSet() {
            // SLOW AS FUCK (10x) but can set color
            Console.SetCursorPosition(0, 0);
            for (int y = 0; y < size.Height; y++) {
                for (int x = 0; x < size.Width; x++) {
                    Console.Write(canvas[x, y]);
                    canvas[x, y] = ' '; // clear canvas
                }
            }
        }

        public void DrawHorizontalLine(char c, int x, int y, int width) {
            for (int i = 0; i < width; i++) {
                int xx = x + i;
                if (xx >= this.size.Width) {
                    continue;
                }
                canvas[xx, y] = c;
            }
        }

        public void DrawVerticalLine(char c, int x, int y, int height) {
            for (int i = 0; i < height; i++) {
                int yy = y + i;
                if (yy >= this.size.Height) {
                    continue;
                }
                canvas[x, yy] = c;
            }
        }

        public void DrawRectangle(int x, int y, int width, int height) {
            bool renderLeft = true;
            bool renderTop = true;
            if (x < 0) {
                width -= Math.Abs(x);
                renderLeft = false;
                x = 0;
            } else if (x > this.size.Width) {
                return;
            }
            if (y < 0) {
                height -= Math.Abs(y);
                renderTop = false;
                y = 0;
            }

            if (width < 0 || height < 0) {
                return;
            }

            int botY = y + height - 1;
            for (int pX = x + 1; pX < x + width; pX++) {
                if (pX >= this.size.Width) {
                    continue;
                }

                if (renderTop) {
                    canvas[pX, y] = '-';
                }

                if (botY < this.size.Height) {
                    canvas[pX, botY] = '-';
                }
            }

            int rightX = x + width;
            for (int pY = y; pY <= botY; pY++) {
                if (pY >= this.size.Height) {
                    continue;
                }
                if (renderLeft) {
                    canvas[x, pY] = '|';
                }

                if (rightX < this.size.Width) {
                    canvas[rightX, pY] = '|';
                }
            }
        }

        public void FillRectangle(int x, int y, int width, int height, char c) {
            for (int pX = x; pX < x + width; pX++) {
                if (pX >= this.size.Width) {
                    continue;
                }

                for (int pY = y; pY < y + height; pY++) {
                    if (pY >= this.size.Height) {
                        continue;
                    }
                    canvas[pX, pY] = c;
                }
            }
        }

        //public void DrawCircle(int x, int y, int radius) {
        //    int diameter = (int)(radius / 2.0f);
        //    Rectangle ellipse = new Rectangle(x, y, radius, radius);
        //    Vector2 center = new Vector2(x + (ellipse.Width / 2.0f), y + (ellipse.Height / 2.0f));
        //    for (int pX = x; pX < x + ellipse.Width; pX++) {
        //        for (int pY = y; pY < y + ellipse.Height; pY++) {
        //            Vector2 pt = new Vector2(pX, pY);
        //            float dist = Vector2.Distance(pt, center);
        //            if (dist > diameter ||
        //                dist < diameter * 0.90f) {
        //                continue;
        //            }

        //            Vector2 dir = Vector2.Normalize(pt - center);
        //            float angle = (float)Math.Atan2(dir.Y, dir.X);
        //            canvas[pX, pY] = 'o';
        //        }
        //    }
        //}

        //public void FillCircle(int x, int y, int radius, char c) {
        //    int diameter = (int)(radius / 2.0f);
        //    Rectangle ellipse = new Rectangle(x, y, radius, radius);
        //    Vector2 center = new Vector2(x + (ellipse.Width / 2.0f), y + (ellipse.Height / 2.0f));
        //    for (int pX = x; pX < x + ellipse.Width; pX++) {
        //        for (int pY = y; pY < y + ellipse.Height; pY++) {
        //            Vector2 pt = new Vector2(pX, pY);
        //            float dist = Vector2.Distance(pt, center);
        //            if (dist > diameter) {
        //                continue;
        //            }

        //            Vector2 dir = Vector2.Normalize(pt - center);
        //            float angle = (float)Math.Atan2(dir.Y, dir.X);
        //            canvas[pX, pY] = c;
        //        }
        //    }
        //}

        //public void DrawEllipse(int x, int y, int width, int height) {
        //    Rectangle ellipse = new Rectangle(x, y, width, height);
        //    Vector2 center = new Vector2(x + (width / 2.0f), y + (height / 2.0f));
        //    for (int pX = x; pX < x + width; pX++) {
        //        for (int pY = y; pY < y + height; pY++) {
        //            Vector2 pt = new Vector2(pX, pY);
        //            if (ElipseContains(ellipse, pt)) {
        //                if (Vector2.Distance(pt, center) > 10) {
        //                    continue;
        //                }

        //                Vector2 dir = Vector2.Normalize(pt - center);
        //                float angle = (float)Math.Atan2(dir.Y, dir.X);
        //                canvas[pX, pY] = getAngleChar(angle);
        //                //canvas[x, y] = ;
        //            }
        //        }
        //    }
        //}

        //public void FillEllipse(int x, int y, int width, int height, char c) {
        //    Rectangle ellipse = new Rectangle(x, y, width, height);
        //    for (int pX = x; pX < x + width; pX++) {
        //        for (int pY = y; pY < y + height; pY++) {
        //            if (ElipseContains(ellipse, new Vector2(pX, pY))) {
        //                canvas[pX, pY] = c;
        //            }
        //        }
        //    }
        //}
    }
}