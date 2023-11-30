using System.ComponentModel.DataAnnotations;

namespace HubWallet.Models
{
    public class Wallet
    {
        public int Id { get; set; }
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
        public string? Name { get; set; }
        public string? Type { get; set; }
        [Required(ErrorMessage ="Account Number is required")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Account Number must contain only numeric digits.")]
        public string? AccountNumber {  get; set; }
        public string? AccountScheme {  get; set; }
        public DateTime CreatedAt { get; set; }
        [RegularExpression("^[0-9]+$", ErrorMessage = "Owner must be a phone number.")]
        public string? Owner {  get; set; }
    }

    //public enum WalletType
    //{
    //    Momo = 1,
    //    Card = 2
    //}

    //public enum AccountScheme
    //{
    //    Visa,
    //    Mastercard,
    //    Mtn,
    //    Vodafone,
    //    Airteltigo
    //}
}
