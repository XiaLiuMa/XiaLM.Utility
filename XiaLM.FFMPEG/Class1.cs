using System;
using System.Diagnostics;
using System.Linq;

namespace XiaLM.FFMPEG
{
    public class Class1
    {
        ///// <summary>
        ///// 推流到RTMP服务器
        ///// </summary>
        //public void Push()
        //{
        //    Validate();

        //    if (_destType != TargetType.Live)
        //    {
        //        throw new ApplicationException("当推流到RTMP服务器的时候，源类型必须是'RtmpType.Live'类型.");
        //    }

        //    //参数为false的时候则为推流
        //    var @params = GetParams(false);

        //    Process.FFmpeg(@params);
        //}

        ///// <summary>
        ///// 把流从RTMP服务器拉取--读取视频数据 ==pull a stream from rtmp server
        ///// </summary>
        //public void Pull()
        //{
        //    Validate();

        //    if (!TestRtmpServer(_source, true))
        //        throw new ApplicationException("RTMP服务器发送错误.");

        //    if (_sourceType != TargetType.Live)
        //    {
        //        throw new ApplicationException("必须是RTMP服务器.");
        //    }
        //    //参数为true的时候则为读取视频流
        //    var @params = GetParams(false);

        //    Processor.FFmpeg(@params);
        //}

        ///// <summary>
        ///// 检测输出输入源以及过滤器
        ///// </summary>
        //private void Validate()
        //{
        //    if (_sourceType == TargetType.Default)
        //        throw new ApplicationException("源错误.请输入源！");

        //    if (_destType == TargetType.Default)
        //        throw new ApplicationException("dest错误.请输入一个dest");

        //    var supportFilters = new[] { "Resize", "Segment", "X264", "AudioRate", "AudioBitrate" };

        //    if (_filters.Any(x => !supportFilters.Contains(x.Name)))
        //    {
        //        throw new ApplicationException(string.Format("过滤器不支持，过滤器只支持:{0} 类型",
        //            supportFilters.Aggregate(string.Empty, (current, filter) => current + (filter + ",")).TrimEnd(new[] { ',' })));
        //    }
        //}

    }
}
