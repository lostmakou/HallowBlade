using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeldaWPF
{
    internal class Notes
    {
        public string NoteString;
        public int Id;
        public Notes(string NoteString, int Id) {
            this.NoteString = NoteString;
            this.Id = Id;
        }
    }
}
