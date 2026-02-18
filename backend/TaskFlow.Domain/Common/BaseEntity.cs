using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Domain.Common
{
    //Abstract Class (Soyut Sınıf), aslında bir "taslak" veya "yarım kalmış bir plan" gibidir.
    //Kendi başına bir nesneye dönüşemez (yani new anahtar kelimesiyle oluşturulamaz), ancak diğer sınıfların ne yapması gerektiğini dikte eder.
    //Kod Tekrarını Önlemek: Birden fazla sınıfta aynı olan özellikleri ve metotları bir üst sınıfta toplayıp ortak bir temel oluşturursunuz.
    //Zorunlu Kurallar Koymak: Alt sınıfların "mutlaka" uygulaması gereken kuralları belirlersiniz.
    //Esneklik (Polimorfizm): Kodunuzda spesifik sınıflar yerine ortak olan abstract sınıfı referans alarak daha genel ve esnek yapılar kurabilirsiniz.
    public abstract class BaseEntity
    {
        public int Id { get; protected set; }
        public DateTime CreatedDate { get; protected set; } = DateTime.UtcNow;
        public DateTime UpdatedDate { get; protected set; }
    }
}
