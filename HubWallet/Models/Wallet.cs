namespace HubWallet.Models
{
    public class Wallet
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? AccountNumber {  get; set; }
        public string? AccountScheme {  get; set; }
        public DateTime CreatedAt { get; set; }
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
