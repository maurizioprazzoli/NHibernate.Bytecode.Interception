using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Item
    {
        public virtual Guid Id { get; set; }

        private Int32 Version { get; set; }

        public virtual String Description { get; set; }

        public virtual IList<Bid> Bids { get; set; }

        public virtual void AddBid(Guid idBid, String bidDescription)
        {
            Bid bid = new Bid();
            bid.Id = idBid;
            bid.Description = bidDescription;
            bid.Item = this;
            this.Bids.Add(bid);
        }

        public Item()
        {
            this.Bids = new List<Bid>();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("Item Id: {0}{1}", Id, Environment.NewLine);
            sb.AppendFormat("Description: {0}{1}", Description, Environment.NewLine);

            foreach (var bid in Bids)
            {
                sb.AppendLine(bid.ToString());
            }

            return sb.ToString();
        }
    }
}
