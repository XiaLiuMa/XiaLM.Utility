//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.InteropServices;
//using XlmUtility.source.common;

//namespace XiaLM.Tool450.source.tcp
//{
//    public class XlmParser: IParser
//    {
//        List<byte> bytesBuff = new List<byte>();
//        List<byte> datas = new List<byte>();
//        /// <summary>
//        /// 包头标志
//        /// </summary>
//        private byte[] head = new byte[2] { 0xEA, 0x56 };
//        /// <summary>
//        /// 是否CRC16校验
//        /// </summary>
//        private bool isCrc = false;

//        public event Action<byte[]> ReceiveDataEvent;

//        public XlmParser() : this(false) { }
//        public XlmParser(bool iscrc)
//        {
//            isCrc = iscrc;
//        }

//        /// <summary>
//        /// 编码
//        /// </summary>
//        /// <param name="buffer"></param>
//        /// <returns></returns>
//        public byte[] Encoding(byte[] buffer)
//        {
//            List<byte> list = new List<byte>();
//            int totalLength = isCrc ? buffer.Length + 2 : buffer.Length;
//            list.AddRange(head);//标识头
//            list.AddRange(BitConverter.GetBytes(buffer.Length).Reverse().ToArray());//数据区长度
//            list.AddRange(buffer);//数据
//            if (isCrc)
//            {
//                ushort crcNum = CrcVerifyHelper.CRC16_CCITT(buffer);
//                byte[] crcBs = BitConverter.GetBytes(crcNum).Reverse().ToArray();
//                list.AddRange(crcBs);//校验码
//            }
//            return list.ToArray();
//        }

//        public void Decoding(byte[] playload, DataDecodResults callback)
//        {
//            bytesBuff.AddRange(playload);
//            while (true)
//            {
//                if (datas.Count == 0)
//                {
//                    if (bytesBuff.Count >= 2)
//                    {
//                        byte[] head = bytesBuff.Take(2).ToArray();
//                        if (isHead(head))
//                        {
//                            if (bytesBuff.Count <= fixHeadLenght) return;
//                            byte crc = _list.Skip(2).FirstOrDefault();
//                            int length = BitConverter.ToInt32(_list.Skip(2).Take(4).Reverse().ToArray(), 0);
//                            if (length < 0)
//                            {
//                                bytesBuff.RemoveAt(0);
//                                continue;
//                            }
//                            dataLenght = length + (_isCrc ? 2 : 0);//如果crc则数据长度加2
//                            int tempLength = (bytesBuff.Count - fixHeadLenght);
//                            if ((tempLength - dataLenght) == 0)
//                            {
//                                datas.AddRange(bytesBuff.Skip(fixHeadLenght));
//                                bytesBuff.RemoveRange(0, fixHeadLenght + dataLenght);
//                                dataResult(datas, callback);
//                                break;
//                            }
//                            else
//                            if ((tempLength - dataLenght) < 0)
//                            {
//                                datas.AddRange(bytesBuff.Skip(fixHeadLenght));
//                            }
//                            else
//                            {
//                                datas.AddRange(bytesBuff.Skip(fixHeadLenght).Take(dataLenght));
//                            }
//                            bytesBuff.RemoveRange(0, fixHeadLenght + datas.Count);
//                        }
//                        else
//                        {
//                            bytesBuff.RemoveAt(0);
//                        }
//                    }
//                    else
//                    {
//                        if (bytesBuff.Count <= 0) break;
//                        if (bytesBuff.Count == 1)
//                        {
//                            if (!bytesBuff.First().Equals(0xEA))
//                            {
//                                bytesBuff.RemoveAt(0);
//                            }
//                            break;
//                        }
//                    }
//                }
//                else
//                {
//                    int d = datas.Count;
//                    int c = dataLenght - d;
//                    if (c == 0)
//                    {
//                        dataResult(datas, callback);
//                        continue;
//                    }
//                    else
//                    {
//                        if (bytesBuff.Count == 0) break;
//                        if (bytesBuff.Count >= c)
//                        {
//                            datas.AddRange(_list.Take(c));
//                            bytesBuff.RemoveRange(0, c);
//                        }
//                        else
//                        {
//                            datas.AddRange(bytesBuff);
//                            bytesBuff.RemoveRange(0, bytesBuff.Count);
//                        }
//                    }
//                }
//            }
//        }

//        private bool isHead(byte[] bs)
//        {
//            if (bs.Length != 2) return false;
//            var b = true;
//            int i = 0;
//            foreach (var item in head)
//            {
//                if (!item.Equals(bs[i]))
//                    b = false;
//                i++;
//            }
//            return b;
//        }

//        private void dataResult(List<byte> list, DataDecodResults callback)
//        {
//            if (isCrc)
//            {
//                byte[] data = list.Take(list.Count - 2).ToArray();
//                ushort crcNum = BitConverter.ToUInt16(list.Skip(list.Count - 2).Reverse().ToArray(), 0);//数据的校验码
//                var cn = CrcVerifyHelper.CRC16_CCITT(data);
//                if (cn == crcNum)
//                {
//                    callback?.Invoke(new TcpBuffer(data, 0, data.Length));
//                }
//            }
//            else
//            {
//                var data = list.ToArray();
//                callback?.Invoke(new TcpBuffer(data, 0, data.Length));
//            }
//            datas.Clear();
//            dataLenght = 0;
//        }

//    }
//}
