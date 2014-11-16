using System;
using System.Collections.Generic;
using System.Text;
using LinqToTwitter;
using System.Threading.Tasks;

namespace Quack
{
    /// <summary>
    /// 인증에서 사용할 웹뷰의 인터페이스
    /// </summary>
    interface IAuthWebView
    {
        void SetWebViewLocation(Uri IAuthWebPage);
    }

    class QTwitter
    {
        private static QTwitter instance = null;

        Dictionary<string, TwitterContext> StoreMap = null;

        /// <summary>
        /// 싱글턴 획득 함수
        /// </summary>
        /// <returns>QTwitter의 싱글턴 객체</returns>
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

        QTwitter()
        {
            StoreMap = new Dictionary<string, TwitterContext>();
        }

        /// <summary>
        /// 이전에 인증된 계정들 정보를 로드
        /// </summary>
        void LoadFromStore()
        {
            // TODO: 이미 인증되었던 계정의 토큰을 로드
        }

/*        /// <summary>
        /// 새로 인증된 계정들의 정보를 저장소에 저장
        /// </summary>
        async void SaveToStore()
        {
            // TODO: 인증된 계정들의 토큰을 저장
        }*/

        /// <summary>
        /// TwitterContext획득
        /// </summary>
        /// <param name="userID">계정 고유 ID</param>
        /// <returns>TwitterContext</returns>
        /// <remarks>계정 고유 ID가 유효하지 않은 경우 예외 발생</remarks>
        public static TwitterContext GetContext(string userID)
        {
            QTwitter factory = instance;
            if (factory.StoreMap.ContainsKey(userID))
            {
                return factory.StoreMap[userID];
            }

            throw new ArgumentException("Invalid UserID");
        }

        /// <summary>
        /// Factory에 계정을 추가하기 위한 인증 시작
        /// </summary>
        /// <param name="page">인증에 사용될 WebView</param>
        /// <returns>인증 객체</returns>
        public static IAuthorizer BeginAuthorize(IAuthWebView page)
        {
            SingleUserInMemoryCredentialStore credential = new SingleUserInMemoryCredentialStore();
            credential.ConsumerKey = Constants.CONSUMER_KEY;
            credential.ConsumerSecret = Constants.CONSUMER_SECRET;

            PinAuthorizer auth = new PinAuthorizer
            {
                CredentialStore = credential,
                GoToTwitterAuthorization = pageLink =>
                {
                    Utilities.BeginInvoke(() =>
                    {
                        page.SetWebViewLocation(new Uri(pageLink));
                    });
                }
            };

            auth.BeginAuthorizeAsync();

            return auth;
        }

        /// <summary>
        /// 계정을 추가하기 위한 인증의 마무리
        /// </summary>
        /// <param name="auth">인증 객체</param>
        /// <param name="pin">인증용 PIN번호</param>
        /// <returns>생성된 TwitterContext</returns>
        public static TwitterContext CompleteAuthorize(IAuthorizer auth, string pin)
        {
            var pinAuth = (PinAuthorizer)auth;

            pinAuth.CompleteAuthorizeAsync(pin);

            TwitterContext context = new TwitterContext(auth);

//            instance.StoreMap.Add("temp", context);

//            instance.SaveToStore();

            return context;
        }
    }
}
