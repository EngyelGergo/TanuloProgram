using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TanuloProgram.MVVM.Model
{
    public class Word
    {
        [Key]
        public int Id { get; set; }
        public int IsWord { get; set; }
        public string NativeWord { get; set; }
        public string ForeignWord { get; set; }
    }
}
