using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Well.Api.Mapper
{
    interface IMapper<in TSource, out TTarget> where TSource : class where TTarget : class
    {
        TTarget Map(TSource source);
    }
}
