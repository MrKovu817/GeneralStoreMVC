using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

    public class TransactionDetailModel
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int CustomerId { get; set; }

        public int Quantity { get; set; }

        public DateTime DateOfTransaction { get; set; }
    }
