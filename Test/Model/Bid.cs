using System;
using System.Text;

namespace Model
{
    public class Bid
    {
        public virtual Guid Id { get; set; }

        public virtual Item Item { get; set; }

        private Int32 Version { get; set; }

        public virtual string Description { get; set; }

        public Bid()
        {
            EntityCreationCounterHelper.NewBidCall++;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("Bid Id: {0}{1}", Id, Environment.NewLine);
            sb.AppendFormat("Description: {0}{1}", Description, Environment.NewLine);

            return sb.ToString();
        }
    }
}
