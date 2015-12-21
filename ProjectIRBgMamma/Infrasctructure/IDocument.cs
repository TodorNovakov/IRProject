using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectIRBgMamma.Infrasctructure
{
    /// <summary>
    /// Basic interface where every document should inherit from
    /// </summary>
    public interface IDocument
    {
        int Id { get; set; }
    }
}
