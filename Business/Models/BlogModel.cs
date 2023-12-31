﻿#nullable disable

using AppCore.Records.Bases;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class BlogModel : RecordBase // modeller de RecordBase'den miras almalıdır ki hem Id ve Guid alanlarını
                                        // miras alsın hem de servislerde tip olarak kullanılabilsin.
    {
        // ilgili entity'de referans olmayan özellikler veya başka bir deyişle veritabanındaki ilgili tablosundaki
        // sütun karşılığı olan özellikler entity'den kopyalanır.

        // SOLID prensipleri gereği bu class'ta validasyon için data annotation'lar kullanmak yerine MVC FluentValidation
        // gibi bir kütüphane üzerinden başka bir class'ta validasyonları yönetmek daha uygun olacaktır.
    
        #region Entity'den Kopyalanan Özellikler
        [Required]
        [StringLength(150)]
        public string Title { get; set; }

        [Required]
        [StringLength(300)]
        public string Content { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        [Range(1, 5)]
        public byte? Score { get; set; }

        [DisplayName("User")]
        [Required]
        public int? UserId { get; set; }
        #endregion

        // ihtiyaç halinde view'larda gösterim veya veri girişi için entity verilerini özelleştirip (formatlama,
        // ilişkili entity referansı üzerinden özellik kullanma, vb.) kullanacağımız yeni özellikler oluşturulabilir.
        #region View'larda Gösterim veya Veri Girişi için Kullanacağımız Özellikler
        [DisplayName("Create Date")]
        public string CreateDateDisplay { get; set; }

        [DisplayName("Update Date")]
        public string UpdateDateDisplay { get; set; }

        [DisplayName("User")]
        public string UserNameDisplay { get; set; }

		[DisplayName("Score")]
		public string ScoreDisplay { get; set; }

		// Many to many ilişkili kayıtları getirme 1. yöntem
		[DisplayName("Tags")]
        public List<TagModel> TagsDisplay { get; set; }

        // Many to many ilişkili kayıtları getirme 2. yöntem
        //[DisplayName("Tags")]
        //public string TagsDisplay { get; set; }

        [DisplayName("Tags")]
        [Required(ErrorMessage ="{0} are required!")]
        public List<int> TagIds { get; set; }
        #endregion

        /*
        Entity ve model özelliklerinde kullanılabilecek bazı genel data annotation'lar (attribute): 
        NOT: Data annotation'lar ile sadece model verisi üzerinden basit validasyonlar yapılabilir, örneğin veritabanındaki bir tablo üzerinden 
        validasyon gerekiyorsa bu validasyon service class'larında yapılmalıdır.

        Key (Entity): Özelliğin birincil anahtar olduğunu belirtir ve veritabanı oluşturulurken tablodaki sütun karşılığı otomatik artan sayı olarak ayarlanır.
        Required (Entity ve Model): Özelliğin zorunlu olduğunu belirtir.
        Column (Entity): Özelliğin veritabanı tablosundaki sütunu ile ilgili ayarlarını belirtir, örneğin sütun adı (Name), sütun veri tipi (TypeName) ve sütun sırası (Order: çoklu key için kullanılır).
        DataType (Model): Özelliğin veri tipi için kullanılır, örneğin Text, Date, Time, DateTime, Currency, EmailAddress, PhoneNumber, Password, v.b.
        ReadOnly (Model): Özelliğin sadece okunabilir olması için kullanılır.
        DisplayFormat (Model): Metinsel veri gösteriminde kullanılacak format'ı belirtir ve genellikle tarih, ondalık sayı, v.b. formatlama işlemleri için kullanılır.
        Table (Entity): Veritabanında oluşacak tablonun adını (Name) değiştirmek için kullanılır.
        StringLength (Entity ve Model): Metinsel tipte özellikler için girilecek karakter sayısının maksimumunu belirtmede kullanılır.
        MinLength (Model): Metinsel tipte özellikler için girilecek karakter sayısının minimumunu belirtmede kullanılır.
        MaxLength (Model): Metinsel tipte özellikler için girilecek karakter sayısının maksimumunu belirtmede kullanılır.
        Compare (Model): Tanımlandığı özelliğin başka bir özellik üzerinden verilerinin karşılaştırılması için kullanılır.
        RegularExpression (Model): Verilerin daha detaylı validasyonu için öğrenilip kullanılabilecek bir doğrulama desenidir.
        Range (Model): Sayısal değerler için aralık belirtmede kullanılır.
        EmailAddress (Model): Özellik verisinin e-posta formatında olması için kullanılır.
        Phone (Model): Özellik verisinin telefon formatında olması için kullanılır.
        NotMapped (Entity): Özelliğin veritabanında ilgili tablosunda sütununun oluşturulmaması için kullanılır.
        JsonIgnore (Model): Özelliğin oluşturulacak JSON formatındaki veriye dahil edilmemesini sağlar.
        */
    }
}
