using InTwitter.Models.Stories;
using System;
using System.Collections.Generic;
using System.Text;

namespace InTwitter.Mocks
{
    public class PostDataState
    {
        #region ---Public Static Properties---

        private static PostDataState _instance;

        public static PostDataState Instance => _instance ??= new PostDataState();

        #endregion

        #region ---Constructors---

        private PostDataState()
        {
            this.PostDatas = new List<PostData>();
        }

        #endregion

        #region ---Public Properties---

        public List<PostData> PostDatas { get; set; }

        #endregion
    }
}
