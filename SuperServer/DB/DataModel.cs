using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.DB
{
    [Table("Account")]
    [Index(nameof(AccountName), IsUnique = true)]
    public class Account
    {
        public int AccountId { get; set; }

        public string AccountName { get; set; }
        public ICollection<Player> Players { get; set; }
    }

    [Table("Player")]
    [Index(nameof(PlayerName), IsUnique = true)]
    public class Player
    {
        public int PlayerId {get; set;}
        [ForeignKey("PlayerName")]
        public string PlayerName { get; set; }
        public Account Account { get; set; }
}
