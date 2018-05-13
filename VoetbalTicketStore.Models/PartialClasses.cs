using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoetbalTicketStore.Models
{
    public class PartialClasses
    {
        [MetadataType(typeof(BezoekerMetadata))]
        public partial class Bezoeker
        {
        }
    }
}
