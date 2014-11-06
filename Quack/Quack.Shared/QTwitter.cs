using System;
using System.Collections.Generic;
using System.Text;
using LinqToTwitter;
using System.Threading.Tasks;

namespace Quack
{
    class QTwitter
    {
        static QTwitter instance = null;
        static TwitterContext context = null;

        QTwitter()
        {

        }

        public static QTwitter GetInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new QTwitter();
                }

                return instance;
            }
        }

        public static TwitterContext GetContext
        {
            get
            {
                return context;
            }
        }

        public static PinAuthorizer GetPinAuthorizer()
        {
            return new PinAuthorizer()
            {
                CredentialStore = new InMemoryCredentialStore
                {
                    ConsumerKey = Constants.CONSUMER_KEY,
                    ConsumerSecret = Constants.CONSUMER_SECRET
                }
            };
        }
    }
}
