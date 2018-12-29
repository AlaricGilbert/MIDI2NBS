using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIDIConventor
{
    public enum BaseType
    {
        Bass,
        SnareDrum,
        Piano,
        Click,
        BassDrum
    }
    public class NoteInfo
    {
        public BaseType BaseType { get; set; } = BaseType.Bass;
        public int Note { get; set; } = 0;
        public int ActualNote { get; set; } = 0;
        public override string ToString()
        {
            return String.Format("base type: {0}, note: {1}", BaseType, Note);
        }
    }
}
