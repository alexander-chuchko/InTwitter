using System;
using System.Collections.Generic;
using InTwitter.Models;

namespace InTwitter.Mocks
{
    public class MarkState
    {
        #region ---Public Static Properties---

        private static MarkState _instance;

        public static MarkState Instance => _instance ??= new MarkState();

        #endregion

        #region ---Constructors---

        private MarkState()
        {
            this.Marks = new List<Mark>();
        }

        #endregion

        #region ---Public Properties---

        public List<Mark> Marks { get; set; }

        #endregion

    }
}
