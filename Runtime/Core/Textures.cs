using System;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SD = System.Diagnostics;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Experimental.Rendering;


namespace RanterTools.Textures
{
    public class Textures : MonoBehaviour
    {
        #region Events

        #endregion Events

        #region Global State

        #endregion Global State

        #region Global Methods
        public static async Task<string> Texture2DToBase64Async(Texture2D texture)
        {
            SD.Stopwatch stopwatch = SD.Stopwatch.StartNew();
            byte[] imageData = ImageConversion.EncodeToPNG(texture);
            var outer = Task.Factory.StartNew<string>(() =>      // внешняя задача
            {

                string base64 = Convert.ToBase64String(imageData).Insert(0, "data:image/png;base64,");
                stopwatch = SD.Stopwatch.StartNew();
                return base64;
            });
            await outer;
            return outer.Result;
        }

        public static string Texture2DToBase64(Texture2D texture)
        {
            SD.Stopwatch stopwatch = SD.Stopwatch.StartNew();
            byte[] imageData = ImageConversion.EncodeToPNG(texture);
            string base64 = Convert.ToBase64String(imageData).Insert(0, "data:image/png;base64,");
            return base64;
        }

        public async static Task<Texture2D> Base64ToTexture2DAsync(string encodedData)
        {
            Texture2D texture;
            SD.Stopwatch stopwatch = SD.Stopwatch.StartNew();
            texture = new Texture2D(2, 2);
            texture.filterMode = FilterMode.Point;

            var outer = Task.Factory.StartNew<byte[]>(() =>      // внешняя задача
            {

                byte[] imageData = Convert.FromBase64String(encodedData.Replace("data:image/png;base64,", ""));
                stopwatch = SD.Stopwatch.StartNew();
                return imageData;
            });
            await outer;
            var path = Path.Combine(Application.persistentDataPath, $"{encodedData.GetHashCode()}.png");
            File.WriteAllBytes(path, outer.Result);
            RanterTools.Base.ToolsDebug.Log($"Path to texture file:{Path.Combine("file://", path.TrimStart('/'))}");
            UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(Path.Combine("file://", path.TrimStart('/')));
            var result = unityWebRequest.SendWebRequest();
            while (!result.isDone)
            {
                await Task.Yield();
            }
            Texture2D image = new Texture2D(2, 2, TextureFormat.ARGB32, false);
            image = DownloadHandlerTexture.GetContent(unityWebRequest);
            Debug.Log($"End convert texture from base64 with {stopwatch.ElapsedMilliseconds}ms");
            File.Delete(path);
            return image;
        }

        public static Texture2D Base64ToTexture2D(string encodedData)
        {
            Texture2D texture;
            SD.Stopwatch stopwatch = SD.Stopwatch.StartNew();
            texture = new Texture2D(2, 2);
            texture.filterMode = FilterMode.Point;
            byte[] imageData = Convert.FromBase64String(encodedData.Replace("data:image/png;base64,", ""));
            stopwatch = SD.Stopwatch.StartNew();
            ImageConversion.LoadImage(texture, imageData, false);
            Debug.Log($"End convert texture from base64 with {stopwatch.ElapsedMilliseconds}ms");
            return texture;
        }


        static int ReadInt(byte[] imageData, int offset)
        {
            return (imageData[offset] << 8) | imageData[offset + 1];
        }



        #endregion Global Methods

        #region Parameters

        #endregion Parameters

        #region State

        #endregion State

        #region Methods

        #endregion Methods
    }


    public class TextureScale
    {
        public class ThreadData
        {
            public int start;
            public int end;
            public ThreadData(int s, int e)
            {
                start = s;
                end = e;
            }
        }

        private static Color[] texColors;
        private static Color[] newColors;
        private static int w;
        private static float ratioX;
        private static float ratioY;
        private static int w2;
        private static int finishCount;
        private static Mutex mutex;

        public static void Point(Texture2D tex, int newWidth, int newHeight)
        {
            ThreadedScale(tex, newWidth, newHeight, false);
        }

        public static void Bilinear(Texture2D tex, int newWidth, int newHeight)
        {
            ThreadedScale(tex, newWidth, newHeight, true);
        }

        private static void ThreadedScale(Texture2D tex, int newWidth, int newHeight, bool useBilinear)
        {
            texColors = tex.GetPixels();
            newColors = new Color[newWidth * newHeight];
            if (useBilinear)
            {
                ratioX = 1.0f / ((float)newWidth / (tex.width - 1));
                ratioY = 1.0f / ((float)newHeight / (tex.height - 1));
            }
            else
            {
                ratioX = ((float)tex.width) / newWidth;
                ratioY = ((float)tex.height) / newHeight;
            }
            w = tex.width;
            w2 = newWidth;
            var cores = Mathf.Min(SystemInfo.processorCount, newHeight);
            var slice = newHeight / cores;

            finishCount = 0;
            if (mutex == null)
            {
                mutex = new Mutex(false);
            }
            if (cores > 1)
            {
                int i = 0;
                ThreadData threadData;
                for (i = 0; i < cores - 1; i++)
                {
                    threadData = new ThreadData(slice * i, slice * (i + 1));
                    ParameterizedThreadStart ts = useBilinear ? new ParameterizedThreadStart(BilinearScale) : new ParameterizedThreadStart(PointScale);
                    Thread thread = new Thread(ts);
                    thread.Start(threadData);
                }
                threadData = new ThreadData(slice * i, newHeight);
                if (useBilinear)
                {
                    BilinearScale(threadData);
                }
                else
                {
                    PointScale(threadData);
                }
                while (finishCount < cores)
                {
                    Thread.Sleep(1);
                }
            }
            else
            {
                ThreadData threadData = new ThreadData(0, newHeight);
                if (useBilinear)
                {
                    BilinearScale(threadData);
                }
                else
                {
                    PointScale(threadData);
                }
            }

            tex.Resize(newWidth, newHeight);
            tex.SetPixels(newColors);
            tex.Apply();

            texColors = null;
            newColors = null;
        }

        public static void BilinearScale(System.Object obj)
        {
            ThreadData threadData = (ThreadData)obj;
            for (var y = threadData.start; y < threadData.end; y++)
            {
                int yFloor = (int)Mathf.Floor(y * ratioY);
                var y1 = yFloor * w;
                var y2 = (yFloor + 1) * w;
                var yw = y * w2;

                for (var x = 0; x < w2; x++)
                {
                    int xFloor = (int)Mathf.Floor(x * ratioX);
                    var xLerp = x * ratioX - xFloor;
                    newColors[yw + x] = ColorLerpUnclamped(ColorLerpUnclamped(texColors[y1 + xFloor], texColors[y1 + xFloor + 1], xLerp),
                                                           ColorLerpUnclamped(texColors[y2 + xFloor], texColors[y2 + xFloor + 1], xLerp),
                                                           y * ratioY - yFloor);
                }
            }

            mutex.WaitOne();
            finishCount++;
            mutex.ReleaseMutex();
        }

        public static void PointScale(System.Object obj)
        {
            ThreadData threadData = (ThreadData)obj;
            for (var y = threadData.start; y < threadData.end; y++)
            {
                var thisY = (int)(ratioY * y) * w;
                var yw = y * w2;
                for (var x = 0; x < w2; x++)
                {
                    newColors[yw + x] = texColors[(int)(thisY + ratioX * x)];
                }
            }

            mutex.WaitOne();
            finishCount++;
            mutex.ReleaseMutex();
        }

        private static Color ColorLerpUnclamped(Color c1, Color c2, float value)
        {
            return new Color(c1.r + (c2.r - c1.r) * value,
                              c1.g + (c2.g - c1.g) * value,
                              c1.b + (c2.b - c1.b) * value,
                              c1.a + (c2.a - c1.a) * value);
        }
    }

}