//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Тест.file
{
    using System;
    using System.Collections.Generic;
    
    public partial class Клиенты
    {
        public int Код { get; set; }
        public int КодНомера { get; set; }
        public int КодПользователя { get; set; }
        public Nullable<System.DateTime> ДатаВьезда { get; set; }
        public Nullable<System.DateTime> ДатаВыезда { get; set; }
    
        public virtual Комнаты Комнаты { get; set; }
        public virtual Пользователи Пользователи { get; set; }
    }
}
