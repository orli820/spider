//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace spider
{
    using System;
    using System.Collections.Generic;
    
    public partial class 電影導演MovieDirector
    {
        public int 電影導演編號MD_ID { get; set; }
        public int 電影編號Movie_ID { get; set; }
        public int 導演編號Director_ID { get; set; }
    
        public virtual 電影Movies 電影Movies { get; set; }
        public virtual 導演總表Director 導演總表Director { get; set; }
    }
}
