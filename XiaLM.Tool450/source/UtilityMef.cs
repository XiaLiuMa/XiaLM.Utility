using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XiaLM.Tool450.source
{
    /// <summary>
    /// MEF实用帮助类
    /// </summary>
    public class UtilityMef
    {
        private static UtilityMef mef;
        private readonly static object lockObj = new object();
        /// <summary>
        /// 得到MEF
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static UtilityMef GetInstance(params DirectoryCatalog[] directoryCatalogs)
        {
            if (mef == null)
            {
                lock (lockObj)
                {
                    if (mef == null)
                    {
                        if (directoryCatalogs == null || directoryCatalogs.Length <= 0)
                        {
                            mef = new UtilityMef();
                        }
                        else
                        {
                            mef = new UtilityMef(directoryCatalogs);
                        }
                    }
                }

            }
            return mef;
        }

        private readonly Lazy<CompositionContainer> container;
        private UtilityMef(DirectoryCatalog[] directoryCatalogs = null)
        {
            container = new Lazy<CompositionContainer>(() =>
            {
                AggregateCatalog catlog = new AggregateCatalog();
                List<string> list = new List<string>();
                if (directoryCatalogs == null)
                {
                    directoryCatalogs = new DirectoryCatalog[] { new DirectoryCatalog(AppDomain.CurrentDomain.BaseDirectory, "*.*") };
                }
                foreach (var dc in directoryCatalogs)
                {
                    list.AddRange(dc.LoadedFiles.ToArray());
                }
                foreach (var item in list)
                {
                    try
                    {

                        if (item.EndsWith(".dll", StringComparison.CurrentCultureIgnoreCase) || item.EndsWith(".exe", StringComparison.CurrentCultureIgnoreCase))
                        {
                            var ac = new AssemblyCatalog(item);
                            var c = ac.Parts.Count();
                            if (ac.Parts.Any())
                            {
                                catlog.Catalogs.Add(ac);
                            }
                        }

                    }
                    catch (ReflectionTypeLoadException)
                    {
                    }
                    catch (BadImageFormatException)
                    {
                    }
                }
                catlog.Catalogs.Add(new AssemblyCatalog(Assembly.GetEntryAssembly()));
                return new CompositionContainer(catlog);

            });
        }
        public void ComposeParts(object obj)
        {
            try
            {
                container.Value.ComposeParts(obj);
            }
            catch (ChangeRejectedException ex)
            {
                Trace.WriteLine(ex);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
        }
    }
}
