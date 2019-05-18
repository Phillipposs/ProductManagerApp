
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTest
{
    public class SessionIndexer
    {
        private ISession Session;
        public SessionIndexer(ISession Session)
        {
            this.Session = Session;
        }
        public byte[] this[string key]
        {
            set
            {
                Session.Set(key, value);
            }
            get
            {
                return Session.Get(key);
            }
        }
    }
}
